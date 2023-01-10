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
using ModernWMS.Core.Models;
using ModernWMS.Core.JWT;
using Microsoft.Extensions.Localization;
using ModernWMS.Core.DynamicSearch;
using System.Text;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  Warehouse Service
    /// </summary>
    public class WarehouseService : BaseService<WarehouseEntity>, IWarehouseService
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
        ///Warehouse  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public WarehouseService(
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
        /// get select items
        /// </summary>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<List<FormSelectItem>> GetSelectItemsAsnyc(CurrentUser currentUser)
        {
            var res = new List<FormSelectItem>();
            var DBSet = _dBContext.GetDbSet<WarehouseEntity>();
            res.AddRange(await (from db in DBSet.AsNoTracking()
                                where db.is_valid == true && db.tenant_id == currentUser.tenant_id
                                select new FormSelectItem
                                {
                                    code = "warehouse_name",
                                    name = db.warehouse_name,
                                    value = db.id.ToString(),
                                    comments = "warehouse datas",
                                }).ToListAsync());
            return res;
        }

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<WarehouseViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var DbSet = _dBContext.GetDbSet<WarehouseEntity>();
            var query = DbSet.AsNoTracking()
                .Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                .Where(queries.AsExpression<WarehouseEntity>());
            if (pageSearch.sqlTitle == "select")
            {
                query = query.Where(t => t.is_valid == true);
            }
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.create_time)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list.Adapt<List<WarehouseViewModel>>(), totals);
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<List<WarehouseViewModel>> GetAllAsync(CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<WarehouseEntity>();
            var data = await DbSet.AsNoTracking().Where(t => t.tenant_id.Equals(currentUser.tenant_id)).ToListAsync();
            return data.Adapt<List<WarehouseViewModel>>();
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <returns></returns>
        public async Task<WarehouseViewModel> GetAsync(int id)
        {
            var DbSet = _dBContext.GetDbSet<WarehouseEntity>();
            var entity = await DbSet.AsNoTracking().FirstOrDefaultAsync(t => t.id.Equals(id));
            if (entity == null)
            {
                return null;
            }
            return entity.Adapt<WarehouseViewModel>();
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(WarehouseViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<WarehouseEntity>();
            if (await DbSet.AnyAsync(t => t.warehouse_name == viewModel.warehouse_name && t.tenant_id == currentUser.tenant_id))
            {
               return(0,  string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["warehouse_name"], viewModel.warehouse_name));
            }
            var entity = viewModel.Adapt<WarehouseEntity>();
            entity.id = 0;
            entity.create_time = DateTime.Now;
            entity.creator = currentUser.user_name;
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
        public async Task<(bool flag, string msg)> UpdateAsync(WarehouseViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<WarehouseEntity>();
            if (await DbSet.AnyAsync(t => t.id != viewModel.id && t.warehouse_name == viewModel.warehouse_name && t.tenant_id == currentUser.tenant_id))
            {
               return(false,  string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["warehouse_name"], viewModel.warehouse_name));
            }
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            entity.id = viewModel.id;
            entity.warehouse_name = viewModel.warehouse_name;
            entity.city = viewModel.city;
            entity.address = viewModel.address;
            entity.email = viewModel.email;
            entity.manager = viewModel.manager;
            entity.contact_tel = viewModel.contact_tel;
            entity.is_valid = viewModel.is_valid;
            entity.last_update_time = DateTime.Now;
            var warehousearea_DBSet = _dBContext.GetDbSet<WarehouseareaEntity>();
            var wadatas =await warehousearea_DBSet.Where(t => t.warehouse_id == entity.id).ToListAsync();
            wadatas.ForEach(t =>
            {
                t.is_valid = entity.is_valid;
            });
            var goodslocation_DBSet = _dBContext.GetDbSet<GoodslocationEntity>();
            var gldatas = await goodslocation_DBSet.Where(t => t.warehouse_area_id == entity.id).ToListAsync();
            gldatas.ForEach(t =>
            {
                t.warehouse_name = entity.warehouse_name;
                t.is_valid = entity.is_valid;
            });
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
            if (await _dBContext.GetDbSet<GoodslocationEntity>().AnyAsync(t => t.warehouse_id == id))
            {
                return (false, _stringLocalizer["exist_warehousearea_not_delete"]);
            }
            var qty = await _dBContext.GetDbSet<WarehouseEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
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
        /// import warehouses by excel
        /// </summary>
        /// <param name="datas">excel datas</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ExcelAsync(List<WarehouseExcelImportViewModel> datas, CurrentUser currentUser)
        {
            StringBuilder sb = new StringBuilder();
            var DbSet = _dBContext.GetDbSet<WarehouseEntity>();
            var warehouse_name_repeat_excel = datas.GroupBy(t => t.warehouse_name).Select(t => new { warehouse_name = t.Key, cnt = t.Count() }).Where(t => t.cnt > 1).ToList();
            foreach (var repeat in warehouse_name_repeat_excel)
            {
                sb.AppendLine(string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["warehouse_name"], repeat.warehouse_name));
            }
            if (warehouse_name_repeat_excel.Count > 0)
            {
                return (false, sb.ToString());
            }

            var warehouse_name_repeat_exists = await DbSet.Where(t=>t.tenant_id == currentUser.tenant_id).Where(t => datas.Select(t => t.warehouse_name).ToList().Contains(t.warehouse_name)).Select(t => t.warehouse_name).ToListAsync();
            foreach (var repeat in warehouse_name_repeat_exists)
            {
                sb.AppendLine(string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["warehouse_name"], repeat));
            }
            if (warehouse_name_repeat_exists.Count > 0)
            {
                return (false, sb.ToString());
            }

            var entities = datas.Adapt<List<WarehouseEntity>>();
            entities.ForEach(t =>
            {
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

