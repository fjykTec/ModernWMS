using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModernWMS.Core.DBContext;
using ModernWMS.Core.JWT;
using ModernWMS.Core.Models;
using ModernWMS.Core.Utility;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernWMS.Core
{
    public class FunctionHelper
    {
        private readonly SqlDBContext _dBContext;
        private readonly IHttpContextAccessor _accessor;

        public FunctionHelper(SqlDBContext dBContext
             , IHttpContextAccessor accessor)
        {
            _dBContext = dBContext;
            _accessor = accessor;
        }

        /// <summary>
        /// Get the current user information in the token
        /// </summary>
        /// <returns></returns>
        public CurrentUser GetCurrentUser()
        {
            if (_accessor.HttpContext == null)
            {
                return new CurrentUser();
            }
            var token = _accessor.HttpContext.Request.Headers["Authorization"].ObjToString();
            if (!token.StartsWith("Bearer"))
            {
                return new CurrentUser();
            }
            token = token.Replace("Bearer ", "");
            if (token.Length > 0)
            {
                var principal = new JwtSecurityTokenHandler().ValidateToken(token,
                                                                        new TokenValidationParameters
                                                                        {
                                                                            ValidateAudience = false,
                                                                            ValidateIssuer = false,
                                                                            ValidateIssuerSigningKey = true,
                                                                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GlobalConsts.SigningKey)),
                                                                            ValidateLifetime = false
                                                                        },
                                                                        out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return new CurrentUser();
                }
                var user = JsonHelper.DeserializeObject<CurrentUser>(principal.Claims.First(claim => claim.Type == ClaimValueTypes.Json).Value);
                if (user != null)
                {
                    return user;
                }
                else
                {
                    return new CurrentUser();
                }
            }
            else
            {
                return new CurrentUser();
            }
        }

        /// <summary>
        /// 序号表获取单据编号
        /// </summary>
        /// <param name="table_name">表名</param>
        /// <param name="prefix_char">前缀</param>
        /// <param name="reset_rule">重置规则</param>
        /// <returns></returns>
        public async Task<string> GetFormNoAsync(string table_name, string prefix_char = "", ResetRule reset_rule = ResetRule.Day)
        {
            var current_user = GetCurrentUser();
            var nums = await GetFormNoListAsync(table_name, 1, current_user.tenant_id, prefix_char, reset_rule);
            if (nums == null)
            {
                return "";
            }
            else
            {
                return nums[0];
            }
        }

        /// <summary>
        /// 序号表获取单据编号
        /// </summary>
        /// <param name="table_name">表名</param>
        /// <param name="tenant_id">租户id</param>
        /// <param name="prefix_char">前缀</param>
        /// <param name="Qty">编号数量</param>
        /// <param name="reset_rule">重置规则</param>
        /// <returns></returns>
        public async Task<List<string>> GetFormNoListAsync(string table_name, int Qty = 1, long tenant_id = 1, string prefix_char = "", ResetRule reset_rule = ResetRule.Day)
        {
            List<string> nums = new List<string>();
            string _reset_rule = "yyyyMMdd";
            if (reset_rule == ResetRule.Year) _reset_rule = "yyyy";
            else if (reset_rule == ResetRule.Month) _reset_rule = "yyyyMM";

            var dbSet = _dBContext.Set<GlobalUniqueSerialEntity>();
            var entity = await dbSet.FirstOrDefaultAsync(t => t.table_name == table_name && t.prefix_char == prefix_char && t.reset_rule == _reset_rule);
            if (entity == null)
            {
                for (int index = 1; index <= Qty; index++)
                {
                    nums.Add($"{prefix_char}{DateTime.Now.ToString(_reset_rule)}-{index.ToString().PadLeft(4, '0')}");
                }
                entity = new GlobalUniqueSerialEntity
                {
                    table_name = table_name,
                    prefix_char = prefix_char,
                    reset_rule = _reset_rule,
                    current_no = Qty + 1,
                    last_update_time = DateTime.Now,
                    tenant_id = tenant_id
                };
                dbSet.Add(entity);
                await _dBContext.SaveChangesAsync();
            }
            else
            {
                int current_no = entity.current_no;
                if (!DateTime.Now.ToString(_reset_rule).Equals(entity.last_update_time.ToString(_reset_rule)))
                {
                    current_no = 1;
                }
                for (int index = 1; index <= Qty; index++)
                {
                    nums.Add($"{prefix_char}{DateTime.Now.ToString(_reset_rule)}-{current_no.ToString().PadLeft(4, '0')}");
                    current_no++;
                }
                entity.current_no = current_no;
                entity.last_update_time = DateTime.Now;
                await _dBContext.SaveChangesAsync();
            }
            return nums;
        }

        /// <summary>
        /// 重置规则
        /// </summary>
        public enum ResetRule
        {
            /// <summary>
            /// 年
            /// </summary>
            Year,

            /// <summary>
            /// 月
            /// </summary>
            Month,

            /// <summary>
            /// 日
            /// </summary>
            Day
        }
    }
}