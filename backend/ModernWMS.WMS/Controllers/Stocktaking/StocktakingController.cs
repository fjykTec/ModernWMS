/*
 * date：2022-12-30
 * developer：AMo
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
     /// stocktaking controller
     /// </summary>
     [Route("stocktaking")]
     [ApiController]
     [ApiExplorerSettings(GroupName = "WMS")]
     public class StocktakingController : BaseController
     {
         #region Args
 
         /// <summary>
         /// stocktaking Service
         /// </summary>
         private readonly IStocktakingService _stocktakingService;
 
         /// <summary>
         /// Localizer Service
         /// </summary>
         private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
         #endregion
 
         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         /// <param name="stocktakingService">stocktaking Service</param>
        /// <param name="stringLocalizer">Localizer</param>
         public StocktakingController(
             IStocktakingService stocktakingService
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
         {
             this._stocktakingService = stocktakingService;
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
         public async Task<ResultModel<PageData<StocktakingViewModel>>> PageAsync(PageSearch pageSearch)
         {
             var (data, totals) = await _stocktakingService.PageAsync(pageSearch, CurrentUser);
              
             return ResultModel<PageData<StocktakingViewModel>>.Success(new PageData<StocktakingViewModel>
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
         public async Task<ResultModel<StocktakingViewModel>> GetAsync(int id)
         {
             var data = await _stocktakingService.GetAsync(id);
             if (data!=null)
             {
                 return ResultModel<StocktakingViewModel>.Success(data);
             }
             else
             {
                 return ResultModel<StocktakingViewModel>.Error(_stringLocalizer["not_exists_entity"]);
             }
         }
         /// <summary>
         /// add a new record
         /// </summary>
         /// <param name="viewModel">args</param>
         /// <returns></returns>
         [HttpPost]
         public async Task<ResultModel<int>> AddAsync(StocktakingBasicViewModel viewModel)
         { 
             var (id, msg) = await _stocktakingService.AddAsync(viewModel,CurrentUser);
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
        /// Confirm a record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPut]
         public async Task<ResultModel<bool>> PutAsync(StocktakingConfirmViewModel viewModel)
         {
             var (flag, msg) = await _stocktakingService.PutAsync(viewModel, CurrentUser);
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
        /// confrim a record and change stock and add to stockadjust
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpPut("adjustment-confirm")]
        public async Task<ResultModel<bool>> ConfirmAsync(int id)
        {
            var (flag, msg) = await _stocktakingService.ConfirmAsync(id, CurrentUser);
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
             var (flag, msg) = await _stocktakingService.DeleteAsync(id);
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
 
