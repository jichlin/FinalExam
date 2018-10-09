using Data.Infrastructure;
using FinalExamModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IUserRepository : IRepository<User>
    {

    } 
    public class UserRepository : RepositoryBase<User> , IUserRepository
    {
        public UserRepository(IDbFactory DbFactory) : base(DbFactory){ }
    }
}
