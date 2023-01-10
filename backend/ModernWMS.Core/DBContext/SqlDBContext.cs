
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Reflection;
using ModernWMS.Core;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ModernWMS.Core.DBContext
{
    /// <summary>
    /// SqlDBContext
    /// </summary>
    public class SqlDBContext : DbContext
    {
        /// <summary>
        ///  current user's tenant_id
        /// </summary>
        public byte tenant_id { get; set; } = 1;

        /// <summary>
        /// Database
        /// </summary>
        /// <returns></returns>
        public DatabaseFacade GetDatabase() => Database;




        /// <summary>
        /// s
        /// </summary>
        /// <param name="options">options</param>
        public SqlDBContext(DbContextOptions options) : base(options)
        {
           
        }

        #region overwrite
        /// <summary>
        /// Auto Mapping Entity
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder</param>
        private void MappingEntityTypes(ModelBuilder modelBuilder)
        {
            var baseType = typeof(Models.BaseModel);
            var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
            var referencedAssemblies = System.IO.Directory.GetFiles(path, $"ModernWMS*.dll").Select(Assembly.LoadFrom).ToArray();
            var list = referencedAssemblies
                .SelectMany(a => a.DefinedTypes)
                .Select(type => type.AsType())
                .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToList();
            if (list != null && list.Any())
            {
                list.ForEach(t =>
                {
                    var entityType = modelBuilder.Model.FindEntityType(t);
                    if (entityType == null)
                    {
                        modelBuilder.Model.AddEntityType(t);
                    }
                });
            }
        }
        /// <summary>
        /// overwrite OnModelCreating
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappingEntityTypes(modelBuilder);
/*            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(Models.IHasTenant).IsAssignableFrom(entityType.ClrType))
                {
                    ConfigureGlobalFiltersMethodInfo
                       .MakeGenericMethod(entityType.ClrType)
                       .Invoke(this, new object[] { modelBuilder });
                }
            }*/
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// create DbSet 
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <returns></returns>
        public virtual DbSet<T> GetDbSet<T>() where T : class
        {
            if (Model.FindEntityType(typeof(T)) != null)
            {
                return Set<T>();
            }
            else
            {
                throw new Exception($"type {typeof(T).Name} is not add into DbContext ");
            }
        }
        /// <summary>
        /// over write  EnsureCreated
        /// </summary>
        /// <returns></returns>
        public virtual bool EnsureCreated()
        {
            return Database.EnsureCreated();
        }
        /// <summary>
        /// over write OnConfiguring
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

    
        #endregion

       


        
    }
}
