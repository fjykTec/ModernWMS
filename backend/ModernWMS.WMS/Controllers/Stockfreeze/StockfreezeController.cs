/*
 * date：2022-12-26
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
     /// stockfreeze controller
     /// </summary>
     [Route("stockfreeze")]
     [ApiController]
     [ApiExplorerSettings(GroupName = "WMS")]
     public class StockfreezeController : BaseController
     {
         #region Args
 
         /// <summary>
         /// stockfreeze Service
         /// </summary>
         private readonly IStockfreezeService _stockfreezeService;
 
         /// <summary>
         /// Localizer Service
         /// </summary>
         private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
         #endregion
 
         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         /// <param name="stockfreezeService">stockfreeze Service</param>
        /// <param name="stringLocalizer">Localizer</param>
         public StockfreezeController(
             IStockfreezeService stockfreezeService
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
         {
             this._stockfreezeService = stockfreezeService;
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
         public async Task<ResultModel<PageData<StockfreezeViewModel>>> PageAsync(PageSearch pageSearch)
         {
             var (data, totals) = await _stockfreezeService.PageAsync(pageSearch, CurrentUser);
              
             return ResultModel<PageData<StockfreezeViewModel>>.Success(new PageData<StockfreezeViewModel>
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
         public async Task<ResultModel<List<StockfreezeViewModel>>> GetAllAsync()
         {
             var data = await _stockfreezeService.GetAllAsync(CurrentUser);
             if (data.Any())
             {
                 return ResultModel<List<StockfreezeViewModel>>.Success(data);
             }
             else
             {
                 return ResultModel<List<StockfreezeViewModel>>.Success(new List<StockfreezeViewModel>());
             }
         }
 
         /// <summary>
         /// get a record by id
         /// </summary>
         /// <returns>args</returns>
         [HttpGet]
         public async Task<ResultModel<StockfreezeViewModel>> GetAsync(int id)
         {
             var data = await _stockfreezeService.GetAsync(id);
             if (data!=null)
             {
                 return ResultModel<StockfreezeViewModel>.Success(data);
             }
             else
             {
                 return ResultModel<StockfreezeViewModel>.Error(_stringLocalizer["not_exists_entity"]);
             }
         }
         /// <summary>
         /// add a new record
         /// </summary>
         /// <param name="viewModel">args</param>
         /// <returns></returns>
         [HttpPost]
         public async Task<ResultModel<int>> AddAsync(StockfreezeViewModel viewModel)
         {
             var (id, msg) = await _stockfreezeService.AddAsync(viewModel,CurrentUser);
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
         public async Task<ResultModel<bool>> UpdateAsync(StockfreezeViewModel viewModel)
         {
             var (flag, msg) = await _stockfreezeService.UpdateAsync(viewModel);
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
             var (flag, msg) = await _stockfreezeService.DeleteAsync(id);
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
 
