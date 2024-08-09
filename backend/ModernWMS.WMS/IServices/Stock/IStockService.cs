/*
 * date：2022-12-22
 * developer：NoNo
 */

using ModernWMS.Core.Services;
using ModernWMS.Core.Models;
using ModernWMS.Core.JWT;
using ModernWMS.WMS.Entities.Models;
using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.WMS.Entities.ViewModels.Stock;

namespace ModernWMS.WMS.IServices
{
    /// <summary>
    /// Interface of StockService
    /// </summary>
    public interface IStockService : IBaseService<StockEntity>
    {
        #region Api

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        Task<(List<StockManagementViewModel> data, int totals)> StockPageAsync(PageSearch pageSearch, CurrentUser currentUser);

        /// <summary>
        /// location stock page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        Task<(List<LocationStockManagementViewModel> data, int totals)> LocationStockPageAsync(PageSearch pageSearch, CurrentUser currentUser);

        /// <summary>
        ///  page search select
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        Task<(List<StockViewModel> data, int totals)> SelectPageAsync(PageSearch pageSearch, CurrentUser currentUser);

        /// <summary>
        /// sku page search select
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        Task<(List<SkuSelectViewModel> data, int totals)> SkuSelectPageAsync(PageSearch pageSearch, CurrentUser currentUser);

        /// <summary>
        /// safety stock
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        Task<(List<SafetyStockManagementViewModel> data, int totals)> SafetyStockPageAsync(PageSearch pageSearch, CurrentUser currentUser);

        /// <summary>
        /// get stock infomation by phone
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        Task<List<LocationStockManagementViewModel>> LocationStockForPhoneAsync(LocationStockForPhoneSearchViewModel input, CurrentUser currentUser);

        /// <summary>
        /// delivery statistic
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        Task<(List<DeliveryStatisticViewModel> datas, int totals)> DeliveryStatistic(DeliveryStatisticSearchViewModel input, CurrentUser currentUser);


        /// <summary>
        /// stock age page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        Task<(List<StockAgeViewModel> data, int totals)> StockAgePageAsync(StockAgeSearchViewModel input, CurrentUser currentUser);
        #endregion Api
    }
}