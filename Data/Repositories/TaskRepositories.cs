using Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalExamModels;

namespace Data.Repositories
{
    public interface ITaskRepository : IRepository<WorkTask>
    {

    }
    public class TaskRepositories : RepositoryBase<WorkTask>, ITaskRepository
    {
        public TaskRepositories(IDbFactory DbFactory) : base(DbFactory) { }

    }
}
