/*
 * date：2022-12-20
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
    /// rolemenu controller
    /// </summary>
    [Route("rolemenu")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Base")]
    public class RolemenuController : BaseController
    {
        #region Args

        /// <summary>
        /// rolemenu Service
        /// </summary>
        private readonly IRolemenuService _rolemenuService;

        /// <summary>
        /// Localizer Service
        /// </summary>
        private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="rolemenuService">rolemenu Service</param>
        /// <param name="stringLocalizer">Localizer</param>
        public RolemenuController(
            IRolemenuService rolemenuService
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            )
        {
            this._rolemenuService = rolemenuService;
            this._stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Api
        /// <summary>
        /// Get menu's authority by user role id
        /// </summary>
        /// <param name="userrole_id">user role id</param>
        /// <returns></returns>
        [HttpGet("authority")]
        public async  Task<ResultModel<List<MenuViewModel>>> GetMenusByRoleId(int userrole_id)
        {
            var data = await _rolemenuService.GetMenusByRoleId(userrole_id);
            if (data.Any())
            {
                return ResultModel<List<MenuViewModel>>.Success(data);
            }
            else
            {
                return ResultModel<List<MenuViewModel>>.Success(new List<MenuViewModel>());
            }
        }
        /// <summary>
        /// get all records
        /// </summary>
        /// <returns>args</returns>
        [HttpGet("all")]
        public async Task<ResultModel<List<RolemenuListViewModel>>> GetAllAsync()
        {
            var data = await _rolemenuService.GetAllAsync(CurrentUser);
            if (data.Any())
            {
                return ResultModel<List<RolemenuListViewModel>>.Success(data);
            }
            else
            {
                return ResultModel<List<RolemenuListViewModel>>.Success(new List<RolemenuListViewModel>());
            }
        }
        /// <summary>
        /// Get all menus
        /// </summary>
        /// <returns></returns>
        [HttpGet("menus")]
        public async Task<ResultModel<List<MenuViewModel>>> GetAllMenusAsync()
        {
            var data = await _rolemenuService.GetAllMenusAsync(CurrentUser);
            if (data.Any())
            {
                return ResultModel<List<MenuViewModel>>.Success(data);
            }
            else
            {
                return ResultModel<List<MenuViewModel>>.Success(new List<MenuViewModel>());
            }
        }
        /// <summary>
        /// get a record by id
        /// </summary>
        /// <returns>args</returns>
        [HttpGet]
        public async Task<ResultModel<RolemenuBothViewModel>> GetAsync(int userrole_id)
        {
            var data = await _rolemenuService.GetAsync(userrole_id);
            if (data != null && data.userrole_id > 0)
            {
                return ResultModel<RolemenuBothViewModel>.Success(data);
            }
            else
            {
                return ResultModel<RolemenuBothViewModel>.Error(_stringLocalizer["not_exists_entity"]);
            }
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<int>> AddAsync(RolemenuBothViewModel viewModel)
        {
            var (id, msg) = await _rolemenuService.AddAsync(viewModel, CurrentUser);
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
        public async Task<ResultModel<bool>> UpdateAsync(RolemenuBothViewModel viewModel)
        {
            var (flag, msg) = await _rolemenuService.UpdateAsync(viewModel, CurrentUser);
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
        /// <param name="userrole_id">userrole id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<ResultModel<string>> DeleteAsync(int userrole_id)
        {
            var (flag, msg) = await _rolemenuService.DeleteAsync(userrole_id);
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
 
