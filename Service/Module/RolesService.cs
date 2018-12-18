using Data.Repositories;
using FinalExamModel.FinalExamModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamService
{

    public interface IRolesService
    {
        Task<IEnumerable<Roles>> getRoles();
    }

    public class RolesService : IRolesService
    {
        private readonly IRolesRepository RoleRepository;

        public RolesService(IRolesRepository RoleRepository)
        {
            this.RoleRepository = RoleRepository;
        }

        public async Task<IEnumerable<Roles>> getRoles()
        {

            return await RoleRepository.ExecSPToListAsync("sp_getRoles");

        }

    }
}
