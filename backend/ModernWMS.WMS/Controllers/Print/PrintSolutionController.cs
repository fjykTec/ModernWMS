/*
 * date：2023-09-11
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
    /// PrintSolution controller
    /// </summary>
    [Route("PrintSolution")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "WMS")]
    public class PrintSolutionController : BaseController
    {
        #region Args

        /// <summary>
        /// PrintSolution Service
        /// </summary>
        private readonly IPrintSolutionService _PrintSolutionService;

        /// <summary>
        /// Localizer Service
        /// </summary>
        private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;

        #endregion Args

        #region constructor

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="user_defined_print_solutionService">user_defined_print_solution Service</param>
        /// <param name="stringLocalizer">Localizer</param>
        public PrintSolutionController(
            IPrintSolutionService user_defined_print_solutionService
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            )
        {
            this._PrintSolutionService = user_defined_print_solutionService;
            this._stringLocalizer = stringLocalizer;
        }

        #endregion constructor

        #region Api

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<ResultModel<PageData<PrintSolutionViewModel>>> PageAsync(PageSearch pageSearch)
        {
            var (data, totals) = await _PrintSolutionService.PageAsync(pageSearch, CurrentUser);

            return ResultModel<PageData<PrintSolutionViewModel>>.Success(new PageData<PrintSolutionViewModel>
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
        public async Task<ResultModel<List<PrintSolutionViewModel>>> GetAllAsync()
        {
            var data = await _PrintSolutionService.GetAllAsync(CurrentUser);
            if (data.Any())
            {
                return ResultModel<List<PrintSolutionViewModel>>.Success(data);
            }
            else
            {
                return ResultModel<List<PrintSolutionViewModel>>.Success(new List<PrintSolutionViewModel>());
            }
        }

        /// <summary>
        /// get a record by id
        /// </summary>
        /// <returns>args</returns>
        [HttpGet]
        public async Task<ResultModel<PrintSolutionViewModel>> GetAsync(int id)
        {
            var data = await _PrintSolutionService.GetAsync(id);
            if (data != null)
            {
                return ResultModel<PrintSolutionViewModel>.Success(data);
            }
            else
            {
                return ResultModel<PrintSolutionViewModel>.Error(_stringLocalizer["not_exists_entity"]);
            }
        }

        /// <summary>
        /// get a record by path
        /// </summary>
        /// <returns>args</returns>
        [HttpPost("get-by-path")]
        public async Task<ResultModel<List<PrintSolutionViewModel>>> GetByPathAsync(PrintSolutionGetByPathInputViewModel input)
        {
            var data = await _PrintSolutionService.GetByPathAsync(input, CurrentUser);
            if (data != null)
            {
                return ResultModel<List<PrintSolutionViewModel>>.Success(data);
            }
            else
            {
                return ResultModel<List<PrintSolutionViewModel>>.Error(_stringLocalizer["not_exists_entity"]);
            }
        }

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<int>> AddAsync(PrintSolutionViewModel viewModel)
        {
            var (id, msg) = await _PrintSolutionService.AddAsync(viewModel, CurrentUser);
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
        public async Task<ResultModel<bool>> UpdateAsync(PrintSolutionViewModel viewModel)
        {
            var (flag, msg) = await _PrintSolutionService.UpdateAsync(viewModel);
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
            var (flag, msg) = await _PrintSolutionService.DeleteAsync(id);
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