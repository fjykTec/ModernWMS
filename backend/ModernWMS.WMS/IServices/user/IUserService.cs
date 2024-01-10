/*
 * date：2022-12-20
 * developer：NoNo
 */

using ModernWMS.Core.Services;
using ModernWMS.WMS.Entities.Models;
using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.Core.Models;
using ModernWMS.Core.JWT;

namespace ModernWMS.WMS.IServices
{
    /// <summary>
    /// Interface of UserService
    /// </summary>
    public interface IUserService : IBaseService<userEntity>
    {
        #region Api

        /// <summary>
        /// get select items
        /// </summary>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        Task<List<FormSelectItem>> GetSelectItemsAsnyc(CurrentUser currentUser);

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        Task<(List<UserViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser);

        /// <summary>
        /// Get all datas
        /// </summary>
        /// <returns></returns>
        Task<List<UserViewModel>> GetAllAsync(CurrentUser currentUser);

        /// <summary>
        /// Get a data by id
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns></returns>
        Task<UserViewModel> GetAsync(int id);

        /// <summary>
        /// add a new data
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <returns></returns>
        Task<(int id, string msg)> AddAsync(UserViewModel viewModel, CurrentUser currentUser);

        /// <summary>
        /// update a data
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> UpdateAsync(UserViewModel viewModel, CurrentUser currentUser);

        /// <summary>
        /// delete a data
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> DeleteAsync(int id);

        /// <summary>
        /// import users by excel
        /// </summary>
        /// <param name="datas">excel datas</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> ExcelAsync(List<UserExcelImportViewModel> datas, CurrentUser currentUser);

        /// <summary>
        /// reset password
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <returns></returns>
        Task<(bool, string)> ResetPwd(BatchOperationViewModel viewModel);

        /// <summary>
        /// change password
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> ChangePwd(UserChangePwdViewModel viewModel);

        /// <summary>
        /// register a new tenant
        /// </summary>
        /// <param name="viewModel">viewModel</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> Register(RegisterViewModel viewModel);

        #endregion Api
    }
}