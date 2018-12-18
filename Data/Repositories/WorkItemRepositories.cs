using Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalExamModel.FinalExamModels;
using FinalExamModels;

namespace Data.Repositories
{
    public interface IWorkItemRepository : IRepository<WorkItem>
    {

    }
    public class WorkItemRepositories : RepositoryBase<WorkItem>, IWorkItemRepository
    {
        public WorkItemRepositories(IDbFactory DbFactory) : base(DbFactory) { }

    }
}
