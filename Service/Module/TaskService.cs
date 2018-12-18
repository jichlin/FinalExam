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

    public interface ITaskService
    {
        Task<IEnumerable<WorkTask>> getTasksByWorkItemID(int WorkItemID);
        Task<WorkTask> getTasksByTaskID(int TaskID);
        Task<ExecuteResult> InsertUpdateWorkTask(WorkTask task);
        Task<ExecuteResult> DeleteWorkTask(int TaskID);



    }

    public class TaskService : ITaskService
    {
        private readonly ITaskRepository TaskRepository;

        public TaskService(ITaskRepository TaskRepository)
        {
            this.TaskRepository = TaskRepository;
        }

        public async Task<WorkTask> getTasksByTaskID(int TaskID)
        {
            return await TaskRepository.ExecSPToSingleAsync("sp_GetWorkTaskByWorkTaskID '" + TaskID + "'");
        }

        public async Task<IEnumerable<WorkTask>> getTasksByWorkItemID(int WorkItemID)
        {
            return await TaskRepository.ExecSPToListAsync("sp_getTaskByWorkItemID '" + WorkItemID + "'");
        }

        public async Task<ExecuteResult> InsertUpdateWorkTask(WorkTask task)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> Data = new List<StoredProcedure>();

            Data.Add(new StoredProcedure
            {
                SPName = "sp_InsertUpdateWorkTask " +
                    "@WorkItemID = '" + task.WorkItemID +
                    "' ," + "@TaskID = '" + task.TaskID + "'" +
                    " ," + "@TaskName = '" + task.TaskName + "'" +
                    " ," + "@StateID = '" + task.StateID + "'" +
                    " ," + "@UserID = '" + task.UserID + "'"
            });

            ReturnValue = await TaskRepository.ExecMultipleSPWithTransaction(Data);
            return ReturnValue;
        }

        public async Task<ExecuteResult> DeleteWorkTask(int TaskID)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> Data = new List<StoredProcedure>();

            Data.Add(new StoredProcedure
            {
                SPName = "sp_DeleteTaskByTaskID " +
                    "@TaskID = '" + TaskID + "'"

            });

            ReturnValue = await TaskRepository.ExecMultipleSPWithTransaction(Data);
            return ReturnValue;

        }
    }
}
