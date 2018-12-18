using Data.Infrastructure;
using FinalExamModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IStateRepository : IRepository<States>
    {

    }

    public class StateRepositories : RepositoryBase<States>, IStateRepository
    {
        public StateRepositories(IDbFactory DbFactory) : base(DbFactory)
        {

        }

    }
}
