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
    /// supplier controller
    /// </summary>
     [Route("supplier")]
     [ApiController]
     [ApiExplorerSettings(GroupName = "Base")]
     public class SupplierController : BaseController
     {
         #region Args
 
         /// <summary>
         /// supplier Service
         /// </summary>
         private readonly ISupplierService _supplierService;
 
         /// <summary>
         /// Localizer Service
         /// </summary>
         private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
         #endregion
 
         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         /// <param name="supplierService">supplier Service</param>
        /// <param name="stringLocalizer">Localizer</param>
         public SupplierController(
             ISupplierService supplierService
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
         {
             this._supplierService = supplierService;
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
         public async Task<ResultModel<PageData<SupplierViewModel>>> PageAsync(PageSearch pageSearch)
         {
             var (data, totals) = await _supplierService.PageAsync(pageSearch, CurrentUser);
              
             return ResultModel<PageData<SupplierViewModel>>.Success(new PageData<SupplierViewModel>
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
         public async Task<ResultModel<List<SupplierViewModel>>> GetAllAsync()
         {
             var data = await _supplierService.GetAllAsync(CurrentUser);
             if (data.Any())
             {
                 return ResultModel<List<SupplierViewModel>>.Success(data);
             }
             else
             {
                 return ResultModel<List<SupplierViewModel>>.Success(new List<SupplierViewModel>());
             }
         }
 
         /// <summary>
         /// get a record by id
         /// </summary>
         /// <returns>args</returns>
         [HttpGet]
         public async Task<ResultModel<SupplierViewModel>> GetAsync(int id)
         {
             var data = await _supplierService.GetAsync(id);
             if (data!=null)
             {
                 return ResultModel<SupplierViewModel>.Success(data);
             }
             else
             {
                 return ResultModel<SupplierViewModel>.Error(_stringLocalizer["not_exists_entity"]);
             }
         }
         /// <summary>
         /// add a new record
         /// </summary>
         /// <param name="viewModel">args</param>
         /// <returns></returns>
         [HttpPost]
         public async Task<ResultModel<int>> AddAsync(SupplierViewModel viewModel)
         {
             var (id, msg) = await _supplierService.AddAsync(viewModel,CurrentUser);
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
         public async Task<ResultModel<bool>> UpdateAsync(SupplierViewModel viewModel)
         {
             var (flag, msg) = await _supplierService.UpdateAsync(viewModel, CurrentUser);
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
             var (flag, msg) = await _supplierService.DeleteAsync(id);
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
        /// import suppliers by excel
        /// </summary>
        /// <param name="excel_datas">excel datas</param>
        /// <returns></returns>
        [HttpPost("excel")]
        public async Task<ResultModel<string>> ExcelAsync(List<SupplierExcelImportViewModel> excel_datas)
        {
            var (flag, msg) = await _supplierService.ExcelAsync(excel_datas, CurrentUser);
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
 
