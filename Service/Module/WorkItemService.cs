using Data.Repositories;
using FinalExamModels;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamService
{
    public interface IWorkItemService
    {
        Task<WorkItem> getWorkItemByWorkItemID(int WorkItemID);
        Task<IEnumerable<WorkItem>> getWorkItemBySprintID(int SprintID);
        Task<ExecuteResult> InsertUpdateWorkItems(WorkItem workItem);
        Task<ExecuteResult> DeleteWorkItem(int WorkTask);

    }

    public class WorkItemService: IWorkItemService
    {
        private readonly IWorkItemRepository WorkItemRepository;

        public WorkItemService(IWorkItemRepository WorkItemRepository)
        {
            this.WorkItemRepository = WorkItemRepository;
        }

        public async Task<ExecuteResult> DeleteWorkItem(int WorkItemID)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> Data = new List<StoredProcedure>();

            Data.Add(new StoredProcedure
            {
                SPName = "sp_DeleteWorkItemByWorkItemID " +
                    "@WorkItemID = '" + WorkItemID + "'"

            });

            ReturnValue = await WorkItemRepository.ExecMultipleSPWithTransaction(Data);
            return ReturnValue;

        }

        public async Task<IEnumerable<WorkItem>> getWorkItemBySprintID(int SprintID)
        {
            return await WorkItemRepository.ExecSPToListAsync("sp_getWorkItemBySprintID '" + SprintID + "'");
        }

        public async Task<WorkItem> getWorkItemByWorkItemID(int WorkItemID)
        {
            return await WorkItemRepository.ExecSPToSingleAsync("sp_getWorkItemByWorkItemID '" + WorkItemID + "'");
        }

        public async Task<ExecuteResult> InsertUpdateWorkItems(WorkItem workItem)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> Data = new List<StoredProcedure>();

            Data.Add(new StoredProcedure
            {
                SPName = "sp_InsertUpdateWorkItem " +
                    "@SprintID = '" + workItem.SprintID + "'" +
                    " ," + "@WorkItemID = '" + workItem.WorkItemID + "'" +
                    " ," + "@WorkItemName = '" + workItem.WorkItemName + "'" +
                    " ," + "@StateID = '" + workItem.StateID + "'"

            });

            ReturnValue = await WorkItemRepository.ExecMultipleSPWithTransaction(Data);
            return ReturnValue;
        }
    }
}
