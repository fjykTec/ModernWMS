/*
 * date：2022-12-20
 * developer：NoNo
 */
using Mapster;
using Microsoft.EntityFrameworkCore;
using ModernWMS.Core.DBContext;
using ModernWMS.Core.Services;
using ModernWMS.WMS.Entities.Models;
using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.WMS.IServices;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using ModernWMS.Core.JWT;
using System.Text;
using System.Collections.Immutable;
using ModernWMS.Core.Models;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  Userrole Service
    /// </summary>
    public class UserroleService : BaseService<UserroleEntity>, IUserroleService
    {
        #region Args
        /// <summary>
        /// The DBContext
        /// </summary>
        private readonly SqlDBContext _dBContext;

        /// <summary>
        /// Localizer Service
        /// </summary>
        private readonly IStringLocalizer<ModernWMS.Core.MultiLanguage> _stringLocalizer;
        #endregion

        #region constructor
        /// <summary>
        ///Userrole  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public UserroleService(
            SqlDBContext dBContext
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            )
        {
            this._dBContext = dBContext;
            this._stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Api
        /// <summary>
        /// bulk save records
        /// </summary>
        /// <param name="viewModels">viewmodel</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> BulkSaveAsync(List<UserroleViewModel> viewModels, CurrentUser currentUser)
        {
            StringBuilder sb = new StringBuilder();
            var DBSet = _dBContext.GetDbSet<UserroleEntity>();
            var repaet_role_name_viewmodel = viewModels.Where(t => t.id >= 0).GroupBy(t => t.role_name).Select(t => new { role_name = t.Key, cnt = t.Count() }).Where(t => t.cnt > 1).ToList();
            foreach (var repeat in repaet_role_name_viewmodel)
            {
                sb.AppendLine(string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["role_name"], repeat));
            }
            if (repaet_role_name_viewmodel.Count() > 0)
            {
                return (false, sb.ToString());
            }
            var delete_viewmodels = viewModels.Where(t => t.id < 0).ToList();
            var enties_check = await DBSet.AsNoTracking().Where(t => t.tenant_id == currentUser.tenant_id && !delete_viewmodels.Select(e => -e.id).ToList().Contains(t.id)).ToListAsync();
            var role_name_viewmodels = viewModels.Where(t => t.id >= 0).Select(t => t.role_name).ToList();
            var repaet_role_name_exists = enties_check.Where(t => role_name_viewmodels.Contains(t.role_name)).ToList();
            foreach (var repeat in repaet_role_name_exists)
            {
                sb.AppendLine(string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["role_name"], repeat.role_name));
            }
            if (repaet_role_name_exists.Count() > 0)
            {
                return (false, sb.ToString());
            }
            var add_entities = (from vm in viewModels
                                where vm.id == 0
                                select new UserroleEntity
                                {
                                    id = 0,
                                    create_time = DateTime.Now,
                                    last_update_time = DateTime.Now,
                                    is_valid = true,
                                    role_name = vm.role_name,
                                    tenant_id = currentUser.tenant_id,
                                }).ToList();
            await DBSet.AddRangeAsync(add_entities);
            var update_viewmodel_id = viewModels.Where(t => t.id > 0).Select(t => t.id).ToList();
            var update_entities = await DBSet.Where(t => update_viewmodel_id.Contains(t.id)).ToListAsync();
            update_entities.ForEach(t =>
            {
                var update_vm = viewModels.FirstOrDefault(e => e.id == t.id);
                if (update_vm != null)
                {
                    t.last_update_time = DateTime.Now;
                    t.is_valid = update_vm.is_valid;
                    t.role_name = update_vm.role_name;
                }
            });

            var delete_entites = await DBSet.Where(t => delete_viewmodels.Select(e => -e.id).ToList().Contains(t.id)).ToListAsync();
            DBSet.RemoveRange(delete_entites);
            await _dBContext.SaveChangesAsync();
            return (true, _stringLocalizer["save_success"]);
        }
        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserroleViewModel>> GetAllAsync(CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<UserroleEntity>();
            var data = await DbSet.AsNoTracking().Where(t => t.tenant_id == currentUser.tenant_id).ToListAsync();
            return data.Adapt<List<UserroleViewModel>>();
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <returns></returns>
        public async Task<UserroleViewModel> GetAsync(int id)
        {
            var DbSet = _dBContext.GetDbSet<UserroleEntity>();
            var entity = await DbSet.AsNoTracking().FirstOrDefaultAsync(t => t.id.Equals(id));
            if (entity == null)
            {
                return null;
            }
            return entity.Adapt<UserroleViewModel>();
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(UserroleViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<UserroleEntity>();
            if (await DbSet.AnyAsync(t => t.role_name == viewModel.role_name && t.tenant_id ==  currentUser.tenant_id))
            {
                return (0, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["role_name"], viewModel.role_name));
            }
            var entity = viewModel.Adapt<UserroleEntity>();
            entity.id = 0;
            entity.create_time = DateTime.Now;
            entity.last_update_time = DateTime.Now;
            entity.tenant_id = currentUser.tenant_id;
            await DbSet.AddAsync(entity);
            await _dBContext.SaveChangesAsync();
            if (entity.id > 0)
            {
                return (entity.id, _stringLocalizer["save_success"]);
            }
            else
            {
                return (0, _stringLocalizer["save_failed"]);
            }
        }
        /// <summary>
        /// update a record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UpdateAsync(UserroleViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<UserroleEntity>();
            if (await DbSet.AnyAsync(t => t.id != viewModel.id && t.role_name == viewModel.role_name && t.tenant_id == currentUser.tenant_id))
            {
                return (false, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["role_name"], viewModel.role_name));
            }
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not exists entity"]);
            }
            var user_DBSet = _dBContext.GetDbSet<userEntity>();
            var this_role_users = await user_DBSet.Where(t=>t.user_role == entity.role_name).ToListAsync();
            this_role_users.ForEach(t=>t.user_role=viewModel.role_name);
            entity.id = viewModel.id;
            entity.role_name = viewModel.role_name;
            entity.is_valid = viewModel.is_valid;
            entity.last_update_time = DateTime.Now;
            var qty = await _dBContext.SaveChangesAsync();
            if (qty > 0)
            {
                return (true, _stringLocalizer["save_success"]);
            }
            else
            {
                return (false, _stringLocalizer["save_failed"]);
            }
        }
        /// <summary>
        /// delete a record
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> DeleteAsync(int id)
        {
            var qty = await _dBContext.GetDbSet<UserroleEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
            if (qty > 0)
            {
                return (true, _stringLocalizer["delete_success"]);
            }
            else
            {
                return (false, _stringLocalizer["delete_failed"]);
            }
        }
        #endregion
    }
}

