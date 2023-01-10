using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModernWMS.Core.DBContext;
using ModernWMS.Core.DynamicSearch;
using ModernWMS.Core.JWT;
using ModernWMS.Core.Models;
using ModernWMS.Core.Services;
using ModernWMS.WMS.Entities.Models;
using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.WMS.IServices;
using System.Linq;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    /// customer Service
    /// </summary>
    public class CustomerService : BaseService<CustomerEntity>, ICustomerService
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
        public CustomerService(
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
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<CustomerViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var DbSet = _dBContext.GetDbSet<CustomerEntity>();
            var query = DbSet.AsNoTracking()
                .Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                .Where(queries.AsExpression<CustomerEntity>());
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.create_time)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list.Adapt<List<CustomerViewModel>>(), totals);
        }
        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<List<CustomerViewModel>> GetAllAsync(CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<CustomerEntity>();
            var data = await DbSet.AsNoTracking().Where(t => t.tenant_id == currentUser.tenant_id)
                .OrderByDescending(t => t.create_time).ToListAsync();
            return data.Adapt<List<CustomerViewModel>>();
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<CustomerViewModel> GetAsync(int id)
        {
            var entity = await _dBContext.GetDbSet<CustomerEntity>().AsNoTracking().FirstOrDefaultAsync(t => t.id.Equals(id));
            if (entity != null)
            {
                return entity.Adapt<CustomerViewModel>();
            }
            else
            {
                return new CustomerViewModel();
            }
        }

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(CustomerViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<CustomerEntity>();
            if (await DbSet.AnyAsync(t => t.tenant_id.Equals(currentUser.tenant_id) && t.customer_name.Equals(viewModel.customer_name)))
            {
                return (0, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["customer_name"], viewModel.customer_name));
            }
            var entity = viewModel.Adapt<CustomerEntity>();
            entity.id = 0;
            entity.creator = currentUser.user_name;
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
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UpdateAsync(CustomerViewModel viewModel)
        {
            var DbSet = _dBContext.GetDbSet<CustomerEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            if (await DbSet.AnyAsync(t => !t.id.Equals(viewModel.id) && t.tenant_id.Equals(entity.tenant_id) && t.customer_name.Equals(viewModel.customer_name)))
            {
                return (false, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["customer_name"], viewModel.customer_name));
            }
            entity.customer_name = viewModel.customer_name;
            entity.city = viewModel.city;
            entity.address = viewModel.address;
            entity.email = viewModel.email;
            entity.manager = viewModel.manager;
            entity.contact_tel = viewModel.contact_tel;
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
            var Dispatchlists = _dBContext.GetDbSet<DispatchlistEntity>();
            if(await Dispatchlists.AsNoTracking().AnyAsync(t => t.customer_id.Equals(id)))
            {
                return (false, _stringLocalizer["delete_referenced"]);
            }
            var qty = await _dBContext.GetDbSet<CustomerEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
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

        #region Import
        /// <summary>
        /// import customers by excel
        /// </summary>
        /// <param name="input">excel data</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, List<CustomerImportViewModel> errorData)> ExcelAsync(List<CustomerImportViewModel> input, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<CustomerEntity>();
            var existsDatas = await DbSet.AsNoTracking().Where(t => t.tenant_id.Equals(currentUser.tenant_id)).Select(t => new { t.customer_name}).ToListAsync();
            input.ForEach(async t =>
            {
                t.errorMsg = string.Empty;
                if (existsDatas.Any(d => d.customer_name.Equals(t.customer_name)))
                {
                    t.errorMsg = string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["customer_name"], t.customer_name);
                }
                else
                {
                    await DbSet.AddAsync(new CustomerEntity
                    {
                        customer_name = t.customer_name,
                        city = t.city,
                        address = t.address,
                        email = t.email,
                        manager = t.manager,
                        contact_tel = t.contact_tel,
                        creator = currentUser.user_name,
                        create_time = DateTime.Now,
                        last_update_time = DateTime.Now,
                        is_valid = true,
                        tenant_id = currentUser.tenant_id
                    });
                }
            });
            if (input.Any(t => t.errorMsg.Length > 0))
            {
                return (false, input.Where(t => t.errorMsg.Length > 0).ToList());
            }
            var qty = await _dBContext.SaveChangesAsync();
            if (qty > 0)
            {
                return (true, new List<CustomerImportViewModel>());
            }
            else
            {
                return (false, new List<CustomerImportViewModel>());
            }
        }

        #endregion
    }
}
