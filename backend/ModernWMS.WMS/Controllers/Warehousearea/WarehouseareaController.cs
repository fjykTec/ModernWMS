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
    /// warehousearea controller
    /// </summary>
     [Route("warehousearea")]
     [ApiController]
     [ApiExplorerSettings(GroupName = "Base")]
     public class WarehouseareaController : BaseController
     {
         #region Args
 
         /// <summary>
         /// warehousearea Service
         /// </summary>
         private readonly IWarehouseareaService _warehouseareaService;
 
         /// <summary>
         /// Localizer Service
         /// </summary>
         private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
         #endregion
 
         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         /// <param name="warehouseareaService">warehousearea Service</param>
        /// <param name="stringLocalizer">Localizer</param>
         public WarehouseareaController(
             IWarehouseareaService warehouseareaService
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
         {
             this._warehouseareaService = warehouseareaService;
            this._stringLocalizer= stringLocalizer;
         }
        #endregion

        #region Api
        /// <summary>
        /// get warehousearea  select items by warehouse_id
        /// </summary>
        /// <returns></returns>
        [HttpGet("areas-by-warehouse_id")]
        public async Task<ResultModel<List<FormSelectItem>>> GetSelectItemsAsnyc(int warehouse_id)
        {
            var datas = await _warehouseareaService.GetWarehouseareaByWarehouse_id(warehouse_id,CurrentUser);
            return ResultModel<List<FormSelectItem>>.Success(datas);
        }

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("list")]
         public async Task<ResultModel<PageData<WarehouseareaViewModel>>> PageAsync(PageSearch pageSearch)
         {
             var (data, totals) = await _warehouseareaService.PageAsync(pageSearch, CurrentUser);
              
             return ResultModel<PageData<WarehouseareaViewModel>>.Success(new PageData<WarehouseareaViewModel>
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
         public async Task<ResultModel<List<WarehouseareaViewModel>>> GetAllAsync(int warehouse_id)
         {
             var data = await _warehouseareaService.GetAllAsync( warehouse_id, CurrentUser);
             if (data.Any())
             {
                 return ResultModel<List<WarehouseareaViewModel>>.Success(data);
             }
             else
             {
                 return ResultModel<List<WarehouseareaViewModel>>.Success(new List<WarehouseareaViewModel>());
             }
         }
 
         /// <summary>
         /// get a record by id
         /// </summary>
         /// <returns>args</returns>
         [HttpGet]
         public async Task<ResultModel<WarehouseareaViewModel>> GetAsync(int id)
         {
             var data = await _warehouseareaService.GetAsync(id);
             if (data!=null)
             {
                 return ResultModel<WarehouseareaViewModel>.Success(data);
             }
             else
             {
                 return ResultModel<WarehouseareaViewModel>.Error(_stringLocalizer["not_exists_entity"]);
             }
         }
         /// <summary>
         /// add a new record
         /// </summary>
         /// <param name="viewModel">args</param>
         /// <returns></returns>
         [HttpPost]
         public async Task<ResultModel<int>> AddAsync(WarehouseareaViewModel viewModel)
         {
             var (id, msg) = await _warehouseareaService.AddAsync(viewModel,CurrentUser);
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
         public async Task<ResultModel<bool>> UpdateAsync(WarehouseareaViewModel viewModel)
         {
             var (flag, msg) = await _warehouseareaService.UpdateAsync(viewModel,CurrentUser);
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
             var (flag, msg) = await _warehouseareaService.DeleteAsync(id);
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
 
