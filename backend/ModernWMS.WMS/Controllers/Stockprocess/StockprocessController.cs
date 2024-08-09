/*
 * date：2022-12-23
 * developer：NoNo
 * modify:20240202
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
     /// stockprocess controller
     /// </summary>
     [Route("stockprocess")]
     [ApiController]
     [ApiExplorerSettings(GroupName = "WMS")]
     public class StockprocessController : BaseController
     {
         #region Args
 
         /// <summary>
         /// stockprocess Service
         /// </summary>
         private readonly IStockprocessService _stockprocessService;
 
         /// <summary>
         /// Localizer Service
         /// </summary>
         private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
         #endregion
 
         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         /// <param name="stockprocessService">stockprocess Service</param>
        /// <param name="stringLocalizer">Localizer</param>
         public StockprocessController(
             IStockprocessService stockprocessService
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
         {
             this._stockprocessService = stockprocessService;
            this._stringLocalizer= stringLocalizer;
         }
         #endregion
 
         #region Api
         /// <summary>
         /// page search
         /// </summary>
         /// <param name="pageSearch">args</param>
         /// <returns></returns>
         [HttpPost("list")]
         public async Task<ResultModel<PageData<StockprocessGetViewModel>>> PageAsync(PageSearch pageSearch)
         {
             var (data, totals) = await _stockprocessService.PageAsync(pageSearch, CurrentUser);
              
             return ResultModel<PageData<StockprocessGetViewModel>>.Success(new PageData<StockprocessGetViewModel>
             {
                 Rows = data,
                 Totals = totals
             });
         }
 
         /// <summary>
         /// get all records
         /// </summary>
         /// <returns>args</returns>
        [HttpGet("all")]
         public async Task<ResultModel<List<StockprocessGetViewModel>>> GetAllAsync()
         {
             var data = await _stockprocessService.GetAllAsync(CurrentUser);
             if (data.Any())
             {
                 return ResultModel<List<StockprocessGetViewModel>>.Success(data);
             }
             else
             {
                 return ResultModel<List<StockprocessGetViewModel>>.Success(new List<StockprocessGetViewModel>());
             }
         }
 
         /// <summary>
         /// get a record by id
         /// </summary>
         /// <returns>args</returns>
         [HttpGet]
         public async Task<ResultModel<StockprocessWithDetailViewModel>> GetAsync(int id)
         {
             var data = await _stockprocessService.GetAsync(id);
             if (data!=null)
             {
                 return ResultModel<StockprocessWithDetailViewModel>.Success(data);
             }
             else
             {
                 return ResultModel<StockprocessWithDetailViewModel>.Error(_stringLocalizer["not_exists_entity"]);
             }
         }
         /// <summary>
         /// add a new record
         /// </summary>
         /// <param name="viewModel">args</param>
         /// <returns></returns>
         [HttpPost]
         public async Task<ResultModel<int>> AddAsync(StockprocessViewModel viewModel)
         {
             var (id, msg) = await _stockprocessService.AddAsync(viewModel,CurrentUser);
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
         public async Task<ResultModel<bool>> UpdateAsync(StockprocessViewModel viewModel)
         {
             var (flag, msg) = await _stockprocessService.UpdateAsync(viewModel);
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
             var (flag, msg) = await _stockprocessService.DeleteAsync(id);
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
        /// confirm processing
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("process-confirm")]
        public async Task<ResultModel<string>> ConfirmProcess(int id)
        {
            var (flag, msg) = await _stockprocessService.ConfirmProcess(id,CurrentUser);
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
        /// confirm adjustment
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("adjustment-confirm")]
        public async Task<ResultModel<string>> ConfirmAdjustment(int id)
        {
            var (flag, msg) = await _stockprocessService.ConfirmAdjustment(id, CurrentUser);
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
 
