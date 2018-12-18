using Data.Repositories;
using FinalExamModel.FinalExamModels;
using FinalExamModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamService
{

    public interface IStateService
    {
        Task<IEnumerable<States>> getState();
    }

    public class StateService : IStateService
    {
        private readonly IStateRepository StateRepository;

        public StateService(IStateRepository StateRepository)
        {
            this.StateRepository = StateRepository;
        }

        Task<IEnumerable<States>> IStateService.getState()
        {
            return StateRepository.ExecSPToListAsync("sp_getStates");
        }
    }
}
