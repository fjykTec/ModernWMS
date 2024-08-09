/*
 * date：2022-12-22
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
using System.Runtime.Intrinsics.Arm;
using System.Net.WebSockets;
using System.Linq;
using ModernWMS.WMS.Entities.ViewModels.Stock;
using ModernWMS.Core.Utility;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
using Pomelo.EntityFrameworkCore.MySql.Query.Internal;
using Microsoft.Extensions.Configuration;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  Stock Service
    /// </summary>
    public class StockService : BaseService<StockEntity>, IStockService
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

        public IConfiguration Configuration { get; }

        #endregion Args

        #region constructor

        /// <summary>
        ///Stock  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public StockService(
            SqlDBContext dBContext
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            , IConfiguration configuration
            )
        {
            this._dBContext = dBContext;
            this._stringLocalizer = stringLocalizer;
            this.Configuration = configuration;
        }

        #endregion constructor

        #region Api

        /// <summary>
        /// stock page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<StockManagementViewModel> data, int totals)> StockPageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }

            var DbSet = _dBContext.GetDbSet<StockEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var asn_DBSet = _dBContext.GetDbSet<AsnEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var dispatch_DBSet = _dBContext.GetDbSet<DispatchlistEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var sku_DBSet = _dBContext.GetDbSet<SkuEntity>().AsNoTracking();
            var spu_DBSet = _dBContext.GetDbSet<SpuEntity>().AsNoTracking();
            var processdetail_DBSet = _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking();
            var move_DBSet = _dBContext.GetDbSet<StockmoveEntity>();
            var stock_group_datas = from stock in DbSet.AsNoTracking()
                                    join gl in _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking() on stock.goods_location_id equals gl.id
                                    group new { stock, gl } by stock.sku_id into sg
                                    select new
                                    {
                                        sku_id = sg.Key,
                                        qty_frozen = sg.Where(t => t.stock.is_freeze == true).Sum(e => e.stock.qty),
                                        qty = sg.Sum(t => t.stock.qty),
                                        qty_normal = sg.Where(t => t.gl.warehouse_area_property != 5).Sum(t => t.stock.qty),
                                        qty_normal_frozen = sg.Where(t => t.gl.warehouse_area_property != 5 && t.stock.is_freeze == true).Sum(t => t.stock.qty),
                                    };
            var asn_group_datas = from asn in asn_DBSet.AsNoTracking()
                                  group asn by asn.sku_id into ag
                                  select new
                                  {
                                      sku_id = ag.Key,
                                      qty_asn = ag.Where(t => t.asn_status == 0).Sum(t => t.asn_qty),
                                      qty_to_unload = ag.Where(t => t.asn_status == 1).Sum(t => t.asn_qty),
                                      qty_to_sort = ag.Where(t => t.asn_status == 2).Sum(t => t.asn_qty),
                                      qty_sorted = ag.Where(t => t.asn_status == 3).Sum(t => t.sorted_qty),
                                      shortage_qty = ag.Where(t => t.asn_status == 4).Sum(t => t.shortage_qty)
                                  };
            var dispatch_group_datas = from dp in dispatch_DBSet.AsNoTracking()
                                       group dp by dp.sku_id into dg
                                       select new
                                       {
                                           sku_id = dg.Key,
                                           qty_locked = dg.Sum(t => t.lock_qty)
                                       };
            var process_locked_group_datas = from pd in processdetail_DBSet
                                             join gl in _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking() on pd.goods_location_id equals gl.id
                                             where pd.is_update_stock == false && pd.is_source == true
                                             group new { pd, gl } by pd.sku_id into pdg
                                             select new
                                             {
                                                 sku_id = pdg.Key,
                                                 qty_locked = pdg.Sum(t => t.pd.qty),
                                                 qty_normal_locked = pdg.Where(t => t.gl.warehouse_area_property != 5).Sum(t => t.pd.qty),
                                             };
            var move_locked_group_datas = from m in move_DBSet.AsNoTracking()
                                          join gl in _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking() on m.orig_goods_location_id equals gl.id
                                          where m.move_status == 0
                                          group new { m, gl } by m.sku_id into mg
                                          select new
                                          {
                                              sku_id = mg.Key,
                                              qty_locked = mg.Sum(t => t.m.qty),
                                              qty_normal_locked = mg.Where(t => t.gl.warehouse_area_property != 5).Sum(t => t.m.qty),
                                          };

            var query = from sku in sku_DBSet
                        join ag in asn_group_datas on sku.id equals ag.sku_id into ag_left
                        from ag in ag_left.DefaultIfEmpty()
                        join sg in stock_group_datas on sku.id equals sg.sku_id into sg_left
                        from sg in sg_left.DefaultIfEmpty()
                        join dp in dispatch_group_datas on sg.sku_id equals dp.sku_id into dp_left
                        from dp in dp_left.DefaultIfEmpty()
                        join pl in process_locked_group_datas on sku.id equals pl.sku_id into pl_left
                        from pl in pl_left.DefaultIfEmpty()
                        join m in move_locked_group_datas on sku.id equals m.sku_id into m_left
                        from m in m_left.DefaultIfEmpty()
                        join spu in spu_DBSet on sku.spu_id equals spu.id
                        where spu.tenant_id == currentUser.tenant_id
                        select new StockManagementViewModel
                        {
                            sku_id = sku.id,
                            spu_name = spu.spu_name,
                            spu_code = spu.spu_code,
                            sku_code = sku.sku_code,
                            qty_asn = ag.qty_asn == null ? 0 : ag.qty_asn,
                            qty_available = (sg.qty_normal == null ? 0 : sg.qty_normal) - (sg.qty_normal_frozen == null ? 0 : sg.qty_normal_frozen) - (dp.qty_locked == null ? 0 : dp.qty_locked) - (pl.qty_normal_locked == null ? 0 : pl.qty_normal_locked) - (m.qty_normal_locked == null ? 0 : m.qty_normal_locked),
                            qty_frozen = sg.qty_frozen == null ? 0 : sg.qty_frozen,
                            qty_locked = (dp.qty_locked == null ? 0 : dp.qty_locked) + (pl.qty_locked == null ? 0 : pl.qty_locked) + (m.qty_locked == null ? 0 : m.qty_locked),
                            qty_sorted = ag.qty_sorted == null ? 0 : ag.qty_sorted,
                            qty_to_sort = ag.qty_to_sort == null ? 0 : ag.qty_to_sort,
                            shortage_qty = ag.shortage_qty == null ? 0 : ag.shortage_qty,
                            qty_to_unload = ag.qty_to_unload == null ? 0 : ag.qty_to_unload,
                            qty = sg.qty == null ? 0 : sg.qty,
                        };
            query = query.Where(t => t.qty_asn > 0 || t.qty > 0).Where(queries.AsExpression<StockManagementViewModel>());
            int totals = await query.CountAsync();
            var list = await query.OrderBy(t => t.sku_code)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list, totals);
        }

        /// <summary>
        /// location stock page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<LocationStockManagementViewModel> data, int totals)> LocationStockPageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }

            var DbSet = _dBContext.GetDbSet<StockEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var dispatchpick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var dispatch_DBSet = _dBContext.GetDbSet<DispatchlistEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var sku_DBSet = _dBContext.GetDbSet<SkuEntity>().AsNoTracking();
            var spu_DBSet = _dBContext.GetDbSet<SpuEntity>().AsNoTracking();
            var location_DBSet = _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking();
            var processdetail_DBSet = _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking();
            var move_DBSet = _dBContext.GetDbSet<StockmoveEntity>();

            var stock_group_datas = from stock in DbSet.AsNoTracking()
                                    join gw in _dBContext.GetDbSet<GoodsownerEntity>().AsNoTracking() on stock.goods_owner_id equals gw.id into gw_left
                                    from gw in gw_left.DefaultIfEmpty()
                                    where stock.tenant_id == currentUser.tenant_id
                                    group new { stock, gw } by new { stock.sku_id, stock.goods_location_id, stock.goods_owner_id, stock.series_number, gw.goods_owner_name, stock.expiry_date, stock.price, stock.putaway_date } into sg
                                    select new
                                    {
                                        sku_id = sg.Key.sku_id,
                                        goods_location_id = sg.Key.goods_location_id,
                                        goods_owner_id = sg.Key.goods_owner_id,
                                        goods_owner_name = sg.Key.goods_owner_name,
                                        series_number = sg.Key.series_number,
                                        sg.Key.expiry_date,
                                        sg.Key.price,
                                        sg.Key.putaway_date,
                                        qty_frozen = sg.Where(t => t.stock.is_freeze == true).Sum(e => e.stock.qty),
                                        qty = sg.Sum(t => t.stock.qty)
                                    };

            var dispatch_group_datas = from dp in dispatch_DBSet.AsNoTracking()
                                       join dpp in dispatchpick_DBSet.AsNoTracking() on dp.id equals dpp.dispatchlist_id
                                       where dp.dispatch_status > 1 && dp.dispatch_status < 6
                                       group dpp by new { dpp.sku_id, dpp.goods_location_id, dpp.goods_owner_id, dpp.series_number, dpp.expiry_date, dpp.price, dpp.putaway_date } into dg
                                       select new
                                       {
                                           sku_id = dg.Key.sku_id,
                                           goods_location_id = dg.Key.goods_location_id,
                                           goods_owner_id = dg.Key.goods_owner_id,
                                           series_number = dg.Key.series_number,
                                           dg.Key.expiry_date,
                                           dg.Key.price,
                                           dg.Key.putaway_date,
                                           qty_locked = dg.Sum(t => t.pick_qty)
                                       };
            var process_locked_group_datas = from pd in processdetail_DBSet
                                             where pd.is_update_stock == false && pd.is_source == true
                                             group pd by new { pd.sku_id, pd.goods_location_id, pd.goods_owner_id, pd.series_number, pd.expiry_date, pd.price, pd.putaway_date } into pdg
                                             select new
                                             {
                                                 sku_id = pdg.Key.sku_id,
                                                 goods_location_id = pdg.Key.goods_location_id,
                                                 goods_owner_id = pdg.Key.goods_owner_id,
                                                 series_number = pdg.Key.series_number,
                                                 pdg.Key.expiry_date,
                                                 pdg.Key.price,
                                                 pdg.Key.putaway_date,
                                                 qty_locked = pdg.Sum(t => t.qty)
                                             };

            var move_locked_group_datas = from m in move_DBSet.AsNoTracking()
                                          where m.move_status == 0
                                          group m by new { m.sku_id, m.orig_goods_location_id, m.goods_owner_id, m.series_number, m.expiry_date, m.price, m.putaway_date } into mg
                                          select new
                                          {
                                              sku_id = mg.Key.sku_id,
                                              goods_location_id = mg.Key.orig_goods_location_id,
                                              goods_owner_id = mg.Key.goods_owner_id,
                                              series_number = mg.Key.series_number,
                                              mg.Key.expiry_date,
                                              mg.Key.price,
                                              mg.Key.putaway_date,
                                              qty_locked = mg.Sum(t => t.qty)
                                          };
            var query = from sg in stock_group_datas
                        join dp in dispatch_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price, sg.putaway_date } equals new { dp.sku_id, dp.goods_location_id, dp.goods_owner_id, dp.series_number, dp.expiry_date, dp.price, dp.putaway_date } into dp_left
                        from dp in dp_left.DefaultIfEmpty()
                        join pl in process_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price, sg.putaway_date } equals new { pl.sku_id, pl.goods_location_id, pl.goods_owner_id, pl.series_number, pl.expiry_date, pl.price, pl.putaway_date } into pl_left
                        from pl in pl_left.DefaultIfEmpty()
                        join m in move_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price, sg.putaway_date } equals new { m.sku_id, m.goods_location_id, m.goods_owner_id, m.series_number, m.expiry_date, m.price, m.putaway_date } into m_left
                        from m in m_left.DefaultIfEmpty()
                        join sku in sku_DBSet on sg.sku_id equals sku.id
                        join spu in spu_DBSet on sku.spu_id equals spu.id
                        join gl in location_DBSet on sg.goods_location_id equals gl.id
                        select new LocationStockManagementViewModel
                        {
                            sku_id = sg.sku_id,
                            goods_owner_name = sg.goods_owner_name,
                            spu_name = spu.spu_name,
                            spu_code = spu.spu_code,
                            sku_code = sku.sku_code,
                            sku_name = sku.sku_name,
                            qty_available = gl.warehouse_area_property == 5 ? 0 : (sg.qty - sg.qty_frozen - (dp.qty_locked == null ? 0 : dp.qty_locked) - (pl.qty_locked == null ? 0 : pl.qty_locked) - (m.qty_locked == null ? 0 : m.qty_locked)),
                            qty_frozen = sg.qty_frozen,
                            qty_locked = (dp.qty_locked == null ? 0 : dp.qty_locked) + (pl.qty_locked == null ? 0 : pl.qty_locked) + (m.qty_locked == null ? 0 : m.qty_locked),
                            qty = sg.qty,
                            location_name = gl.location_name,
                            warehouse_name = gl.warehouse_name,
                            series_number = sg.series_number,
                            expiry_date = sg.expiry_date,
                            price = sg.price,
                            putaway_date = sg.putaway_date,
                        };
            query = query.Where(t => t.qty > 0).Where(queries.AsExpression<LocationStockManagementViewModel>());
            int totals = await query.CountAsync();
            var list = await query.OrderBy(t => t.sku_code)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list, totals);
        }

        /// <summary>
        /// safety stock
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<SafetyStockManagementViewModel> data, int totals)> SafetyStockPageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }

            var DbSet = _dBContext.GetDbSet<StockEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var dispatchpick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var dispatch_DBSet = _dBContext.GetDbSet<DispatchlistEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var sku_DBSet = _dBContext.GetDbSet<SkuEntity>().AsNoTracking();
            var spu_DBSet = _dBContext.GetDbSet<SpuEntity>().AsNoTracking();
            var location_DBSet = _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking();
            var processdetail_DBSet = _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking();
            var move_DBSet = _dBContext.GetDbSet<StockmoveEntity>();
            var sku_safety_DBSet = _dBContext.GetDbSet<SkuSafetyStockEntity>();
            var stock_group_datas = from stock in DbSet.AsNoTracking()
                                    join gl in location_DBSet.AsNoTracking() on stock.goods_location_id equals gl.id
                                    where stock.tenant_id == currentUser.tenant_id
                                    group new { stock, gl } by new { stock.sku_id, gl.warehouse_id } into sg
                                    select new
                                    {
                                        sku_id = sg.Key.sku_id,
                                        warehouse_id = sg.Key.warehouse_id,
                                        qty_frozen = sg.Where(t => t.stock.is_freeze == true).Sum(e => e.stock.qty),
                                        qty = sg.Sum(t => t.stock.qty)
                                    };

            var dispatch_group_datas = from dp in dispatch_DBSet.AsNoTracking()
                                       join dpp in dispatchpick_DBSet.AsNoTracking() on dp.id equals dpp.dispatchlist_id
                                       join gl in location_DBSet.AsNoTracking() on dpp.goods_location_id equals gl.id
                                       where dp.dispatch_status > 1 && dp.dispatch_status < 6
                                       group dpp by new { dpp.sku_id, gl.warehouse_id } into dg
                                       select new
                                       {
                                           sku_id = dg.Key.sku_id,
                                           warehouse_id = dg.Key.warehouse_id,
                                           qty_locked = dg.Sum(t => t.pick_qty)
                                       };
            var process_locked_group_datas = from pd in processdetail_DBSet
                                             join gl in location_DBSet.AsNoTracking() on pd.goods_location_id equals gl.id
                                             where pd.is_update_stock == false && pd.is_source == true
                                             group new { pd, gl } by new { pd.sku_id, gl.warehouse_id } into pdg
                                             select new
                                             {
                                                 sku_id = pdg.Key.sku_id,
                                                 warehouse_id = pdg.Key.warehouse_id,
                                                 qty_locked = pdg.Sum(t => t.pd.qty)
                                             };

            var move_locked_group_datas = from m in move_DBSet.AsNoTracking()
                                          join gl in location_DBSet.AsNoTracking() on m.orig_goods_location_id equals gl.id
                                          where m.move_status == 0
                                          group new { m, gl } by new { m.sku_id, gl.warehouse_id } into mg
                                          select new
                                          {
                                              sku_id = mg.Key.sku_id,
                                              warehouse_id = mg.Key.warehouse_id,
                                              qty_locked = mg.Sum(t => t.m.qty)
                                          };
            var query = from sg in stock_group_datas
                        join dp in dispatch_group_datas on new { sg.sku_id, sg.warehouse_id } equals new { dp.sku_id, dp.warehouse_id } into dp_left
                        from dp in dp_left.DefaultIfEmpty()
                        join pl in process_locked_group_datas on new { sg.sku_id, sg.warehouse_id } equals new { pl.sku_id, pl.warehouse_id } into pl_left
                        from pl in pl_left.DefaultIfEmpty()
                        join m in move_locked_group_datas on new { sg.sku_id, sg.warehouse_id } equals new { m.sku_id, m.warehouse_id } into m_left
                        from m in m_left.DefaultIfEmpty()
                        join sku in sku_DBSet on sg.sku_id equals sku.id
                        join spu in spu_DBSet on sku.spu_id equals spu.id
                        join gl in location_DBSet on sg.warehouse_id equals gl.id
                        join sss in sku_safety_DBSet on new { sg.sku_id, sg.warehouse_id } equals new { sss.sku_id, sss.warehouse_id } into sss_left
                        from sss in sss_left.DefaultIfEmpty()
                        select new SafetyStockManagementViewModel
                        {
                            sku_id = sg.sku_id,
                            spu_name = spu.spu_name,
                            spu_code = spu.spu_code,
                            sku_code = sku.sku_code,
                            sku_name = sku.sku_name,
                            qty_available = gl.warehouse_area_property == 5 ? 0 : (sg.qty - sg.qty_frozen - (dp.qty_locked == null ? 0 : dp.qty_locked) - (pl.qty_locked == null ? 0 : pl.qty_locked) - (m.qty_locked == null ? 0 : m.qty_locked)),
                            qty_frozen = sg.qty_frozen,
                            qty_locked = (dp.qty_locked == null ? 0 : dp.qty_locked) + (pl.qty_locked == null ? 0 : pl.qty_locked) + (m.qty_locked == null ? 0 : m.qty_locked),
                            qty = sg.qty,
                            warehouse_name = gl.warehouse_name,
                            safety_stock_qty = sss.safety_stock_qty == null ? 0 : sss.safety_stock_qty,
                        };
            query = query.Where(queries.AsExpression<SafetyStockManagementViewModel>());
            int totals = await query.CountAsync();
            var list = await query.OrderBy(t => t.sku_code)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list, totals);
        }

        /// <summary>
        ///  page search select
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<StockViewModel> data, int totals)> SelectPageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }

            var DbSet = _dBContext.GetDbSet<StockEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var dispatchpick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var dispatch_DBSet = _dBContext.GetDbSet<DispatchlistEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var sku_DBSet = _dBContext.GetDbSet<SkuEntity>().AsNoTracking();
            var spu_DBSet = _dBContext.GetDbSet<SpuEntity>().AsNoTracking();
            var location_DBSet = _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking();
            var processdetail_DBSet = _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking();
            var move_DBSet = _dBContext.GetDbSet<StockmoveEntity>();

            var dispatch_group_datas = from dp in dispatch_DBSet.AsNoTracking()
                                       join dpp in dispatchpick_DBSet.AsNoTracking() on dp.id equals dpp.dispatchlist_id
                                       where dp.dispatch_status > 1 && dp.dispatch_status < 6
                                       group dpp by new { dpp.sku_id, dpp.goods_location_id, dpp.goods_owner_id, dpp.series_number, dpp.expiry_date, dpp.price, dpp.putaway_date } into dg
                                       select new
                                       {
                                           goods_owner_id = dg.Key.goods_owner_id,
                                           sku_id = dg.Key.sku_id,
                                           goods_location_id = dg.Key.goods_location_id,
                                           series_number = dg.Key.series_number,
                                           dg.Key.expiry_date,
                                           dg.Key.price,
                                           dg.Key.putaway_date,
                                           qty_locked = dg.Sum(t => t.pick_qty)
                                       };
            var process_locked_group_datas = from pd in processdetail_DBSet
                                             where pd.is_update_stock == false && pd.is_source == true
                                             group pd by new { pd.sku_id, pd.goods_location_id, pd.goods_owner_id, pd.series_number, pd.expiry_date, pd.price, pd.putaway_date } into pdg
                                             select new
                                             {
                                                 goods_owner_id = pdg.Key.goods_owner_id,
                                                 sku_id = pdg.Key.sku_id,
                                                 goods_location_id = pdg.Key.goods_location_id,
                                                 series_number = pdg.Key.series_number,
                                                 pdg.Key.expiry_date,
                                                 pdg.Key.price,
                                                 pdg.Key.putaway_date,
                                                 qty_locked = pdg.Sum(t => t.qty)
                                             };
            var move_locked_group_datas = from m in move_DBSet.AsNoTracking()
                                          where m.move_status == 0
                                          group m by new { m.sku_id, m.orig_goods_location_id, m.goods_owner_id, m.series_number, m.expiry_date, m.price, m.putaway_date } into mg
                                          select new
                                          {
                                              goods_owner_id = mg.Key.goods_owner_id,
                                              sku_id = mg.Key.sku_id,
                                              goods_location_id = mg.Key.orig_goods_location_id,
                                              series_number = mg.Key.series_number,
                                              mg.Key.expiry_date,
                                              mg.Key.price,
                                              mg.Key.putaway_date,
                                              qty_locked = mg.Sum(t => t.qty)
                                          };
            var query = from sg in DbSet.AsNoTracking()
                        join dp in dispatch_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price, sg.putaway_date } equals new { dp.sku_id, dp.goods_location_id, dp.goods_owner_id, dp.series_number, dp.expiry_date, dp.price, dp.putaway_date } into dp_left
                        from dp in dp_left.DefaultIfEmpty()
                        join pl in process_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price, sg.putaway_date } equals new { pl.sku_id, pl.goods_location_id, pl.goods_owner_id, pl.series_number, pl.expiry_date, pl.price, pl.putaway_date } into pl_left
                        from pl in pl_left.DefaultIfEmpty()
                        join m in move_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price, sg.putaway_date } equals new { m.sku_id, m.goods_location_id, m.goods_owner_id, m.series_number, m.expiry_date, m.price, m.putaway_date } into m_left
                        from m in m_left.DefaultIfEmpty()
                        join sku in sku_DBSet on sg.sku_id equals sku.id
                        join spu in spu_DBSet on sku.spu_id equals spu.id
                        join gl in location_DBSet on sg.goods_location_id equals gl.id
                        join owner in _dBContext.GetDbSet<GoodsownerEntity>().AsNoTracking() on sg.goods_owner_id equals owner.id into o_left
                        from owner in o_left.DefaultIfEmpty()
                        where sg.tenant_id == currentUser.tenant_id
                        group new { sg, dp, pl, m, sku, spu, gl } by new
                        {
                            sg.sku_id,
                            spu.spu_name,
                            spu.spu_code,
                            sku.sku_code,
                            sku.sku_name,
                            sg.goods_location_id,
                            sg.goods_owner_id,
                            owner.goods_owner_name,
                            sg.qty,
                            gl.location_name,
                            sg.is_freeze,
                            gl.warehouse_name,
                            sg.id,
                            sku.unit,
                            sg.series_number,
                            sg.expiry_date,
                            sg.price,
                            sg.putaway_date,
                            sg.tenant_id
                        } into g
                        select new StockViewModel
                        {
                            sku_id = g.Key.sku_id,
                            spu_name = g.Key.spu_name,
                            spu_code = g.Key.spu_code,
                            sku_code = g.Key.sku_code,
                            sku_name = g.Key.sku_name,
                            qty_available = g.Key.is_freeze ? 0 : (g.Key.qty - g.Sum(t => t.dp.qty_locked == null ? 0 : t.dp.qty_locked) - g.Sum(t => t.pl.qty_locked == null ? 0 : t.pl.qty_locked) - g.Sum(t => (t.m.qty_locked == null ? 0 : t.m.qty_locked))),
                            qty = g.Key.qty,
                            goods_location_id = g.Key.goods_location_id,
                            goods_owner_id = g.Key.goods_owner_id,
                            location_name = g.Key.location_name,
                            warehouse_name = g.Key.warehouse_name,
                            series_number = g.Key.series_number,
                            expiry_date = g.Key.expiry_date,
                            price = g.Key.price,
                            putaway_date = g.Key.putaway_date,
                            is_freeze = g.Key.is_freeze,
                            id = g.Key.id,
                            tenant_id = g.Key.tenant_id,
                            unit = g.Key.unit,
                            goods_owner_name = g.Key.goods_owner_name == null ? "" : g.Key.goods_owner_name,
                        };
            if (pageSearch.sqlTitle == "")
            {
                query = query.Where(t => t.qty_available > 0);
            }
            else if (pageSearch.sqlTitle == "all")
            {
            }
            else if (pageSearch.sqlTitle == "frozen")
            {
                query = query.Where(t => t.is_freeze == true);
            }
            query = query.Where(queries.AsExpression<StockViewModel>());
            int totals = await query.CountAsync();
            var list = await query.OrderBy(t => t.sku_code)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list, totals);
        }

        /// <summary>
        ///  sku page search select
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<SkuSelectViewModel> data, int totals)> SkuSelectPageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var sku_DBSet = _dBContext.GetDbSet<SkuEntity>();
            var spu_DBSet = _dBContext.GetDbSet<SpuEntity>();
            var query = from sku in sku_DBSet.AsNoTracking()
                        join spu in spu_DBSet.AsNoTracking() on sku.spu_id equals spu.id
                        where spu.tenant_id == currentUser.tenant_id
                        select new SkuSelectViewModel
                        {
                            spu_id = sku.spu_id,
                            sku_code = sku.sku_code,
                            sku_name = sku.sku_name,
                            unit = sku.unit,
                            spu_code = spu.spu_code,
                            spu_name = spu.spu_name,
                            supplier_id = spu.supplier_id,
                            supplier_name = spu.supplier_name,
                            brand = spu.brand,
                            origin = spu.origin,
                            sku_id = sku.id,
                        };
            query = query.Where(queries.AsExpression<SkuSelectViewModel>());
            int totals = await query.CountAsync();
            var list = await query.OrderBy(t => t.sku_code)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list, totals);
        }

        /// <summary>
        /// get stock infomation by phone
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<List<LocationStockManagementViewModel>> LocationStockForPhoneAsync(LocationStockForPhoneSearchViewModel input, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StockEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var dispatchpick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var dispatch_DBSet = _dBContext.GetDbSet<DispatchlistEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var sku_DBSet = _dBContext.GetDbSet<SkuEntity>().AsNoTracking();
            var spu_DBSet = _dBContext.GetDbSet<SpuEntity>().AsNoTracking();
            var location_DBSet = _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking();
            var processdetail_DBSet = _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking();
            var move_DBSet = _dBContext.GetDbSet<StockmoveEntity>();

            var stock_group_datas = from stock in DbSet.AsNoTracking()
                                    join gw in _dBContext.GetDbSet<GoodsownerEntity>().AsNoTracking() on stock.goods_owner_id equals gw.id into gw_left
                                    from gw in gw_left.DefaultIfEmpty()
                                    join gl in _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking() on stock.goods_location_id equals gl.id
                                    join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on stock.sku_id equals sku.id
                                    join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                                    where stock.tenant_id == currentUser.tenant_id && (input.sku_id == 0 || stock.sku_id == input.sku_id)
                                    && (input.goods_location_id == 0 || stock.goods_location_id == input.goods_location_id)
                                    && (input.warehouse_id == 0 || gl.warehouse_id == input.warehouse_id)
                                    && (input.spu_name == "" || spu.spu_name.Contains(input.spu_name))
                                    && (input.location_name == "" || gl.location_name.Contains(input.location_name))
                                    && (input.series_number == "" || stock.series_number == input.series_number)
                                    group new { stock, gw } by new { stock.sku_id, stock.goods_location_id, stock.goods_owner_id, gw.goods_owner_name, stock.series_number, stock.expiry_date, stock.price, stock.putaway_date } into sg
                                    select new
                                    {
                                        sku_id = sg.Key.sku_id,
                                        goods_location_id = sg.Key.goods_location_id,
                                        goods_owner_id = sg.Key.goods_owner_id,
                                        goods_owner_name = sg.Key.goods_owner_name,
                                        series_number = sg.Key.series_number,
                                        sg.Key.expiry_date,
                                        sg.Key.price,
                                        sg.Key.putaway_date,
                                        qty_frozen = sg.Where(t => t.stock.is_freeze == true).Sum(e => e.stock.qty),
                                        qty = sg.Sum(t => t.stock.qty)
                                    };

            var dispatch_group_datas = from dp in dispatch_DBSet.AsNoTracking()
                                       join dpp in dispatchpick_DBSet.AsNoTracking() on dp.id equals dpp.dispatchlist_id
                                       where dp.dispatch_status > 1 && dp.dispatch_status < 6
                                       group dpp by new { dpp.sku_id, dpp.goods_location_id, dpp.goods_owner_id, dpp.series_number, dpp.expiry_date, dpp.price, dpp.putaway_date } into dg
                                       select new
                                       {
                                           sku_id = dg.Key.sku_id,
                                           goods_location_id = dg.Key.goods_location_id,
                                           goods_owner_id = dg.Key.goods_owner_id,
                                           series_number = dg.Key.series_number,
                                           dg.Key.expiry_date,
                                           dg.Key.price,
                                           dg.Key.putaway_date,
                                           qty_locked = dg.Sum(t => t.pick_qty)
                                       };
            var process_locked_group_datas = from pd in processdetail_DBSet
                                             where pd.is_update_stock == false && pd.is_source == true
                                             group pd by new { pd.sku_id, pd.goods_location_id, pd.goods_owner_id, pd.series_number, pd.expiry_date, pd.price, pd.putaway_date } into pdg
                                             select new
                                             {
                                                 sku_id = pdg.Key.sku_id,
                                                 goods_location_id = pdg.Key.goods_location_id,
                                                 goods_owner_id = pdg.Key.goods_owner_id,
                                                 series_number = pdg.Key.series_number,
                                                 pdg.Key.expiry_date,
                                                 pdg.Key.price,
                                                 pdg.Key.putaway_date,
                                                 qty_locked = pdg.Sum(t => t.qty)
                                             };

            var move_locked_group_datas = from m in move_DBSet.AsNoTracking()
                                          where m.move_status == 0
                                          group m by new { m.sku_id, m.orig_goods_location_id, m.goods_owner_id, m.series_number, m.expiry_date, m.price, m.putaway_date } into mg
                                          select new
                                          {
                                              sku_id = mg.Key.sku_id,
                                              goods_location_id = mg.Key.orig_goods_location_id,
                                              goods_owner_id = mg.Key.goods_owner_id,
                                              series_number = mg.Key.series_number,
                                              mg.Key.expiry_date,
                                              mg.Key.price,
                                              mg.Key.putaway_date,
                                              qty_locked = mg.Sum(t => t.qty)
                                          };
            var query = from sg in stock_group_datas
                        join dp in dispatch_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price, sg.putaway_date } equals new { dp.sku_id, dp.goods_location_id, dp.goods_owner_id, dp.series_number, dp.expiry_date, dp.price, dp.putaway_date } into dp_left
                        from dp in dp_left.DefaultIfEmpty()
                        join pl in process_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price, sg.putaway_date } equals new { pl.sku_id, pl.goods_location_id, pl.goods_owner_id, pl.series_number, pl.expiry_date, pl.price, pl.putaway_date } into pl_left
                        from pl in pl_left.DefaultIfEmpty()
                        join m in move_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price, sg.putaway_date } equals new { m.sku_id, m.goods_location_id, m.goods_owner_id, m.series_number, m.expiry_date, m.price, m.putaway_date } into m_left
                        from m in m_left.DefaultIfEmpty()
                        join sku in sku_DBSet on sg.sku_id equals sku.id
                        join spu in spu_DBSet on sku.spu_id equals spu.id
                        join gl in location_DBSet on sg.goods_location_id equals gl.id
                        select new LocationStockManagementViewModel
                        {
                            sku_id = sg.sku_id,
                            goods_owner_name = sg.goods_owner_name,
                            spu_name = spu.spu_name,
                            spu_code = spu.spu_code,
                            sku_code = sku.sku_code,
                            sku_name = sku.sku_name,
                            qty_available = gl.warehouse_area_property == 5 ? 0 : (sg.qty - sg.qty_frozen - (dp.qty_locked == null ? 0 : dp.qty_locked) - (pl.qty_locked == null ? 0 : pl.qty_locked) - (m.qty_locked == null ? 0 : m.qty_locked)),
                            qty_frozen = sg.qty_frozen,
                            qty_locked = (dp.qty_locked == null ? 0 : dp.qty_locked) + (pl.qty_locked == null ? 0 : pl.qty_locked) + (m.qty_locked == null ? 0 : m.qty_locked),
                            qty = sg.qty,
                            location_name = gl.location_name,
                            warehouse_name = gl.warehouse_name,
                            series_number = sg.series_number,
                            expiry_date = sg.expiry_date,
                            price = sg.price,
                            putaway_date = sg.putaway_date,
                            goods_location_id = sg.goods_location_id
                        };

            var list = await query.OrderBy(t => t.sku_code)
                       .ToListAsync();
            return list;
        }

        /// <summary>
        /// delivery statistic
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(List<DeliveryStatisticViewModel> datas, int totals)> DeliveryStatistic(DeliveryStatisticSearchViewModel input, CurrentUser currentUser)
        {
            var dispatch_DBSet = _dBContext.GetDbSet<DispatchlistEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var dispatchpick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var sku_DBSet = _dBContext.GetDbSet<SkuEntity>();
            var spu_DBSet = _dBContext.GetDbSet<SpuEntity>();
            var location_DBSet = _dBContext.GetDbSet<GoodslocationEntity>();
            var warehouse_DBSet = _dBContext.GetDbSet<WarehouseEntity>();
            var owner_DbSet = _dBContext.GetDbSet<GoodsownerEntity>();
            if (input.delivery_date_from > UtilConvert.MinDate)
            {
                dispatch_DBSet = dispatch_DBSet.Where(t => t.create_time >= input.delivery_date_from);
            }
            if (input.delivery_date_to > UtilConvert.MinDate)
            {
                dispatch_DBSet = dispatch_DBSet.Where(t => t.create_time <= input.delivery_date_to);
            }
            var query = from dp in dispatch_DBSet.AsNoTracking()
                        join dpp in dispatchpick_DBSet.AsNoTracking() on dp.id equals dpp.dispatchlist_id
                        join sku in sku_DBSet.AsNoTracking() on dp.sku_id equals sku.id
                        join spu in spu_DBSet.AsNoTracking() on sku.spu_id equals spu.id
                        join location in location_DBSet.AsNoTracking() on dpp.goods_location_id equals location.id
                        join wh in warehouse_DBSet.AsNoTracking() on location.warehouse_id equals wh.id
                        join go in owner_DbSet.AsNoTracking() on dpp.goods_owner_id equals go.id
                        where dp.dispatch_status >= 6 && spu.spu_name.Contains(input.spu_name) && spu.spu_code.Contains(input.spu_code)
                        && sku.sku_name.Contains(input.sku_name) && sku.sku_code.Contains(input.sku_code) && wh.warehouse_name.Contains(input.warehouse_name)
                        && dp.customer_name.Contains(input.customer_name) && go.goods_owner_name.Contains(input.goods_owner_name)
                        group new { dpp, dp, spu, sku } by
                        new
                        {
                            dp.dispatch_no,
                            wh.warehouse_name,
                            location_name = location.location_name,
                            spu.spu_name,
                            spu.spu_code,
                            sku.sku_name,
                            sku.sku_code,
                            dpp.series_number,
                            dpp.price,
                            dpp.expiry_date,
                            dpp.putaway_date,
                            dp.customer_name,
                            dp.create_time,
                            dpp.goods_owner_id,
                            go.goods_owner_name,
                        }
                        into dg
                        select new DeliveryStatisticViewModel
                        {
                            dispatch_no = dg.Key.dispatch_no,
                            warehouse_name = dg.Key.warehouse_name,
                            location_name = dg.Key.location_name,
                            spu_name = dg.Key.spu_name,
                            spu_code = dg.Key.spu_code,
                            sku_name = dg.Key.sku_name,
                            sku_code = dg.Key.sku_code,
                            series_number = dg.Key.series_number,
                            expiry_date = dg.Key.expiry_date,
                            price = dg.Key.price,
                            putaway_date = dg.Key.putaway_date,
                            customer_name = dg.Key.customer_name,
                            delivery_date = dg.Key.create_time,
                            goods_owner_name = dg.Key.goods_owner_name,
                            delivery_qty = dg.Sum(t => t.dpp.picked_qty),
                            delivery_amount = dg.Sum(t => t.dpp.picked_qty * t.sku.price)
                        };
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.delivery_date)
                                              .Skip((input.pageIndex - 1) * input.pageSize)
                                              .Take(input.pageSize).ToListAsync();
            return (list, totals);
        }

        /// <summary>
        /// stock age page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<StockAgeViewModel> data, int totals)> StockAgePageAsync(StockAgeSearchViewModel input, CurrentUser currentUser)
        {
            var database_config = Configuration.GetSection("Database")["db"].ToUpper();
            var DbSet = _dBContext.GetDbSet<StockEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var sku_DBSet = _dBContext.GetDbSet<SkuEntity>().AsNoTracking();
            var spu_DBSet = _dBContext.GetDbSet<SpuEntity>().AsNoTracking();
            var location_DBSet = _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking();
            if (input.expiry_date_from > UtilConvert.MinDate)
            {
                DbSet = DbSet.Where(t => t.expiry_date >= input.expiry_date_from);
            }
            if (input.expiry_date_to > UtilConvert.MinDate)
            {
                DbSet = DbSet.Where(t => t.expiry_date <= input.expiry_date_to);
            }
            var stock_group_datas = from stock in DbSet.AsNoTracking()
                                    join gw in _dBContext.GetDbSet<GoodsownerEntity>().AsNoTracking() on stock.goods_owner_id equals gw.id into gw_left
                                    from gw in gw_left.DefaultIfEmpty()
                                    where stock.tenant_id == currentUser.tenant_id
                                    group new { stock, gw } by new { stock.sku_id, stock.goods_location_id, stock.goods_owner_id, stock.series_number, gw.goods_owner_name, stock.expiry_date, stock.price, stock.putaway_date } into sg
                                    select new
                                    {
                                        sku_id = sg.Key.sku_id,
                                        goods_location_id = sg.Key.goods_location_id,
                                        goods_owner_id = sg.Key.goods_owner_id,
                                        goods_owner_name = sg.Key.goods_owner_name,
                                        series_number = sg.Key.series_number,
                                        sg.Key.expiry_date,
                                        sg.Key.price,
                                        sg.Key.putaway_date,
                                        qty_frozen = sg.Where(t => t.stock.is_freeze == true).Sum(e => e.stock.qty),
                                        qty = sg.Sum(t => t.stock.qty)
                                    };
            var today = DateTime.Today;
            var query = from sg in stock_group_datas
                        join sku in sku_DBSet on sg.sku_id equals sku.id
                        join spu in spu_DBSet on sku.spu_id equals spu.id
                        join gl in location_DBSet on sg.goods_location_id equals gl.id
                        where spu.spu_name.Contains(input.spu_name) && sku.sku_name.Contains(input.sku_name)
                        && sku.sku_code.Contains(input.sku_code) && spu.spu_code.Contains(input.spu_code)
                        && gl.warehouse_name.Contains(input.warehouse_name)
                        select new StockAgeViewModel
                        {
                            sku_id = sg.sku_id,
                            goods_owner_name = sg.goods_owner_name,
                            spu_name = spu.spu_name,
                            spu_code = spu.spu_code,
                            sku_code = sku.sku_code,
                            sku_name = sku.sku_name,
                            qty = sg.qty,
                            location_name = gl.location_name,
                            warehouse_name = gl.warehouse_name,
                            series_number = sg.series_number,
                            expiry_date = sg.expiry_date,
                            price = sg.price,
                            putaway_date = sg.putaway_date,
                            stock_age = sg.putaway_date == UtilConvert.MinDate ? 0 : database_config == "MYSQL" ? Microsoft.EntityFrameworkCore.MySqlDbFunctionsExtensions.DateDiffDay(EF.Functions, sg.putaway_date.Date, today) : Microsoft.EntityFrameworkCore.SqlServerDbFunctionsExtensions.DateDiffDay(EF.Functions, sg.putaway_date.Date, today),
                        };

            if (input.stock_age_from > 0)
            {
                query = query.Where(t => t.stock_age >= input.stock_age_from);
            }
            if (input.stock_age_to > 0)
            {
                query = query.Where(t => t.stock_age <= input.stock_age_to);
            }
            query = query.Where(t => t.qty > 0);
            int totals = await query.CountAsync();
            var list = await query.OrderBy(t => t.sku_code)
                       .Skip((input.pageIndex - 1) * input.pageSize)
                       .Take(input.pageSize)
                       .ToListAsync();
            return (list, totals);
        }

        #endregion Api
    }
}