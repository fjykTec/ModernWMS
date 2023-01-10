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
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Net.WebSockets;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  Warehousearea Service
    /// </summary>
    public class WarehouseareaService : BaseService<WarehouseareaEntity>, IWarehouseareaService
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
        ///Warehousearea  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public WarehouseareaService(
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
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<WarehouseareaViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var DbSet = _dBContext.GetDbSet<WarehouseareaEntity>();
            var warehouse_DBSet = _dBContext.GetDbSet<WarehouseEntity>();

            var query = from wa in DbSet.AsNoTracking()
                        join w in warehouse_DBSet.AsNoTracking() on wa.warehouse_id equals w.id
                        select new WarehouseareaViewModel
                        {
                            id = wa.id,
                            warehouse_id = wa.warehouse_id,
                            warehouse_name = w.warehouse_name,
                            area_name = wa.area_name,
                            parent_id = wa.parent_id,
                            create_time = wa.create_time,
                            last_update_time = wa.last_update_time,
                            is_valid = wa.is_valid,
                            tenant_id = wa.tenant_id,
                            area_property = wa.area_property,
                        };
            if (pageSearch.sqlTitle == "select")
            {
                query = query.Where(t => t.is_valid == true);
            }
            query = query.Where(t => t.tenant_id.Equals(currentUser.tenant_id)).Where(queries.AsExpression<WarehouseareaViewModel>());
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.create_time)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list, totals);
        }
        /// <summary>
        /// get warehouseareas of the warehouse by warehouse_id
        /// </summary>
        /// <param name="warehouse_id">warehouse's id</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<List<FormSelectItem>> GetWarehouseareaByWarehouse_id(int warehouse_id, CurrentUser currentUser)
        {
            var res = new List<FormSelectItem>();
            var DbSet = _dBContext.GetDbSet<WarehouseareaEntity>();
            res = await (from wa in DbSet.AsNoTracking()
                         where wa.is_valid == true && wa.tenant_id == currentUser.tenant_id && wa.warehouse_id == warehouse_id
                         select new FormSelectItem
                         {
                             code = "warehousearea",
                             comments = "warehouseareas of the warehouse",
                             name = wa.area_name,
                             value = wa.id.ToString(),
                         }).ToListAsync();
            return res;
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<List<WarehouseareaViewModel>> GetAllAsync(int warehouse_id, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<WarehouseareaEntity>().AsNoTracking();
            if (warehouse_id > 0)
            {
                DbSet = DbSet.Where(t=>t.warehouse_id == warehouse_id);
            }
            var data = await DbSet.Where(t =>t.is_valid == true && t.tenant_id.Equals(currentUser.tenant_id)).ToListAsync();
            return data.Adapt<List<WarehouseareaViewModel>>();
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <returns></returns>
        public async Task<WarehouseareaViewModel> GetAsync(int id)
        {
            var DbSet = _dBContext.GetDbSet<WarehouseareaEntity>();
            var entity = await DbSet.AsNoTracking().FirstOrDefaultAsync(t => t.id.Equals(id));
            if (entity == null)
            {
                return null;
            }
            return entity.Adapt<WarehouseareaViewModel>();
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(WarehouseareaViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<WarehouseareaEntity>();
            if (await DbSet.AnyAsync(t => t.warehouse_id == viewModel.warehouse_id && t.area_name == viewModel.area_name && t.tenant_id == currentUser.tenant_id))
            {
                return (0, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["area_name"], viewModel.area_name));
            }
            var entity = viewModel.Adapt<WarehouseareaEntity>();
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
        public async Task<(bool flag, string msg)> UpdateAsync(WarehouseareaViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<WarehouseareaEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (await DbSet.AnyAsync(t => t.id != viewModel.id && t.warehouse_id == viewModel.warehouse_id && t.area_name == viewModel.area_name && t.tenant_id == currentUser.tenant_id))
            {
                return (false, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["area_name"], viewModel.area_name));
            }
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            entity.id = viewModel.id;
            entity.warehouse_id = viewModel.warehouse_id;
            entity.area_name = viewModel.area_name;
            entity.parent_id = viewModel.parent_id;
            entity.is_valid = viewModel.is_valid;
            entity.area_property = viewModel.area_property;
            entity.last_update_time = DateTime.Now;
            var goodslocation_DBSet = _dBContext.GetDbSet<GoodslocationEntity>();
            var gldatas = await goodslocation_DBSet.Where(t => t.warehouse_area_id == entity.id).ToListAsync();
            gldatas.ForEach(t =>
            {
                t.warehouse_area_name = entity.area_name;
                t.warehouse_area_property = entity.area_property;
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
            if (await _dBContext.GetDbSet < GoodslocationEntity>().AnyAsync(t=>t.warehouse_area_id  == id))
            {
                return (false, _stringLocalizer["exist_location_not_delete"]);
            }
            var qty = await _dBContext.GetDbSet<WarehouseareaEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
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

