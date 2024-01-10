/*
 * date：2023-08-24
 * developer：NoNo
 */

using Mapster;
using Microsoft.EntityFrameworkCore;
using ModernWMS.Core.DBContext;
using ModernWMS.Core.Services;
using ModernWMS.WMS.Entities.Models;
using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.WMS.IServices;
using Microsoft.Extensions.Localization;
using ModernWMS.Core.DynamicSearch;
using ModernWMS.Core.JWT;
using ModernWMS.Core.Models;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  ActionLog Service
    /// </summary>
    public class ActionLogService : BaseService<ActionLogEntity>, IActionLogService
    {
        #region Args

        /// <summary>
        /// The DBContext
        /// </summary>
        private readonly SqlDBContext _dBContext;

        /// <summary>
        /// Localizer Service
        /// </summary>
        private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;

        #endregion Args

        #region constructor

        /// <summary>
        ///ActionLog  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public ActionLogService(
            SqlDBContext dBContext
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            )
        {
            this._dBContext = dBContext;
            this._stringLocalizer = stringLocalizer;
        }

        #endregion constructor

        #region Api

        /// <summary>
        /// add a new log record
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddLogAsync(string vue_path, string content, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<ActionLogEntity>();
            var entity = new ActionLogEntity();
            entity.vue_path = vue_path;
            entity.action_content = content;
            entity.id = 0;
            entity.action_time = DateTime.Now;
            entity.tenant_id = currentUser.tenant_id;
            entity.user_name = currentUser.user_name;
            await DbSet.AddAsync(entity);
            await _dBContext.SaveChangesAsync();
            if (entity.id > 0)
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<ActionLogViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var query = from log in _dBContext.GetDbSet<ActionLogEntity>().AsNoTracking()
                        where log.tenant_id == currentUser.tenant_id
                        select new ActionLogViewModel
                        {
                            id = log.id,
                            vue_path = log.vue_path,
                            user_name = log.user_name,
                            action_content = log.action_content,
                            action_time = log.action_time,
                        };
            query = query.Where(queries.AsExpression<ActionLogViewModel>());
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.action_time)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list, totals);
        }

        #endregion Api
    }
}