/*
 * date：2022-12-27
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
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System;
using ModernWMS.Core.Utility;
using ModernWMS.Core;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  Stockmove Service
    /// </summary>
    public class StockmoveService : BaseService<StockmoveEntity>, IStockmoveService
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
        /// Function Helper
        /// </summary>
        private readonly FunctionHelper _functionHelper;

        #endregion Args

        #region constructor

        /// <summary>
        ///Stockmove  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public StockmoveService(
            SqlDBContext dBContext
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            , FunctionHelper functionHelper
            )
        {
            this._dBContext = dBContext;
            this._stringLocalizer = stringLocalizer;
            this._functionHelper = functionHelper;
        }

        #endregion constructor

        #region Api

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<StockmoveViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var DbSet = _dBContext.GetDbSet<StockmoveEntity>();
            var location_DBSet = _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking();
            var query = from m in DbSet.AsNoTracking()
                        join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on m.sku_id equals sku.id
                        join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                        join orig_location in location_DBSet on m.orig_goods_location_id equals orig_location.id
                        join dest_location in location_DBSet on m.dest_googs_location_id equals dest_location.id
                        select new StockmoveViewModel
                        {
                            id = m.id,
                            job_code = m.job_code,
                            move_status = m.move_status,
                            sku_id = m.sku_id,
                            orig_goods_location_id = m.orig_goods_location_id,
                            dest_googs_location_id = m.dest_googs_location_id,
                            qty = m.qty,
                            goods_owner_id = m.goods_owner_id,
                            handler = m.handler,
                            handle_time = m.handle_time,
                            creator = m.creator,
                            create_time = m.create_time,
                            last_update_time = m.last_update_time,
                            tenant_id = m.tenant_id,
                            sku_code = sku.sku_code,
                            sku_name = sku.sku_name,
                            spu_code = spu.spu_code,
                            spu_name = spu.spu_name,
                            dest_googs_location_name = dest_location.location_name,
                            dest_googs_warehouse = dest_location.warehouse_name,
                            orig_goods_location_name = orig_location.location_name,
                            orig_goods_warehouse = orig_location.warehouse_name,
                            series_number = m.series_number,
                            expiry_date = m.expiry_date,
                            price = m.price,
                            putaway_date = m.putaway_date,
                        };
            query = query.Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                .Where(queries.AsExpression<StockmoveViewModel>());
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.last_update_time)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list, totals);
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<List<StockmoveViewModel>> GetAllAsync(CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StockmoveEntity>();
            var location_DBSet = _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking();
            var data = await (from m in DbSet.AsNoTracking().Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                              join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on m.sku_id equals sku.id
                              join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                              join orig_location in location_DBSet on m.orig_goods_location_id equals orig_location.id
                              join dest_location in location_DBSet on m.dest_googs_location_id equals dest_location.id
                              select new StockmoveViewModel
                              {
                                  id = m.id,
                                  job_code = m.job_code,
                                  move_status = m.move_status,
                                  sku_id = m.sku_id,
                                  orig_goods_location_id = m.orig_goods_location_id,
                                  dest_googs_location_id = m.dest_googs_location_id,
                                  qty = m.qty,
                                  goods_owner_id = m.goods_owner_id,
                                  handler = m.handler,
                                  handle_time = m.handle_time,
                                  creator = m.creator,
                                  create_time = m.create_time,
                                  last_update_time = m.last_update_time,
                                  tenant_id = m.tenant_id,
                                  sku_code = sku.sku_code,
                                  sku_name = sku.sku_name,
                                  spu_code = spu.spu_code,
                                  spu_name = spu.spu_name,
                                  dest_googs_location_name = dest_location.location_name,
                                  dest_googs_warehouse = dest_location.warehouse_name,
                                  orig_goods_location_name = orig_location.location_name,
                                  orig_goods_warehouse = orig_location.warehouse_name,
                                  series_number = m.series_number,
                                  expiry_date = m.expiry_date,
                                  price = m.price,
                                  putaway_date = m.putaway_date,
                              }
            ).ToListAsync();
            return data.Adapt<List<StockmoveViewModel>>();
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <returns></returns>
        public async Task<StockmoveViewModel> GetAsync(int id)
        {
            var DbSet = _dBContext.GetDbSet<StockmoveEntity>();
            var location_DBSet = _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking();
            var data = await (from m in DbSet.AsNoTracking()
                              join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on m.sku_id equals sku.id
                              join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                              join orig_location in location_DBSet on m.orig_goods_location_id equals orig_location.id
                              join dest_location in location_DBSet on m.dest_googs_location_id equals dest_location.id
                              where m.id == id
                              select new StockmoveViewModel
                              {
                                  id = m.id,
                                  job_code = m.job_code,
                                  move_status = m.move_status,
                                  sku_id = m.sku_id,
                                  orig_goods_location_id = m.orig_goods_location_id,
                                  dest_googs_location_id = m.dest_googs_location_id,
                                  qty = m.qty,
                                  goods_owner_id = m.goods_owner_id,
                                  handler = m.handler,
                                  handle_time = m.handle_time,
                                  creator = m.creator,
                                  create_time = m.create_time,
                                  last_update_time = m.last_update_time,
                                  tenant_id = m.tenant_id,
                                  sku_code = sku.sku_code,
                                  sku_name = sku.sku_name,
                                  spu_code = spu.spu_code,
                                  spu_name = spu.spu_name,
                                  dest_googs_location_name = dest_location.location_name,
                                  dest_googs_warehouse = dest_location.warehouse_name,
                                  orig_goods_location_name = orig_location.location_name,
                                  orig_goods_warehouse = orig_location.warehouse_name,
                                  series_number = m.series_number,
                                  expiry_date = m.expiry_date,
                                  price = m.price,
                                  putaway_date = m.putaway_date,
                              }).FirstOrDefaultAsync();

            return data;
        }

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(StockmoveViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StockmoveEntity>();
            var stock_DBSet = _dBContext.GetDbSet<StockEntity>();
            var entity = viewModel.Adapt<StockmoveEntity>();
            var processdetail_DBSet = _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking();
            var dispatchpick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var dispatch_DBSet = _dBContext.GetDbSet<DispatchlistEntity>().Where(t => t.tenant_id.Equals(currentUser.tenant_id));
            var dispatch_group_datas = from dp in dispatch_DBSet.AsNoTracking()
                                       join dpp in dispatchpick_DBSet.AsNoTracking() on dp.id equals dpp.dispatchlist_id
                                       where dp.dispatch_status > 1 && dp.dispatch_status < 6
                                       && dpp.goods_owner_id == entity.goods_owner_id && dpp.series_number == entity.series_number && dpp.goods_location_id == entity.orig_goods_location_id && dpp.sku_id == entity.sku_id && dpp.expiry_date == entity.expiry_date && dpp.price == entity.price && dpp.putaway_date == entity.putaway_date
                                       group dpp by new { dpp.sku_id, dpp.goods_location_id } into dg
                                       select new
                                       {
                                           sku_id = dg.Key.sku_id,
                                           goods_location_id = dg.Key.goods_location_id,
                                           qty_locked = dg.Sum(t => t.pick_qty)
                                       };
            var process_locked_group_datas = from pd in processdetail_DBSet
                                             where pd.is_update_stock == false
                                             && pd.sku_id == entity.sku_id && pd.goods_location_id == entity.orig_goods_location_id
                                             && pd.goods_owner_id == entity.goods_owner_id && pd.series_number == entity.series_number && pd.expiry_date == entity.expiry_date && pd.price == entity.price && pd.putaway_date == entity.putaway_date
                                             group pd by new { pd.sku_id, pd.goods_location_id } into pdg
                                             select new
                                             {
                                                 sku_id = pdg.Key.sku_id,
                                                 goods_location_id = pdg.Key.goods_location_id,
                                                 qty_locked = pdg.Sum(t => t.qty)
                                             };
            var move_locked_group_datas = from sm in DbSet.AsNoTracking()
                                          where sm.move_status == 0 && sm.sku_id == entity.sku_id && sm.orig_goods_location_id == entity.orig_goods_location_id
                                          && sm.goods_owner_id == entity.goods_owner_id && sm.series_number == entity.series_number && sm.expiry_date == entity.expiry_date && sm.price == entity.price && sm.putaway_date == entity.putaway_date
                                          group sm by new { sm.sku_id, goods_location_id = sm.orig_goods_location_id } into smg
                                          select new
                                          {
                                              smg.Key.sku_id,
                                              smg.Key.goods_location_id,
                                              qty_locked = smg.Sum(t => t.qty)
                                          };

            var orig_stock = await
                (from sg in stock_DBSet.AsNoTracking()
                 join dp in dispatch_group_datas on new { sg.sku_id, sg.goods_location_id } equals new { dp.sku_id, dp.goods_location_id } into dp_left
                 from dp in dp_left.DefaultIfEmpty()
                 join pl in process_locked_group_datas on new { sg.sku_id, sg.goods_location_id } equals new { pl.sku_id, pl.goods_location_id } into pl_left
                 from pl in pl_left.DefaultIfEmpty()
                 join sm in move_locked_group_datas on new { sg.sku_id, sg.goods_location_id } equals new { sm.sku_id, goods_location_id = sm.goods_location_id } into sm_left
                 from sm in sm_left.DefaultIfEmpty()
                 where sg.sku_id == entity.sku_id && sg.goods_location_id == entity.orig_goods_location_id
                 && sg.goods_owner_id == entity.goods_owner_id && sg.series_number == entity.series_number
                 && sg.expiry_date == entity.expiry_date && sg.price == entity.price && sg.putaway_date == entity.putaway_date
                 select new
                 {
                     id = sg.id,
                     qty_available = sg.is_freeze ? 0 : (sg.qty - (dp.qty_locked == null ? 0 : dp.qty_locked) - (pl.qty_locked == null ? 0 : pl.qty_locked) - (sm.qty_locked == null ? 0 : sm.qty_locked)),
                 }
                ).FirstOrDefaultAsync();
            var dest_stock = await stock_DBSet.FirstOrDefaultAsync(t => t.goods_owner_id == entity.goods_owner_id && t.series_number == entity.series_number && t.goods_location_id == entity.dest_googs_location_id && t.sku_id == entity.sku_id && t.expiry_date == entity.expiry_date && t.price == entity.price && t.putaway_date == entity.putaway_date);
            if (orig_stock == null || orig_stock.qty_available < entity.qty)
            {
                return (0, _stringLocalizer["qty_not_available"]);
            }
            if (dest_stock != null && dest_stock.is_freeze == true)
            {
                return (0, _stringLocalizer["dest_stock_freeze"]);
            }
            entity.id = 0;
            entity.move_status = 0;
            entity.create_time = DateTime.Now;
            entity.creator = currentUser.user_name;
            entity.last_update_time = DateTime.Now;
            entity.tenant_id = currentUser.tenant_id;
            entity.job_code = await _functionHelper.GetFormNoAsync("Stockmove");
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
        /// confirm move
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> Confirm(int id, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StockmoveEntity>();
            var stock_DBSet = _dBContext.GetDbSet<StockEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            var now_time = DateTime.Now;
            entity.handler = currentUser.user_name;
            entity.handle_time = now_time;
            entity.move_status = 1;
            entity.last_update_time = now_time;
            var orig_stock = await stock_DBSet.FirstOrDefaultAsync(t => t.goods_owner_id == entity.goods_owner_id && t.series_number == entity.series_number && t.goods_location_id == entity.orig_goods_location_id && t.sku_id == entity.sku_id && t.expiry_date == entity.expiry_date && t.price == entity.price && t.putaway_date == entity.putaway_date);
            var dest_stock = await stock_DBSet.FirstOrDefaultAsync(t => t.goods_owner_id == entity.goods_owner_id && t.series_number == entity.series_number && t.goods_location_id == entity.dest_googs_location_id && t.sku_id != entity.sku_id && t.expiry_date == entity.expiry_date && t.price == entity.price && t.putaway_date == entity.putaway_date);
            if (orig_stock != null)
            {
                if (orig_stock.qty == entity.qty)
                {
                    stock_DBSet.Remove(orig_stock);
                }
                else
                {
                    orig_stock.qty -= entity.qty;
                    orig_stock.last_update_time = now_time;
                }
            }
            if (dest_stock == null)
            {
                dest_stock = new StockEntity
                {
                    goods_location_id = entity.dest_googs_location_id,
                    sku_id = entity.sku_id,
                    goods_owner_id = entity.goods_owner_id,
                    is_freeze = false,
                    last_update_time = now_time,
                    qty = entity.qty,
                    tenant_id = entity.tenant_id,
                    series_number = entity.series_number,
                    expiry_date = entity.expiry_date,
                    price = entity.price,
                    putaway_date = entity.putaway_date,
                };
                await stock_DBSet.AddAsync(dest_stock);
            }
            else
            {
                dest_stock.qty += entity.qty;
                dest_stock.last_update_time = now_time;
            }
            var saved = false;
            int res = 0;
            while (!saved)
            {
                try
                {
                    // Attempt to save changes to the database
                    res = await _dBContext.SaveChangesAsync();
                    saved = true;
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    foreach (var entry in ex.Entries)
                    {
                        if (entry.Entity is StockEntity)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();
                            if (UtilConvert.ObjToInt(proposedValues["id"]) == orig_stock.id)
                            {
                                if (UtilConvert.ObjToInt(databaseValues["qty"]) - entity.qty < 0)
                                {
                                    throw new NotSupportedException(_stringLocalizer["try_agin"]);
                                }
                                else if (UtilConvert.ObjToInt(databaseValues["qty"]) - entity.qty == 0)
                                {
                                    entry.State = EntityState.Deleted;
                                }
                                else
                                {
                                    entry.State = EntityState.Modified;
                                    proposedValues["qty"] = UtilConvert.ObjToInt(databaseValues["qty"]) - entity.qty;
                                }
                            }
                            else if (UtilConvert.ObjToInt(proposedValues["id"]) == dest_stock.id)
                            {
                                proposedValues["qty"] = UtilConvert.ObjToInt(databaseValues["qty"]) + entity.qty;
                            }
                            // Refresh original values to bypass next concurrency check
                            entry.OriginalValues.SetValues(databaseValues);
                        }
                        else
                        {
                            throw new NotSupportedException(_stringLocalizer["try_agin"]);
                        }
                    }
                }
            }
            if (res > 0)
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
            var qty = await _dBContext.GetDbSet<StockmoveEntity>().Where(t => t.id.Equals(id) && t.move_status == 0).ExecuteDeleteAsync();
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
        /// get next order code number
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOrderCode(CurrentUser currentUser)
        {
            string code;
            string date = DateTime.Now.ToString("yyyy" + "MM" + "dd");
            string maxNo = await _dBContext.GetDbSet<StockmoveEntity>().AsNoTracking().Where(t => t.tenant_id == currentUser.tenant_id).MaxAsync(t => t.job_code);
            if (maxNo == null)
            {
                code = date + "-0001";
            }
            else
            {
                string maxDate = maxNo.Substring(0, 8);
                string maxDateNo = maxNo.Substring(9, 4);
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

            return code;
        }

        #endregion Api
    }
}