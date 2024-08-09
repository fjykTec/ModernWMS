/*
 * date：2022-12-27
 * developer：NoNo
 */

using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModernWMS.Core;
using ModernWMS.Core.DBContext;
using ModernWMS.Core.DynamicSearch;
using ModernWMS.Core.JWT;
using ModernWMS.Core.Models;
using ModernWMS.Core.Services;
using ModernWMS.Core.Utility;
using ModernWMS.WMS.Entities.Models;
using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.WMS.IServices;
using System.Collections.Generic;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  Dispatchlist Service
    /// </summary>
    public class DispatchlistService : BaseService<DispatchlistEntity>, IDispatchlistService
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

        #endregion Args

        #region constructor

        /// <summary>
        ///Dispatchlist  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public DispatchlistService(
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
        public async Task<(List<DispatchlistViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var DbSet = _dBContext.GetDbSet<DispatchlistEntity>().AsNoTracking();
            if (pageSearch.sqlTitle.Contains("dispatch_status"))
            {
                var dispatch_status = Convert.ToByte(pageSearch.sqlTitle.Trim().ToLower().Replace("dispatch_status", "").Replace("：", "").Replace(":", "").Replace("=", ""));
                DbSet = DbSet.Where(t => t.dispatch_status.Equals(dispatch_status));
            }
            else if (pageSearch.sqlTitle.Equals("package"))
            {
                DbSet = DbSet.Where(t => t.picked_qty == t.qty && (t.dispatch_status.Equals(3) || (t.package_qty < t.picked_qty && t.dispatch_status.Equals(5)) || t.dispatch_status.Equals(4)));
            }
            else if (pageSearch.sqlTitle.Equals("weight"))
            {
                DbSet = DbSet.Where(t => t.picked_qty == t.qty && (t.dispatch_status.Equals(3) || (t.weighing_qty < t.picked_qty && t.dispatch_status.Equals(4)) || t.dispatch_status.Equals(5)));
            }
            else if (pageSearch.sqlTitle.Equals("delivery"))
            {
                DbSet = DbSet.Where(t => t.picked_qty == t.qty && (t.dispatch_status.Equals(3) || t.dispatch_status.Equals(4) || t.dispatch_status.Equals(5) || t.dispatch_status.Equals(6)));
            }

            var query = from d in DbSet.AsNoTracking()
                        join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on d.sku_id equals sku.id
                        join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                        select new DispatchlistViewModel
                        {
                            id = d.id,
                            dispatch_no = d.dispatch_no,
                            dispatch_status = d.dispatch_status,
                            customer_id = d.customer_id,
                            customer_name = d.customer_name,
                            sku_id = d.sku_id,
                            qty = d.qty,
                            weight = d.weight,
                            volume = d.volume,
                            creator = d.creator,
                            create_time = d.create_time,
                            damage_qty = d.damage_qty,
                            lock_qty = d.lock_qty,
                            picked_qty = d.picked_qty,
                            intrasit_qty = d.intrasit_qty,
                            package_qty = d.package_qty,
                            unpackage_qty = d.picked_qty - d.package_qty,
                            weighing_qty = d.weighing_qty,
                            unweighing_qty = d.picked_qty - d.weighing_qty,
                            actual_qty = d.actual_qty,
                            sign_qty = d.sign_qty,
                            package_no = d.package_no,
                            package_person = d.package_person,
                            package_time = d.package_time,
                            weighing_no = d.weighing_no,
                            weighing_person = d.weighing_person,
                            weighing_weight = d.weighing_weight,
                            waybill_no = d.waybill_no,
                            carrier = d.carrier,
                            freightfee = d.freightfee,
                            last_update_time = d.last_update_time,
                            tenant_id = d.tenant_id,
                            sku_code = sku.sku_code,
                            spu_code = spu.spu_code,
                            spu_description = spu.spu_description,
                            spu_name = spu.spu_name,
                            bar_code = sku.bar_code,
                            unpicked_qty = d.qty - d.picked_qty,
                            length_unit = spu.length_unit,
                            volume_unit = spu.volume_unit,
                            weight_unit = spu.weight_unit,
                            pick_checker = d.pick_checker,
                            pick_checker_id = d.pick_checker_id,
                            is_todo = pageSearch.sqlTitle.Contains("dispatch_status") || (pageSearch.sqlTitle.Equals("package") && d.dispatch_status.Equals(4))
                                            || (pageSearch.sqlTitle.Equals("weight") && d.dispatch_status.Equals(5))
                                            || (pageSearch.sqlTitle.Equals("delivery") && d.dispatch_status.Equals(6)) ? false : true,
                        };
            query = query.Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                 .Where(queries.AsExpression<DispatchlistViewModel>());

            int totals = await query.CountAsync();
            var list = await query.OrderBy(t => t.is_todo == true ? 0 : 1).ThenByDescending(t => t.last_update_time)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();

            return (list, totals);
        }

        /// <summary>
        /// get dispatchlist by dispatch_no
        /// </summary>
        /// <param name="dispatch_no"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<List<DispatchlistViewModel>> GetByDispatchlistNo(string dispatch_no, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var datas = await (from d in DbSet.AsNoTracking()
                               join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on d.sku_id equals sku.id
                               join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                               where d.dispatch_no == dispatch_no && d.tenant_id == currentUser.tenant_id
                               select new DispatchlistViewModel
                               {
                                   id = d.id,
                                   dispatch_no = d.dispatch_no,
                                   dispatch_status = d.dispatch_status,
                                   customer_id = d.customer_id,
                                   customer_name = d.customer_name,
                                   sku_id = d.sku_id,
                                   qty = d.qty,
                                   weight = d.weight,
                                   volume = d.volume,
                                   creator = d.creator,
                                   create_time = d.create_time,
                                   damage_qty = d.damage_qty,
                                   lock_qty = d.lock_qty,
                                   picked_qty = d.picked_qty,
                                   intrasit_qty = d.intrasit_qty,
                                   package_qty = d.package_qty,
                                   unpackage_qty = d.picked_qty - d.package_qty,
                                   weighing_qty = d.weighing_qty,
                                   unweighing_qty = d.picked_qty - d.weighing_qty,
                                   actual_qty = d.actual_qty,
                                   sign_qty = d.sign_qty,
                                   package_no = d.package_no,
                                   package_person = d.package_person,
                                   package_time = d.package_time,
                                   weighing_no = d.weighing_no,
                                   weighing_person = d.weighing_person,
                                   weighing_weight = d.weighing_weight,
                                   waybill_no = d.waybill_no,
                                   carrier = d.carrier,
                                   freightfee = d.freightfee,
                                   last_update_time = d.last_update_time,
                                   tenant_id = d.tenant_id,
                                   sku_code = sku.sku_code,
                                   spu_code = spu.spu_code,
                                   spu_description = spu.spu_description,
                                   spu_name = spu.spu_name,
                                   bar_code = sku.bar_code,
                                   unpicked_qty = d.qty - d.picked_qty,
                                   sku_name = sku.sku_name,
                                   unit = sku.unit,
                                   pick_checker = d.pick_checker,
                                   pick_checker_id = d.pick_checker_id,
                               }).ToListAsync();
            return datas;
        }

        /// <summary>
        /// update dispatchlist with same dispatch_no
        /// </summary>
        /// <param name="viewModels"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UpdateAsycn(List<DispatchlistViewModel> viewModels, CurrentUser currentUser)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var dispatch_no = viewModels.FirstOrDefault().dispatch_no;
            var dispatch_status = viewModels.FirstOrDefault().dispatch_status;
            var entities = await (DBSet.Where(t => t.dispatch_no == dispatch_no && t.tenant_id == currentUser.tenant_id)).ToListAsync();
            var delete_id_list = new List<int>();
            var sku_id_list = viewModels.Select(t => t.sku_id).ToList();
            var skus = await (_dBContext.GetDbSet<SkuEntity>().AsNoTracking().Where(t => sku_id_list.Contains(t.id))).ToListAsync();
            var now_time = DateTime.Now;
            if (entities.Any(t => t.dispatch_status != 1 && t.dispatch_status != 0))
            {
                return (false, "[202]" + _stringLocalizer["data_changed"]);
            }
            foreach (var vm in viewModels)
            {
                if (vm.id < 0)
                {
                    var entity = entities.FirstOrDefault(t => t.id == -vm.id);
                    if (entity == null)
                    {
                        return (false, "[202]" + _stringLocalizer["data_changed"]);
                    }
                    DBSet.Remove(entity);
                    delete_id_list.Add(entity.id);
                }
                else if (vm.id > 0)
                {
                    var entity = entities.FirstOrDefault(t => t.id == vm.id);
                    if (entity == null)
                    {
                        return (false, "[202]" + _stringLocalizer["data_changed"]);
                    }
                    entity.sku_id = vm.sku_id;
                    entity.qty = vm.qty;
                    entity.last_update_time = now_time;
                    var sku = skus.FirstOrDefault(t => t.id == entity.sku_id);
                    if (sku != null)
                    {
                        entity.volume = sku.volume * entity.qty;
                        entity.weight = sku.weight * entity.qty;
                    }
                }
                else if (vm.id == 0)
                {
                    var entity = new DispatchlistEntity
                    {
                        id = 0,
                        dispatch_no = dispatch_no,
                        creator = currentUser.user_name,
                        create_time = now_time,
                        last_update_time = now_time,
                        dispatch_status = dispatch_status,
                        sku_id = vm.sku_id,
                        qty = vm.qty
                    };
                    var sku = skus.FirstOrDefault(t => t.id == entity.sku_id);
                    if (sku != null)
                    {
                        entity.volume = sku.volume * entity.qty;
                        entity.weight = sku.weight * entity.qty;
                    }
                    entities.Add(entity);
                    DBSet.Add(entity);
                }
            }
            var repeat_skus_id_list = entities.Where(t => !delete_id_list.Contains(t.id)).GroupBy(t => t.sku_id).Select(t => new { t.Key, cnt = t.Count() }).Where(t => t.cnt > 1).Select(t => t.Key).ToList();
            if (repeat_skus_id_list.Count > 0)
            {
                var repeat_skus = (skus.Where(t => repeat_skus_id_list.Contains(t.id)).Select(t => t.sku_code).ToList());
                var msg = "";
                foreach (var sku in repeat_skus)
                {
                    msg += string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["sku_code"], sku);
                }
                return (false, msg);
            }
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
        /// get pick list by dispatch_id
        /// </summary>
        /// <param name="dispatch_id">dispatch_id</param>
        /// <returns></returns>
        public async Task<List<DispatchpicklistViewModel>> GetPickListByDispatchID(int dispatch_id)
        {
            var datas = await (from dpl in _dBContext.GetDbSet<DispatchpicklistEntity>().AsNoTracking()
                               join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on dpl.sku_id equals sku.id
                               join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                               join owner in _dBContext.GetDbSet<GoodsownerEntity>().AsNoTracking() on dpl.goods_owner_id equals owner.id into o_left
                               from owner in o_left.DefaultIfEmpty()
                               join location in _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking() on dpl.goods_location_id equals location.id
                               where dpl.dispatchlist_id == dispatch_id
                               select new DispatchpicklistViewModel
                               {
                                   id = dpl.id,
                                   dispatchlist_id = dpl.dispatchlist_id,
                                   goods_owner_id = dpl.goods_owner_id,
                                   goods_location_id = dpl.goods_location_id,
                                   sku_id = dpl.sku_id,
                                   pick_qty = dpl.pick_qty,
                                   picked_qty = dpl.picked_qty,
                                   goods_owner_name = owner.goods_owner_name == null ? "" : owner.goods_owner_name,
                                   sku_code = sku.sku_code,
                                   spu_code = spu.spu_code,
                                   spu_description = spu.spu_description,
                                   spu_name = spu.spu_name,
                                   bar_code = sku.bar_code,
                                   location_name = location.location_name,
                                   warehouse_area_name = location.warehouse_area_name,
                                   warehouse_area_property = location.warehouse_area_property,
                                   warehouse_name = location.warehouse_name,
                                   series_number = dpl.series_number,
                                   expiry_date = dpl.expiry_date,
                                   price = dpl.price,
                                   picker = dpl.picker,
                                   picker_id = dpl.picker_id,
                                   putaway_date = dpl.putaway_date,
                               }).ToListAsync();
            return datas;
        }

        /// <summary>
        /// advanced dispatch order page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<PreDispatchlistViewModel> data, int totals)> AdvancedDispatchlistPageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var DbSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var query = from d in DbSet.AsNoTracking().Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                            //join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on d.sku_id equals sku.id
                            //join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                        group new { d/*, spu*/ } by new { d.dispatch_no, d.dispatch_status, d.customer_id, d.customer_name, d.creator }
                        into dg
                        select new PreDispatchlistViewModel
                        {
                            dispatch_no = dg.Key.dispatch_no,
                            dispatch_status = dg.Key.dispatch_status,
                            customer_id = dg.Key.customer_id,
                            customer_name = dg.Key.customer_name,
                            qty = dg.Sum(t => t.d.qty),
                            creator = dg.Key.creator,
                            /*dispatch_no = dg.Key.dispatch_no,
                            dispatch_status = dg.Key.dispatch_status,
                            customer_id = dg.Key.customer_id,
                            customer_name = dg.Key.customer_name,
                            qty = dg.Sum(t => t.d.qty),
                            volume = dg.Sum(t =>t.spu.volume_unit==1?  t.d.volume:(t.spu.volume_unit==0?t.d.volume/1000:t.d.volume*1000)),
                            weight = dg.Sum(t =>t.spu.weight_unit==0?t.d.weight/1000000:(t.spu.weight_unit==1? t.d.weight/1000:t.d.weight)),
                            creator = dg.Key.creator,*/
                        };
            query = query.Where(queries.AsExpression<PreDispatchlistViewModel>());
            if (pageSearch.sqlTitle.Contains("dispatch_status"))
            {
                var dispatch_status = Convert.ToByte(pageSearch.sqlTitle.Trim().ToLower().Replace("dispatch_status", "").Replace("：", "").Replace(":", "").Replace("=", ""));
                query = query.Where(t => t.dispatch_status.Equals(dispatch_status));
            }
            else if (pageSearch.sqlTitle.Equals("todo"))
            {
                query = query.Where(t => t.dispatch_status >= 2 && t.dispatch_status <= 5);
            }
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.dispatch_no)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();

            #region sqlite cannot sum data of decimal type

            var dispatch_no_list = list.Select(t => t.dispatch_no).Distinct().ToList();
            var d_datas = await (from d in DbSet.AsNoTracking()
                                 join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on d.sku_id equals sku.id
                                 join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                                 where d.tenant_id == currentUser.tenant_id && dispatch_no_list.Contains(d.dispatch_no)
                                 select new
                                 {
                                     d.dispatch_no,
                                     volume = spu.volume_unit == 1 ? d.volume : (spu.volume_unit == 0 ? d.volume / 1000 : d.volume * 1000),
                                     weight = spu.weight_unit == 0 ? d.weight / 1000000 : (spu.weight_unit == 1 ? d.weight / 1000 : d.weight)
                                 }).ToListAsync();
            list.ForEach(t =>
            {
                t.volume = d_datas.Where(d => d.dispatch_no == t.dispatch_no).Sum(t => t.volume);
                t.weight = d_datas.Where(d => d.dispatch_no == t.dispatch_no).Sum(t => t.weight);
            });

            #endregion sqlite cannot sum data of decimal type

            return (list, totals);
        }

        /// <summary>
        /// Get dispatchlist by dispatch_no
        /// </summary>
        /// <returns></returns>
        public async Task<List<DispatchlistDetailViewModel>> GetAllAsync(string dispatch_no, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var Spus = _dBContext.GetDbSet<SpuEntity>();
            var Skus = _dBContext.GetDbSet<SkuEntity>();
            var data = await (from dl in DbSet.AsNoTracking()
                              join sku in Skus.AsNoTracking() on dl.sku_id equals sku.id
                              join spu in Spus.AsNoTracking() on sku.spu_id equals spu.id
                              where dl.dispatch_no == dispatch_no && dl.tenant_id == currentUser.tenant_id
                              select new DispatchlistDetailViewModel
                              {
                                  id = dl.id,
                                  dispatch_no = dl.dispatch_no,
                                  sku_code = sku.sku_code,
                                  spu_code = spu.spu_code,
                                  spu_description = spu.spu_description,
                                  spu_name = spu.spu_name,
                                  bar_code = sku.bar_code,
                                  dispatch_status = dl.dispatch_status,
                                  customer_id = dl.customer_id,
                                  customer_name = dl.customer_name,
                                  sku_id = dl.sku_id,
                                  qty = dl.qty,
                                  weight = dl.weight,
                                  volume = dl.volume,
                                  creator = dl.creator,
                                  create_time = dl.create_time,
                                  damage_qty = dl.damage_qty,
                                  lock_qty = dl.lock_qty,
                                  picked_qty = dl.picked_qty,
                                  intrasit_qty = dl.intrasit_qty,
                                  package_qty = dl.package_qty,
                                  weighing_qty = dl.weighing_qty,
                                  actual_qty = dl.actual_qty,
                                  sign_qty = dl.sign_qty,
                                  package_no = dl.package_no,
                                  package_person = dl.package_person,
                                  package_time = dl.package_time,
                                  weighing_no = dl.weighing_no,
                                  weighing_person = dl.weighing_person,
                                  weighing_weight = dl.weighing_weight,
                                  waybill_no = dl.waybill_no,
                                  carrier = dl.carrier,
                                  freightfee = dl.freightfee,
                                  pick_checker = dl.pick_checker,
                                  pick_checker_id = dl.pick_checker_id,
                              }
                              ).ToListAsync();
            return data.Adapt<List<DispatchlistDetailViewModel>>();
        }

        /// <summary>
        /// add a new Dispatchlist
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> AddAsync(List<DispatchlistAddViewModel> viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var entities = viewModel.Adapt<List<DispatchlistEntity>>();
            var sku_id_list = entities.Select(t => t.sku_id).ToList();
            var skus = await _dBContext.GetDbSet<SkuEntity>().Where(t => sku_id_list.Contains(t.id)).ToListAsync();
            var dispatch_no = await _functionHelper.GetFormNoAsync("Dispatchlist");
            var now_time = DateTime.Now;
            foreach (var entity in entities)
            {
                var sku = skus.FirstOrDefault(t => t.id == entity.sku_id);
                entity.id = 0;
                entity.create_time = now_time;
                entity.creator = currentUser.user_name;
                entity.last_update_time = now_time;
                entity.tenant_id = currentUser.tenant_id;
                if (sku != null)
                {
                    entity.volume = entity.qty * sku.volume;
                    entity.weight = entity.qty * sku.weight;
                }
                entity.dispatch_no = dispatch_no;
            }
            await DbSet.AddRangeAsync(entities);
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
        /// <param name="dispatch_no">dispatch_no</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> DeleteAsync(string dispatch_no, CurrentUser currentUser)
        {
            var entities = await _dBContext.GetDbSet<DispatchlistEntity>().Where(t => t.dispatch_no.Equals(dispatch_no) && t.tenant_id == currentUser.tenant_id).ToListAsync();
            if (entities.Any(t => t.dispatch_status > 1))
            {
                return (false, _stringLocalizer["status_not_delete"]);
            }
            _dBContext.GetDbSet<DispatchlistEntity>().RemoveRange(entities);
            var qty = await _dBContext.SaveChangesAsync();
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
        /// Dispatchlist details with available stock
        /// </summary>
        /// <param name="dispatch_no">dispatch_no</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<List<DispatchlistConfirmDetailViewModel>> ConfirmOrderCheck(string dispatch_no, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var dispatchpick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var stock_DbSet = _dBContext.GetDbSet<StockEntity>();
            var asn_DBSet = _dBContext.GetDbSet<AsnEntity>();
            var dispatch_DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var sku_DBSet = _dBContext.GetDbSet<SkuEntity>().AsNoTracking();
            var spu_DBSet = _dBContext.GetDbSet<SpuEntity>().AsNoTracking();
            var processdetail_DBSet = _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking();
            var move_DBSet = _dBContext.GetDbSet<StockmoveEntity>();
            var owner_DBSet = _dBContext.GetDbSet<GoodsownerEntity>();
            var location_DBSet = _dBContext.GetDbSet<GoodslocationEntity>();
            var stock_group_datas = from stock in stock_DbSet.AsNoTracking()
                                    join gl in _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking() on stock.goods_location_id equals gl.id
                                    where stock.tenant_id == currentUser.user_id
                                    group stock by new { stock.id, stock.sku_id, stock.goods_location_id, stock.goods_owner_id, stock.series_number, stock.expiry_date, stock.price,stock.putaway_date } into sg
                                    select new
                                    {
                                        stock_id = sg.Key.id,
                                        goods_owner_id = sg.Key.goods_owner_id,
                                        sku_id = sg.Key.sku_id,
                                        goods_location_id = sg.Key.goods_location_id,
                                        series_number = sg.Key.series_number,
                                        sg.Key.expiry_date,
                                        sg.Key.price,
                                        sg.Key.putaway_date,
                                        qty_frozen = sg.Where(t => t.is_freeze == true).Sum(e => e.qty),
                                        qty = sg.Sum(t => t.qty)
                                    };
            var dispatch_group_datas = from dp in DbSet.AsNoTracking()
                                       join dpp in dispatchpick_DBSet.AsNoTracking() on dp.id equals dpp.dispatchlist_id
                                       where dp.dispatch_status > 1 && dp.dispatch_status < 6
                                       group dpp by new { dpp.sku_id, dpp.goods_location_id, dpp.goods_owner_id, dpp.series_number, dpp.expiry_date, dpp.price,dpp.putaway_date } into dg
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
                                             where pd.is_update_stock == false
                                             group pd by new { pd.sku_id, pd.goods_location_id, pd.goods_owner_id, pd.series_number, pd.expiry_date, pd.price,pd.putaway_date } into pdg
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
                                          group m by new { m.sku_id, m.orig_goods_location_id, m.goods_owner_id, m.series_number, m.expiry_date, m.price,m.putaway_date } into mg
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
            var datas = await (from dl in DbSet
                               join sg in stock_group_datas on dl.sku_id equals sg.sku_id into sg_left
                               from sg in sg_left.DefaultIfEmpty()
                               join dp in dispatch_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price,sg.putaway_date } equals new { dp.sku_id, dp.goods_location_id, dp.goods_owner_id, dp.series_number, dp.expiry_date, dp.price,dp.putaway_date } into dp_left
                               from dp in dp_left.DefaultIfEmpty()
                               join pl in process_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price,sg.putaway_date } equals new { pl.sku_id, pl.goods_location_id, pl.goods_owner_id, pl.series_number, pl.expiry_date, pl.price,pl.putaway_date } into pl_left
                               from pl in pl_left.DefaultIfEmpty()
                               join m in move_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price,sg.putaway_date } equals new { m.sku_id, m.goods_location_id, m.goods_owner_id, m.series_number, m.expiry_date, m.price,m.putaway_date } into m_left
                               from m in m_left.DefaultIfEmpty()
                               join sku in sku_DBSet on dl.sku_id equals sku.id
                               join spu in spu_DBSet on sku.spu_id equals spu.id
                               join owner in owner_DBSet.AsNoTracking() on sg.goods_owner_id equals owner.id into owner_left
                               from owner in owner_left.DefaultIfEmpty()
                               join gl in location_DBSet.Where(t => t.warehouse_area_property != 5).AsNoTracking() on sg.goods_location_id equals gl.id into gl_left
                               from gl in gl_left.DefaultIfEmpty()
                               where dl.dispatch_no == dispatch_no && dl.tenant_id == currentUser.tenant_id && (dl.dispatch_status == 0 || dl.dispatch_status == 1)
                               select new
                               {
                                   stock_id = sg.stock_id == null ? 0 : sg.stock_id,
                                   goods_owner_name = owner.goods_owner_name == null ? "" : owner.goods_owner_name,
                                   goods_location_id = sg.goods_location_id == null ? 0 : sg.goods_location_id,
                                   goods_owner_id = sg.goods_owner_id == null ? 0 : sg.goods_owner_id,
                                   spu_name = spu.spu_name,
                                   spu_code = spu.spu_code,
                                   sku_code = sku.sku_code,
                                   qty_available = (sg.qty == null ? 0 : sg.qty) - (sg.qty_frozen == null ? 0 : sg.qty_frozen) - (dp.qty_locked == null ? 0 : dp.qty_locked) - (pl.qty_locked == null ? 0 : pl.qty_locked) - (m.qty_locked == null ? 0 : m.qty_locked),
                                   qty = dl.qty,
                                   sku_id = dl.sku_id,
                                   id = dl.id,
                                   spu_description = spu.spu_description,
                                   dispatch_status = dl.dispatch_status,
                                   bar_code = sku.bar_code,
                                   customer_id = dl.customer_id,
                                   customer_name = dl.customer_name,
                                   dispatch_no = dl.dispatch_no,
                                   location_name = gl.location_name == null ? "" : gl.location_name,
                                   warehouse_area_name = gl.warehouse_area_name == null ? "" : gl.warehouse_area_name,
                                   warehouse_name = gl.warehouse_name == null ? "" : gl.warehouse_name,
                                   series_number = sg.series_number==null ? "":sg.series_number,
                                   expiry_date = sg.expiry_date==null ? UtilConvert.MinDate : sg.expiry_date,
                                   price = sg.price == null ? 0:sg.price,
                                    putaway_date = sg.putaway_date == null ? UtilConvert.MinDate:sg.putaway_date,
                               }).ToListAsync();
            var res = (from d in datas
                       group d by new
                       {
                           d.spu_name,
                           d.spu_code,
                           d.sku_code,
                           d.qty,
                           d.sku_id,
                           d.id,
                           d.spu_description,
                           d.dispatch_status,
                           d.bar_code,
                           d.customer_id,
                           d.customer_name,
                           d.dispatch_no,
                       }
                       into dg
                       select new DispatchlistConfirmDetailViewModel
                       {
                           dispatchlist_id = dg.Key.id,
                           sku_id = dg.Key.sku_id,
                           dispatch_no = dg.Key.dispatch_no,
                           sku_code = dg.Key.sku_code,
                           spu_code = dg.Key.spu_code,
                           dispatch_status = dg.Key.dispatch_status,
                           spu_description = dg.Key.spu_description,
                           spu_name = dg.Key.spu_name,
                           bar_code = dg.Key.bar_code,
                           customer_id = dg.Key.customer_id,
                           customer_name = dg.Key.customer_name,
                           qty = dg.Key.qty,
                           qty_available = dg.Sum(t => t.qty_available),
                           confirm = dg.Key.qty > dg.Sum(t => t.qty_available) ? false : true
                       }).ToList();
            foreach (var r in res)
            {
                var picklist = (from d in datas.Where(t => t.sku_id == r.sku_id && t.stock_id > 0).OrderBy(o => o.qty_available)
                                select new DispatchlistConfirmPickDetailViewModel
                                {
                                    stock_id = d.stock_id,
                                    dispatchlist_id = r.dispatchlist_id,
                                    goods_location_id = d.goods_location_id,
                                    qty_available = d.qty_available,
                                    goods_owner_id = d.goods_owner_id,
                                    goods_owner_name = d.goods_owner_name,
                                    location_name = d.location_name,
                                    warehouse_area_name = d.warehouse_area_name,
                                    warehouse_name = d.warehouse_name,
                                    pick_qty = 0,
                                    series_number = d.series_number,
                                    expiry_date = d.expiry_date,
                                    price = d.price,
                                    putaway_date=d.putaway_date,
                                }
                              ).OrderByDescending(o => o.qty_available).ToList();
                int pick_qty = 0;
                foreach (var pick in picklist)
                {
                    if (pick_qty >= r.qty)
                    {
                        break;
                    }
                    pick.pick_qty = (r.qty <= (pick_qty + pick.qty_available)) ? (r.qty - pick_qty) : pick.qty_available;
                    pick_qty += pick.pick_qty;
                }
                r.pick_list = picklist.Where(t => t.qty_available > 0).ToList();
            }

            return res;
        }

        /// <summary>
        ///  Confirm orders and create  dispatchpicklist
        /// </summary>
        /// <param name="viewModels">viewModels</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ConfirmOrder(List<DispatchlistConfirmDetailViewModel> viewModels, CurrentUser currentUser)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var dispatchlist_id_list = viewModels.Select(t => t.dispatchlist_id).ToList();
            var dispatchlist_datas = await DBSet.Where(t => dispatchlist_id_list.Contains(t.id)).ToListAsync();
            var pick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var stock_DBSet = _dBContext.GetDbSet<StockEntity>();
            var pick_datas = new List<DispatchpicklistEntity>();
            var stock_id_list = new List<int>();
            var processdetail_DBSet = _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking();
            var move_DBSet = _dBContext.GetDbSet<StockmoveEntity>();
            var new_dispatchlists = new List<DispatchlistEntity>();
            var topick_viewmodels = new List<StockViewModel>();
            var sku_id_list = viewModels.Select(t => t.sku_id).ToList();
            var now_time = DateTime.Now;
            foreach (var vm in viewModels.Where(t => t.confirm == true).ToList())
            {
                stock_id_list.AddRange(vm.pick_list.Where(t => t.pick_qty > 0).Select(t => t.stock_id).ToList());
            }

            foreach (var vm in viewModels)
            {
                var d = dispatchlist_datas.Where(t => t.id == vm.dispatchlist_id).FirstOrDefault();
                if (d == null)
                {
                    return (false, "[202]" + _stringLocalizer["data_changed"]);
                }
                if (vm.confirm == true)
                {
                    d.dispatch_status = 2;
                    d.last_update_time = now_time;
                    d.lock_qty = vm.pick_list.Sum(t => t.pick_qty);
                    foreach (var p in vm.pick_list.Where(t => t.pick_qty > 0).ToList())
                    {
                        pick_datas.Add(new DispatchpicklistEntity
                        {
                            sku_id = vm.sku_id,
                            is_update_stock = false,
                            dispatchlist_id = p.dispatchlist_id,
                            goods_location_id = p.goods_location_id,
                            goods_owner_id = p.goods_owner_id,
                            last_update_time = now_time,
                            series_number = p.series_number,
                            expiry_date = p.expiry_date,
                            price = p.price,
                            pick_qty = p.pick_qty,
                            putaway_date = p.putaway_date,
                        });
                        topick_viewmodels.Add(new StockViewModel { id = p.stock_id, qty = p.pick_qty });
                    }
                    if (d.lock_qty < d.qty)
                    {
                        new_dispatchlists.Add(new DispatchlistEntity
                        {
                            sku_id = vm.sku_id,
                            dispatch_status = 1,
                            qty = d.qty - d.lock_qty,
                            tenant_id = currentUser.tenant_id,
                            customer_id = d.customer_id,
                            customer_name = d.customer_name,
                        });
                        d.qty = d.lock_qty;
                    }
                }
                else
                {
                    new_dispatchlists.Add(new DispatchlistEntity
                    {
                        sku_id = vm.sku_id,
                        dispatch_status = 1,
                        qty = vm.qty,
                        tenant_id = currentUser.tenant_id,
                        customer_id = d.customer_id,
                        customer_name = d.customer_name,
                    });
                    DBSet.Remove(d);
                }
            }
            var stock_group_datas = from stock in stock_DBSet.AsNoTracking()
                                    where stock_id_list.Contains(stock.id)
                                    group stock by new { stock.id, stock.sku_id, stock.goods_location_id, stock.goods_owner_id, stock.series_number, stock.expiry_date, stock.price,stock.putaway_date } into sg
                                    select new
                                    {
                                        stock_id = sg.Key.id,
                                        goods_owner_id = sg.Key.goods_owner_id,
                                        sku_id = sg.Key.sku_id,
                                        goods_location_id = sg.Key.goods_location_id,
                                        series_number = sg.Key.series_number,
                                        sg.Key.expiry_date,
                                        sg.Key.price,
                                        sg.Key.putaway_date,
                                        qty_frozen = sg.Where(t => t.is_freeze == true).Sum(e => e.qty),
                                        qty = sg.Sum(t => t.qty)
                                    };
            var dispatch_group_datas = from dp in DBSet.AsNoTracking()
                                       join dpp in pick_DBSet.AsNoTracking() on dp.id equals dpp.dispatchlist_id
                                       where dp.dispatch_status > 1 && dp.dispatch_status < 6
                                       group dpp by new { dpp.sku_id, dpp.goods_location_id, dpp.goods_owner_id, dpp.series_number, dpp.expiry_date, dpp.price,dpp.putaway_date } into dg
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
                                             where pd.is_update_stock == false
                                             group pd by new { pd.sku_id, pd.goods_location_id, pd.goods_owner_id, pd.series_number, pd.expiry_date, pd.price,pd.putaway_date } into pdg
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
                                          group m by new { m.sku_id, m.orig_goods_location_id, m.goods_owner_id, m.series_number, m.expiry_date, m.price,m.putaway_date } into mg
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
            var stock_datas = await (from sg in stock_group_datas
                                     join dp in dispatch_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price,sg.putaway_date } equals new { dp.sku_id, dp.goods_location_id, dp.goods_owner_id, dp.series_number, dp.expiry_date, dp.price,dp.putaway_date } into dp_left
                                     from dp in dp_left.DefaultIfEmpty()
                                     join pl in process_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price,sg.putaway_date } equals new { pl.sku_id, pl.goods_location_id, pl.goods_owner_id, pl.series_number, pl.expiry_date, pl.price,pl.putaway_date } into pl_left
                                     from pl in pl_left.DefaultIfEmpty()
                                     join m in move_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number, sg.expiry_date, sg.price,sg.putaway_date } equals new { m.sku_id, m.goods_location_id, m.goods_owner_id, m.series_number, m.expiry_date, m.price,m.putaway_date } into m_left
                                     from m in m_left.DefaultIfEmpty()
                                     select new
                                     {
                                         stock_id = sg.stock_id,
                                         qty_available = (sg.qty == null ? 0 : sg.qty) - (sg.qty_frozen == null ? 0 : sg.qty_frozen) - (dp.qty_locked == null ? 0 : dp.qty_locked) - (pl.qty_locked == null ? 0 : pl.qty_locked) - (m.qty_locked == null ? 0 : m.qty_locked),
                                     }).ToListAsync();
            var if_not_stock = (from tp in topick_viewmodels
                                join s in stock_datas on tp.id equals s.stock_id
                                where tp.qty > s.qty_available
                                select tp).Any();
            if (if_not_stock)
            {
                return (false, "[202]" + _stringLocalizer["data_changed"]);
            }
            await pick_DBSet.AddRangeAsync(pick_datas);
            var dispatch_no = await _functionHelper.GetFormNoAsync("Dispatchlist");
            var sku_datas = await _dBContext.GetDbSet<SkuEntity>().Where(t => sku_id_list.Contains(t.id)).ToListAsync();
            foreach (var nd in new_dispatchlists)
            {
                nd.dispatch_no = dispatch_no;
                nd.creator = currentUser.user_name;
                nd.create_time = DateTime.Now;
                var sku = sku_datas.FirstOrDefault(e => e.id == nd.sku_id);
                if (sku != null)
                {
                    nd.weight = nd.qty * sku.weight;
                    nd.volume = nd.qty * sku.volume;
                }
            };
            await DBSet.AddRangeAsync(new_dispatchlists);
            var qty = await _dBContext.SaveChangesAsync();

            return (true, _stringLocalizer["operation_success"]);
 

        }

        /// <summary>
        ///  cancel order opration
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> CancelOrderOpration(CancelOrderOprationViewModel viewModel, CurrentUser currentUser)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var pick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var entities = await DBSet.Where(t => t.dispatch_no == viewModel.dispatch_no && t.tenant_id == currentUser.tenant_id && t.dispatch_status == viewModel.dispatch_status).ToListAsync();
            if (entities.Count == 0)
            {
                return (false, _stringLocalizer["status_changed"]);
            }
            var now_time = DateTime.Now;
            var dispatch_id_list = entities.Select(t => t.id).ToList();
            var pick_entities = await pick_DBSet.Where(t => dispatch_id_list.Contains(t.dispatchlist_id)).ToListAsync();
            if (viewModel.dispatch_status == 3)
            {
                foreach (var pick in pick_entities)
                {
                    pick.picked_qty = 0;
                    pick.last_update_time = now_time;
                }
                foreach (var entity in entities)
                {
                    entity.picked_qty = 0;
                    entity.last_update_time = now_time;
                    entity.dispatch_status = 2;
                }
            }
            else if (viewModel.dispatch_status == 2)
            {
                pick_DBSet.RemoveRange(pick_entities);
                foreach (var entity in entities)
                {
                    entity.lock_qty = 0;
                    entity.last_update_time = now_time;
                    entity.dispatch_status = 1;
                }
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
                        if (entry.Entity is DispatchlistEntity)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();
                            if (UtilConvert.ObjToInt(databaseValues["dispatch_status"]) != viewModel.dispatch_status)
                                return (false, "[202]" + _stringLocalizer["data_changed"]);
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
        /// cancel dispatchlist detail opration
        /// </summary>
        /// <param name="id">dispatchlist_id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> CancelDispatchlistDetailOpration(int id)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var entity = await DBSet.Where(t => t.id == id).FirstOrDefaultAsync();
            var now_time = DateTime.Now;
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            if (entity.dispatch_status == 4)
            {
                if (entity.weighing_no == "")
                {
                    entity.dispatch_status = 3;
                }
                else
                {
                    entity.dispatch_status = 5;
                }
                entity.package_no = "";
                entity.package_qty = 0;
                entity.package_time = UtilConvert.MinDate;
                entity.package_person = "";
            }
            else if (entity.dispatch_status == 5)
            {
                if (entity.package_no == "")
                {
                    entity.dispatch_status = 3;
                }
                else
                {
                    entity.dispatch_status = 4;
                }
                entity.weighing_no = "";
                entity.weighing_qty = 0;
                entity.weighing_weight = 0;
                entity.weighing_person = "";
            }
            else
            {
                return (false, _stringLocalizer["status_changed"]);
            }
            entity.last_update_time = now_time;
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
        /// confirm dispatchpicklist picked by dispatch_no
        /// </summary>
        /// <param name="dispatch_no">dispatch_no</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ConfirmPickByDispatchNo(string dispatch_no, CurrentUser currentUser)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var pick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var entities = await DBSet.Where(t => t.dispatch_status == 2 && t.dispatch_no == dispatch_no && t.tenant_id == currentUser.tenant_id).ToListAsync();
            var dispatchlist_id_list = entities.Select(t => t.id).ToList();
            var pick_datas = await pick_DBSet.Where(t => dispatchlist_id_list.Contains(t.dispatchlist_id)).ToListAsync();
            var now_time = DateTime.Now;
            entities.ForEach(t =>
            {
                t.picked_qty = t.lock_qty;
                t.dispatch_status = 3;
                t.last_update_time = now_time;
                t.pick_checker = currentUser.user_name;
                t.pick_checker_id = currentUser.user_id;
            });
            pick_datas.ForEach(t =>
            {
                t.picked_qty = t.pick_qty;
                t.last_update_time = now_time;
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
        /// confirm pick detail
        /// </summary>
        /// <param name="picklist_id">dispatch list pick detail id</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ConfirmPickDetail(List<int> picklist_id, CurrentUser currentUser)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var pick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var pick_datas = await pick_DBSet.Where(t => picklist_id.Contains(t.id)).ToListAsync();
            if (pick_datas.Any(t=>t.picker_id > 0) || pick_datas.Any(t=>t.picked_qty>0))
            {
                return (false, _stringLocalizer["data_changed"]);
            }
            pick_datas.ForEach(t=> 
            { 
                t.picker = currentUser.user_name;
                t.picker_id = currentUser.user_id;
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
        /// cancel confirm pick detail
        /// </summary>
        /// <param name="picklist_id">dispatch list pick detail id</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> CancelConfirmPickDetail(List<int> picklist_id, CurrentUser currentUser)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var pick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var pick_datas = await pick_DBSet.Where(t => picklist_id.Contains(t.id)  ).ToListAsync();
            if (pick_datas.Any(t =>t.picker_id == 0) || pick_datas.Any(t => t.picked_qty > 0))
            {
                return (false, _stringLocalizer["data_changed"]);
            }
            pick_datas.ForEach(t => 
            {
                t.picker = "";
                t.picker_id = 0;
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
        ///  package
        /// </summary>
        /// <param name="viewModels">viewModels</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<(bool flag, string msg)> Package(List<DispatchlistPackageViewModel> viewModels, CurrentUser currentUser)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var dispatchlist_id_list = viewModels.Select(t => t.id).ToList();
            var entities = await DBSet.Where(t => dispatchlist_id_list.Contains(t.id)).ToListAsync();
            var now_time = DateTime.Now;
            var code = GetPackageOrWeightCode();
            foreach (var vm in viewModels)
            {
                var entity = entities.FirstOrDefault(t => t.id == vm.id && t.dispatch_status == vm.dispatch_status);
                if (entity == null)
                {
                    return (false, "[202]" + _stringLocalizer["data_changed"]);
                }
                if ((entity.package_qty + vm.package_qty) > entity.picked_qty)
                {
                    return (false, "[202]" + _stringLocalizer["unpackgeqty_lessthen"]);
                }
                entity.last_update_time = now_time;
                entity.package_person = currentUser.user_name;
                entity.package_qty += vm.package_qty;
                entity.package_time = now_time;
                entity.package_no = code;
                entity.dispatch_status = 4;
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
                        if (entry.Entity is DispatchlistEntity)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();
                            var t_vm = viewModels.FirstOrDefault(t => t.id == UtilConvert.ObjToInt(databaseValues["id"]));
                            if (t_vm == null)
                            {
                                return (false, "[202]" + _stringLocalizer["data_changed"]);
                            }
                            if (UtilConvert.ObjToInt(databaseValues["package_qty"]) + t_vm.package_qty > t_vm.picked_qty)
                            {
                                return (false, "[202]" + _stringLocalizer["data_changed"]);
                            }
                            else
                            {
                                proposedValues["package_qty"] = UtilConvert.ObjToInt(databaseValues["package_qty"]) + t_vm.package_qty;
                                proposedValues["last_update_time"] = DateTime.Now;
                                if (UtilConvert.ObjToInt(databaseValues["package_qty"]) + t_vm.package_qty == UtilConvert.ObjToInt(databaseValues["picked_qty"]))
                                {
                                    proposedValues["dispatch_status"] = 4;
                                }
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
        ///  weight
        /// </summary>
        /// <param name="viewModels">viewModels</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<(bool flag, string msg)> Weight(List<DispatchlistWeightViewModel> viewModels, CurrentUser currentUser)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var dispatchlist_id_list = viewModels.Select(t => t.id).ToList();
            var entities = await DBSet.Where(t => dispatchlist_id_list.Contains(t.id)).ToListAsync();
            var now_time = DateTime.Now;
            var code = GetPackageOrWeightCode();
            foreach (var vm in viewModels)
            {
                var entity = entities.FirstOrDefault(t => t.id == vm.id && t.dispatch_status == vm.dispatch_status);
                if (entity == null)
                {
                    return (false, "[202]" + _stringLocalizer["data_changed"]);
                }
                if ((entity.weighing_qty + vm.weighing_qty) > entity.picked_qty)
                {
                    return (false, "[202]" + _stringLocalizer["unweightqty_lessthen"]);
                }
                entity.last_update_time = now_time;
                entity.weighing_person = currentUser.user_name;
                entity.weighing_qty += vm.weighing_qty;
                entity.weighing_weight += vm.weighing_weight;
                entity.weighing_no = code;
                entity.dispatch_status = 5;
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

                            var t_vm = viewModels.FirstOrDefault(t => t.id == UtilConvert.ObjToInt(databaseValues["id"]));
                            if (t_vm == null)
                            {
                                return (false, "[202]" + _stringLocalizer["data_changed"]);
                            }
                            if (UtilConvert.ObjToInt(databaseValues["weighing_qty"]) + t_vm.weighing_qty > t_vm.picked_qty)
                            {
                                return (false, "[202]" + _stringLocalizer["data_changed"]);
                            }
                            else
                            {
                                proposedValues["weighing_qty"] = UtilConvert.ObjToInt(databaseValues["weighing_qty"]) + t_vm.weighing_qty;
                                proposedValues["weighing_weight"] = UtilConvert.ObjToInt(databaseValues["weighing_weight"]) + t_vm.weighing_weight;
                                if (UtilConvert.ObjToInt(databaseValues["weighing_qty"]) + t_vm.weighing_qty == UtilConvert.ObjToInt(databaseValues["picked_qty"]))
                                {
                                    proposedValues["dispatch_status"] = 5;
                                }
                                proposedValues["last_update_time"] = now_time;
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
        /// dispatchpicklist outbound delivery
        /// </summary>
        /// <param name="viewModels">viewModels</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<(bool flag, string msg)> Delivery(List<DispatchlistDeliveryViewModel> viewModels, CurrentUser currentUser)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var dispatchlist_id_list = viewModels.Select(t => t.id).ToList();
            var pick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var stock_DBSet = _dBContext.GetDbSet<StockEntity>();
            var entities = await DBSet.Where(t => dispatchlist_id_list.Contains(t.id)).ToListAsync();
            var now_time = DateTime.Now;
            foreach (var entity in entities)
            {
                if (entity.dispatch_status != 3 && entity.dispatch_status != 4 && entity.dispatch_status != 5)
                {
                    return (false, "[202]" + _stringLocalizer["data_changed"]);
                }
                entity.last_update_time = now_time;
                entity.dispatch_status = 6;
                entity.lock_qty = 0;
                entity.actual_qty = entity.picked_qty;
                entity.intrasit_qty = entity.picked_qty;
            }
            var pick_sql = pick_DBSet.Where(t => dispatchlist_id_list.Contains(t.dispatchlist_id));
            var pick_datas = await pick_sql.ToListAsync();
            var picks_g = pick_sql.AsNoTracking().GroupBy(e => new { e.goods_location_id, e.sku_id, e.goods_owner_id, e.series_number, e.expiry_date, e.price,e.putaway_date }).Select(c => new { c.Key.goods_location_id, c.Key.sku_id, c.Key.goods_owner_id, c.Key.series_number, c.Key.expiry_date, c.Key.price,c.Key.putaway_date, picked_qty = c.Sum(t => t.picked_qty) });
            var picks = await picks_g.ToListAsync();
            var stocks = await (from stock in stock_DBSet
                                where pick_sql.Any(t => t.goods_location_id == stock.goods_location_id && t.sku_id == stock.sku_id && t.goods_owner_id == stock.goods_owner_id && t.series_number == stock.series_number && t.expiry_date == stock.expiry_date && t.price == stock.price && t.putaway_date == stock.putaway_date)
                                select stock).ToListAsync();
            foreach (var pick in picks)
            {
                var s = stocks.FirstOrDefault(t => t.goods_location_id == pick.goods_location_id && t.sku_id == pick.sku_id && t.goods_owner_id == pick.goods_owner_id && t.series_number == pick.series_number && t.expiry_date == pick.expiry_date && t.price == pick.price && t.putaway_date == pick.putaway_date);
                if (s == null)
                {
                    return (false, "[202]" + _stringLocalizer["data_changed"]);
                }
                s.qty -= pick.picked_qty;
                s.last_update_time = now_time;
                stock_DBSet.Update(s);
            }
            foreach (var pick in pick_datas)
            {
                pick.is_update_stock = true;
                pick.last_update_time = now_time;
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
                        if (entry.Entity is DispatchlistEntity)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();
                            if (UtilConvert.ObjToInt(databaseValues["dispatch_status"]) != 3 && UtilConvert.ObjToInt(databaseValues["dispatch_status"]) != 4 && UtilConvert.ObjToInt(databaseValues["dispatch_status"]) != 5)
                            {
                                return (false, "[202]" + _stringLocalizer["data_changed"]);
                            }
                            proposedValues["last_update_time"] = now_time;
                        }
                        else if (entry.Entity is StockEntity)
                        {
                            var proposedValues = entry.CurrentValues;
                            var databaseValues = entry.GetDatabaseValues();
                            var t_p = picks.FirstOrDefault(t => t.goods_location_id == UtilConvert.ObjToInt(databaseValues["goods_location_id"]) && t.sku_id == UtilConvert.ObjToInt(databaseValues["sku_id"]) && t.goods_owner_id == UtilConvert.ObjToInt(databaseValues["goods_owner_id"]));
                            if (t_p == null)
                            {
                                return (false, "[202]" + _stringLocalizer["data_changed"]);
                            }
                            proposedValues["qty"] = UtilConvert.ObjToInt(databaseValues["qty"]) - t_p.picked_qty;
                            proposedValues["last_update_time"] = now_time;
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
        ///  set dispatchlist freightfee
        /// </summary>
        /// <param name="viewModels"></param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> SetFreightfee(List<DispatchlistFreightfeeViewModel> viewModels)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var dispatchlist_id_list = viewModels.Select(t => t.id).ToList();
            var freightfee_id_list = viewModels.Select(t => t.freightfee_id).Distinct().ToList();
            var entities = await DBSet.Where(t => dispatchlist_id_list.Contains(t.id)).ToListAsync();
            var freightfees = await _dBContext.GetDbSet<FreightfeeEntity>().Where(t => freightfee_id_list.Contains(t.id)).ToListAsync();
            var now_time = DateTime.Now;
            foreach (var entity in entities)
            {
                var vm = viewModels.FirstOrDefault(t => t.id == entity.id);
                if (vm != null)
                {
                    var freightfee = freightfees.FirstOrDefault(t => t.id == vm.freightfee_id);
                    if (freightfee != null)
                    {
                        entity.last_update_time = now_time;
                        entity.carrier = freightfee.carrier;
                        entity.waybill_no = vm.waybill_no;
                        if (entity.weighing_no != "")
                        {
                            entity.freightfee = entity.weighing_weight * freightfee.price_per_weight > freightfee.min_payment ? entity.weighing_weight * freightfee.price_per_weight : freightfee.min_payment;
                        }
                        else
                        {
                            entity.freightfee = Math.Max(Math.Max(entity.weight * freightfee.price_per_weight, entity.volume * freightfee.price_per_volume), freightfee.min_payment);
                        }
                    }
                }
            }
            var res = await _dBContext.SaveChangesAsync();
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
        /// sign for arrival
        /// </summary>
        /// <param name="viewModels">viewModels</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> SignForArrival(List<DispatchlistSignViewModel> viewModels)
        {
            var DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var dispatchlist_id_list = viewModels.Select(t => t.id).ToList();
            var entities = await DBSet.Where(t => dispatchlist_id_list.Contains(t.id)).ToListAsync();
            var now_time = DateTime.Now;
            foreach (var entity in entities)
            {
                var vm = viewModels.FirstOrDefault(t => t.id == t.id && t.dispatch_status == entity.dispatch_status);
                if (vm == null)
                {
                    return (false, "[202]" + _stringLocalizer["data_changed"]);
                }
                entity.sign_qty = entity.actual_qty - vm.damage_qty;
                entity.damage_qty = vm.damage_qty;
                entity.last_update_time = now_time;
                entity.dispatch_status = 7;
            }
            var res = await _dBContext.SaveChangesAsync();
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
        /// get next order code number
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOrderCode(CurrentUser currentUser)
        {
            string code;
            string date = DateTime.Now.ToString("yyyy" + "MM" + "dd");
            string maxNo = await _dBContext.GetDbSet<DispatchlistEntity>().Where(t => t.tenant_id == currentUser.tenant_id).MaxAsync(t => t.dispatch_no);
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

        /// <summary>
        /// Excel Import
        /// </summary>
        /// <param name="viewModels">viewModels</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> Import(List<DispatchlistImportViewModel> viewModels, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var import_sku_code = viewModels.Select(e => e.sku_code).ToList();
            var import_customer_name = viewModels.Select(e => e.customer_name).ToList();
            var sku_list = await (from sku in _dBContext.GetDbSet<SkuEntity>()
                                  join spu in _dBContext.GetDbSet<SpuEntity>() on sku.spu_id equals spu.id
                                  where spu.tenant_id == currentUser.tenant_id && import_sku_code.Contains(sku.sku_code)
                                  select sku).ToListAsync();
            var customer_list = await _dBContext.GetDbSet<CustomerEntity>().Where(t => t.tenant_id == currentUser.tenant_id && import_customer_name.Contains(t.customer_name)).ToListAsync();
            var entities = new List<DispatchlistEntity>();
            var groups = viewModels.Select(t => t.import_group).Distinct().ToList();
            var groups_code = await _functionHelper.GetFormNoListAsync("Dispatchlist", groups.Count);
            var group_code_dic = new Dictionary<int, string>();
            var now_time = DateTime.Now;
            for (int i = 0; i < groups.Count(); i++)
            {
                group_code_dic.Add(groups[i], groups_code[i]);
            }
            foreach (var vm in viewModels)
            {
                var customer = customer_list.FirstOrDefault(t => t.customer_name == vm.customer_name);
                if (customer == null)
                {
                    return (false, _stringLocalizer["customer_name"] + ":" + vm.customer_name + " " + _stringLocalizer["not_exists_entity"]);
                }
                var sku = sku_list.FirstOrDefault(t => t.sku_code == vm.sku_code);
                if (sku == null)
                {
                    return (false, _stringLocalizer["sku_name"] + ":" + vm.sku_name + "-" + _stringLocalizer["sku_code"] + ":" + vm.sku_code + " " + _stringLocalizer["not_exists_entity"]);
                }
                entities.Add(new DispatchlistEntity
                {
                    customer_id = customer.id,
                    customer_name = vm.customer_name,
                    sku_id = sku.id,
                    qty = vm.qty,
                    creator = currentUser.user_name,
                    create_time = now_time,
                    last_update_time = now_time,
                    tenant_id = currentUser.tenant_id,
                    dispatch_no = group_code_dic[vm.import_group],
                });
            }
            await DbSet.AddRangeAsync(entities);
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
        /// get next order code number
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetOrderCodeList(CurrentUser currentUser, int cnt)
        {
            List<string> code = new List<string>();
            string date = DateTime.Now.ToString("yyyy" + "MM" + "dd");
            string maxNo = await _dBContext.GetDbSet<DispatchlistEntity>().Where(t => t.tenant_id == currentUser.tenant_id).MaxAsync(t => t.dispatch_no);
            if (maxNo == null)
            {
                for (int i = 1; i <= cnt; i++)
                {
                    code.Add(date + "-" + cnt.ToString("0000"));
                }
            }
            else
            {
                string maxDate = maxNo.Substring(0, 8);
                string maxDateNo = maxNo.Substring(9, 4);
                if (date == maxDate)
                {
                    int.TryParse(maxDateNo, out int dd);
                    for (int i = 1; i <= cnt; i++)
                    {
                        code.Add(date + "-" + (dd + cnt).ToString("0000"));
                    }
                }
                else
                {
                    for (int i = 1; i <= cnt; i++)
                    {
                        code.Add(date + "-" + cnt.ToString("0000"));
                    }
                }
            }

            return code;
        }

        /// <summary>
        /// get package or weight  code
        /// </summary>
        /// <returns></returns>
        public string GetPackageOrWeightCode()
        {
            string date = DateTime.Now.ToString("yyyy" + "MM" + "dd");
            DateTime _dtStart = new DateTime(1970, 1, 1, 8, 0, 0);
            long timeStamp = Convert.ToInt32(DateTime.Now.Subtract(_dtStart).TotalSeconds);
            return date + timeStamp.ToString();
        }

        /*public async Task<bool> AllocateInventory(DateTime TimeBegin, DateTime TimeEnd, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var dispatchpick_DBSet = _dBContext.GetDbSet<DispatchpicklistEntity>();
            var to_pick_query = DbSet.Where(t => t.tenant_id == currentUser.tenant_id && t.create_time >= TimeBegin && t.create_time <= TimeEnd && t.dispatch_status == 1);
            var to_pick_list = await dispatchpick_DBSet.Where(t => to_pick_query.Any(e => e.id == t.dispatchlist_id)).OrderBy(t => t.id).ToListAsync();
            var to_pick = await to_pick_query.ToListAsync();
            var pick_sku = to_pick.Select(t => t.sku_id).ToList();

            var stock_DbSet = _dBContext.GetDbSet<StockEntity>();
            var asn_DBSet = _dBContext.GetDbSet<AsnEntity>();
            var dispatch_DBSet = _dBContext.GetDbSet<DispatchlistEntity>();
            var sku_DBSet = _dBContext.GetDbSet<SkuEntity>().AsNoTracking();
            var spu_DBSet = _dBContext.GetDbSet<SpuEntity>().AsNoTracking();
            var processdetail_DBSet = _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking();
            var move_DBSet = _dBContext.GetDbSet<StockmoveEntity>();
            var owner_DBSet = _dBContext.GetDbSet<GoodsownerEntity>();
            var location_DBSet = _dBContext.GetDbSet<GoodslocationEntity>();
            var stock_group_datas = stock_DbSet.Where(t => t.tenant_id == currentUser.tenant_id && pick_sku.Any(s => s == t.sku_id));

            var dispatch_group_datas = from dp in DbSet.AsNoTracking()
                                       join dpp in dispatchpick_DBSet.AsNoTracking() on dp.id equals dpp.dispatchlist_id
                                       where dp.dispatch_status > 1 && dp.dispatch_status < 6 && dp.tenant_id == currentUser.tenant_id
                                       group dpp by new { dpp.sku_id, dpp.goods_location_id, dpp.goods_owner_id, dpp.series_number } into dg
                                       select new
                                       {
                                           goods_owner_id = dg.Key.goods_owner_id,
                                           sku_id = dg.Key.sku_id,
                                           goods_location_id = dg.Key.goods_location_id,
                                           series_number = dg.Key.series_number,
                                           qty_locked = dg.Sum(t => t.pick_qty)
                                       };
            var process_locked_group_datas = from pd in processdetail_DBSet
                                             where pd.is_update_stock == false && pd.tenant_id == currentUser.tenant_id
                                             group pd by new { pd.sku_id, pd.goods_location_id, pd.goods_owner_id, pd.series_number } into pdg
                                             select new
                                             {
                                                 goods_owner_id = pdg.Key.goods_owner_id,
                                                 sku_id = pdg.Key.sku_id,
                                                 goods_location_id = pdg.Key.goods_location_id,
                                                 series_number = pdg.Key.series_number,
                                                 qty_locked = pdg.Sum(t => t.qty)
                                             };
            var move_locked_group_datas = from m in move_DBSet.AsNoTracking()
                                          where m.move_status == 0 && m.tenant_id == currentUser.tenant_id
                                          group m by new { m.sku_id, m.orig_goods_location_id, m.goods_owner_id, m.series_number } into mg
                                          select new
                                          {
                                              goods_owner_id = mg.Key.goods_owner_id,
                                              sku_id = mg.Key.sku_id,
                                              goods_location_id = mg.Key.orig_goods_location_id,
                                              series_number = mg.Key.series_number,
                                              qty_locked = mg.Sum(t => t.qty)
                                          };
            var datas = await (from sg in stock_group_datas
                               join dp in dispatch_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number } equals new { dp.sku_id, dp.goods_location_id, dp.goods_owner_id, dp.series_number } into dp_left
                               from dp in dp_left.DefaultIfEmpty()
                               join pl in process_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number } equals new { pl.sku_id, pl.goods_location_id, pl.goods_owner_id, pl.series_number } into pl_left
                               from pl in pl_left.DefaultIfEmpty()
                               join m in move_locked_group_datas on new { sg.sku_id, sg.goods_location_id, sg.goods_owner_id, sg.series_number } equals new { m.sku_id, m.goods_location_id, m.goods_owner_id, m.series_number } into m_left
                               from m in m_left.DefaultIfEmpty()
                               where pick_sku.Contains(sg.sku_id)
                               select new
                               {
                                   stock_id = sg.stock_id,
                                   goods_location_id = sg.goods_location_id,
                                   goods_owner_id = sg.goods_owner_id,
                                   qty_available = sg.qty - sg.qty_frozen - (dp.qty_locked == null ? 0 : dp.qty_locked) - (pl.qty_locked == null ? 0 : pl.qty_locked) - (m.qty_locked == null ? 0 : m.qty_locked),
                                   sku_id = sg.sku_id,
                                   series_number = sg.series_number
                               }).ToListAsync();
            foreach (var p in to_pick_list)
            {
                var picklist = datas.Where(t => t.sku_id == p.sku_id && t.stock_id > 0).OrderBy(o => o.qty_available).OrderByDescending(o => o.qty_available).ToList();
                int pick_qty = 0;
                foreach (var pick in picklist)
                {
                    if (pick_qty >= d.qty)
                    {
                        break;
                    }
                    pick.pick_qty = (r.qty <= (pick_qty + pick.qty_available)) ? (r.qty - pick_qty) : pick.qty_available;
                    pick_qty += pick.pick_qty;
                }
                r.pick_list = picklist.Where(t => t.qty_available > 0).ToList();
            }
        }*/

        #endregion Api
    }
}