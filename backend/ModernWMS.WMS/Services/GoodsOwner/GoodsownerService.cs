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
    /// Goods owner Service
    /// </summary>
    public class GoodsownerService : BaseService<GoodsownerEntity>, IGoodsownerService
    {
        #region Args
        /// <summary>
        /// The DBContext
        /// </summary>
        private readonly SqlDBContext _dBContext;

        /// <summary>
        /// Localization
        /// </summary>
        private readonly IStringLocalizer<Core.MultiLanguage> _stringLocalizer;
        #endregion

        #region constructor
        /// <summary>
        /// company service constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localization</param>
        public GoodsownerService(
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
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<GoodsownerViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var DbSet = _dBContext.GetDbSet<GoodsownerEntity>();
            var query = DbSet.AsNoTracking()
                .Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                .Where(queries.AsExpression<GoodsownerEntity>());
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.create_time)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list.Adapt<List<GoodsownerViewModel>>(), totals);
        }
        /// <summary>
        /// Get all records
        /// </summary>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<List<GoodsownerViewModel>> GetAllAsync(CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<GoodsownerEntity>();
            var data = await DbSet.AsNoTracking().Where(t => t.tenant_id == currentUser.tenant_id)
                .OrderByDescending(t => t.create_time).ToListAsync();
            return data.Adapt<List<GoodsownerViewModel>>();
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<GoodsownerViewModel> GetAsync(int id)
        {
            var entity = await _dBContext.GetDbSet<GoodsownerEntity>().AsNoTracking().FirstOrDefaultAsync(t => t.id.Equals(id));
            if (entity != null)
            {
                return entity.Adapt<GoodsownerViewModel>();
            }
            else
            {
                return new GoodsownerViewModel();
            }
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(GoodsownerViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<GoodsownerEntity>();
            if (await DbSet.AnyAsync(t => t.tenant_id.Equals(currentUser.tenant_id) && t.goods_owner_name.Equals(viewModel.goods_owner_name)))
            {
                return (0, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["goods_owner_name"], viewModel.goods_owner_name));
            }
            var entity = viewModel.Adapt<GoodsownerEntity>();
            entity.id = 0;
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
        /// update a record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UpdateAsync(GoodsownerViewModel viewModel)
        {
            var DbSet = _dBContext.GetDbSet<GoodsownerEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            if (await DbSet.AnyAsync(t => !t.id.Equals(viewModel.id) && t.tenant_id.Equals(entity.tenant_id) && t.goods_owner_name.Equals(viewModel.goods_owner_name)))
            {
                return (false, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["goods_owner_name"], viewModel.goods_owner_name));
            }
            entity.goods_owner_name = viewModel.goods_owner_name;
            entity.city = viewModel.city;
            entity.address = viewModel.address;
            entity.manager = viewModel.manager;
            entity.contact_tel = viewModel.contact_tel;
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
            var qty = await _dBContext.GetDbSet<GoodsownerEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
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


        #region Import
        /// <summary>
        /// import goodsowners by excel
        /// </summary>
        /// <param name="input">excel data</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(bool flag, List<GoodsownerImportViewModel> errorData)> ExcelAsync(List<GoodsownerImportViewModel> input, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<GoodsownerEntity>();
            var existsDatas = await DbSet.AsNoTracking().Where(t => t.tenant_id.Equals(currentUser.tenant_id)).Select(t => new { t.goods_owner_name }).ToListAsync();
            input.ForEach(async t =>
            {
                t.errorMsg = string.Empty;
                if (existsDatas.Any(d => d.goods_owner_name.Equals(t.goods_owner_name)))
                {
                    t.errorMsg = string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["goods_owner_name"], t.goods_owner_name);
                }
                else
                {
                    await DbSet.AddAsync(new GoodsownerEntity
                    {
                        goods_owner_name = t.goods_owner_name,
                        city = t.city,
                        address = t.address,
                        manager = t.manager,
                        contact_tel = t.contact_tel,
                        creator = currentUser.user_name,
                        create_time = DateTime.Now,
                        last_update_time = DateTime.Now,
                        is_valid = true,
                        tenant_id = currentUser.tenant_id
                    });
                }
            });
            if (input.Any(t => t.errorMsg.Length > 0))
            {
                return (false, input.Where(t => t.errorMsg.Length > 0).ToList());
            }
            var qty = await _dBContext.SaveChangesAsync();
            if (qty > 0)
            {
                return (true, new List<GoodsownerImportViewModel>());
            }
            else
            {
                return (false, new List<GoodsownerImportViewModel>());
            }
        }

        #endregion
    }
}
