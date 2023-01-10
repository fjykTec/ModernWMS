using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using ModernWMS.Core.Controller;
using ModernWMS.Core.JWT;
using ModernWMS.Core.Models;
using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.WMS.IServices;

namespace ModernWMS.WMS.Controllers
{
    /// <summary>
    /// customer controller
    /// </summary>
    [Route("customer")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Base")]
    public class CustomerController : BaseController
    {
        #region Args

        /// <summary>
        /// customer Service
        /// </summary>
        private readonly ICustomerService _customerService;
        /// <summary>
        /// Localizer Service
        /// </summary>
        private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;

        #endregion

        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="customerService">customer Service</param>
        /// <param name="stringLocalizer">Localizer</param>
        public CustomerController(
            ICustomerService customerService
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            )
        {
            this._customerService = customerService;
            this._stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Api
        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <returns></returns>
        [HttpPost("list")]
        public async Task<ResultModel<PageData<CustomerViewModel>>> PageAsync(PageSearch pageSearch)
        {
            var (data, totals) = await _customerService.PageAsync(pageSearch, CurrentUser);
             
            return ResultModel<PageData<CustomerViewModel>>.Success(new PageData<CustomerViewModel>
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
        public async Task<ResultModel<List<CustomerViewModel>>> GetAllAsync()
        {
            var data = await _customerService.GetAllAsync(CurrentUser);
            if (data.Any())
            {
                return ResultModel<List<CustomerViewModel>>.Success(data);
            }
            else
            {
                return ResultModel<List<CustomerViewModel>>.Success(new List<CustomerViewModel>());
            }
        }

        /// <summary>
        /// get a record by id
        /// </summary>
        /// <returns>args</returns>
        [HttpGet]
        public async Task<ResultModel<CustomerViewModel>> GetAsync(int id)
        {
            var data = await _customerService.GetAsync(id);
            if (data != null && data.id > 0)
            {
                return ResultModel<CustomerViewModel>.Success(data);
            }
            else
            {
                return ResultModel<CustomerViewModel>.Error(_stringLocalizer["not_exists_entity"]);
            }
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ResultModel<int>> AddAsync(CustomerViewModel viewModel)
        {
            var (id, msg) = await _customerService.AddAsync(viewModel, CurrentUser);
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
        public async Task<ResultModel<bool>> UpdateAsync(CustomerViewModel viewModel)
        {
            var (flag, msg) = await _customerService.UpdateAsync(viewModel);
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
            var (flag, msg) = await _customerService.DeleteAsync(id);
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

        #region Import
        /// <summary>
        /// import customers by excel
        /// </summary>
        /// <param name="input">excel data</param>
        /// <returns></returns>
        [HttpPost("excel")]
        public async Task<ResultModel<List<CustomerImportViewModel>>> ExcelAsync(List<CustomerImportViewModel> input)
        {
            var (flag, errorData) = await _customerService.ExcelAsync(input, CurrentUser);
            if (flag)
            {
                return ResultModel<List<CustomerImportViewModel>>.Success(errorData);
            }
            else
            {
                return ResultModel<List<CustomerImportViewModel>>.Error("", 400, errorData);
            }
        }
        #endregion
    }
}
