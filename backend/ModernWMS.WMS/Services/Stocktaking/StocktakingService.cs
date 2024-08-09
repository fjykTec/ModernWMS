/*
 * date：2022-12-30
 * developer：AMo
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
using System.Linq;
using ModernWMS.Core;
using ModernWMS.Core.Utility;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  Stocktaking Service
    /// </summary>
    public class StocktakingService : BaseService<StocktakingEntity>, IStocktakingService
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

        /// <summary>
        /// functions
        /// </summary>
        private readonly FunctionHelper _functionHelper;

        #endregion

        #region constructor
        /// <summary>
        ///Stocktaking  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        /// <param name="functionHelper">functionHelper</param>
        public StocktakingService(
            SqlDBContext dBContext
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
          , FunctionHelper functionHelper 
            )
        {
            this._dBContext = dBContext;
            this._stringLocalizer = stringLocalizer;
            _functionHelper = functionHelper;
        }
        #endregion

        #region Api
        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<StocktakingViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var Stocktakings = _dBContext.GetDbSet<StocktakingEntity>();
            var Spus = _dBContext.GetDbSet<SpuEntity>();
            var Skus = _dBContext.GetDbSet<SkuEntity>();
            var Goodsowners = _dBContext.GetDbSet<GoodsownerEntity>();
            var Goodslocations = _dBContext.GetDbSet<GoodslocationEntity>();
            var Stockadjusts = _dBContext.GetDbSet<StockadjustEntity>();
            var queryAdjust = Stockadjusts.AsNoTracking().Where(t => t.job_type == 1).Select(t => new { t.id, t.source_table_id });

            var query = from st in Stocktakings.AsNoTracking()
                        join sku in Skus.AsNoTracking() on st.sku_id equals sku.id
                        join spu in Spus.AsNoTracking() on sku.spu_id equals spu.id
                        join gsl in Goodslocations.AsNoTracking() on st.goods_location_id equals gsl.id
                        join gso in Goodsowners.AsNoTracking() on st.goods_owner_id equals gso.id into gsoJoin
                        from gso in gsoJoin.DefaultIfEmpty()
                        join adj in queryAdjust on st.id equals adj.source_table_id into adjJoin
                        from adj in adjJoin.DefaultIfEmpty()
                        where st.tenant_id == currentUser.tenant_id
                        select new StocktakingViewModel
                        {
                            id = st.id,
                            job_code = st.job_code,
                            job_status = st.job_status,
                            adjust_status = adj.id == null ? false : true,
                            sku_id = sku.id,
                            sku_code = sku.sku_code,
                            sku_name = sku.sku_name,
                            spu_code = spu.spu_code,
                            spu_name = spu.spu_name,
                            goods_location_id = st.goods_location_id,
                            warehouse_name = gsl.warehouse_name,
                            location_name = gsl.location_name,
                            goods_owner_id = st.goods_owner_id,
                            goods_owner_name = gso.goods_owner_name == null ? string.Empty : gso.goods_owner_name,
                            expiry_date = st.expiry_date,
                            price = st.price,
                            putaway_date = st.putaway_date,
                            series_number = st.series_number,
                            book_qty = st.book_qty,
                            counted_qty = st.counted_qty,
                            difference_qty = st.difference_qty,
                            creator = st.creator,
                            create_time = st.create_time,
                            handler = st.handler,
                            handle_time = st.handle_time,
                            last_update_time = st.last_update_time
                        };
            query = query.Where(queries.AsExpression<StocktakingViewModel>());
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.last_update_time)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list, totals);
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <returns></returns>
        public async Task<StocktakingViewModel> GetAsync(int id)
        {
            var Stocktakings = _dBContext.GetDbSet<StocktakingEntity>();
            var Spus = _dBContext.GetDbSet<SpuEntity>();
            var Skus = _dBContext.GetDbSet<SkuEntity>();
            var Goodsowners = _dBContext.GetDbSet<GoodsownerEntity>();
            var Goodslocations = _dBContext.GetDbSet<GoodslocationEntity>();
            var Stockadjusts = _dBContext.GetDbSet<StockadjustEntity>();
            var queryAdjust = Stockadjusts.AsNoTracking().Where(t => t.job_type == 1).Select(t => new { t.id, t.source_table_id });

            var query = from st in Stocktakings.AsNoTracking()
                        join sku in Skus.AsNoTracking() on st.sku_id equals sku.id
                        join spu in Spus.AsNoTracking() on sku.spu_id equals spu.id
                        join gsl in Goodslocations.AsNoTracking() on st.goods_location_id equals gsl.id
                        join gso in Goodsowners.AsNoTracking() on st.goods_owner_id equals gso.id into gsoJoin
                        from gso in gsoJoin.DefaultIfEmpty()
                        join adj in queryAdjust on st.id equals adj.source_table_id into adjJoin
                        from adj in adjJoin.DefaultIfEmpty()
                        where st.id == id
                        select new StocktakingViewModel
                        {
                            id = st.id,
                            job_code = st.job_code,
                            job_status = st.job_status,
                            adjust_status = adj.id == null ? false : true,
                            sku_id = sku.id,
                            sku_code = sku.sku_code,
                            sku_name = sku.sku_name,
                            spu_code = spu.spu_code,
                            spu_name = spu.spu_name,
                            goods_location_id = st.goods_location_id,
                            warehouse_name = gsl.warehouse_name,
                            location_name = gsl.location_name,
                            goods_owner_id = st.goods_owner_id,
                            goods_owner_name = gso.goods_owner_name == null ? string.Empty : gso.goods_owner_name,
                            expiry_date = st.expiry_date,
                            price = st.price,
                            putaway_date = st.putaway_date,
                            series_number = st.series_number,
                            book_qty = st.book_qty,
                            counted_qty = st.counted_qty,
                            difference_qty = st.difference_qty,
                            creator = st.creator,
                            create_time = st.create_time,
                            handler = st.handler,
                            handle_time = st.handle_time,
                            last_update_time = st.last_update_time
                        };
            var data = await query.FirstOrDefaultAsync();
            if (data != null)
            {
                return data;
            }
            else
            {
                return new StocktakingViewModel();
            }
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(StocktakingBasicViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StocktakingEntity>();
            var entity = viewModel.Adapt<StocktakingEntity>();
            entity.id = 0;
            entity.job_code = await _functionHelper.GetFormNoAsync("Stocktaking");
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
        /// get next code number
        /// </summary>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<string> GetOrderCode(CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StocktakingEntity>();
            string code = "";
            string date = DateTime.Now.ToString("yyyy" + "MM" + "dd");
            string maxNo = await DbSet.AsNoTracking().Where(t => t.tenant_id.Equals(currentUser.tenant_id)).MaxAsync(t => t.job_code);
            if (string.IsNullOrEmpty(maxNo))
            {
                code = date + "-0001";
            }
            else
            {
                try
                {
                    string maxDate = maxNo[..8];
                    string maxDateNo = maxNo[9..];
                    if (date == maxDate)
                    {
                        int.TryParse(maxDateNo, out int dd);
                        int newDateNo = dd + 1;
                        code = date + "-" + newDateNo.ToString("0000");
                    }
                    else
                    {
                        code = date + "-0001";
                    }
                }
                catch
                {
                    code = date + "-0001";
                }
            }

            return code;
        }
        /// <summary>
        /// update  counted_qty
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> PutAsync(StocktakingConfirmViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StocktakingEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            entity.counted_qty = viewModel.counted_qty;
            entity.difference_qty =  viewModel.counted_qty - entity.book_qty;
            entity.last_update_time = DateTime.Now;
            entity.handler = currentUser.user_name;
            entity.handle_time = DateTime.Now;
            entity.job_status = true;            
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
        /// Confirm a record
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ConfirmAsync(int id, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StocktakingEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            // change stock sku qty
            var Stocks = _dBContext.GetDbSet<StockEntity>();
            var stockEntity = await Stocks.FirstOrDefaultAsync(t => t.sku_id.Equals(entity.sku_id)
                                                                 && t.goods_owner_id.Equals(entity.goods_owner_id)
                                                                 && t.goods_location_id.Equals(entity.goods_location_id)
                                                                 && t.series_number.Equals(entity.series_number)
                                                                 && t.expiry_date.Equals(entity.expiry_date)
                                                                 && t.price.Equals(entity.price)
                                                                 && t.putaway_date.Equals(entity.putaway_date)
                                                                 );
            if (stockEntity == null)
            {
                await Stocks.AddAsync(new StockEntity
                {
                    sku_id = entity.sku_id,
                    goods_location_id = entity.goods_location_id,
                    qty = entity.difference_qty,
                    goods_owner_id = entity.goods_owner_id,
                    series_number = entity.series_number,
                    expiry_date = entity.expiry_date,
                    price = entity.price,
                    putaway_date = DateTime.Now.ToString("yyyy-MM-dd").ObjToDate(),
                    is_freeze = false,
                    last_update_time = DateTime.Now,
                    tenant_id = currentUser.tenant_id
                });
            }
            else
            {
                stockEntity.qty += entity.difference_qty;
                stockEntity.last_update_time = DateTime.Now;
            }
            // add a record to stockadjust
            var Stockadjusts = _dBContext.GetDbSet<StockadjustEntity>();
            await Stockadjusts.AddAsync(new StockadjustEntity
            {
                job_code = entity.job_code,
                sku_id = entity.sku_id,
                goods_location_id = entity.goods_location_id,
                goods_owner_id = entity.goods_owner_id,
                series_number = entity.series_number,
                expiry_date = entity.expiry_date,
                price = entity.price,
                putaway_date = entity.putaway_date,
                qty = entity.difference_qty,
                creator = currentUser.user_name,
                create_time = DateTime.Now,
                last_update_time = DateTime.Now,
                tenant_id = currentUser.tenant_id,
                is_update_stock = true,
                job_type = 1,
                source_table_id = entity.id
            });

            var qty = await _dBContext.SaveChangesAsync();
            if (qty > 0)
            {
                return (true, _stringLocalizer["operation_success"]);
            }
            else
            {
                return (false, _stringLocalizer["operation_failed"]);
            }
        }
        /// <summary>
        /// delete a record
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> DeleteAsync(int id)
        {
            var qty = await _dBContext.GetDbSet<StocktakingEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
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
 
