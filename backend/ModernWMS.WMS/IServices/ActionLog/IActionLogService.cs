/*
 * date：2023-08-24
 * developer：NoNo
 */

using ModernWMS.Core.JWT;
using ModernWMS.Core.Models;
using ModernWMS.Core.Services;
using ModernWMS.WMS.Entities.Models;
using ModernWMS.WMS.Entities.ViewModels;

namespace ModernWMS.WMS.IServices
{
    /// <summary>
    /// Interface of Action_logService
    /// </summary>
    public interface IActionLogService : IBaseService<ActionLogEntity>
    {
        #region Api

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        Task<(List<ActionLogViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser);

        /// <summary>
        /// add a new log record
        /// </summary>
        /// <returns></returns>
        Task<bool> AddLogAsync(string vue_path, string content, CurrentUser currentUser);

        #endregion Api
    }
}