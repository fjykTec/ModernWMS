/*
 * date：2023-09-11
 * developer：NoNo
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

namespace ModernWMS.WMS.Services
{
    /// <summary>
    ///  User_defined_print_solution Service
    /// </summary>
    public class PrintSolutionService : BaseService<PrintSolutionEntity>, IPrintSolutionService
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

        #endregion Args

        #region constructor

        /// <summary>
        ///User_defined_print_solution  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public PrintSolutionService(
            SqlDBContext dBContext
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            )
        {
            this._dBContext = dBContext;
            this._stringLocalizer = stringLocalizer;
        }

        #endregion constructor

        #region Api

        /// <summary>
        /// page search
        /// </summary>
        /// <param name="pageSearch">args</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<(List<PrintSolutionViewModel> data, int totals)> PageAsync(PageSearch pageSearch, CurrentUser currentUser)
        {
            QueryCollection queries = new QueryCollection();
            if (pageSearch.searchObjects.Any())
            {
                pageSearch.searchObjects.ForEach(s =>
                {
                    queries.Add(s);
                });
            }
            var DbSet = _dBContext.GetDbSet<PrintSolutionEntity>();
            var query = DbSet.AsNoTracking()
                .Where(t => t.tenant_id.Equals(currentUser.tenant_id))
                .Where(queries.AsExpression<PrintSolutionEntity>());
            int totals = await query.CountAsync();
            var list = await query.OrderByDescending(t => t.id)
                       .Skip((pageSearch.pageIndex - 1) * pageSearch.pageSize)
                       .Take(pageSearch.pageSize)
                       .ToListAsync();
            return (list.Adapt<List<PrintSolutionViewModel>>(), totals);
        }

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        public async Task<List<PrintSolutionViewModel>> GetAllAsync(CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<PrintSolutionEntity>();
            var data = await DbSet.AsNoTracking().Where(t => t.tenant_id.Equals(currentUser.tenant_id)).ToListAsync();
            return data.Adapt<List<PrintSolutionViewModel>>();
        }

        /// <summary>
        /// Get a record by id
        /// </summary>
        /// <returns></returns>
        public async Task<PrintSolutionViewModel> GetAsync(int id)
        {
            var DbSet = _dBContext.GetDbSet<PrintSolutionEntity>();
            var entity = await DbSet.AsNoTracking().FirstOrDefaultAsync(t => t.id.Equals(id));
            if (entity == null)
            {
                return null;
            }
            return entity.Adapt<PrintSolutionViewModel>();
        }

        /// <summary>
        /// get a record  by path
        /// </summary>
        /// <param name="input">input</param>
        /// <param name="currentUser">currentUser</param>
        /// <returns></returns>
        public async Task<List<PrintSolutionViewModel>> GetByPathAsync(PrintSolutionGetByPathInputViewModel input, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<PrintSolutionEntity>();
            var entity = await DbSet.AsNoTracking().Where(t => t.tenant_id.Equals(currentUser.tenant_id) && t.vue_path.Equals(input.vue_path) && t.tab_page.Equals(input.tab_page)).ToListAsync();
            if (entity == null)
            {
                return null;
            }
            return entity.Adapt<List<PrintSolutionViewModel>>();
        }

        /// <summary>
        /// add a new record
        /// </summary>
        /// <param name="viewModel">viewmodel</param>
        /// <returns></returns>
        public async Task<(int id, string msg)> AddAsync(PrintSolutionViewModel viewModel, CurrentUser currentUser)
        {
            var DbSet = _dBContext.GetDbSet<PrintSolutionEntity>();
            var entity = viewModel.Adapt<PrintSolutionEntity>();
            entity.id = 0;
            entity.tenant_id = currentUser.tenant_id;
            entity.last_update_time = DateTime.Now;
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
        public async Task<(bool flag, string msg)> UpdateAsync(PrintSolutionViewModel viewModel)
        {
            var DbSet = _dBContext.GetDbSet<PrintSolutionEntity>();
            var entity = await DbSet.FirstOrDefaultAsync(t => t.id.Equals(viewModel.id));
            if (entity == null)
            {
                return (false, _stringLocalizer["not_exists_entity"]);
            }
            entity.vue_path = viewModel.vue_path;
            entity.tab_page = viewModel.tab_page;
            entity.solution_name = viewModel.solution_name;
            entity.config_json = viewModel.config_json;
            entity.report_length = viewModel.report_length;
            entity.report_width = viewModel.report_width;
            entity.report_direction = viewModel.report_direction;
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
            var qty = await _dBContext.GetDbSet<PrintSolutionEntity>().Where(t => t.id.Equals(id)).ExecuteDeleteAsync();
            if (qty > 0)
            {
                return (true, _stringLocalizer["delete_success"]);
            }
            else
            {
                return (false, _stringLocalizer["delete_failed"]);
            }
        }

        #endregion Api
    }
}