/*
 * date：2022-12-26
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
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using ModernWMS.Core;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  Stockfreeze Service
    /// </summary>
    public class StockfreezeService : BaseService<StockfreezeEntity>, IStockfreezeService
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
        ///Stockfreeze  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public StockfreezeService(
            SqlDBContext dBContext
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
             , FunctionHelper functionHelper)
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
        public async Task<(List<StockfreezeViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var DbSet = _dBContext.GetDbSet<StockfreezeEntity>();

            var query = from m in DbSet.AsNoTracking()
                        join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on m.sku_id equals sku.id
                        join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                        join location in _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking() on m.goods_location_id equals location.id
                        select new StockfreezeViewModel
                        {
                            id = m.id,
                            job_code = m.job_code,
                            job_type = m.job_type,
                            sku_id = m.sku_id,
                            goods_owner_id = m.goods_owner_id,
                            goods_location_id = m.goods_location_id,
                            handler = m.handler,
                            handle_time = m.handle_time,
                            last_update_time = m.last_update_time,
                            tenant_id = m.tenant_id,
                            sku_code = sku.sku_code,
                            spu_code = spu.spu_code,
                            spu_name = spu.spu_name,
                            location_name = location.location_name,
                            warehouse_name = location.warehouse_name,
                            series_number = m.series_number,
                        };
            query = query
                .Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                .Where(queries.AsExpression<StockfreezeViewModel>());
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
        public async Task<List<StockfreezeViewModel>> GetAllAsync(CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StockfreezeEntity>();
            var data = await DbSet.AsNoTracking().Where(t => t.tenant_id.Equals(currentUser.tenant_id)).ToListAsync();
            return data.Adapt<List<StockfreezeViewModel>>();
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <returns></returns>
        public async Task<StockfreezeViewModel> GetAsync(int id)
        {
            var DbSet = _dBContext.GetDbSet<StockfreezeEntity>();
            var data = await (from m in DbSet.AsNoTracking()
                              join sku in _dBContext.GetDbSet<SkuEntity>().AsNoTracking() on m.sku_id equals sku.id
                              join spu in _dBContext.GetDbSet<SpuEntity>().AsNoTracking() on sku.spu_id equals spu.id
                              join location in _dBContext.GetDbSet<GoodslocationEntity>().AsNoTracking() on m.goods_location_id equals location.id
                              where m.id == id
                              select new StockfreezeViewModel
                              {
                                  id = m.id,
                                  job_code = m.job_code,
                                  job_type = m.job_type,
                                  sku_id = m.sku_id,
                                  goods_owner_id = m.goods_owner_id,
                                  goods_location_id = m.goods_location_id,
                                  handler = m.handler,
                                  handle_time = m.handle_time,
                                  last_update_time = m.last_update_time,
                                  tenant_id = m.tenant_id,
                                  sku_code = sku.sku_code,
                                  spu_code = spu.spu_code,
                                  spu_name = spu.spu_name,
                                  location_name = location.location_name,
                                  warehouse_name = location.warehouse_name,
                                  series_number = m.series_number,
                              }).FirstOrDefaultAsync();

            return data;
        }

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">current user</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(StockfreezeViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<StockfreezeEntity>();
            var entity = viewModel.Adapt<StockfreezeEntity>();
            entity.id = 0;
            entity.handle_time = DateTime.Now;
            entity.handler = currentUser.user_name;
            entity.last_update_time = DateTime.Now;
            entity.tenant_id = currentUser.tenant_id;
            entity.job_code = await _functionHelper.GetFormNoAsync("Stockfreeze");
            var stock_DBSet = _dBContext.GetDbSet<StockEntity>();
            var stocks = await stock_DBSet.Where(t => t.goods_location_id == entity.goods_location_id && t.goods_owner_id == entity.goods_owner_id && t.sku_id == entity.sku_id && t.series_number == entity.series_number  ).ToListAsync();
            foreach (var stock in stocks)
            {
                if (entity.job_type == true)
                    stock.is_freeze = true;
                else
                    stock.is_freeze = false;
            }
            await DbSet.AddAsync(entity);
            if (await (_dBContext.GetDbSet<StockprocessdetailEntity>().AnyAsync(t => t.goods_location_id == entity.goods_location_id && t.goods_owner_id == entity.goods_owner_id && t.sku_id == entity.sku_id && t.is_update_stock == false)))
            {
                return (0, _stringLocalizer["process_not_comfirm"]);
            }
            else if (await (_dBContext.GetDbSet<DispatchpicklistEntity>().AnyAsync(t => t.goods_location_id == entity.goods_location_id && t.sku_id == entity.sku_id && t.is_update_stock == false)))
            {
                return (0, _stringLocalizer["dispatch_not_comfirm"]);
            }
            else if (await (_dBContext.GetDbSet<StockmoveEntity>().AnyAsync(t => (t.orig_goods_location_id == entity.goods_location_id || t.dest_googs_location_id == entity.goods_location_id) && t.sku_id == entity.sku_id && t.move_status == 0)))
            {
                return (0, _stringLocalizer["move_not_comfirm"]);
            }
            else
            {
                await _dBContext.SaveChangesAsync();
            }
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
        public async Task<(bool flag, string msg)> UpdateAsync(StockfreezeViewModel viewModel)
        {
            var DbSet = _dBContext.GetDbSet<StockfreezeEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            entity.id = viewModel.id;
            entity.job_code = viewModel.job_code;
            entity.job_type = viewModel.job_type;
            entity.sku_id = viewModel.sku_id;
            entity.goods_owner_id = viewModel.goods_owner_id;
            entity.goods_location_id = viewModel.goods_location_id;
            entity.handler = viewModel.handler;
            entity.handle_time = viewModel.handle_time;
            entity.last_update_time = DateTime.Now;
            entity.series_number = viewModel.series_number;
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
            var qty = await _dBContext.GetDbSet<StockfreezeEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
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
            string maxNo = await _dBContext.GetDbSet<StockfreezeEntity>().AsNoTracking().Where(t => t.tenant_id == currentUser.tenant_id).MaxAsync(t => t.job_code);
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