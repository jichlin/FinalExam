using Data.Repositories;
using Model;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserService
    {
        Task<User> GetUserData(int IDUser = 0);
        Task<ExecuteResult> InsertUpdateUser(User user);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository UserRepository;

        public UserService(IUserRepository UserRepository)
        {
            this.UserRepository = UserRepository;
        }


        public async Task<User> GetUserData(int IDUser = 0)
        {
            var param = new[] { new SqlParameter("@IDAnggota", IDUser) };
            return await UserRepository.ExecSPToSingleAsync("ex_GetUser",param);
        }

        public Task<ExecuteResult> InsertUpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
