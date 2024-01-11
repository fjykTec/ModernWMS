/*
 * date：2023-09-11
 * developer：NoNo
 */

using ModernWMS.Core.JWT;
using ModernWMS.Core.Models;
using ModernWMS.Core.Services;
using ModernWMS.WMS.Entities.Models;
using ModernWMS.WMS.Entities.ViewModels;

namespace ModernWMS.WMS.IServices
{
    /// <summary>
    /// Interface of User_defined_print_solutionService
    /// </summary>
    public interface IPrintSolutionService : IBaseService<PrintSolutionEntity>
    {
        #region Api

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        Task<(List<PrintSolutionViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser);

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        Task<List<PrintSolutionViewModel>> GetAllAsync(CurrentUser currentUser);

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns></returns>
        Task<PrintSolutionViewModel> GetAsync(int id);

        /// <summary>
        /// get a record by path
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        Task<List<PrintSolutionViewModel>> GetByPathAsync(PrintSolutionGetByPathInputViewModel input, CurrentUser currentUser);

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <returns></returns>
        Task<(int id, string msg)> AddAsync(PrintSolutionViewModel viewModel, CurrentUser currentUser);

        /// <summary>
        /// update a record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> UpdateAsync(PrintSolutionViewModel viewModel);

        /// <summary>
        /// delete a record
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> DeleteAsync(int id);

        #endregion Api
    }
}