/*
 * date：2022-12-21
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
 using ModernWMS.Core.DynamicSearch;
using ModernWMS.Core.Models;
using ModernWMS.Core.JWT;
using ModernWMS.Core.Utility;
using System.Text;

namespace ModernWMS.WMS.Services
 {
     /// <summary>
     ///  Supplier Service
     /// </summary>
     public class SupplierService : BaseService<SupplierEntity>, ISupplierService
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
         ///Supplier  constructor
         /// </summary>
         /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
         public SupplierService(
             SqlDBContext dBContext
           , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             )
         {
             this._dBContext = dBContext;
            this._stringLocalizer= stringLocalizer;
         }
         #endregion
 
         #region Api
         /// <summary>
         /// page search
         /// </summary>
         /// <param name="pageSearch">args</param>
         /// <param name="currentUser">currentUser</param>
         /// <returns></returns>
         public async Task<(List<SupplierViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
         {
             QueryCollection queries = new QueryCollection();
             if (pageSearch.searchObjects.Any())
             {
                 pageSearch.searchObjects.ForEach(s =>
                 {
                     queries.Add(s);
                 });
             }
             var DbSet = _dBContext.GetDbSet<SupplierEntity>();
             var query = DbSet.AsNoTracking()
                 .Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                 .Where(queries.AsExpression<SupplierEntity>());
            if(pageSearch.sqlTitle == "select")
            {
                query = query.Where(t => t.is_valid == true);
            }
             int totals = await query.CountAsync();
             var list = await query.OrderByDescending(t => t.create_time)
                        .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                        .Take(pageSearch.pageSize)
                        .ToListAsync();
             return (list.Adapt<List<SupplierViewModel>>(), totals);
         }
 
         /// <summary>
         /// Get all records
         /// </summary>
         /// <returns></returns>
         public async Task<List<SupplierViewModel>> GetAllAsync(CurrentUser currentUser)
         {
             var DbSet = _dBContext.GetDbSet<SupplierEntity>();
             var data = await DbSet.AsNoTracking().Where(t=>t.tenant_id.Equals(currentUser.tenant_id)).ToListAsync();
             return data.Adapt<List<SupplierViewModel>>();
         }
 
         /// <summary>
         /// Get a record by id
         /// </summary>
         /// <returns></returns>
         public async Task<SupplierViewModel> GetAsync(int id)
         {
             var DbSet = _dBContext.GetDbSet<SupplierEntity>();
             var entity = await DbSet.AsNoTracking().FirstOrDefaultAsync(t=>t.id.Equals(id));
             if (entity == null)
             {
                 return null;
             }
             return entity.Adapt<SupplierViewModel>();
         }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(SupplierViewModel viewModel, CurrentUser currentUser) 
        {
            var DbSet = _dBContext.GetDbSet<SupplierEntity>();
            var entity = viewModel.Adapt<SupplierEntity>();
             entity.id = 0;
             entity.create_time = DateTime.Now;
             entity.creator = currentUser.user_name;
             entity.last_update_time = DateTime.Now;
             entity.tenant_id = currentUser.tenant_id;
             await DbSet.AddAsync(entity);
            if (await DbSet.AnyAsync(t => t.supplier_name == viewModel.supplier_name && t.tenant_id == currentUser.tenant_id))
            {
                return (0, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["supplier_name"], viewModel.supplier_name));
            }
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
        public async Task<(bool flag, string msg)> UpdateAsync(SupplierViewModel viewModel, CurrentUser currentUser)
         {
             var DbSet = _dBContext.GetDbSet<SupplierEntity>();
            if (await DbSet.AnyAsync(t => t.id != viewModel.id && t.supplier_name == viewModel.supplier_name && t.tenant_id == currentUser.tenant_id))
            {
               return(false, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["supplier_name"], viewModel.supplier_name));
            }
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
             if (entity == null)
             {
                 return (false,_stringLocalizer[ "not_exists_entity"]);
             }
             entity.id = viewModel.id;
             entity.supplier_name = viewModel.supplier_name;
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
             var qty = await _dBContext.GetDbSet<SupplierEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
             if (qty > 0)
             {
                 return (true, _stringLocalizer["delete_success"]);
             }
             else
             {
                 return (false, _stringLocalizer["delete_failed"]);
             }
         }

        /// <summary>
        /// import suppliers by excel
        /// </summary>
        /// <param name="datas">excel datas</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ExcelAsync(List<SupplierExcelImportViewModel> datas,CurrentUser currentUser)
        {
            StringBuilder sb = new StringBuilder();
            var DbSet = _dBContext.GetDbSet<SupplierEntity>();
            var supplier_name_repeat_excel = datas.GroupBy(t => t.supplier_name).Select(t => new { supplier_name = t.Key, cnt = t.Count() }).Where(t => t.cnt > 1).ToList();
            foreach (var repeat in supplier_name_repeat_excel)
            {
                sb.AppendLine(string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["supplier_name"], repeat.supplier_name));
            }
            if (supplier_name_repeat_excel.Count > 0)
            {
                return (false, sb.ToString());
            }

            var supplier_name_repeat_exists = await DbSet.Where(t=>t.tenant_id == currentUser.tenant_id).Where(t => datas.Select(t => t.supplier_name).ToList().Contains(t.supplier_name)).Select(t => t.supplier_name).ToListAsync();
            foreach (var repeat in supplier_name_repeat_exists)
            {
                sb.AppendLine(string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["supplier_name"], repeat));
            }
            if (supplier_name_repeat_exists.Count > 0)
            {
                return (false, sb.ToString());
            }

            var entities = datas.Adapt<List<SupplierEntity>>();
            entities.ForEach(t => {
                t.creator = currentUser.user_name;
                t.tenant_id = currentUser.tenant_id;
                t.create_time = DateTime.Now;
                t.last_update_time = DateTime.Now;
                t.is_valid = true;
            });
            await DbSet.AddRangeAsync(entities);
            var res = await _dBContext.SaveChangesAsync();
            if (res > 0)
            {
                return (true, _stringLocalizer["save_success"]);
            }
            return (false, _stringLocalizer["save_failed"]);
        }
        #endregion
    }
 }
 
