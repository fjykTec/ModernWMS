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
using ModernWMS.WMS.Entities.Models;

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


        #region Arrival list 
        /// <summary>
        /// Arrival list
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("asnmaster/list")]
        public async Task<ResultModel<PageData<AsnmasterBothViewModel>>> PageAsnmasterAsync(PageSearch pageSearch)
        {
            var (data, totals) = await _asnService.PageAsnmasterAsync(pageSearch, CurrentUser);

            return ResultModel<PageData<AsnmasterBothViewModel>>.Success(new PageData<AsnmasterBothViewModel>
            {
                Rows = data,
                Totals = totals
            });
        }
        /// <summary>
        /// get Arrival list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("asnmaster")]
        public async Task<ResultModel<AsnmasterBothViewModel>> GetAsnmasterAsync(int id)
        {
            var data = await _asnService.GetAsnmasterAsync(id, CurrentUser);
            if (data != null && data.id > 0)
            {
                return ResultModel<AsnmasterBothViewModel>.Success(data);
            }
            else
            {
                return ResultModel<AsnmasterBothViewModel>.Error(_stringLocalizer["not_exists_entity"]);
            }
        }

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <returns></returns>
        [HttpPost("asnmaster")]
        public async Task<ResultModel<int>> AddAsnmasterAsync(AsnmasterBothViewModel viewModel)
        {
            var (id, msg) = await _asnService.AddAsnmasterAsync(viewModel, CurrentUser);
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
        /// update record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <returns></returns>
        [HttpPut("asnmaster")]
        public async Task<ResultModel<bool>> UpdateAsnmasterAsync(AsnmasterBothViewModel viewModel)
        {
            var (flag, msg) = await _asnService.UpdateAsnmasterAsync(viewModel, CurrentUser);
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
        [HttpDelete("asnmaster")]
        public async Task<ResultModel<string>> DeleteAsnmasterAsync(int id)
        {
            var (flag, msg) = await _asnService.DeleteAsnmasterAsync(id);
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

        #region New Flow Api
        /// <summary>
        /// Confirm Delivery
        /// change the asn_status from 0 to 1
        /// </summary>
        /// <param name="viewModels">args</param>
        /// <returns></returns>
        [HttpPut("confirm")]
        public async Task<ResultModel<string>> ConfirmAsync(List<AsnConfirmInputViewModel> viewModels)
        {
            var (flag, msg) = await _asnService.ConfirmAsync(viewModels);
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
        /// <param name="idList">id list</param>
        /// <returns></returns>
        [HttpPut("confirm-cancel")]
        public async Task<ResultModel<string>> ConfirmCancelAsync(List<int> idList)
        {
            var (flag, msg) = await _asnService.ConfirmCancelAsync(idList);
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
        /// <param name="viewModels">args</param>
        /// <returns></returns>
        [HttpPut("unload")]
        public async Task<ResultModel<string>> UnloadAsync(List<AsnUnloadInputViewModel> viewModels)
        {
            var (flag, msg) = await _asnService.UnloadAsync(viewModels, CurrentUser);
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
        /// <param name="idList">id list</param>
        /// <returns></returns>
        [HttpPut("unload-cancel")]
        public async Task<ResultModel<string>> UnloadCancelAsync(List<int> idList)
        {
            var (flag, msg) = await _asnService.UnloadCancelAsync(idList);
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
        /// <param name="viewModels">args</param>
        /// <returns></returns>
        [HttpPut("sorting")]
        public async Task<ResultModel<string>> SortingAsync(List<AsnsortInputViewModel> viewModels)
        {
            var (flag, msg) = await _asnService.SortingAsync(viewModels, CurrentUser);
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
        /// get asnsorts list by asn_id
        /// </summary>
        /// <param name="asn_id">asn id</param>
        /// <returns></returns>
        [HttpGet("sorting")]
        public async Task<ResultModel<List<AsnsortViewModel>>> GetAsnsortsAsync(int asn_id)
        {
            var data = await _asnService.GetAsnsortsAsync(asn_id);
            return ResultModel<List<AsnsortViewModel>>.Success(data);
        }

        /// <summary>
        /// update or delete asnsorts data
        /// </summary>
        /// <param name="entities">data</param>
        /// <returns></returns>
        [HttpPut("sorting-modify")]
        public async Task<ResultModel<string>> ModifyAsnsortsAsync(List<AsnsortEntity> entities)
        {
            var (flag, msg) = await _asnService.ModifyAsnsortsAsync(entities, CurrentUser);
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
        /// <param name="idList">id list</param>
        /// <returns></returns>
        [HttpPut("sorted")]
        public async Task<ResultModel<string>> SortedAsync(List<int> idList)
        {
            var (flag, msg) = await _asnService.SortedAsync(idList);
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
        /// <param name="idList">id list</param>
        /// <returns></returns>
        [HttpPut("sorted-cancel")]
        public async Task<ResultModel<string>> SortedCancelAsync(List<int> idList)
        {
            var (flag, msg) = await _asnService.SortedCancelAsync(idList);
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
        /// get pending putaway data by asn_id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("pending-putaway")]
        public async Task<ResultModel<List<AsnPendingPutawayViewModel>>> GetPendingPutawayDataAsync(int id)
        {
            var data = await _asnService.GetPendingPutawayDataAsync(id);
            data ??= new List<AsnPendingPutawayViewModel>();
            return ResultModel<List<AsnPendingPutawayViewModel>>.Success(data);
        }

        /// <summary>
        /// PutAway
        /// </summary>
        /// <param name="viewModels">args</param>
        /// <returns></returns>
        [HttpPut("putaway")]
        public async Task<ResultModel<string>> PutAwayAsync(List<AsnPutAwayInputViewModel> viewModels)
        {
            var (flag, msg) = await _asnService.PutAwayAsync(viewModels, CurrentUser);
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

        #region print series number
        /// <summary>
        /// print series number
        /// </summary>
        /// <param name="input">selected asn id</param>
        /// <returns></returns>
        [HttpPost("print-sn")]
        public async Task<ResultModel<List<AsnPrintSeriesNumberViewModel>>> GetAsnPrintSeriesNumberAsync(List<int> input)
        {
            var data = await _asnService.GetAsnPrintSeriesNumberAsync(input);
            return ResultModel<List<AsnPrintSeriesNumberViewModel>>.Success(data);
        }
        #endregion
    }
}
 
