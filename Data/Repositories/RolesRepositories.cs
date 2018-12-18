using Data.Infrastructure;
using FinalExamModel.FinalExamModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IRolesRepository : IRepository<Roles>
    {

    }
    public class RolesRepositories : RepositoryBase<Roles>, IRolesRepository
    {
        public RolesRepositories(IDbFactory DbFactory) : base(DbFactory) { }

    }


}
