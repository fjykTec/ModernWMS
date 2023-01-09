/*
 * date：2022-12-20
 * developer：NoNo
 */
using ModernWMS.Core.JWT;
using ModernWMS.Core.Services;
 using ModernWMS.WMS.Entities.Models;
 using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.Core.Models;
 namespace ModernWMS.WMS.IServices
 {
     /// <summary>
     /// Interface of UserroleService
     /// </summary>
     public interface IUserroleService : IBaseService<UserroleEntity>
     {
        #region Api
        /// <summary>
        /// bulk save records
        /// </summary>
        /// <param name="viewModels">viewmodel</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> BulkSaveAsync(List<UserroleViewModel> viewModels, CurrentUser currentUser);
         /// <summary>
         /// Get all records
         /// </summary>
         /// <returns></returns>
         Task<List<UserroleViewModel>> GetAllAsync(CurrentUser currentUser);
         /// <summary>
         /// Get a record by id
         /// </summary>
         /// <param name="id">primary key</param>
         /// <returns></returns>
         Task<UserroleViewModel> GetAsync(int id);
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>>
        /// <param name="currentUser">current user</param>>
        /// <returns></returns>
        Task<(int id, string msg)> AddAsync(UserroleViewModel viewModel,CurrentUser currentUser);
        /// <summary>
        /// update a record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        Task<(bool flag, string msg)> UpdateAsync(UserroleViewModel viewModel, CurrentUser currentUser);
 
         /// <summary>
         /// delete a record
         /// </summary>
         /// <param name="id">id</param>
         /// <returns></returns>
         Task<(bool flag, string msg)> DeleteAsync(int id);
         #endregion
     }
 }
 
