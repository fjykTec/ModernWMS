/*
 * date：2022-12-21
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
    /// Interface of SpuService
    /// </summary>
    public interface ISpuService : IBaseService<SpuEntity>
    {
        #region Api
        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        Task<(List<SpuBothViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser);
        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns></returns>
        Task<SpuBothViewModel> GetAsync(int id);
        /// <summary>
        /// get sku info by sku_id
        /// </summary>
        /// <param name="sku_id">sku_id</param>
        /// <returns></returns>
        Task<SkuDetailViewModel> GetSkuAsync(int sku_id);

        /// <summary>
        /// get sku info by bar_code
        /// </summary>
        /// <param name="bar_code">bar_code</param>
        /// <returns></returns>
        Task<SkuDetailViewModel> GetSkuByBarCodeAsync(string bar_code);

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        Task<(int id, string msg)> AddAsync(SpuBothViewModel viewModel, CurrentUser currentUser);
        /// <summary>
        /// update a record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> UpdateAsync(SpuBothViewModel viewModel);

        /// <summary>
        /// delete a record
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> DeleteAsync(int id);
        #endregion

        #region add or update sku_safety_stock
        /// <summary>
        /// add or update sku_safety_stock
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> InsertOrUpdateSkuSafetyStockAsync(SkuSafetyStockPutViewModel viewModel);
        #endregion
    }
}
 
