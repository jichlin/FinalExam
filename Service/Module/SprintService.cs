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

    public interface ISprintService
    {
        Task<IEnumerable<Sprint>> GetSprintDataByProjectID(int ProjectID);
        Task<ExecuteResult> UpdateSprint(Sprint sprint);
        Task<Sprint> GetSprintBySprintID(int SprintID);

    }

    public class SprintService : ISprintService
    {
        private readonly ISprintRepository SprintRepository;

        public SprintService(ISprintRepository SprintRepository)
        {
            this.SprintRepository = SprintRepository;
        }


        public async Task<IEnumerable<Sprint>> GetSprintDataByProjectID(int ProjectID)
        {
            return await SprintRepository.ExecSPToListAsync("sp_getSprintByProjectID '" + ProjectID + "'");
        }

        public async Task<Sprint> GetSprintBySprintID(int SprintID)
        {
            return await SprintRepository.ExecSPToSingleAsync("sp_getSprintBySprintID '" + SprintID + "'");
        }

        public async Task<ExecuteResult> UpdateSprint(Sprint sprint)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> Data = new List<StoredProcedure>();

            Data.Add(new StoredProcedure
            {
                SPName = "sp_UpdateSprint " +
               "@SprintID = '" + sprint.SprintID
               + "' ," + "@SprintName = '" + sprint.SprintName
               + "' ," + "@SprintStartDate = '" + sprint.SprintStartDate
               + "' ," + "@SprintEndDate = '" + sprint.SprintEndDate + "'"

            });


            ReturnValue = await SprintRepository.ExecMultipleSPWithTransaction(Data);
            return ReturnValue;

        }
    }
}
