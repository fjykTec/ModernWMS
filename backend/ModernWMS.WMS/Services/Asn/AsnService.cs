/*
 * date：2022-12-22
 * developer：AMo
 */

using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
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
using System.Linq;

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

        /// <summary>
        /// functions
        /// </summary>
        private readonly FunctionHelper _functionHelper;

        #endregion Args

        #region constructor

        /// <summary>
        ///Asn  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public AsnService(
            SqlDBContext dBContext
            , IStringLocalizer<Core.MultiLanguage> stringLocalizer
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
            var Asns = _dBContext.GetDbSet<AsnEntity>().AsNoTracking();
            if (pageSearch.sqlTitle.ToLower().Contains("asn_status:alltodo"))
            {
                Asns = Asns.Where(t => t.asn_status <= 3);
            }
            else if (pageSearch.sqlTitle.ToLower().Contains("asn_status"))
            {
                asn_status = Convert.ToByte(pageSearch.sqlTitle.Trim().ToLower().Replace("asn_status", "").Replace("：", "").Replace(":", "").Replace("=", ""));
                //asn_status = asn_status.Equals(4) ? (Byte)255 : asn_status;
                Asns = Asns.Where(t => t.asn_status == asn_status);
            }
            var Spus = _dBContext.GetDbSet<SpuEntity>().AsNoTracking();
            var Skus = _dBContext.GetDbSet<SkuEntity>().AsNoTracking();
            var Asnmasters = _dBContext.GetDbSet<AsnmasterEntity>().AsNoTracking();

            var query = from m in Asns
                        join am in Asnmasters on m.asnmaster_id equals am.id
                        join p in Spus on m.spu_id equals p.id
                        join k in Skus on m.sku_id equals k.id
                        where m.tenant_id == currentUser.tenant_id
                        select new AsnViewModel
                        {
                            id = m.id,
                            asnmaster_id = m.asnmaster_id,
                            asn_no = m.asn_no,
                            asn_batch = am.asn_batch,
                            estimated_arrival_time = am.estimated_arrival_time,
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
                            price = m.price,
                            asn_qty = m.asn_qty,
                            actual_qty = m.actual_qty,
                            arrival_time = m.arrival_time,
                            unload_person = m.unload_person,
                            unload_person_id = m.unload_person_id,
                            unload_time = m.unload_time,
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
                            is_valid = m.is_valid, 
                            expiry_date = m.expiry_date
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
            var Asnmasters = _dBContext.GetDbSet<AsnmasterEntity>();
            var query = from m in Asns.AsNoTracking()
                        join am in Asnmasters.AsNoTracking() on m.asnmaster_id equals am.id
                        join p in Spus.AsNoTracking() on m.spu_id equals p.id
                        join k in Skus.AsNoTracking() on m.sku_id equals k.id
                        select new AsnViewModel
                        {
                            id = m.id,
                            asnmaster_id = m.asnmaster_id,
                            asn_no = m.asn_no,
                            asn_batch = am.asn_batch,
                            estimated_arrival_time = am.estimated_arrival_time,
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
                            price = m.price,
                            asn_qty = m.asn_qty,
                            actual_qty = m.actual_qty,
                            arrival_time = m.arrival_time,
                            unload_person = m.unload_person,
                            unload_person_id = m.unload_person_id,
                            unload_time = m.unload_time,
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
                            is_valid = m.is_valid,
                            expiry_date = m.expiry_date
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
            entity.asn_no = await _functionHelper.GetFormNoAsync("Asn");
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
            entity.price = viewModel.price;
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

        #endregion Api

        #region Flow Api

        /// <summary>
        /// Confirm Delivery
        /// change the asn_status from 0 to 1
        /// </summary>
        /// <param name="viewModels">args</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ConfirmAsync(List<AsnConfirmInputViewModel> viewModels)
        {
            var idList = viewModels.Where(t => t.id > 0).Select(t => t.id).ToList();
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entities = await Asns.Where(t => idList.Contains(t.id)).ToListAsync();
            var last_update_time = DateTime.Now;
            if (!entities.Any())
            {
                return (false, "[202]" + _stringLocalizer["not_exists_entity"]);
            }
            else if (entities.Any(t => t.asn_status > 0))
            {
                return (false, "[202]" + $"{_stringLocalizer["ASN_Status_Is_Not_Pre_Delivery"]}");
            }
            // get asnmaster data
            var asnmaster_id = entities.Select(t => t.asnmaster_id).FirstOrDefault();
            var Asnmaster = _dBContext.GetDbSet<AsnmasterEntity>();
            var asnmasterentity = await Asnmaster.FirstOrDefaultAsync(t => t.id.Equals(asnmaster_id));
            if (asnmasterentity == null)
            {
                return (false, "[202]" + _stringLocalizer["not_exists_entity"]);
            }
            // update asnmaster last_update_time
            asnmasterentity.last_update_time = last_update_time;

            entities.ForEach(t =>
            {
                var vm = viewModels.FirstOrDefault(t => t.id == t.id);
                if (vm != null)
                {
                    t.asn_status = 1;
                    t.arrival_time = vm.arrival_time;
                    t.last_update_time = last_update_time;
                }
            });
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
        /// <param name="idList">id list</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ConfirmCancelAsync(List<int> idList)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entities = await Asns.Where(t => idList.Contains(t.id)).ToListAsync();
            var last_update_time = DateTime.Now;
            if (!entities.Any())
            {
                return (false, "[202]" + _stringLocalizer["not_exists_entity"]);
            }
            if (entities.Any(t => t.asn_status != (byte)1))
            {
                return (false, "[202]" + $"{_stringLocalizer["ASN_Status_Is_Not_Pre_Delivery"]}");
            }
            // get asnmaster data
            var asnmaster_id = entities.Select(t => t.asnmaster_id).FirstOrDefault();
            var Asnmaster = _dBContext.GetDbSet<AsnmasterEntity>();
            var asnmasterentity = await Asnmaster.FirstOrDefaultAsync(t => t.id.Equals(asnmaster_id));
            if (asnmasterentity == null)
            {
                return (false, "[202]" + _stringLocalizer["not_exists_entity"]);
            }
            // update asnmaster last_update_time
            asnmasterentity.last_update_time = last_update_time;

            entities.ForEach(e =>
            {
                e.asn_status = 0;
                e.arrival_time = Core.Utility.UtilConvert.MinDate;
                e.last_update_time = last_update_time;
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
        /// Unload
        /// change the asn_status from 1 to 2
        /// </summary>
        /// <param name="viewModels">args</param>
        /// <param name="user">user</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UnloadAsync(List<AsnUnloadInputViewModel> viewModels, CurrentUser user)
        {
            var idList = viewModels.Where(t => t.id > 0).Select(t => t.id).ToList();
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entities = await Asns.Where(t => idList.Contains(t.id)).ToListAsync();
            if (!entities.Any())
            {
                return (false, "[202]" + _stringLocalizer["not_exists_entity"]);
            }
            else if (entities.Any(t => t.asn_status > 1))
            {
                return (false, "[202]" + $"{_stringLocalizer["ASN_Status_Is_Not_Pre_Load"]}");
            }
            entities.ForEach(t =>
            {
                var vm = viewModels.FirstOrDefault(t => t.id == t.id);
                if (vm != null)
                {
                    t.asn_status = 2;
                    t.last_update_time = DateTime.Now;
                    t.unload_time = vm.unload_time;
                    t.unload_person_id = vm.unload_person_id == 0 ? user.user_id : vm.unload_person_id;
                    t.unload_person = vm.unload_person_id == 0 ? user.user_name : vm.unload_person;
                }
            });
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
        /// <param name="idList">id list</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UnloadCancelAsync(List<int> idList)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entities = await Asns.Where(t => idList.Contains(t.id)).ToListAsync();
            if (!entities.Any())
            {
                return (false, "[202]" + _stringLocalizer["not_exists_entity"]);
            }
            if (entities.Any(t => t.asn_status != (byte)2))
            {
                return (false, "[202]" + $"{_stringLocalizer["ASN_Status_Is_Not_Pre_Load"]}");
            }

            entities.ForEach(e =>
            {
                e.asn_status = 1;
                e.unload_time = Core.Utility.UtilConvert.MinDate;
                e.unload_person_id = 0;
                e.unload_person = string.Empty;
                e.last_update_time = DateTime.Now;
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
        /// sorting， add a new asnsort record and update asn sorted_qty
        /// </summary>
        /// <param name="viewModels">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> SortingAsync(List<AsnsortInputViewModel> viewModels, CurrentUser currentUser)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var Asnsorts = _dBContext.GetDbSet<AsnsortEntity>();
            var idList = viewModels.Select(t => t.asn_id).ToList().Distinct().ToList();
            var entities = await Asns.Where(t => idList.Contains(t.id)).ToListAsync();

            if (!entities.Any())
            {
                return (false, "[202]" + _stringLocalizer["not_exists_entity"]);
            }
            else if (entities.Any(t => t.asn_status != 2))
            {
                return (false, "[202]" + $"{_stringLocalizer["ASN_Status_Is_Not_Pre_Sort"]}");
            }
            var models = viewModels.Where(v => entities.Select(e => e.id).ToList().Contains(v.asn_id)).ToList();
            List<AsnsortEntity> sortEntities = new List<AsnsortEntity>();
            foreach (var v in models)
            {
                if (v.sorted_qty > 1 && v.is_auto_num)
                {
                    List<string> snlist = await _functionHelper.GetFormNoListAsync("Asnsort", v.sorted_qty, currentUser.tenant_id, "sn");
                    for (int i = 0; i < v.sorted_qty; i++)
                    {
                        sortEntities.Add(new AsnsortEntity
                        {
                            id = 0,
                            asn_id = v.asn_id,
                            sorted_qty = 1,
                            series_number = snlist[i],
                            create_time = DateTime.Now,
                            creator = currentUser.user_name,
                            is_valid = true,
                            last_update_time = DateTime.Now,
                            tenant_id = currentUser.tenant_id
                        });
                    }
                }
                else
                {
                    string sn = await _functionHelper.GetFormNoAsync("Asnsort", "sn");
                    sortEntities.Add(new AsnsortEntity
                    {
                        id = 0,
                        asn_id = v.asn_id,
                        sorted_qty = v.sorted_qty,
                        series_number = sn,
                        create_time = DateTime.Now,
                        creator = currentUser.user_name,
                        is_valid = true,
                        last_update_time = DateTime.Now,
                        tenant_id = currentUser.tenant_id
                    });
                }
            }
            await Asnsorts.AddRangeAsync(sortEntities);

            entities.ForEach(e =>
            {
                int sum_sorted_qty = viewModels.Where(t => t.asn_id.Equals(e.id)).Sum(v => v.sorted_qty);
                var expiry_date = viewModels.Where(t => t.asn_id.Equals(e.id)).Select(v => v.expiry_date).FirstOrDefault();
                e.sorted_qty += sum_sorted_qty;
                e.last_update_time = DateTime.Now;
                e.expiry_date = expiry_date;
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
        /// get asnsorts list by asn_id
        /// </summary>
        /// <param name="asn_id">asn id</param>
        /// <returns></returns>
        public async Task<List<AsnsortViewModel>> GetAsnsortsAsync(int asn_id)
        {
            var Asnsorts = _dBContext.GetDbSet<AsnsortEntity>();
            var asns = _dBContext.Set<AsnEntity>().AsNoTracking();

            var data = await (from m in asns
                              join d in Asnsorts on m.id equals d.asn_id
                              where m.id == asn_id
                              select new AsnsortViewModel
                              {
                                  id = d.id,
                                  asn_id = asn_id,
                                  sorted_qty = d.sorted_qty,
                                  series_number = d.series_number,
                                  putaway_qty = d.putaway_qty,
                                  expiry_date = m.expiry_date,
                                  creator = d.creator,
                                  create_time = d.create_time,
                                  last_update_time = d.last_update_time,
                                  is_valid = d.is_valid,
                                  tenant_id = d.tenant_id
                              }).ToListAsync();
            if (data != null && data.Count > 0)
            {
                return data;
            }
            else
            {
                return new List<AsnsortViewModel>();
            }
        }

        /// <summary>
        /// update or delete asnsorts data
        /// </summary>
        /// <param name="entities">data</param>
        /// <param name="user">CurrentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> ModifyAsnsortsAsync(List<AsnsortEntity> entities, CurrentUser user)
        {
            var Asnsorts = _dBContext.GetDbSet<AsnsortEntity>();
            if (entities.Any(t => t.id < 0 || t.sorted_qty == 0))
            {
                var delIDList = entities.Where(t => t.id < 0).Select(t => Math.Abs(t.id)).ToList();
                await Asnsorts.Where(t => delIDList.Contains(t.id)).ExecuteDeleteAsync();
            }
            var updateEntities = entities.Where(t => t.id > 0 && t.sorted_qty > 0).ToList();
            if (updateEntities.Any())
            {
                updateEntities.ForEach(t =>
                {
                    t.last_update_time = DateTime.Now;
                    t.is_valid = true;
                });
                Asnsorts.UpdateRange(updateEntities);
            }

            var qty = await _dBContext.SaveChangesAsync();
            if (qty >= 0)
            {
                var Asns = _dBContext.GetDbSet<AsnEntity>();
                var asnids = entities.Select(t => t.asn_id).Distinct().ToList();

                var sumQty = await Asnsorts.AsNoTracking()
                    .Where(t => asnids.Contains(t.asn_id))
                    .GroupBy(t => t.asn_id)
                    .Select(g => new
                    {
                        asn_id = g.Key,
                        sorted_qty = g.Sum(o => o.sorted_qty)
                    }).ToListAsync();
                var asnEntities = await Asns.Where(t => asnids.Contains(t.id)).ToListAsync();
                if (asnEntities.Any())
                {
                    asnEntities.ForEach(e =>
                    {
                        var s = sumQty.FirstOrDefault(t => t.asn_id == e.id);
                        if (s != null)
                        {
                            e.sorted_qty = s.sorted_qty;
                        }
                        else
                        {
                            e.sorted_qty = 0;
                        }
                    });
                    await _dBContext.SaveChangesAsync();
                }
                return (true, _stringLocalizer["sorted_success"]);
            }
            else
            {
                return (false, _stringLocalizer["sorted_failed"]);
            }
        }

        /// <summary>
        /// Sorted
        /// change the asn_status from 2 to 3
        /// </summary>
        /// <param name="idList">id list</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> SortedAsync(List<int> idList)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entities = await Asns.Where(t => idList.Contains(t.id)).ToListAsync();
            if (!entities.Any())
            {
                return (false, "[202]" + _stringLocalizer["not_exists_entity"]);
            }
            else if (entities.Any(t => t.sorted_qty < 1))
            {
                return (false, "[202]" + $"{_stringLocalizer["ASN_Status_Is_Not_Sorting"]}");
            }
            entities.ForEach(e =>
            {
                e.asn_status = 3;
                if (e.sorted_qty > e.asn_qty)
                {
                    e.more_qty = e.sorted_qty - e.asn_qty;
                }
                else if (e.sorted_qty < e.asn_qty)
                {
                    e.shortage_qty = e.asn_qty - e.sorted_qty;
                }
                e.last_update_time = DateTime.Now;
            });
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
        /// <param name="idList">id list</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> SortedCancelAsync(List<int> idList)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var entities = await Asns.Where(t => idList.Contains(t.id)).ToListAsync();
            if (!entities.Any())
            {
                return (false, "[202]" + _stringLocalizer["not_exists_entity"]);
            }
            else if (entities.Any(t => t.actual_qty > 0))
            {
                return (false, "[202]" + $"{_stringLocalizer["ASN_Status_Is_Putaway"]}");
            }
            else if (entities.Any(t => t.sorted_qty < 1))
            {
                return (false, "[202]" + $"{_stringLocalizer["ASN_Status_Is_Not_Sorting"]}");
            }
            entities.ForEach(e =>
            {
                e.asn_status = 2;
                e.sorted_qty = 0;
                e.more_qty = 0;
                e.shortage_qty = 0;
                e.last_update_time = DateTime.Now;
            });
            var qty = await _dBContext.SaveChangesAsync();
            if (qty > 0)
            {
                var Asnsorts = _dBContext.GetDbSet<AsnsortEntity>();
                await Asnsorts.Where(t => idList.Contains(t.asn_id)).ExecuteDeleteAsync();
                return (true, _stringLocalizer["save_success"]);
            }
            else
            {
                return (false, _stringLocalizer["save_failed"]);
            }
        }

        /// <summary>
        /// get pending putaway data by asn_id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<AsnPendingPutawayViewModel>> GetPendingPutawayDataAsync(int id)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var Asnsorts = _dBContext.GetDbSet<AsnsortEntity>();

            var data = await (from m in Asns.AsNoTracking()
                              join s in Asnsorts.AsNoTracking() on m.id equals s.asn_id
                              where m.id == id && s.putaway_qty < s.sorted_qty
                              group new { m, s } by new { m.id, m.goods_owner_id, m.goods_owner_name, s.series_number }
                       into g
                              select new AsnPendingPutawayViewModel
                              {
                                  asn_id = g.Key.id,
                                  goods_owner_id = g.Key.goods_owner_id,
                                  goods_owner_name = g.Key.goods_owner_name,
                                  series_number = g.Key.series_number,
                                  sorted_qty = g.Sum(o => o.s.sorted_qty - o.s.putaway_qty)
                              }).ToListAsync();
            return data;
        }

        /// <summary>
        /// PutAway
        /// </summary>
        /// <param name="viewModels">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> PutAwayAsync(List<AsnPutAwayInputViewModel> viewModels, CurrentUser currentUser)
        {
            viewModels.RemoveAll(v => v.putaway_qty < 1);
            if (viewModels.Any(t => t.goods_location_id == 0))
            {
                return (false, "[202]" + string.Format(_stringLocalizer["Required"], _stringLocalizer["location_name"]));
            }
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var Goodslocations = _dBContext.GetDbSet<GoodslocationEntity>();
            var Stocks = _dBContext.GetDbSet<StockEntity>();
            var Asnsorts = _dBContext.GetDbSet<AsnsortEntity>();

            var LocationIdList = viewModels.Where(v => v.goods_location_id > 0)
                                           .Select(v => v.goods_location_id)
                                           .Distinct().ToList();

            var Locations = await Goodslocations.Where(t => LocationIdList.Contains(t.id))
                                                .ToListAsync();
            if (!Locations.Any() || LocationIdList.Count != Locations.Count)
            {
                return (false, "[202]" + string.Format(_stringLocalizer["Required"], _stringLocalizer["location_name"]));
            }
            int sumPutawayQty = viewModels.Sum(v => v.putaway_qty);
            var entity = await Asns.FirstOrDefaultAsync(t => t.id == viewModels[0].asn_id);
            if (entity == null)
            {
                return (false, "[202]" + _stringLocalizer["not_exists_entity"]);
            }
            else if (entity.asn_status != 3)
            {
                return (false, "[202]" + $"{entity.asn_no}{_stringLocalizer["ASN_Status_Is_Not_Sorted"]}");
            }
            else if (entity.actual_qty + sumPutawayQty > entity.sorted_qty)
            {
                return (false, "[202]" + $"{entity.asn_no}{_stringLocalizer["ASN_Total_PutAway_Qty_Greater_Than_Sorted_Qty"]}");
            }
            entity.actual_qty += sumPutawayQty;
            if (entity.actual_qty.Equals(entity.sorted_qty))
            {
                entity.asn_status = 4;
            }
            entity.last_update_time = DateTime.Now;
            // expiry_date
            var expiry_date = entity.expiry_date;

            // 获取已上架数小于分拣数的分拣记录
            var sortEntities = await Asnsorts.Where(t => t.asn_id == viewModels[0].asn_id && t.sorted_qty > t.putaway_qty).ToListAsync();

            foreach (var viewModel in viewModels)
            {
                // 根据sn码，将本次上架数量反写到分拣记录中。如果sn码是空的，则分摊进去
                var sortList = sortEntities.Where(s => s.series_number == viewModel.series_number).ToList();
                if (sortList.Any())
                {
                    int left_putaway_qty = viewModel.putaway_qty;
                    sortList.ForEach(s =>
                    {
                        if (left_putaway_qty > 0)
                        {
                            int can_putaway_qty = s.sorted_qty - s.putaway_qty;
                            if (left_putaway_qty > can_putaway_qty)
                            {
                                s.putaway_qty += can_putaway_qty;
                                left_putaway_qty -= can_putaway_qty;
                            }
                            else
                            {
                                s.putaway_qty += left_putaway_qty;
                                left_putaway_qty = 0;
                            }
                        }
                    });
                }

                var Location = Locations.FirstOrDefault(t => t.id == viewModel.goods_location_id);
                if (Location != null && Location.warehouse_area_property.Equals(5))
                {
                    entity.damage_qty += viewModel.putaway_qty;
                }
                DateTime putaway_date = DateTime.Now.ToString("yyyy-MM-dd").ObjToDate();
                // 2024年3月14日 09:40:25 增加单价
                var stockEntity = await Stocks.FirstOrDefaultAsync(t => t.sku_id.Equals(entity.sku_id)
                                                                              && t.goods_location_id.Equals(viewModel.goods_location_id)
                                                                              && t.goods_owner_id.Equals(viewModel.goods_owner_id)
                                                                              && t.series_number.Equals(viewModel.series_number)
                                                                              && t.expiry_date.Equals(expiry_date)
                                                                              && t.price.Equals(entity.price)
                                                                              && t.putaway_date.Equals(putaway_date)
                                                                              );
                if (stockEntity == null)
                {
                    stockEntity = new StockEntity
                    {
                        sku_id = entity.sku_id,
                        goods_location_id = viewModel.goods_location_id,
                        goods_owner_id = entity.goods_owner_id,
                        series_number = viewModel.series_number,
                        qty = viewModel.putaway_qty,
                        is_freeze = false,
                        last_update_time = DateTime.Now,
                        tenant_id = currentUser.tenant_id,
                        expiry_date = expiry_date,
                        price = entity.price,
                        putaway_date = putaway_date,
                        id = 0
                    };
                    await Stocks.AddAsync(stockEntity);
                }
                else
                {
                    stockEntity.qty += viewModel.putaway_qty;
                    stockEntity.last_update_time = DateTime.Now;
                }
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
            /*

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
            */
        }

        #endregion Flow Api

        #region Arrival list

        /// <summary>
        /// Arrival list
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<AsnmasterBothViewModel> data, int totals)> PageAsnmasterAsync(PageSearch pageSearch, CurrentUser currentUser)
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
                asn_status = Convert.ToByte(pageSearch.sqlTitle.Trim().ToLower().Replace("asn_status", "").Replace("：", "").Replace(":", "").Replace("=", ""));
                asn_status = asn_status.Equals(4) ? (Byte)255 : asn_status;
            }
            var Spus = _dBContext.GetDbSet<SpuEntity>();
            var Skus = _dBContext.GetDbSet<SkuEntity>();
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var Asnmasters = _dBContext.GetDbSet<AsnmasterEntity>();
            var query = from m in Asnmasters.AsNoTracking()
                        where m.tenant_id == currentUser.tenant_id
                        && (asn_status == 255 || m.asn_status == asn_status)
                        select new AsnmasterBothViewModel
                        {
                            id = m.id,
                            asn_no = m.asn_no,
                            asn_batch = m.asn_batch,
                            estimated_arrival_time = m.estimated_arrival_time,
                            asn_status = m.asn_status,
                            weight = m.weight,
                            volume = m.volume,
                            goods_owner_id = m.goods_owner_id,
                            goods_owner_name = m.goods_owner_name,
                            creator = m.creator,
                            create_time = m.create_time,
                            last_update_time = m.last_update_time,
                            detailList = (from a in Asns.AsNoTracking()
                                          join p in Spus.AsNoTracking() on a.spu_id equals p.id
                                          join k in Skus.AsNoTracking() on a.sku_id equals k.id
                                          where a.asnmaster_id == m.id
                                          select new AsnmasterDetailViewModel
                                          {
                                              id = a.id,
                                              asnmaster_id = a.asnmaster_id,
                                              asn_status = a.asn_status,
                                              spu_id = a.spu_id,
                                              spu_code = p.spu_code,
                                              spu_name = p.spu_name,
                                              sku_id = a.sku_id,
                                              sku_code = k.sku_code,
                                              sku_name = k.sku_name,
                                              origin = p.origin,
                                              length_unit = p.length_unit,
                                              volume_unit = p.volume_unit,
                                              weight_unit = p.weight_unit,
                                              asn_qty = a.asn_qty,
                                              actual_qty = a.actual_qty,
                                              weight = a.weight,
                                              volume = a.volume,
                                              supplier_id = a.supplier_id,
                                              supplier_name = a.supplier_name,
                                              is_valid = a.is_valid,
                                              expiry_date = a.expiry_date,
                                              price = a.price,
                                              sorted_qty = a.sorted_qty,
                                          }).ToList()
                        };
            query = query.Where(queries.AsExpression<AsnmasterBothViewModel>());
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.last_update_time)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list, totals);
        }

        /// <summary>
        /// get Arrival list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentUser"></param>
        /// <returns></returns>
        public async Task<AsnmasterBothViewModel> GetAsnmasterAsync(int id, CurrentUser currentUser)
        {
            var Spus = _dBContext.GetDbSet<SpuEntity>();
            var Skus = _dBContext.GetDbSet<SkuEntity>();
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var Asnmasters = _dBContext.GetDbSet<AsnmasterEntity>();
            var query = from m in Asnmasters.AsNoTracking()
                        where m.tenant_id == currentUser.tenant_id
                        && m.id == id
                        select new AsnmasterBothViewModel
                        {
                            id = m.id,
                            asn_no = m.asn_no,
                            asn_batch = m.asn_batch,
                            estimated_arrival_time = m.estimated_arrival_time,
                            asn_status = m.asn_status,
                            weight = m.weight,
                            volume = m.volume,
                            goods_owner_id = m.goods_owner_id,
                            goods_owner_name = m.goods_owner_name,
                            creator = m.creator,
                            create_time = m.create_time,
                            last_update_time = m.last_update_time,
                            detailList = (from a in Asns.AsNoTracking()
                                          join p in Spus.AsNoTracking() on a.spu_id equals p.id
                                          join k in Skus.AsNoTracking() on a.sku_id equals k.id
                                          where a.asnmaster_id == m.id
                                          select new AsnmasterDetailViewModel
                                          {
                                              id = a.id,
                                              asnmaster_id = a.asnmaster_id,
                                              asn_status = a.asn_status,
                                              spu_id = a.spu_id,
                                              spu_code = p.spu_code,
                                              spu_name = p.spu_name,
                                              sku_id = a.sku_id,
                                              sku_code = k.sku_code,
                                              sku_name = k.sku_name,
                                              origin = p.origin,
                                              length_unit = p.length_unit,
                                              volume_unit = p.volume_unit,
                                              weight_unit = p.weight_unit,
                                              asn_qty = a.asn_qty,
                                              actual_qty = a.actual_qty,
                                              weight = a.weight,
                                              volume = a.volume,
                                              supplier_id = a.supplier_id,
                                              supplier_name = a.supplier_name,
                                              is_valid = a.is_valid,
                                              sorted_qty = a.sorted_qty,
                                              expiry_date = a.expiry_date,
                                              price = a.price
                                          }).ToList()
                        };
            var data = await query.FirstOrDefaultAsync();
            return data ?? new AsnmasterBothViewModel();
        }

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsnmasterAsync(AsnmasterBothViewModel viewModel, CurrentUser currentUser)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var Asnmasters = _dBContext.GetDbSet<AsnmasterEntity>();
            string asn_no = await _functionHelper.GetFormNoAsync("Asnmaster");
            var entity = new AsnmasterEntity
            {
                id = 0,
                asn_no = asn_no,
                asn_batch = viewModel.asn_batch,
                estimated_arrival_time = viewModel.estimated_arrival_time,
                asn_status = 0,
                weight = viewModel.weight,
                volume = viewModel.volume,
                goods_owner_id = viewModel.goods_owner_id,
                goods_owner_name = viewModel.goods_owner_name,
                creator = currentUser.user_name,
                create_time = DateTime.Now,
                last_update_time = DateTime.Now,
                tenant_id = currentUser.tenant_id,
                detailList = viewModel.detailList.Select(d => new AsnEntity
                {
                    id = 0,
                    asnmaster_id = 0,
                    asn_no = asn_no,
                    asn_status = 0,
                    spu_id = d.spu_id,
                    sku_id = d.sku_id,
                    asn_qty = d.asn_qty,
                    actual_qty = d.actual_qty,
                    weight = d.weight,
                    volume = d.volume,
                    supplier_id = d.supplier_id,
                    supplier_name = d.supplier_name,
                    goods_owner_id = viewModel.goods_owner_id,
                    goods_owner_name = viewModel.goods_owner_name,
                    creator = currentUser.user_name,
                    create_time = DateTime.Now,
                    last_update_time = DateTime.Now,
                    is_valid = true,
                    tenant_id = currentUser.tenant_id,
                    price = d.price
                }).ToList()
            };
            await Asnmasters.AddAsync(entity);
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
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UpdateAsnmasterAsync(AsnmasterBothViewModel viewModel, CurrentUser currentUser)
        {
            var Asns = _dBContext.GetDbSet<AsnEntity>();
            var Asnmasters = _dBContext.GetDbSet<AsnmasterEntity>();

            var entity = await Asnmasters.Include(t => t.detailList)
                .FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            entity.asn_batch = viewModel.asn_batch;
            entity.estimated_arrival_time = viewModel.estimated_arrival_time;
            entity.weight = viewModel.weight;
            entity.volume = viewModel.volume;
            entity.goods_owner_id = viewModel.goods_owner_id;
            entity.goods_owner_name = viewModel.goods_owner_name;
            entity.last_update_time = DateTime.Now;
            if (viewModel.detailList.Any(t => t.id > 0))
            {
                entity.detailList.ForEach(d =>
                {
                    var vm = viewModel.detailList.Where(t => t.id > 0)
                    .FirstOrDefault(t => t.id == d.id);
                    if (vm != null)
                    {
                        d.spu_id = vm.spu_id;
                        d.sku_id = vm.sku_id;
                        d.asn_qty = vm.asn_qty;
                        d.actual_qty = vm.actual_qty;
                        d.weight = vm.weight;
                        d.volume = vm.volume;
                        d.supplier_id = vm.supplier_id;
                        d.supplier_name = vm.supplier_name;
                        d.goods_owner_id = viewModel.goods_owner_id;
                        d.goods_owner_name = viewModel.goods_owner_name;
                        d.last_update_time = DateTime.Now;
                        d.price = vm.price;
                    }
                });
            }
            if (viewModel.detailList.Any(d => d.id == 0))
            {
                var adds = viewModel.detailList.Where(d => d.id == 0)
                    .Select(d => new AsnEntity
                    {
                        id = 0,
                        asnmaster_id = 0,
                        asn_no = viewModel.asn_no,
                        asn_status = 0,
                        spu_id = d.spu_id,
                        sku_id = d.sku_id,
                        asn_qty = d.asn_qty,
                        actual_qty = d.actual_qty,
                        weight = d.weight,
                        volume = d.volume,
                        supplier_id = d.supplier_id,
                        supplier_name = d.supplier_name,
                        goods_owner_id = viewModel.goods_owner_id,
                        goods_owner_name = viewModel.goods_owner_name,
                        creator = currentUser.user_name,
                        create_time = DateTime.Now,
                        last_update_time = DateTime.Now,
                        is_valid = true,
                        tenant_id = currentUser.tenant_id,
                        price = d.price
                    }).ToList();
                entity.detailList.AddRange(adds);
            }
            if (viewModel.detailList.Any(t => t.id < 0))
            {
                var delIds = viewModel.detailList.Where(t => t.id < 0).Select(t => t.id * -1).ToList();
                entity.detailList.RemoveAll(entity => delIds.Contains(entity.id));
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
        /// delete a record
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> DeleteAsnmasterAsync(int id)
        {
            var qty = await _dBContext.GetDbSet<AsnEntity>().Where(t => t.asnmaster_id.Equals(id)).ExecuteDeleteAsync();
            qty += await _dBContext.GetDbSet<AsnmasterEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
            if (qty > 0)
            {
                return (true, _stringLocalizer["delete_success"]);
            }
            else
            {
                return (false, _stringLocalizer["delete_failed"]);
            }
        }

        #endregion Arrival list

        #region print series number
        /// <summary>
        /// print series number
        /// </summary>
        /// <param name="input">selected asn id</param>
        /// <returns></returns>
        public async Task<List<AsnPrintSeriesNumberViewModel>> GetAsnPrintSeriesNumberAsync(List<int> input)
        {
            var Spus = _dBContext.GetDbSet<SpuEntity>().AsNoTracking();
            var Skus = _dBContext.GetDbSet<SkuEntity>().AsNoTracking();
            var Asns = _dBContext.GetDbSet<AsnEntity>().AsNoTracking();
            var Asnmasters = _dBContext.GetDbSet<AsnmasterEntity>().AsNoTracking();
            var sorts = _dBContext.GetDbSet<AsnsortEntity>().AsNoTracking();

            var query = from m in Asnmasters
                        join a in Asns on m.id equals a.asnmaster_id
                        join p in Spus.AsNoTracking() on a.spu_id equals p.id
                        join k in Skus.AsNoTracking() on a.sku_id equals k.id
                        join s in sorts on a.id equals s.asn_id
                        where input.Contains(a.id)
                        select new AsnPrintSeriesNumberViewModel
                        {
                            asn_id = a.id,
                            asnmaster_id = m.id,
                            asn_no = m.asn_no,
                            sku_id = a.sku_id,
                            sku_code = k.sku_code,
                            sku_name = k.sku_name,
                            spu_code = p.spu_code,
                            spu_name = p.spu_name,
                            series_number = s.series_number
                        };
            var data = await query.OrderBy(t => t.asn_id).ToListAsync();
            data ??= new List<AsnPrintSeriesNumberViewModel>();
            return data;
        }

        #endregion
    }
}