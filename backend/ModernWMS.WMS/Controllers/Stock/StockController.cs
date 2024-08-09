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
using ModernWMS.WMS.Entities.ViewModels.Stock;
using System.Collections.Generic;

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

        #endregion Args

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
            this._stringLocalizer = stringLocalizer;
        }

        #endregion constructor

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
        /// safety stock page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("safety-list")]
        public async Task<ResultModel<PageData<SafetyStockManagementViewModel>>> SafetyStockPageAsync(PageSearch pageSearch)
        {
            var (data, totals) = await _stockService.SafetyStockPageAsync(pageSearch, CurrentUser);

            return ResultModel<PageData<SafetyStockManagementViewModel>>.Success(new PageData<SafetyStockManagementViewModel>
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

        /// <summary>
        /// get stock infomation by phone
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        [HttpPost("qrcode-list")]
        public async Task<ResultModel<List<LocationStockManagementViewModel>>> LocationStockForPhoneAsync(LocationStockForPhoneSearchViewModel input)
        {
            var datas = await _stockService.LocationStockForPhoneAsync(input, CurrentUser);
            return ResultModel<List<LocationStockManagementViewModel>>.Success(datas);
        }

        /// <summary>
        /// delivery statistic
        /// </summary>
        /// <param name="input">input</param>
        /// <returns></returns>
        [HttpPost("delivery-list")]
        public async Task<ResultModel<PageData<DeliveryStatisticViewModel>>> LocationStockForPhoneAsync(DeliveryStatisticSearchViewModel input)
        {
            var (data, totals) = await _stockService.DeliveryStatistic(input, CurrentUser);
            return ResultModel<PageData<DeliveryStatisticViewModel>>.Success(new PageData<DeliveryStatisticViewModel>
            {
                Rows = data,
                Totals = totals
            });
        }



        /// <summary>
        /// stock age page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("stock-age-list")]
        public async Task<ResultModel<PageData<StockAgeViewModel>>> StockAgePageAsync(StockAgeSearchViewModel input)
        {
            var (data, totals) = await _stockService.StockAgePageAsync(input, CurrentUser);

            return ResultModel<PageData<StockAgeViewModel>>.Success(new PageData<StockAgeViewModel>
            {
                Rows = data,
                Totals = totals
            });
        }
        #endregion Api
    }
}