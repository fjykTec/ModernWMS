using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;
using System.Text;
using ModernWMS.Core.DBContext;
using System.Reflection.PortableExecutable;
using Microsoft.Extensions.Logging.Debug;
using ModernWMS.Core.Swagger;
using ModernWMS.Core.JWT;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Cors.Infrastructure;
using ModernWMS.Core.Middleware;
using Microsoft.Extensions.DependencyModel;
using ModernWMS.Core.DI;
using Microsoft.Extensions.Localization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System.Security.AccessControl;
using Hangfire;
using Hangfire.MemoryStorage;

namespace ModernWMS.Core.Extentions
{
    public static class StartupExtensions
    {
        public static void AddExtensionsService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLocalization();
            services.AddSingleton<IStringLocalizer>((sp) =>
            {
                var sharedLocalizer = sp.GetRequiredService<IStringLocalizer<MultiLanguage>>();
                return sharedLocalizer;
            });
            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<CacheManager>();
            services.AddSingleton<IMemoryCache>(factory =>
            {
                var cache = new MemoryCache(new MemoryCacheOptions());
                return cache;
            });

            var database_config = configuration.GetSection("Database")["db"];
            services.AddDbContextPool<SqlDBContext>(t =>
            {
                if (database_config.ToUpper() == "SQLLITE")
                {
                    var SqlLite_connection = configuration.GetConnectionString("SqlLiteConn");
                    t.UseSqlite(SqlLite_connection, b => b.MigrationsAssembly("ModernWMS"));
                }
                else if (database_config.ToUpper() == "MYSQL")
                {
                    var Mysql_connection = configuration.GetConnectionString("MySqlConn");
                    t.UseMySql(Mysql_connection, new MySqlServerVersion(new Version(8, 0, 26)));
                }
                else if (database_config.ToUpper() == "SQLSERVER")
                {
                    var SqlServer_connection = configuration.GetConnectionString("SqlServerConn");
                    t.UseSqlServer(SqlServer_connection);
                }
                else if (database_config.ToUpper() == "POSTGRES")
                {
                    var Postgre_connection = configuration.GetConnectionString("PostGresConn");
                    t.UseNpgsql(Postgre_connection);
                    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
                    AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
                }
                t.EnableSensitiveDataLogging();
                t.UseLoggerFactory(new LoggerFactory(new[] { new DebugLoggerProvider() }));
            }, 100); ;
            services.AddMemoryCache();
            services.AddScoped<MultiTenancy.ITenantProvider, MultiTenancy.TenantProvider>();
            services.AddSwaggerService(configuration, AppContext.BaseDirectory);
            services.AddTokenGeneratorService(configuration);
            services.RegisterAssembly();
            services.AddControllers(c =>
            {
                c.Filters.Add(typeof(ViewModelActionFiter));
                c.MaxModelValidationErrors = 99999;
            }).ConfigureApiBehaviorOptions(o =>
            {
                o.SuppressModelStateInvalidFilter = true;
            })//format
              .AddNewtonsoftJson(options =>
              {
                  options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                  options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                  options.SerializerSettings.Converters.Add(new JsonStringTrimConverter());
                  options.SerializerSettings.Formatting = Formatting.Indented;
                  options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
              }).AddDataAnnotationsLocalization(options =>
              {
                  options.DataAnnotationLocalizerProvider = (type, factory) =>
                      factory.Create(typeof(ModernWMS.Core.MultiLanguage));
              });

            // Hangfire
            services.AddHangfire(x => x.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseStorage(new MemoryStorage()));
            services.AddHangfireServer();
            services.AddScoped<FunctionHelper>();
        }

        public static void UseExtensionsConfigure(this IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseSwaggerConfigure(configuration);
            app.UseRouting();
            app.UseMiddleware<ModernWMS.Core.Middleware.CorsMiddleware>();
            app.UseTokenGeneratorConfigure(configuration);
            app.UseAuthorization();
            app.UseMiddleware<GlobalExceptionMiddleware>();
            var support_languages = new[] { "zh-cn", "en-us" };
            var localization_options = new RequestLocalizationOptions()
                .SetDefaultCulture(support_languages[0])
                .AddSupportedCultures(support_languages)
                .AddSupportedUICultures(support_languages);
            app.UseRequestLocalization(localization_options);

            var option = new BackgroundJobServerOptions
            {
                ServerName = String.Format("{0}.{1}", Environment.MachineName, Guid.NewGuid().ToString()),
                WorkerCount = Environment.ProcessorCount * 5,
                Queues = new[] { "wms" }
            };
            app.UseHangfireServer(option);
            app.UseHangfireDashboard();
            AddHangfireJob(serviceProvider);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        #region Swagger

        /// <summary>
        /// Swagger
        /// </summary>
        /// <param name="services">服务容器</param>
        /// <param name="configuration">配置文件</param>
        /// <param name="BaseDirectory">主目录</param>
        private static void AddSwaggerService(this IServiceCollection services, IConfiguration configuration, string BaseDirectory)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var swaggerSettings = configuration.GetSection("SwaggerSettings");

            var provider = services.Configure<SwaggerSettings>(swaggerSettings).BuildServiceProvider();
            var settings = provider.GetService<IOptions<SwaggerSettings>>()?.Value;

            if (settings != null && settings.Name.Equals("ModernWMS"))
            {
                services.AddSwaggerGen(c =>
                {
                    typeof(CustomApiVersion.ApiVersions).GetEnumNames().ToList().ForEach(version =>
                    {
                        c.SwaggerDoc(version, new OpenApiInfo
                        {
                            Title = settings.ApiTitle,
                            Version = settings.ApiVersion,
                            Description = settings.Description
                        });
                    });

                    if (settings.XmlFiles != null && settings.XmlFiles.Count > 0)
                    {
                        settings.XmlFiles.ForEach(fileName =>
                        {
                            if (File.Exists(Path.Combine(BaseDirectory, fileName)))
                            {
                                c.IncludeXmlComments(Path.Combine(BaseDirectory, fileName), true);
                            }
                        });
                    }

                    c.OperationFilter<AddResponseHeadersFilter>();
                    c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                    c.OperationFilter<SecurityRequirementsOperationFilter>();

                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "please input Bearer {token}",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });
                    c.SwaggerGeneratorOptions.DescribeAllParametersInCamelCase = false;
                });
            }
        }

        /// <summary>
        /// register Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration">配置文件</param>
        private static void UseSwaggerConfigure(this IApplicationBuilder app, IConfiguration configuration)
        {
            var swaggerSettings = configuration.GetSection("SwaggerSettings");

            if (swaggerSettings != null && swaggerSettings["Name"].Equals("ModernWMS"))
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    typeof(CustomApiVersion.ApiVersions).GetEnumNames().OrderBy(e => e).ToList().ForEach(version =>
                    {
                        c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{swaggerSettings["Name"]} {version}");
                    });

                    c.IndexStream = () => Assembly.GetExecutingAssembly().GetManifestResourceStream("ModernWMS.Core.Swagger.index.html");
                    c.RoutePrefix = "";
                });
            }
        }

        #endregion Swagger

        #region JWT

        /// <summary>
        /// register JWT
        /// </summary>
        /// <param name="services">services</param>
        /// <param name="configuration">configuration</param>
        private static void AddTokenGeneratorService(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            var tokenSettings = configuration.GetSection("TokenSettings");
            services.Configure<TokenSettings>(tokenSettings);
            services.AddTransient<ITokenManager, TokenManager>();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = nameof(ApiResponseHandler);
                options.DefaultForbidScheme = nameof(ApiResponseHandler);
            }
            )
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidAudience = tokenSettings["Audience"],
                    ValidateIssuer = true,
                    ValidIssuer = tokenSettings["Issuer"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSettings["SigningKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), o => { });
        }

        private static void UseTokenGeneratorConfigure(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseAuthentication();
        }

        #endregion JWT

        #region dynamic injection

        /// <summary>
        /// judge the dll to be injected by IDependency
        /// </summary>
        /// <param name="services">services</param>
        private static IServiceCollection RegisterAssembly(this IServiceCollection services)
        {
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = System.IO.Directory.GetFiles(path, "ModernWMS*.dll").Select(Assembly.LoadFrom).ToArray();

            var types = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
            .Select(type => type.AsType())
                .Where(x => x != typeof(IDependency) && typeof(IDependency).IsAssignableFrom(x)).ToArray();
            var implementTypes = types.Where(x => x.IsClass).ToArray();
            var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
            foreach (var implementType in implementTypes)
            {
                var interfaceType = interfaceTypes.FirstOrDefault(x => x.IsAssignableFrom(implementType));
                if (interfaceType != null)
                    services.AddScoped(interfaceType, implementType);
            }

            services.AddScoped<Services.IAccountService, Services.AccountService>();

            // Register Job
            var typeJobs = referencedAssemblies
               .SelectMany(a => a.DefinedTypes)
            .Select(type => type.AsType())
               .Where(x => x != typeof(Job.IJob) && typeof(Job.IJob).IsAssignableFrom(x)).ToArray();
            if (types != null && types.Length > 0)
            {
                var implementJobs = typeJobs.Where(x => x.IsClass).ToArray();
                foreach (var implementType in implementJobs)
                {
                    services.AddScoped(implementType);
                }
            }

            return services;
        }

        /// <summary>
        /// AddHangfireJob
        /// </summary>
        /// <param name="serviceProvider"></param>
        private static void AddHangfireJob(IServiceProvider serviceProvider)
        {
            var baseType = typeof(Core.Job.IJob);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = System.IO.Directory.GetFiles(path, "ModernWMS*.dll").Select(Assembly.LoadFrom).ToArray();
            var types = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();
            if (types != null && types.Length > 0)
            {
                var implementTypes = types.Where(x => x.IsClass).ToArray();
                foreach (var implementType in implementTypes)
                {
                    var job = serviceProvider.GetService(implementType) as Core.Job.IJob;
                    if (job != null)
                    {
                        Hangfire.RecurringJob.AddOrUpdate(() => job.Execute(), job.CronExpression, TimeZoneInfo.Local, "wms");
                    }
                }
            }
        }

        #endregion dynamic injection
    }
}