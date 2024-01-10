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
using Microsoft.AspNetCore.Authorization;

namespace ModernWMS.WMS.Controllers
{
    /// <summary>
    /// user controller
    /// </summary>
    [Route("user")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Base")]
    public class UserController : BaseController
    {
        #region Args

        /// <summary>
        /// user Service
        /// </summary>
        private readonly IUserService _userService;

        /// <summary>
        /// Localizer Service
        /// </summary>
        private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;

        #endregion Args

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="userService">user Service</param>
        /// <param name="stringLocalizer">Localizer</param>
        public UserController(
             IUserService userService
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
        {
            this._userService = userService;
            this._stringLocalizer = stringLocalizer;
        }

        #endregion constructor

        #region Api

        /// <summary>
        /// get select items
        /// </summary>
        /// <returns></returns>
        [HttpGet("select-item")]
        public async Task<ResultModel<List<FormSelectItem>>> GetSelectItemsAsnyc()
        {
            var datas = await _userService.GetSelectItemsAsnyc(CurrentUser);
            return ResultModel<List<FormSelectItem>>.Success(datas);
        }

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<ResultModel<PageData<UserViewModel>>> PageAsync(PageSearch pageSearch)
        {
            var (data, totals) = await _userService.PageAsync(pageSearch, CurrentUser);

            return ResultModel<PageData<UserViewModel>>.Success(new PageData<UserViewModel>
            {
                Rows = data,
                Totals = totals
            });
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns>args</returns>
        [HttpGet("all")]
        public async Task<ResultModel<List<UserViewModel>>> GetAllAsync()
        {
            var data = await _userService.GetAllAsync(CurrentUser);
            if (data.Any())
            {
                return ResultModel<List<UserViewModel>>.Success(data);
            }
            else
            {
                return ResultModel<List<UserViewModel>>.Success(new List<UserViewModel>());
            }
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <returns>args</returns>
        [HttpGet]
        public async Task<ResultModel<UserViewModel>> GetAsync(int id)
        {
            var data = await _userService.GetAsync(id);
            if (data != null)
            {
                return ResultModel<UserViewModel>.Success(data);
            }
            else
            {
                return ResultModel<UserViewModel>.Error(_stringLocalizer["not_exists_entity"]);
            }
        }

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<int>> AddAsync(UserViewModel viewModel)
        {
            viewModel.creator = CurrentUser.user_name;
            var (id, msg) = await _userService.AddAsync(viewModel, CurrentUser);
            if (id > 0)
            {
                return ResultModel<int>.Success(id, msg);
            }
            else
            {
                return ResultModel<int>.Error(msg);
            }
        }

        /// <summary>
        /// register a new tenant
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ResultModel<string>> Register(RegisterViewModel viewModel)
        {
            var (flag, msg) = await _userService.Register(viewModel);
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
        /// import users by excel
        /// </summary>
        /// <param name="excel_datas">excel datas</param>
        /// <returns></returns>
        [HttpPost("excel")]
        public async Task<ResultModel<string>> ExcelAsync(List<UserExcelImportViewModel> excel_datas)
        {
            var (flag, msg) = await _userService.ExcelAsync(excel_datas, CurrentUser);
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
        /// update a record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ResultModel<bool>> UpdateAsync(UserViewModel viewModel)
        {
            var (flag, msg) = await _userService.UpdateAsync(viewModel, CurrentUser);
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
            var (flag, msg) = await _userService.DeleteAsync(id);
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
        /// reset password
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <returns></returns>
        [HttpPost("reset-pwd")]
        public async Task<ResultModel<string>> ResetPwd(BatchOperationViewModel viewModel)
        {
            var (flag, msg) = await _userService.ResetPwd(viewModel);
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
        /// change password
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <returns></returns>
        [HttpPost("change-pwd")]
        public async Task<ResultModel<string>> ChangePwd(UserChangePwdViewModel viewModel)
        {
            var (flag, msg) = await _userService.ChangePwd(viewModel);
            if (flag)
            {
                return ResultModel<string>.Success(msg);
            }
            else
            {
                return ResultModel<string>.Error(msg);
            }
        }

        #endregion Api
    }
}