using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModernWMS.Core.DBContext;
using ModernWMS.Core.JWT;
using ModernWMS.Core.Services;
using ModernWMS.WMS.Entities.Models;
using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.WMS.IServices;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    /// CompanyService
    /// </summary>
    public class CompanyService : BaseService<CompanyEntity>, ICompanyService
    {
        #region Args
        /// <summary>
        /// The DBContext
        /// </summary>
        private readonly SqlDBContext _dBContext;

        /// <summary>
        /// Localization
        /// </summary>
        private readonly IStringLocalizer<Core.MultiLanguage> _stringLocalizer;
        #endregion

        #region constructor
        /// <summary>
        /// company service constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localization</param>
        public CompanyService(
            SqlDBContext dBContext
          , IStringLocalizer<Core.MultiLanguage> stringLocalizer
            )
        {
            this._dBContext = dBContext;
            this._stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Api
        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<List<CompanyViewModel>> GetAllAsync(CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<CompanyEntity>();
            var data = await DbSet.AsNoTracking().Where(t => t.tenant_id == currentUser.tenant_id)
                .OrderByDescending(t => t.create_time)
                .ToListAsync();
            return data.Adapt<List<CompanyViewModel>>();
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<CompanyViewModel> GetAsync(int id)
        {
            var entity = await _dBContext.GetDbSet<CompanyEntity>().AsNoTracking().FirstOrDefaultAsync(t => t.id.Equals(id));
            if (entity != null)
            {
                return entity.Adapt<CompanyViewModel>();
            }
            else
            {
                return new CompanyViewModel();
            }
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(CompanyViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<CompanyEntity>();
            if (await DbSet.AnyAsync(t => t.tenant_id.Equals(currentUser.tenant_id) && t.company_name.Equals(viewModel.company_name)))
            {
                return (0, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["company_name"], viewModel.company_name));
            }
            var entity = viewModel.Adapt<CompanyEntity>();
            entity.id = 0;
            entity.create_time = DateTime.Now;
            entity.last_update_time = DateTime.Now;
            entity.tenant_id = currentUser.tenant_id;
            DbSet.Add(entity);
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
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UpdateAsync(CompanyViewModel viewModel)
        {
            var DbSet = _dBContext.GetDbSet<CompanyEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            if (await DbSet.AnyAsync(t => !t.id.Equals(viewModel.id) && t.tenant_id.Equals(entity.tenant_id) && t.company_name.Equals(viewModel.company_name)))
            {
                //国际化
                return (false, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["company_name"], viewModel.company_name));
            }
            entity.company_name = viewModel.company_name;
            entity.city = viewModel.city;
            entity.address = viewModel.address;
            entity.manager = viewModel.manager;
            entity.contact_tel = viewModel.contact_tel;
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
            var qty = await _dBContext.GetDbSet<CompanyEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
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
