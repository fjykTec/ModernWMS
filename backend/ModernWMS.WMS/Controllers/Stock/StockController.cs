/*
 * date：2022-12-22
 * developer：NoNo
 */
 using Microsoft.AspNetCore.Mvc;
 using ModernWMS.Core.Controller;
 using ModernWMS.Core.Models;
 using ModernWMS.WMS.Entities.ViewModels;
 using ModernWMS.WMS.IServices;
 using Microsoft.Extensions.Localization;
 
 namespace ModernWMS.WMS.Controllers
 {
     /// <summary>
     /// stock controller
     /// </summary>
     [Route("stock")]
     [ApiController]
     [ApiExplorerSettings(GroupName = "WMS")]
     public class StockController : BaseController
     {
         #region Args
 
         /// <summary>
         /// stock Service
         /// </summary>
         private readonly IStockService _stockService;
 
         /// <summary>
         /// Localizer Service
         /// </summary>
         private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
         #endregion
 
         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         /// <param name="stockService">stock Service</param>
        /// <param name="stringLocalizer">Localizer</param>
         public StockController(
             IStockService stockService
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
         {
             this._stockService = stockService;
            this._stringLocalizer= stringLocalizer;
         }
         #endregion
 
         #region Api
         /// <summary>
         /// stock details page search
         /// </summary>
         /// <param name="pageSearch">args</param>
         /// <returns></returns>
         [HttpPost("stock-list")]
         public async Task<ResultModel<PageData<StockManagementViewModel>>> StockPageAsync(PageSearch pageSearch)
         {
             var (data, totals) = await _stockService.StockPageAsync(pageSearch, CurrentUser);
              
             return ResultModel<PageData<StockManagementViewModel>>.Success(new PageData<StockManagementViewModel>
             {
                 Rows = data,
                 Totals = totals
             });
         }
        /// <summary>
        /// location stock page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("location-list")]
        public async Task<ResultModel<PageData<LocationStockManagementViewModel>>> LocationStockPageAsync(PageSearch pageSearch)
        {
            var (data, totals) = await _stockService.LocationStockPageAsync(pageSearch, CurrentUser);

            return ResultModel<PageData<LocationStockManagementViewModel>>.Success(new PageData<LocationStockManagementViewModel>
            {
                Rows = data,
                Totals = totals
            });
        }

        /// <summary>
        /// page search select
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("select")]
        public async Task<ResultModel<PageData<StockViewModel>>> SelectPageAsync(PageSearch pageSearch)
        {
            var (data, totals) = await _stockService.SelectPageAsync(pageSearch, CurrentUser);

            return ResultModel<PageData<StockViewModel>>.Success(new PageData<StockViewModel>
            {
                Rows = data,
                Totals = totals
            });
        }

        /// <summary>
        /// sku page search select
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("sku-select")]
        public async Task<ResultModel<PageData<SkuSelectViewModel>>> SkuSelectPageAsync(PageSearch pageSearch)
        {
            var (data, totals) = await _stockService.SkuSelectPageAsync(pageSearch, CurrentUser);

            return ResultModel<PageData<SkuSelectViewModel>>.Success(new PageData<SkuSelectViewModel>
            {
                Rows = data,
                Totals = totals
            });
        }
        #endregion

    }
 }
 
