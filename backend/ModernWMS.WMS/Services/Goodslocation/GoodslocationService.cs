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
 
 namespace ModernWMS.WMS.Services
 {
     /// <summary>
     ///  Goodslocation Service
     /// </summary>
     public class GoodslocationService : BaseService<GoodslocationEntity>, IGoodslocationService
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
         ///Goodslocation  constructor
         /// </summary>
         /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
         public GoodslocationService(
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
        /// get goodslocation of the warehousearea by warehouse_id and warehousearea_id
        /// </summary>
        /// <param name="warehouse_area_id">warehousearea's id</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<List<FormSelectItem>> GetGoodslocationByWarehouse_area_id( int warehouse_area_id, CurrentUser currentUser)
        {
            var res = new List<FormSelectItem>();
            var DbSet = _dBContext.GetDbSet<GoodslocationEntity>();
            res = await (from g in DbSet.AsNoTracking()
                         where g.is_valid == true && g.tenant_id == currentUser.tenant_id  && g.warehouse_area_id== warehouse_area_id
                         select new FormSelectItem
                         {
                             code = "goodslocation",
                             comments = "goodslocations of the warehousearea",
                             name = g.location_name,
                             value = g.id.ToString(),
                         }).ToListAsync();
            return res;
        }

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<GoodslocationViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
         {
             QueryCollection queries = new QueryCollection();
             if (pageSearch.searchObjects.Any())
             {
                 pageSearch.searchObjects.ForEach(s =>
                 {
                     queries.Add(s);
                 });
             }
             var DbSet = _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking();

             var query = DbSet
                 .Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                 .Where(queries.AsExpression<GoodslocationEntity>());
            if (pageSearch.sqlTitle == "select")
            {
                query = query.Where(t => t.is_valid == true);
            }
            int totals = await query.CountAsync();
             var list = await query.OrderByDescending(t => t.create_time)
                        .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                        .Take(pageSearch.pageSize)
                        .ToListAsync();
             return (list.Adapt<List<GoodslocationViewModel>>(), totals);
         }
         
         /// <summary>
         /// Get all records
         /// </summary>
         /// <returns></returns>
         public async Task<List<GoodslocationViewModel>> GetAllAsync(CurrentUser currentUser)
         {
             var DbSet = _dBContext.GetDbSet<GoodslocationEntity>();
             var data = await DbSet.AsNoTracking().Where(t=>t.tenant_id.Equals(currentUser.tenant_id)).ToListAsync();
             return data.Adapt<List<GoodslocationViewModel>>();
         }
 
         /// <summary>
         /// Get a record by id
         /// </summary>
         /// <returns></returns>
         public async Task<GoodslocationViewModel> GetAsync(int id)
         {
             var DbSet = _dBContext.GetDbSet<GoodslocationEntity>();
             var entity = await DbSet.AsNoTracking().FirstOrDefaultAsync(t=>t.id.Equals(id));
             if (entity == null)
             {
                 return null;
             }
             return entity.Adapt<GoodslocationViewModel>();
         }
         /// <summary>
         /// add a new record
         /// </summary>
         /// <param name="viewModel">viewmodel</param>
         /// <param name="currentUser">current user</param>
         /// <returns></returns>
         public async Task<(int id, string msg)> AddAsync(GoodslocationViewModel viewModel, CurrentUser currentUser)
         {
             var DbSet = _dBContext.GetDbSet<GoodslocationEntity>();
            if (await DbSet.AnyAsync(t => t.location_name == viewModel.location_name && t.tenant_id == currentUser.tenant_id))
            {
                return (0, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["location_name"], viewModel.location_name));
            }
            var entity = viewModel.Adapt<GoodslocationEntity>();
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
        public async Task<(bool flag, string msg)> UpdateAsync(GoodslocationViewModel viewModel,CurrentUser currentUser)
         {
             var DbSet = _dBContext.GetDbSet<GoodslocationEntity>();
            if (await DbSet.AnyAsync(t => t.id != viewModel.id && t.warehouse_id == viewModel.warehouse_id && t.location_name == viewModel.location_name && t.tenant_id == currentUser.tenant_id))
            {
                return (false, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["location_name"], viewModel.location_name));
            }
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
             if (entity == null)
             {
                 return (false,_stringLocalizer[ "not_exists_entity"]);
             }
             entity.id = viewModel.id;
             entity.warehouse_id = viewModel.warehouse_id;
             entity.warehouse_name = viewModel.warehouse_name;
             entity.warehouse_area_name = viewModel.warehouse_area_name;
             entity.warehouse_area_property = viewModel.warehouse_area_property;
             entity.location_name = viewModel.location_name;
             entity.location_length = viewModel.location_length;
             entity.location_width = viewModel.location_width;
             entity.location_heigth = viewModel.location_heigth;
             entity.location_volume = viewModel.location_volume;
             entity.location_load = viewModel.location_load;
             entity.roadway_number = viewModel.roadway_number;
             entity.shelf_number = viewModel.shelf_number;
             entity.layer_number = viewModel.layer_number;
             entity.tag_number = viewModel.tag_number;
             entity.is_valid = viewModel.is_valid;
             entity.warehouse_area_id = viewModel.warehouse_area_id;
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
             var exist_stock =await  _dBContext.GetDbSet<StockEntity>().AsNoTracking().Where(t=>t.qty>0&&t.goods_location_id == id ).AnyAsync();
            if (exist_stock)
            {
                return (false, _stringLocalizer["location_exist_stock_not_delete"]);
            }
             var qty = await _dBContext.GetDbSet<GoodslocationEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
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
 
