/*
 * date：2022-12-22
 * developer：AMo
 */
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModernWMS.Core.DBContext;
using ModernWMS.Core.DynamicSearch;
using ModernWMS.Core.JWT;
using ModernWMS.Core.Models;
using ModernWMS.Core.Services;
using ModernWMS.WMS.Entities.Models;
using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.WMS.IServices;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  Asn Service
    /// </summary>
    public class AsnService : BaseService<AsnEntity>, IAsnService
    {
        #region Args
        /// <summary>
        /// The DBContext
        /// </summary>
        private readonly SqlDBContext _dBContext;

        /// <summary>
        /// Localizer Service
        /// </summary>
        private readonly IStringLocalizer<Core.MultiLanguage> _stringLocalizer;
        #endregion

        #region constructor
        /// <summary>
        ///Asn  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public AsnService(
            SqlDBContext dBContext
          , IStringLocalizer<Core.MultiLanguage> stringLocalizer
            )
        {
            this._dBContext = dBContext;
            this._stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Api
        /// <summary>
        /// page search, sqlTitle input asn_status:0 ~ 4
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<AsnViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            Byte asn_status = 255;
            if (pageSearch.sqlTitle.ToLower().Contains("asn_status"))
            {
                asn_status = Convert.ToByte(pageSearch.sqlTitle.Trim().ToLower().Replace("asn_status","").Replace("：", "").Replace(":", "").Replace("=", ""));
                asn_status = asn_status.Equals(4) ? (Byte)255 : asn_status;
            }
            var Spus = _dBContext.GetDbSet<SpuEntity>();
            var Skus = _dBContext.GetDbSet<SkuEntity>();
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var query = from m in Asns.AsNoTracking()
                        join p in Spus.AsNoTracking() on m.spu_id equals p.id
                        join k in Skus.AsNoTracking() on m.sku_id equals k.id
                        where m.tenant_id == currentUser.tenant_id
                        && (asn_status == 255 || m.asn_status == asn_status)
                        select new AsnViewModel
                        {
                            id = m.id,
                            asn_no = m.asn_no,
                            asn_status = m.asn_status,
                            spu_id = m.spu_id,
                            spu_code = p.spu_code,
                            spu_name = p.spu_name,
                            sku_id = m.sku_id,
                            sku_code = k.sku_code,
                            sku_name = k.sku_name,
                            origin = p.origin,
                            length_unit = p.length_unit,
                            volume_unit = p.volume_unit,
                            weight_unit = p.weight_unit,
                            asn_qty = m.asn_qty,
                            actual_qty = m.actual_qty,
                            sorted_qty = m.sorted_qty,
                            shortage_qty = m.shortage_qty,
                            more_qty = m.more_qty,
                            damage_qty = m.damage_qty,
                            weight = k.weight * m.asn_qty,
                            volume = k.volume * m.asn_qty,
                            supplier_id = m.supplier_id,
                            supplier_name = m.supplier_name,
                            goods_owner_id = m.goods_owner_id,
                            goods_owner_name = m.goods_owner_name,
                            creator = m.creator,
                            create_time = m.create_time,
                            last_update_time = m.last_update_time,
                            is_valid = m.is_valid
                        };
            query = query.Where(queries.AsExpression<AsnViewModel>());
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.create_time)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list, totals);
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <returns></returns>
        public async Task<AsnViewModel> GetAsync(int id)
        {
            var Spus = _dBContext.GetDbSet<SpuEntity>();
            var Skus = _dBContext.GetDbSet<SkuEntity>();
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var query = from m in Asns.AsNoTracking()
                        join p in Spus.AsNoTracking() on m.spu_id equals p.id
                        join k in Skus.AsNoTracking() on m.sku_id equals k.id
                        select new AsnViewModel
                        {
                            id = m.id,
                            asn_no = m.asn_no,
                            asn_status = m.asn_status,
                            spu_id = m.spu_id,
                            spu_code = p.spu_code,
                            spu_name = p.spu_name,
                            sku_id = m.sku_id,
                            sku_code = k.sku_code,
                            sku_name = k.sku_name,
                            origin = p.origin,
                            length_unit = p.length_unit,
                            volume_unit = p.volume_unit,
                            weight_unit = p.weight_unit,
                            asn_qty = m.asn_qty,
                            actual_qty = m.actual_qty,
                            sorted_qty = m.sorted_qty,
                            shortage_qty = m.shortage_qty,
                            more_qty = m.more_qty,
                            damage_qty = m.damage_qty,
                            weight = m.weight,
                            volume = m.volume,
                            supplier_id = m.supplier_id,
                            supplier_name = m.supplier_name,
                            goods_owner_id = m.goods_owner_id,
                            goods_owner_name = m.goods_owner_name,
                            creator = m.creator,
                            create_time = m.create_time,
                            last_update_time = m.last_update_time,
                            is_valid = m.is_valid
                        };
            var data = await query.FirstOrDefaultAsync(t => t.id.Equals(id));
            return data ?? new AsnViewModel();
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(AsnViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<AsnEntity>();
            var entity = viewModel.Adapt<AsnEntity>();
            entity.id = 0;
            entity.asn_no = await GetOrderCode(currentUser);
            entity.creator = currentUser.user_name;
            entity.create_time = DateTime.Now;
            entity.last_update_time = DateTime.Now;
            entity.tenant_id = currentUser.tenant_id;
            entity.is_valid = viewModel.is_valid;
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
            var DbSet = _dBContext.GetDbSet<AsnEntity>();
            string code = "";
            string date = DateTime.Now.ToString("yyyy" + "MM" + "dd");
            string maxNo = await DbSet.AsNoTracking().Where(t => t.tenant_id.Equals(currentUser.tenant_id)).MaxAsync(t => t.asn_no);
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
        /// update a record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UpdateAsync(AsnViewModel viewModel)
        {
            var DbSet = _dBContext.GetDbSet<AsnEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            entity.id = viewModel.id;
            entity.asn_no = viewModel.asn_no;
            entity.spu_id = viewModel.spu_id;
            entity.sku_id = viewModel.sku_id;
            entity.asn_qty = viewModel.asn_qty;
            entity.weight = viewModel.weight;
            entity.volume = viewModel.volume;
            entity.supplier_id = viewModel.supplier_id;
            entity.supplier_name = viewModel.supplier_name;
            entity.goods_owner_id = viewModel.goods_owner_id;
            entity.goods_owner_name = viewModel.goods_owner_name;
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
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entity = await Asns.FirstOrDefaultAsync(t => t.id.Equals(id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            if (entity.asn_status.Equals(0))
            {
                Asns.Remove(entity);
            }
            else if (entity.asn_status.Equals(8))
            {
                return (false, _stringLocalizer["asn_had_putaway"]);
            }
            else
            {
                entity.asn_status = (byte)(entity.asn_status - 1);
            }
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
        /// Bulk modify Goodsowner
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> BulkModifyGoodsownerAsync(AsnBulkModifyGoodsOwnerViewModel viewModel)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entities = await Asns.Where(t => viewModel.idList.Contains(t.id)).ToListAsync();
            //需要什么限制？
            entities.ForEach(t =>
            {
                t.goods_owner_id = viewModel.goods_owner_id;
                t.goods_owner_name = viewModel.goods_owner_name;
                t.last_update_time = DateTime.Now;
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
        #endregion

        #region Flow Api
        /// <summary>
        /// Confirm Delivery
        /// change the asn_status from 0 to 1
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ConfirmAsync(int id)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entity = await Asns.FirstOrDefaultAsync(t => t.id == id);
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            else if (entity.asn_status > 0)
            {
                return (false, $"{entity.asn_no}{_stringLocalizer["ASN_Status_Is_Not_Pre_Delivery"]}");
            }
            entity.asn_status = 1;
            var qty = await _dBContext.SaveChangesAsync();
            if (qty > 0)
            {
                return (true, _stringLocalizer["confirm_success"]);
            }
            else
            {
                return (false, _stringLocalizer["confirm_failed"]);
            }
        }
        /// <summary>
        /// Cancel confirm, change asn_status 1 to 0
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ConfirmCancelAsync(int id)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entity = await Asns.FirstOrDefaultAsync(t => t.id == id);
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            else if (entity.asn_status != (byte)1)
            {
                return (false, $"{entity.asn_no}{_stringLocalizer["ASN_Status_Is_Not_Pre_Delivery"]}");
            }
            entity.asn_status = 0;
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
        /// Unload
        /// change the asn_status from 1 to 2
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UnloadAsync(int id)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entity = await Asns.FirstOrDefaultAsync(t => t.id == id);
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            else if (entity.asn_status > 1)
            {
                return (false, $"{entity.asn_no}{_stringLocalizer["ASN_Status_Is_Not_Pre_Load"]}");
            }
            entity.asn_status = 2; 
            entity.last_update_time = DateTime.Now;
            var qty = await _dBContext.SaveChangesAsync();
            if (qty > 0)
            {
                return (true, _stringLocalizer["confirm_success"]);
            }
            else
            {
                return (false, _stringLocalizer["confirm_failed"]);
            }
        }

        /// <summary>
        /// Cancel unload
        /// change the asn_status from 2 to 1
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UnloadCancelAsync(int id)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entity = await Asns.FirstOrDefaultAsync(t => t.id == id);
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            else if (entity.asn_status != (byte)2)
            {
                return (false, $"{entity.asn_no}{_stringLocalizer["ASN_Status_Is_Not_Pre_Load"]}");
            }
            entity.asn_status = 1;
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
        /// sorting， add a new asnsort record and update asn sorted_qty
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> SortingAsync(AsnsortInputViewModel viewModel, CurrentUser currentUser)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var Asnsorts = _dBContext.GetDbSet<AsnsortEntity>();

            var entity = await Asns.FirstOrDefaultAsync(t => t.id == viewModel.asn_id);
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            else if (entity.asn_status != 2)
            {
                return (false, $"{entity.asn_no}{_stringLocalizer["ASN_Status_Is_Not_Pre_Sort"]}");
            }
            await Asnsorts.AddAsync(new AsnsortEntity
            {
                id = 0,
                asn_id = viewModel.asn_id,
                sorted_qty = viewModel.sorted_qty,
                create_time = DateTime.Now,
                creator = currentUser.user_name,
                is_valid = true,
                last_update_time = DateTime.Now, tenant_id = currentUser.tenant_id
            });
            entity.sorted_qty += viewModel.sorted_qty;
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
        /// Sorted
        /// change the asn_status from 2 to 3
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> SortedAsync(int id)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entity = await Asns.FirstOrDefaultAsync(t => t.id == id);
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            else if (entity.sorted_qty < 1)
            {
                return (false, $"{entity.asn_no}{_stringLocalizer["ASN_Status_Is_Not_Sorting"]}");
            }
            entity.asn_status = 3;
            if (entity.sorted_qty > entity.asn_qty)
            {
                entity.more_qty = entity.sorted_qty - entity.asn_qty;
            }
            else if (entity.sorted_qty < entity.asn_qty)
            {
                entity.shortage_qty = entity.asn_qty - entity.sorted_qty;
            }
            entity.last_update_time = DateTime.Now;
            var qty = await _dBContext.SaveChangesAsync();
            if (qty > 0)
            {
                return (true, _stringLocalizer["sorted_success"]);
            }
            else
            {
                return (false, _stringLocalizer["sorted_failed"]);
            }
        }

        /// <summary>
        /// Cancel sorted
        /// change the asn_status from 3 to 2
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> SortedCancelAsync(int id)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entity = await Asns.FirstOrDefaultAsync(t => t.id == id);
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            else if (entity.actual_qty > 0)
            {
                return (false, $"{entity.asn_no}{_stringLocalizer["ASN_Status_Is_Putaway"]}");
            }
            else if (entity.sorted_qty < 1)
            {
                return (false, $"{entity.asn_no}{_stringLocalizer["ASN_Status_Is_Not_Sorting"]}");
            }
            entity.asn_status = 2;
            entity.sorted_qty = 0;
            entity.more_qty = 0;
            entity.shortage_qty = 0;
            entity.last_update_time = DateTime.Now;
            var qty = await _dBContext.SaveChangesAsync();
            if (qty > 0)
            {
                var Asnsorts = _dBContext.GetDbSet<AsnsortEntity>();
                await Asnsorts.Where(t => t.asn_id.Equals(id)).ExecuteDeleteAsync();
                return (true, _stringLocalizer["save_success"]);
            }
            else
            {
                return (false, _stringLocalizer["save_failed"]);
            }
        }
        /// <summary>
        /// PutAway
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> PutAwayAsync(AsnPutAwayInputViewModel viewModel, CurrentUser currentUser)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();

            var Goodslocations = _dBContext.GetDbSet<GoodslocationEntity>();
            var Location = await Goodslocations.FirstOrDefaultAsync(t => t.id.Equals(viewModel.goods_location_id));
            if (Location == null)
            {
                return (false, string.Format(_stringLocalizer["Required"], _stringLocalizer["location_name"]));
            }

            var entity = await Asns.FirstOrDefaultAsync(t => t.id == viewModel.asn_id);
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            else if (entity.asn_status != 3)
            {
                return (false, $"{entity.asn_no}{_stringLocalizer["ASN_Status_Is_Not_Sorted"]}");
            }
            else if (entity.actual_qty + viewModel.putaway_qty > entity.sorted_qty)
            {
                return (false, $"{entity.asn_no}{_stringLocalizer["ASN_Total_PutAway_Qty_Greater_Than_Sorted_Qty"]}");
            }
            entity.actual_qty += viewModel.putaway_qty;
            if (Location.warehouse_area_property.Equals(5))
            {
                entity.damage_qty += viewModel.putaway_qty;
            }
            if (entity.actual_qty.Equals(entity.sorted_qty))
            {
                entity.asn_status = 4;
            }

            entity.last_update_time = DateTime.Now;
            var Stocks = _dBContext.GetDbSet<StockEntity>();
            var stockEntity = await Stocks.FirstOrDefaultAsync(t => t.sku_id.Equals(entity.sku_id) && t.goods_location_id.Equals(viewModel.goods_location_id));
            if (stockEntity == null)
            {
                stockEntity = new StockEntity
                {
                    sku_id = entity.sku_id,
                    goods_location_id = viewModel.goods_location_id,
                    goods_owner_id = entity.goods_owner_id,
                    qty = viewModel.putaway_qty,
                    is_freeze = false,
                    last_update_time = DateTime.Now,
                    tenant_id = currentUser.tenant_id,
                    id = 0
                };
                await Stocks.AddAsync(stockEntity);
            }
            else
            {
                stockEntity.qty += viewModel.putaway_qty;
                stockEntity.last_update_time = DateTime.Now;
            }
            var qty = await _dBContext.SaveChangesAsync();
            if (qty > 0)
            {
                return (true, _stringLocalizer["putaway_success"]);
            }
            else
            {
                return (false, _stringLocalizer["putaway_failed"]);
            }
        }
        #endregion
    }
}
 
