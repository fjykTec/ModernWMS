/*
 * date：2022-12-23
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
using System.Linq.Expressions;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Linq;
using ModernWMS.Core;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  Stockprocess Service
    /// </summary>
    public class StockprocessService : BaseService<StockprocessEntity>, IStockprocessService
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
        ///Stockprocess  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public StockprocessService(
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
        public async Task<(List<StockprocessGetViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var DbSet = _dBContext.GetDbSet<StockprocessEntity>();
            var adjust_DBSet = _dBContext.GetDbSet<StockadjustEntity>().AsNoTracking();
            var adjusted = from a in adjust_DBSet
                           join d in _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking() on a.source_table_id equals d.id
                           where a.job_type == 2
                           group d by d.stock_process_id into ag
                           select new
                           {
                               stockprocess_id = ag.Key
                           };
            var query = from m in DbSet.AsNoTracking()
                        join a in adjusted on m.id equals a.stockprocess_id into a_left
                        from a in a_left.DefaultIfEmpty()
                        select new StockprocessGetViewModel
                        {
                            id = m.id,
                            job_code = m.job_code,
                            job_type = m.job_type,
                            process_status = m.process_status,
                            processor = m.processor,
                            process_time = m.process_time,
                            creator = m.creator,
                            create_time = m.create_time,
                            last_update_time = m.last_update_time,
                            tenant_id = m.tenant_id,
                            adjust_status = (m.process_status && (a.stockprocess_id == null ? false : true)) ? true : false,
                        };
            query = query
                .Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                .Where(queries.AsExpression<StockprocessGetViewModel>());
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
        public async Task<List<StockprocessGetViewModel>> GetAllAsync(CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StockprocessEntity>();
            var data = await DbSet.AsNoTracking().Where(t => t.tenant_id.Equals(currentUser.tenant_id)).ToListAsync();
            return data.Adapt<List<StockprocessGetViewModel>>();
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <returns></returns>
        public async Task<StockprocessWithDetailViewModel> GetAsync(int id)
        {
            var DbSet = _dBContext.GetDbSet<StockprocessEntity>();
            var entity = await DbSet.AsNoTracking().FirstOrDefaultAsync(t => t.id.Equals(id));
            var details = await (from spd in _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking().Where(t => t.stock_process_id == id)
                                 join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on spd.sku_id equals sku.id
                                 join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                                 join gl in _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking() on spd.goods_location_id equals gl.id into gl_left
                                 from gl in gl_left.DefaultIfEmpty()
                                 select new StockprocessdetailViewModel
                                 {
                                     id = spd.id,
                                     stock_process_id = spd.stock_process_id,
                                     sku_id = spd.sku_id,
                                     goods_owner_id = spd.goods_owner_id,
                                     goods_location_id = spd.goods_location_id,
                                     qty = spd.qty,
                                     last_update_time = spd.last_update_time,
                                     tenant_id = spd.tenant_id,
                                     is_source = spd.is_source,
                                     sku_code = sku.sku_code,
                                     spu_code = spu.spu_code,
                                     spu_name = spu.spu_name,
                                     unit = sku.unit,
                                     location_name = gl.location_name == null ? "" : gl.location_name,
                                     series_number = spd.series_number,
                                     expiry_date = spd.expiry_date,
                                     price = spd.price,
                                     putaway_date = spd.putaway_date,
                                 }).ToListAsync();
            if (entity == null)
            {
                return null;
            }
            var res = entity.Adapt<StockprocessWithDetailViewModel>();
            var adjust_DBSet = _dBContext.GetDbSet<StockadjustEntity>().AsNoTracking();
            var adjusted = await (from a in adjust_DBSet
                                  join d in _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking() on a.source_table_id equals d.id
                                  where a.job_type == 2 && d.stock_process_id == id
                                  select a
                           ).AnyAsync();
            res.adjust_status = entity.process_status && adjusted;
            res.source_detail_list = details.Where(t => t.is_source == true).ToList();
            res.target_detail_list = details.Where(t => t.is_source == false).ToList();
            return res;
        }

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(StockprocessViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StockprocessEntity>();
            var entity = viewModel.Adapt<StockprocessEntity>();
            var stock_DBSet = _dBContext.GetDbSet<StockEntity>().AsNoTracking();

            ParameterExpression parameterExpression = Expression.Parameter(typeof(StockEntity), "m");
            Expression exp = null;
            for (int i = 0; i < entity.detailList.Count; i++)
            {
                ConstantExpression t_constan_location = Expression.Constant(entity.detailList[i].goods_location_id);
                PropertyInfo t_prop_location = typeof(StockEntity).GetProperty("goods_location_id");
                MemberExpression t_location_exp = Expression.Property(parameterExpression, t_prop_location);
                BinaryExpression t_location_full_exp = Expression.Equal(t_location_exp, t_constan_location);
                ConstantExpression t_constan_sku = Expression.Constant(entity.detailList[i].sku_id);
                PropertyInfo t_prop_sku = typeof(StockEntity).GetProperty("sku_id");
                MemberExpression t_sku_exp = Expression.Property(parameterExpression, t_prop_sku);
                BinaryExpression t_sku_full_exp = Expression.Equal(t_sku_exp, t_constan_sku);
                ConstantExpression t_constan_owner = Expression.Constant(entity.detailList[i].goods_owner_id);
                PropertyInfo t_prop_owner = typeof(StockEntity).GetProperty("goods_owner_id");
                MemberExpression t_owner_exp = Expression.Property(parameterExpression, t_prop_owner);
                BinaryExpression t_owner_full_exp = Expression.Equal(t_owner_exp, t_constan_owner);
                ConstantExpression t_constan_sn = Expression.Constant(entity.detailList[i].series_number);
                PropertyInfo t_prop_sn = typeof(StockEntity).GetProperty("series_number");
                MemberExpression t_sn_exp = Expression.Property(parameterExpression, t_prop_sn);
                BinaryExpression t_sn_full_exp = Expression.Equal(t_sn_exp, t_constan_sn);
                ConstantExpression t_constan_expiry = Expression.Constant(entity.detailList[i].expiry_date);
                PropertyInfo t_prop_expiry = typeof(StockEntity).GetProperty("expiry_date");
                MemberExpression t_expiry_exp = Expression.Property(parameterExpression, t_prop_sn);
                BinaryExpression t_expiry_full_exp = Expression.Equal(t_sn_exp, t_constan_sn);
                ConstantExpression t_constan_price = Expression.Constant(entity.detailList[i].price);
                PropertyInfo t_prop_price = typeof(StockEntity).GetProperty("price");
                MemberExpression t_price_exp = Expression.Property(parameterExpression, t_prop_sn);
                BinaryExpression t_price_full_exp = Expression.Equal(t_sn_exp, t_constan_sn);
                ConstantExpression t_constan_putaway = Expression.Constant(entity.detailList[i].putaway_date);
                PropertyInfo t_prop_putaway = typeof(StockEntity).GetProperty("putaway_date");
                MemberExpression t_putaway_exp = Expression.Property(parameterExpression, t_prop_sn);
                BinaryExpression t_putaway_full_exp = Expression.Equal(t_sn_exp, t_constan_sn);
                var t_exp = Expression.And(t_location_full_exp, t_sku_full_exp);
                t_exp = Expression.And(t_exp, t_owner_full_exp);
                if (exp != null)
                    exp = Expression.Or(exp, t_exp);
                else
                    exp = t_exp;
            }
            var predicate_res = Expression.Lambda<Func<StockEntity, bool>>(exp, new ParameterExpression[1] { parameterExpression });
            var stocks = await stock_DBSet.Where(predicate_res).ToListAsync();
            var goods_location_id_list = viewModel.detailList.Where(t => t.is_source == true).Select(t => t.goods_location_id).ToList();
            var sku_id_list = viewModel.detailList.Where(t => t.is_source == true).Select(t => t.sku_id).ToList();
            var lockeds = await (from d in _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking()
                                 where d.is_update_stock == false && goods_location_id_list.Contains(d.goods_location_id)
                                 && sku_id_list.Contains(d.sku_id)
                                 group d by new { d.goods_location_id, d.sku_id, d.goods_owner_id, d.series_number, d.expiry_date, d.price,d.putaway_date } into lg
                                 select new
                                 {
                                     sku_id = lg.Key.sku_id,
                                     goods_location_id = lg.Key.goods_location_id,
                                     goods_owner_id = lg.Key.goods_owner_id,
                                     series_number = lg.Key.series_number,
                                     lg.Key.expiry_date,
                                     lg.Key.price,
                                     lg.Key.putaway_date,
                                     qty_locked = lg.Sum(e => e.qty)
                                 }).ToListAsync();
            entity.id = 0;
            entity.create_time = DateTime.Now;
            entity.creator = currentUser.user_name;
            entity.last_update_time = DateTime.Now;
            entity.tenant_id = currentUser.tenant_id;
            entity.job_code = await _functionHelper.GetFormNoAsync("Stockprocess");
            await DbSet.AddAsync(entity);
            foreach (var d in entity.detailList)
            {
                d.tenant_id = currentUser.tenant_id;
                d.last_update_time = DateTime.Now;
                d.id = 0;
                var s = stocks.FirstOrDefault(t => t.sku_id == d.sku_id && t.goods_location_id == d.goods_location_id && t.goods_owner_id == d.goods_owner_id && t.series_number == d.series_number && t.expiry_date == d.expiry_date && t.price == d.price && t.putaway_date == d.putaway_date);
                if (d.is_source == true)
                {
                    if (s == null)
                    {
                        return (0, _stringLocalizer["data_changed"]);
                    }
                    var locked = lockeds.FirstOrDefault(t => t.sku_id == d.sku_id && t.goods_location_id == d.goods_location_id && t.goods_owner_id == d.goods_owner_id && t.series_number == d.series_number && t.expiry_date == d.expiry_date && t.price == d.price && t.putaway_date == d.putaway_date);
                    if ((s.qty - (locked == null ? 0 : locked.qty_locked)) < d.qty)
                    {
                        return (0, _stringLocalizer["data_changed"]);
                    }
                    if (s.is_freeze == true)
                    {
                        return (0, _stringLocalizer["stock_frozen"]);
                    }
                }
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
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UpdateAsync(StockprocessViewModel viewModel)
        {
            var DbSet = _dBContext.GetDbSet<StockprocessEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            entity.id = viewModel.id;
            entity.job_code = viewModel.job_code;
            entity.job_type = viewModel.job_type;
            entity.process_status = viewModel.process_status;
            entity.processor = viewModel.processor;
            entity.process_time = viewModel.process_time;
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
            var entity = await _dBContext.GetDbSet<StockprocessEntity>().Where(t => t.id.Equals(id) && t.process_status == false).Include(e => e.detailList).FirstOrDefaultAsync();
            _dBContext.GetDbSet<StockprocessEntity>().Remove(entity);
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
        /// confirm adjustment
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ConfirmAdjustment(int id, CurrentUser currentUser)
        {
            var DBSet = _dBContext.GetDbSet<StockprocessEntity>();
            var detail_DBSet = _dBContext.GetDbSet<StockprocessdetailEntity>();
            var adjust_DBset = _dBContext.GetDbSet<StockadjustEntity>();
            var entity = await DBSet.FirstOrDefaultAsync(t => t.id == id);
            var now_time = DateTime.Now;
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            var adjusted = await (from a in adjust_DBset
                                  join d in _dBContext.GetDbSet<StockprocessdetailEntity>().AsNoTracking() on a.source_table_id equals d.id
                                  where a.job_type == 2 && d.stock_process_id == id
                                  select a
              ).AnyAsync();
            if (entity.process_status && adjusted)
            {
                return (false, _stringLocalizer["status_changed"]);
            }
            var details = await detail_DBSet.Where(t => t.stock_process_id == id).ToListAsync();
            var adjusts = (from d in details
                           select new StockadjustEntity
                           {
                               sku_id = d.sku_id,
                               source_table_id = d.id,
                               is_update_stock = true,
                               goods_location_id = d.goods_location_id,
                               job_type = 2,
                               goods_owner_id = d.goods_owner_id,
                               qty = d.is_source ? -d.qty : d.qty,
                               create_time = now_time,
                               creator = currentUser.user_name,
                               last_update_time = now_time,
                               tenant_id = currentUser.tenant_id,
                               series_number = d.series_number,
                               expiry_date = d.expiry_date,
                               price = d.price,
                               putaway_date = d.putaway_date,
                           }).ToList();
            entity.last_update_time = now_time;
            var stock_DBSet = _dBContext.GetDbSet<StockEntity>();
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }

            var stocks = await stock_DBSet.Where(s => detail_DBSet.Where(t => t.stock_process_id == id).Any(t => t.goods_location_id == s.goods_location_id && t.sku_id == s.sku_id && t.goods_owner_id == s.goods_owner_id && t.series_number == s.series_number && t.expiry_date == s.expiry_date && t.price == s.price && t.putaway_date == s.putaway_date)).ToListAsync();
            foreach (var d in details)
            {
                var stock = stocks.FirstOrDefault(t => t.goods_location_id == d.goods_location_id && t.sku_id == d.sku_id && t.goods_owner_id == d.goods_owner_id && t.series_number == d.series_number && t.expiry_date == d.expiry_date && t.price == d.price && t.putaway_date == d.putaway_date);
                d.is_update_stock = true;
                d.last_update_time = now_time;
                if (d.is_source)
                {
                    if (stock == null)
                    {
                        return (false, _stringLocalizer["data_changed"]);
                    }
                    stock.qty -= d.qty;
                    stock.last_update_time = now_time;
                }
                else
                {
                    d.putaway_date = DateTime.Today;
                    if (stock == null)
                    {
                        await stock_DBSet.AddAsync(new StockEntity
                        {
                            sku_id = d.sku_id,
                            goods_location_id = d.goods_location_id,
                            goods_owner_id = d.goods_owner_id,
                            series_number = d.series_number,
                            expiry_date = d.expiry_date,
                            price = d.price,
                            putaway_date = d.putaway_date,
                            is_freeze = false,
                            last_update_time = now_time,
                            qty = d.qty,
                            tenant_id = currentUser.tenant_id
                        });
                    }
                    else
                    {
                        stock.qty += d.qty;
                        stock.last_update_time = now_time;
                    }
                }
            }
            var code = await GetAdjustOrderCode(currentUser);
            adjusts.ForEach(t => t.job_code = code);
            await adjust_DBset.AddRangeAsync(adjusts);
            var res = await _dBContext.SaveChangesAsync();
            if (res > 0)
            {
                return (true, _stringLocalizer["operation_success"]);
            }
            return (false, _stringLocalizer["operation_failed"]);
        }

        /// <summary>
        /// confirm processing
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ConfirmProcess(int id, CurrentUser currentUser)
        {
            var DBSet = _dBContext.GetDbSet<StockprocessEntity>();
            var entity = await DBSet.FirstOrDefaultAsync(t => t.id == id);
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            if (entity.process_status == true)
            {
                return (false, _stringLocalizer["status_changed"]);
            }
            entity.process_status = true;
            entity.processor = currentUser.user_name;
            entity.process_time = DateTime.Now;
            entity.last_update_time = DateTime.Now;
            var res = await _dBContext.SaveChangesAsync();
            if (res > 0)
            {
                return (true, _stringLocalizer["operation_success"]);
            }
            return (false, _stringLocalizer["operation_failed"]);
        }

        /// <summary>
        /// get next order code number
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetOrderCode(CurrentUser currentUser)
        {
            string code;
            string date = DateTime.Now.ToString("yyyy" + "MM" + "dd");
            string maxNo = await _dBContext.GetDbSet<StockprocessEntity>().AsNoTracking().Where(t => t.tenant_id == currentUser.tenant_id).MaxAsync(t => t.job_code);
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
        /// get next order code number
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAdjustOrderCode(CurrentUser currentUser)
        {
            string code;
            string date = DateTime.Now.ToString("yyyy" + "MM" + "dd");
            string maxNo = await _dBContext.GetDbSet<StockadjustEntity>().AsNoTracking().Where(t => t.tenant_id == currentUser.tenant_id).MaxAsync(t => t.job_code);
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