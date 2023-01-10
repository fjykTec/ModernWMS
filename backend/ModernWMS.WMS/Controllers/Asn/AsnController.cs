/*
 * date：2022-12-22
 * developer：AMo
 */
 using Microsoft.AspNetCore.Mvc;
 using ModernWMS.Core.Controller;
 using ModernWMS.Core.Models;
 using ModernWMS.WMS.Entities.ViewModels;
 using ModernWMS.WMS.IServices;
 using Microsoft.Extensions.Localization;
using ModernWMS.Core.JWT;

namespace ModernWMS.WMS.Controllers
{
    /// <summary>
    /// asn controller
    /// </summary>
    [Route("asn")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "WMS")]
    public class AsnController : BaseController
    {
        #region Args

        /// <summary>
        /// asn Service
        /// </summary>
        private readonly IAsnService _asnService;

        /// <summary>
        /// Localizer Service
        /// </summary>
        private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="asnService">asn Service</param>
        /// <param name="stringLocalizer">Localizer</param>
        public AsnController(
            IAsnService asnService
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            )
        {
            this._asnService = asnService;
            this._stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Api
        /// <summary>
        /// page search, sqlTitle input asn_status:0 ~ 4
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<ResultModel<PageData<AsnViewModel>>> PageAsync(PageSearch pageSearch)
        {
            var (data, totals) = await _asnService.PageAsync(pageSearch, CurrentUser);

            return ResultModel<PageData<AsnViewModel>>.Success(new PageData<AsnViewModel>
            {
                Rows = data,
                Totals = totals
            });
        }

        /// <summary>
        /// get a record by id
        /// </summary>
        /// <returns>args</returns>
        [HttpGet]
        public async Task<ResultModel<AsnViewModel>> GetAsync(int id)
        {
            var data = await _asnService.GetAsync(id);
            if (data != null && data.id > 0)
            {
                return ResultModel<AsnViewModel>.Success(data);
            }
            else
            {
                return ResultModel<AsnViewModel>.Error(_stringLocalizer["not_exists_entity"]);
            }
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<int>> AddAsync(AsnViewModel viewModel)
        {
            var (id, msg) = await _asnService.AddAsync(viewModel, CurrentUser);
            if (id > 0)
            {
                return ResultModel<int>.Success(id);
            }
            else
            {
                return ResultModel<int>.Error(msg);
            }
        }

        /// <summary>
        /// update a record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultModel<bool>> UpdateAsync(AsnViewModel viewModel)
        {
            var (flag, msg) = await _asnService.UpdateAsync(viewModel);
            if (flag)
            {
                return ResultModel<bool>.Success(flag);
            }
            else
            {
                return ResultModel<bool>.Error(msg, 400, flag);
            }
        }

        /// <summary>
        /// delete a record
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ResultModel<string>> DeleteAsync(int id)
        {
            var (flag, msg) = await _asnService.DeleteAsync(id);
            if (flag)
            {
                return ResultModel<string>.Success(msg);
            }
            else
            {
                return ResultModel<string>.Error(msg);
            }
        }

        /// <summary>
        /// Bulk modify Goodsowner
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPut("bulk-modify-goods-owner")]
        public async Task<ResultModel<bool>> BulkModifyGoodsownerAsync(AsnBulkModifyGoodsOwnerViewModel viewModel)
        {
            var (flag, msg) = await _asnService.BulkModifyGoodsownerAsync(viewModel);
            if (flag)
            {
                return ResultModel<bool>.Success(flag);
            }
            else
            {
                return ResultModel<bool>.Error(msg, 400, flag);
            }
        }

        #endregion

        #region Flow Api
        /// <summary>
        /// Confirm Delivery
        /// change the asn_status from 0 to 1
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpPut("confirm/{id}")]
        public async Task<ResultModel<string>> ConfirmAsync(int id)
        {
            var (flag, msg) = await _asnService.ConfirmAsync(id);
            if (flag)
            {
                return ResultModel<string>.Success(msg);
            }
            else
            {
                return ResultModel<string>.Error(msg);
            }
        }

        /// <summary>
        /// Cancel confirm, change asn_status 1 to 0
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpPut("confirm-cancel/{id}")]
        public async Task<ResultModel<string>> ConfirmCancelAsync(int id)
        {
            var (flag, msg) = await _asnService.ConfirmCancelAsync(id);
            if (flag)
            {
                return ResultModel<string>.Success(msg);
            }
            else
            {
                return ResultModel<string>.Error(msg);
            }
        }

        /// <summary>
        /// Unload
        /// change the asn_status from 1 to 2
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpPut("unload/{id}")]
        public async Task<ResultModel<string>> UnloadAsync(int id)
        {
            var (flag, msg) = await _asnService.UnloadAsync(id);
            if (flag)
            {
                return ResultModel<string>.Success(msg);
            }
            else
            {
                return ResultModel<string>.Error(msg);
            }
        }

        /// <summary>
        /// Cancel unload
        /// change the asn_status from 2 to 1
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpPut("unload-cancel/{id}")]
        public async Task<ResultModel<string>> UnloadCancelAsync(int id)
        {
            var (flag, msg) = await _asnService.UnloadCancelAsync(id);
            if (flag)
            {
                return ResultModel<string>.Success(msg);
            }
            else
            {
                return ResultModel<string>.Error(msg);
            }
        }

        /// <summary>
        /// sorting， add a new asnsort record and update asn sorted_qty
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPut("sorting")]
        public async Task<ResultModel<string>> SortingAsync(AsnsortInputViewModel viewModel)
        {
            var (flag, msg) = await _asnService.SortingAsync(viewModel, CurrentUser);
            if (flag)
            {
                return ResultModel<string>.Success(msg);
            }
            else
            {
                return ResultModel<string>.Error(msg);
            }
        }
        
        /// <summary>
        /// Sorted
        /// change the asn_status from 2 to 3
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpPut("sorted/{id}")]
        public async Task<ResultModel<string>> SortedAsync(int id)
        {
            var (flag, msg) = await _asnService.SortedAsync(id);
            if (flag)
            {
                return ResultModel<string>.Success(msg);
            }
            else
            {
                return ResultModel<string>.Error(msg);
            }
        }

        /// <summary>
        /// Cancel sorted
        /// change the asn_status from 3 to 2
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpPut("sorted-cancel/{id}")]
        public async Task<ResultModel<string>> SortedCancelAsync(int id)
        {
            var (flag, msg) = await _asnService.SortedCancelAsync(id);
            if (flag)
            {
                return ResultModel<string>.Success(msg);
            }
            else
            {
                return ResultModel<string>.Error(msg);
            }
        }

        /// <summary>
        /// PutAway
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPut("putaway")]
        public async Task<ResultModel<string>> PutAwayAsync(AsnPutAwayInputViewModel viewModel)
        {
            var (flag, msg) = await _asnService.PutAwayAsync(viewModel, CurrentUser);
            if (flag)
            {
                return ResultModel<string>.Success(msg);
            }
            else
            {
                return ResultModel<string>.Error(msg);
            }
        }

        #endregion
    }
}
 
