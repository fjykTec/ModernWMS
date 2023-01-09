using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ModernWMS.Core.MultiTenancy
{
    /// <summary>
    /// TenantProvider
    /// </summary>
    public class TenantProvider:ITenantProvider
    {
        /// <summary>
        /// Tenant
        /// </summary>
        private readonly Tenant tenant;
        /// <summary>
        /// TenantProvider
        /// </summary>
        /// <param name="accessor">注入IHttpContextAccessor</param>
        public TenantProvider(IHttpContextAccessor accessor)
        {
            if (accessor.HttpContext != null)
            {
                var headers = accessor.HttpContext.Request.Headers;
                if (headers != null && headers.Count > 0
                    && headers.ContainsKey("TenantName"))
                {
                    var name = headers["TenantName"].FirstOrDefault();
                }
            }
            //Default Value
            tenant = new Tenant();
        }

        /// <summary>
        /// Get Current User's TenantID
        /// </summary>
        /// <returns></returns>
        public byte GetCurrentTenantID()
        {
            return tenant.tenant_id;
        }
    }
}
