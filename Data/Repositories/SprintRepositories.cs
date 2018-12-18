using Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalExamModels;

namespace Data.Repositories
{
    public interface  ISprintRepository : IRepository<Sprint>
    {

    }
    public class SprintRepositories : RepositoryBase<Sprint>, ISprintRepository
    {
        public SprintRepositories(IDbFactory DbFactory) : base(DbFactory) { }

    }
}
