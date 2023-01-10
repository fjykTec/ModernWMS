/*
 * date：2022-12-20
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
using ModernWMS.Core.JWT;
using System.Collections.Immutable;
using System.Collections.Generic;

namespace ModernWMS.WMS.Services
 {
    /// <summary>
    ///  Category Service
    /// </summary>
    public class CategoryService : BaseService<CategoryEntity>, ICategoryService
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
        ///Category  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public CategoryService(
            SqlDBContext dBContext
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            )
        {
            this._dBContext = dBContext;
            this._stringLocalizer = stringLocalizer;
        }
        #endregion

        #region Api
        /// <summary>
        /// Get all records
        /// </summary>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<List<CategoryViewModel>> GetAllAsync(CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<CategoryEntity>();
            var data = await DbSet.AsNoTracking()
               .Where(t => t.tenant_id.Equals(currentUser.tenant_id))
               .ToListAsync();
            return data.Adapt<List<CategoryViewModel>>();
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <returns></returns>
        public async Task<CategoryViewModel> GetAsync(int id)
        {
            var DbSet = _dBContext.GetDbSet<CategoryEntity>();
            var entity = await DbSet.AsNoTracking().FirstOrDefaultAsync(t => t.id.Equals(id));
            if (entity == null)
            {
                return new CategoryViewModel();
            }
            return entity.Adapt<CategoryViewModel>();
        }
        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(CategoryViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<CategoryEntity>();
            if (await DbSet.AsNoTracking().AnyAsync(t => t.tenant_id.Equals(currentUser.tenant_id) && t.category_name.Equals(viewModel.category_name)))
            {
                return (0, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["category_name"], viewModel.category_name));
            }
            var entity = viewModel.Adapt<CategoryEntity>();
            entity.id = 0;
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
        /// update a record
        /// </summary>
        /// <param name="viewModel">args</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> UpdateAsync(CategoryViewModel viewModel)
        {
            var DbSet = _dBContext.GetDbSet<CategoryEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            if (await DbSet.AsNoTracking().AnyAsync(t => !t.id.Equals(viewModel.id) && t.tenant_id.Equals(entity.tenant_id) && t.category_name.Equals(viewModel.category_name)))
            {
                return (false, string.Format(_stringLocalizer["exists_entity"], _stringLocalizer["category_name"], viewModel.category_name));
            }
            entity.id = viewModel.id;
            entity.category_name = viewModel.category_name;
            entity.parent_id = viewModel.parent_id;
            entity.last_update_time = DateTime.Now;
            if (!viewModel.is_valid.Equals(entity.is_valid))
            {
                var entities = await DbSet.Where(t => t.parent_id > 0).ToListAsync();
                List<CategoryEntity> children = new List<CategoryEntity>();
                GetChildren(entities, entity.id, ref children);
                if (children.Any())
                {
                    children.ForEach(c =>
                    {
                        c.is_valid = viewModel.is_valid;
                        c.last_update_time = DateTime.Now;
                    });
                }
            }
            entity.is_valid = viewModel.is_valid;
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
        private void GetChildren(List<CategoryEntity> entities, int parentId, ref List<CategoryEntity> children)
        {
            var data = entities.Where(t => t.parent_id== parentId).ToList();
            if (data.Any())
            {
                foreach (var item in data)
                {
                    children.Add(item);
                    if (entities.Any(t => t.parent_id.Equals(item.id)))
                    {
                        GetChildren(entities, item.parent_id, ref children);
                    }
                }
            }
        }
        /// <summary>
        /// delete a record
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public async Task<(bool flag, string msg)> DeleteAsync(int id)
        {
            var DbSet = _dBContext.GetDbSet<CategoryEntity>();
            var entities = await DbSet.Where(t => t.parent_id.Equals(id)).ToListAsync();
            List<CategoryEntity> children = new List<CategoryEntity>();
            GetChildren(entities, id, ref children);
            List<int> idList = new List<int> { id };
            if (children.Any())
            {
                idList.AddRange(children.Select(t => t.id).ToList());
            }
            // 判断是否引用
            var Spus = _dBContext.GetDbSet<SpuEntity>();
            if (await Spus.AsNoTracking().AnyAsync(t => idList.Contains(t.category_id)))
            {
                return (false, _stringLocalizer["delete_referenced"]);
            }
            var qty = await _dBContext.GetDbSet<CategoryEntity>().Where(t => idList.Contains(t.id)).ExecuteDeleteAsync();
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
 
