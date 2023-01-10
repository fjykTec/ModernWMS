/*
 * date：2022-12-20
 * developer：AMo
 */
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ModernWMS.Core.Controller;
using ModernWMS.Core.Models;
using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.WMS.IServices;

namespace ModernWMS.WMS.Controllers
{
    /// <summary>
    /// category controller
    /// </summary>
    [Route("category")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Base")]
    public class CategoryController : BaseController
    {
        #region Args

        /// <summary>
        /// category Service
        /// </summary>
        private readonly ICategoryService _categoryService;

        /// <summary>
        /// Localizer Service
        /// </summary>
        private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="categoryService">category Service</param>
        /// <param name="stringLocalizer">Localizer</param>
        public CategoryController(
            ICategoryService categoryService
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            )
        {
            this._categoryService = categoryService;
            this._stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Api
        /// <summary>
        /// get all records
        /// </summary>
        /// <returns>args</returns>
        [HttpGet("all")]
        public async Task<ResultModel<List<CategoryViewModel>>> GetAllAsync()
        {
            var data = await _categoryService.GetAllAsync(CurrentUser);
            if (data.Any())
            {
                return ResultModel<List<CategoryViewModel>>.Success(data);
            }
            else
            {
                return ResultModel<List<CategoryViewModel>>.Success(new List<CategoryViewModel>());
            }
        }

        /// <summary>
        /// get a record by id
        /// </summary>
        /// <returns>args</returns>
        [HttpGet]
        public async Task<ResultModel<CategoryViewModel>> GetAsync(int id)
        {
            var data = await _categoryService.GetAsync(id);
            if (data != null)
            {
                return ResultModel<CategoryViewModel>.Success(data);
            }
            else
            {
                return ResultModel<CategoryViewModel>.Error(_stringLocalizer["not_exists_entity"]);
            }
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<int>> AddAsync(CategoryViewModel viewModel)
        {
            var (id, msg) = await _categoryService.AddAsync(viewModel, CurrentUser);
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
        public async Task<ResultModel<bool>> UpdateAsync(CategoryViewModel viewModel)
        {
            var (flag, msg) = await _categoryService.UpdateAsync(viewModel);
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
            var (flag, msg) = await _categoryService.DeleteAsync(id);
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
 
