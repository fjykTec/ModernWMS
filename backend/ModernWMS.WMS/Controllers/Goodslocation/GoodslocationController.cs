/*
 * date：2022-12-21
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
    /// goodslocation controller
    /// </summary>
     [Route("goodslocation")]
     [ApiController]
     [ApiExplorerSettings(GroupName = "Base")]
     public class GoodslocationController : BaseController
     {
         #region Args
 
         /// <summary>
         /// goodslocation Service
         /// </summary>
         private readonly IGoodslocationService _goodslocationService;
 
         /// <summary>
         /// Localizer Service
         /// </summary>
         private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
         #endregion
 
         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         /// <param name="goodslocationService">goodslocation Service</param>
        /// <param name="stringLocalizer">Localizer</param>
         public GoodslocationController(
             IGoodslocationService goodslocationService
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
         {
             this._goodslocationService = goodslocationService;
            this._stringLocalizer= stringLocalizer;
         }
        #endregion

        #region Api
        /// <summary>
        /// get select items
        /// </summary>
        /// <returns></returns>
        [HttpGet("location-by-warehouseare_id")]
        public async Task<ResultModel<List<FormSelectItem>>> GetSelectItemsAsnyc(int warehousearea_id)
        {
            var datas = await _goodslocationService.GetGoodslocationByWarehouse_area_id(warehousearea_id, CurrentUser);
            return ResultModel<List<FormSelectItem>>.Success(datas);
        }

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("list")]
         public async Task<ResultModel<PageData<GoodslocationViewModel>>> PageAsync(PageSearch pageSearch)
         {
             var (data, totals) = await _goodslocationService.PageAsync(pageSearch, CurrentUser);
              
             return ResultModel<PageData<GoodslocationViewModel>>.Success(new PageData<GoodslocationViewModel>
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
         public async Task<ResultModel<List<GoodslocationViewModel>>> GetAllAsync()
         {
             var data = await _goodslocationService.GetAllAsync(CurrentUser);
             if (data.Any())
             {
                 return ResultModel<List<GoodslocationViewModel>>.Success(data);
             }
             else
             {
                 return ResultModel<List<GoodslocationViewModel>>.Success(new List<GoodslocationViewModel>());
             }
         }
 
         /// <summary>
         /// get a record by id
         /// </summary>
         /// <returns>args</returns>
         [HttpGet]
         public async Task<ResultModel<GoodslocationViewModel>> GetAsync(int id)
         {
             var data = await _goodslocationService.GetAsync(id);
             if (data!=null)
             {
                 return ResultModel<GoodslocationViewModel>.Success(data);
             }
             else
             {
                 return ResultModel<GoodslocationViewModel>.Error(_stringLocalizer["not_exists_entity"]);
             }
         }
         /// <summary>
         /// add a new record
         /// </summary>
         /// <param name="viewModel">args</param>
         /// <returns></returns>
         [HttpPost]
         public async Task<ResultModel<int>> AddAsync(GoodslocationViewModel viewModel)
         {
             var (id, msg) = await _goodslocationService.AddAsync(viewModel,CurrentUser);
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
         public async Task<ResultModel<bool>> UpdateAsync(GoodslocationViewModel viewModel)
         {
             var (flag, msg) = await _goodslocationService.UpdateAsync(viewModel, CurrentUser);
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
             var (flag, msg) = await _goodslocationService.DeleteAsync(id);
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
 
