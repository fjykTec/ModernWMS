/*
 * date：2023-08-18
 * developer：NoNo
 */

using ModernWMS.WMS.Entities.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using ModernWMS.Core.DBContext;
using ModernWMS.Core.DynamicSearch;
using ModernWMS.Core.JWT;
using ModernWMS.Core.Models;
using ModernWMS.Core.Services;
using ModernWMS.WMS.Entities.ViewModels;
using ModernWMS.WMS.IServices;
using ModernWMS.WMS.Entities.Models.Approve;
using ModernWMS.WMS.Entities.ViewModels.Approve;

namespace ModernWMS.WMS.Services
{
    /// <summary>
    /// FlowSet Service
    /// </summary>
    public class FlowSetService : BaseService<FlowSetEntity>
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
        /// FlowSet  constructor
        /// </summary>
        /// <param name="dBContext">The DBContext</param>
        /// <param name="stringLocalizer">Localizer</param>
        public FlowSetService(
            SqlDBContext dBContext
          , IStringLocalizer<ModernWMS.Core.MultiLanguage> stringLocalizer
            )
        {
            this._dBContext = dBContext;
            this._stringLocalizer = stringLocalizer;
        }

        #endregion constructor

        /// <summary>
        /// get flowset map by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<FlowSetMapGetViewModel> GetFlowSetMap(int id)
        {
            var main_data = await _dBContext.Set<FlowSetMainEntity>().FirstOrDefaultAsync(t => t.id == id);
            if (main_data == null)
            {
                return null;
            }

            var flowset_data = await _dBContext.Set<FlowSetEntity>().Where(t => t.flowsetmain_id == main_data.id).ToListAsync();
            var user_data = await (from fsu in _dBContext.Set<FlowSetUserEntity>().AsNoTracking()
                                   join user in _dBContext.Set<userEntity>().AsNoTracking() on fsu.user_id equals user.id
                                   where fsu.flowsetmain_id == main_data.id
                                   select new FlowSetUserViewModel
                                   {
                                       id = fsu.id,
                                       flowset_id = fsu.flowset_id,
                                       menu = main_data.menu,
                                       node_guid = fsu.node_guid,
                                       user_id = fsu.user_id,
                                       user_name = user.user_name
                                   }
                                   ).ToListAsync();
            var filter_data = await (from fsf in _dBContext.Set<FlowSetFilterEntity>().AsNoTracking().Where(t => t.flowsetmain_id == main_data.id)
                                     select new FlowSetConditionViewModel
                                     {
                                         id = fsf.id,
                                         flowset_id = fsf.flowset_id,
                                         scheme_name = fsf.scheme_name,
                                         table_name = fsf.table_name,
                                         node_guid = fsf.node_guid,
                                         menu = main_data.menu,
                                         logic = fsf.logic,
                                         c1 = fsf.c1,
                                         c2 = fsf.c2,
                                         col_label = fsf.col_label,
                                         col_name = fsf.col_name,
                                         compare = fsf.compare,
                                         condition_group = fsf.condition_group,
                                         assert_mode = fsf.assert_mode,
                                         formulas = fsf.formulas,
                                         content = fsf.content,
                                         sort = fsf.sort,
                                     }
                                    ).ToListAsync();
            var flowset_vm = flowset_data.Adapt<List<FlowSetMapGetViewModel>>().ToList();
            foreach (var flowset in flowset_vm)
            {
                flowset.user_list = user_data.Where(t => t.node_guid == flowset.node_guid).ToList();
                flowset.filter_list = filter_data.Where(t => t.node_guid == flowset.node_guid).ToList();
            }
            var flow_list = BuildFlow(flowset_vm);
            var res = new FlowSetMapGetViewModel();
            if (flow_list.Count() > 0)
            {
                res = flow_list.FirstOrDefault();
            }
            return res;
        }

        /// <summary>
        /// build flow
        /// </summary>
        /// <param name="fsm_lsit">FlowSetMap List</param>
        /// <param name="prev_node">prev node guid</param>
        /// <returns></returns>
        public List<FlowSetMapGetViewModel> BuildFlow(List<FlowSetMapGetViewModel> fsm_lsit, string? prev_node = "")
        {
            List<FlowSetMapGetViewModel> flowNodes = new List<FlowSetMapGetViewModel>();
            foreach (var fsm in fsm_lsit)
            {
                if (fsm.prev_node_guid == prev_node)
                {
                    var childNodes = BuildFlow(fsm_lsit, fsm.prev_node_guid);
                    fsm.children = childNodes;
                    flowNodes.Add(fsm);
                }
            }
            return flowNodes;
        }
    }
}