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
using ModernWMS.WMS.Services;

namespace ModernWMS.WMS.Controllers
{
    /// <summary>
    /// warehouse controller
    /// </summary>
     [Route("warehouse")]
     [ApiController]
     [ApiExplorerSettings(GroupName = "Base")]
     public class WarehouseController : BaseController
     {
         #region Args
 
         /// <summary>
         /// warehouse Service
         /// </summary>
         private readonly IWarehouseService _warehouseService;
 
         /// <summary>
         /// Localizer Service
         /// </summary>
         private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
         #endregion
 
         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         /// <param name="warehouseService">warehouse Service</param>
        /// <param name="stringLocalizer">Localizer</param>
         public WarehouseController(
             IWarehouseService warehouseService
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
         {
             this._warehouseService = warehouseService;
            this._stringLocalizer= stringLocalizer;
         }
        #endregion

        #region Api
        /// <summary>
        /// get select items
        /// </summary>
        /// <returns></returns>
        [HttpGet("select-item")]
        public async Task<ResultModel<List<FormSelectItem>>> GetSelectItemsAsnyc()
        {
            var datas = await _warehouseService.GetSelectItemsAsnyc(CurrentUser);
            return ResultModel<List<FormSelectItem>>.Success(datas);
        }

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("list")]
         public async Task<ResultModel<PageData<WarehouseViewModel>>> PageAsync(PageSearch pageSearch)
         {
             var (data, totals) = await _warehouseService.PageAsync(pageSearch, CurrentUser);
              
             return ResultModel<PageData<WarehouseViewModel>>.Success(new PageData<WarehouseViewModel>
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
         public async Task<ResultModel<List<WarehouseViewModel>>> GetAllAsync()
         {
             var data = await _warehouseService.GetAllAsync(CurrentUser);
             if (data.Any())
             {
                 return ResultModel<List<WarehouseViewModel>>.Success(data);
             }
             else
             {
                 return ResultModel<List<WarehouseViewModel>>.Success(new List<WarehouseViewModel>());
             }
         }
 
         /// <summary>
         /// get a record by id
         /// </summary>
         /// <returns>args</returns>
         [HttpGet]
         public async Task<ResultModel<WarehouseViewModel>> GetAsync(int id)
         {
             var data = await _warehouseService.GetAsync(id);
             if (data!=null)
             {
                 return ResultModel<WarehouseViewModel>.Success(data);
             }
             else
             {
                 return ResultModel<WarehouseViewModel>.Error(_stringLocalizer["not_exists_entity"]);
             }
         }
         /// <summary>
         /// add a new record
         /// </summary>
         /// <param name="viewModel">args</param>
         /// <returns></returns>
         [HttpPost]
         public async Task<ResultModel<int>> AddAsync(WarehouseViewModel viewModel)
         {
             var (id, msg) = await _warehouseService.AddAsync(viewModel,CurrentUser);
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
         public async Task<ResultModel<bool>> UpdateAsync(WarehouseViewModel viewModel)
         {
             var (flag, msg) = await _warehouseService.UpdateAsync(viewModel,CurrentUser);
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
             var (flag, msg) = await _warehouseService.DeleteAsync(id);
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
        /// import warehouses by excel
        /// </summary>
        /// <param name="excel_datas">excel datas</param>
        /// <returns></returns>
        [HttpPost("excel")]
        public async Task<ResultModel<string>> ExcelAsync(List<WarehouseExcelImportViewModel> excel_datas)
        {
            var (flag, msg) = await _warehouseService.ExcelAsync(excel_datas, CurrentUser);
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
 
