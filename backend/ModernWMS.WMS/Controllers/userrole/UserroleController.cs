/*
 * date：2022-12-20
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
     /// userrole controller
     /// </summary>
     [Route("userrole")]
     [ApiController]
     [ApiExplorerSettings(GroupName = "Base")]
     public class UserroleController : BaseController
     {
         #region Args
 
         /// <summary>
         /// userrole Service
         /// </summary>
         private readonly IUserroleService _userroleService;
 
         /// <summary>
         /// Localizer Service
         /// </summary>
         private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
         #endregion
 
         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         /// <param name="userroleService">userrole Service</param>
        /// <param name="stringLocalizer">Localizer</param>
         public UserroleController(
             IUserroleService userroleService
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
         {
             this._userroleService = userroleService;
            this._stringLocalizer= stringLocalizer;
         }
        #endregion

        #region Api
        /// <summary>
        /// save all records
        /// </summary>
        /// <param name="viewModels">viewmodel</param>
        /// <returns></returns>
        [HttpPost("save")]
        public async Task<ResultModel<string>> SaveAllAsync(List<UserroleViewModel> viewModels)
        {
            var (flag,msg) = await _userroleService.BulkSaveAsync(viewModels, CurrentUser);
            if(flag)
            {
                return ResultModel<string>.Success(msg);
            }
            return ResultModel<string>.Error(msg);
        }

         /// <summary>
         /// get all records
         /// </summary>
         /// <returns>args</returns>
        [HttpGet("all")]
         public async Task<ResultModel<List<UserroleViewModel>>> GetAllAsync()
         {
             var data = await _userroleService.GetAllAsync(CurrentUser);
             if (data.Any())
             {
                 return ResultModel<List<UserroleViewModel>>.Success(data);
             }
             else
             {
                 return ResultModel<List<UserroleViewModel>>.Success(new List<UserroleViewModel>());
             }
         }
 
         /// <summary>
         /// get a record by id
         /// </summary>
         /// <returns>args</returns>
         [HttpGet]
         public async Task<ResultModel<UserroleViewModel>> GetAsync(int id)
         {
             var data = await _userroleService.GetAsync(id);
             if (data!=null)
             {
                 return ResultModel<UserroleViewModel>.Success(data);
             }
             else
             {
                 return ResultModel<UserroleViewModel>.Error(_stringLocalizer["exists_entity"]);
             }
         }
         /// <summary>
         /// add a new record
         /// </summary>
         /// <param name="viewModel">args</param>
         /// <returns></returns>
         [HttpPost]
         public async Task<ResultModel<int>> AddAsync(UserroleViewModel viewModel)
         {
             var (id, msg) = await _userroleService.AddAsync(viewModel,CurrentUser);
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
         public async Task<ResultModel<bool>> UpdateAsync(UserroleViewModel viewModel)
         {
             var (flag, msg) = await _userroleService.UpdateAsync(viewModel, CurrentUser);
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
             var (flag, msg) = await _userroleService.DeleteAsync(id);
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
 
