/*
 * date：2022-12-26
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
    /// stockadjust controller
    /// </summary>
     [Route("stockadjust")]
     [ApiController]
     [ApiExplorerSettings(GroupName = "WMS")]
     public class StockadjustController : BaseController
     {
         #region Args
 
         /// <summary>
         /// stockadjust Service
         /// </summary>
         private readonly IStockadjustService _stockadjustService;
 
         /// <summary>
         /// Localizer Service
         /// </summary>
         private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
         #endregion
 
         #region constructor
         /// <summary>
         /// constructor
         /// </summary>
         /// <param name="stockadjustService">stockadjust Service</param>
        /// <param name="stringLocalizer">Localizer</param>
         public StockadjustController(
             IStockadjustService stockadjustService
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
         {
             this._stockadjustService = stockadjustService;
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
         public async Task<ResultModel<PageData<StockadjustViewModel>>> PageAsync(PageSearch pageSearch)
         {
             var (data, totals) = await _stockadjustService.PageAsync(pageSearch, CurrentUser);
              
             return ResultModel<PageData<StockadjustViewModel>>.Success(new PageData<StockadjustViewModel>
             {
                 Rows = data,
                 Totals = totals
             });
         }
 
        #endregion

    }
 }
 
