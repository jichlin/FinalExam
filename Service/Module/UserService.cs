using Data.Repositories;
using FinalExamModel.FinalExamModels.View_Model;
using FinalExamModels;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamService
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUserData(int IDUser = 0);
        Task<ExecuteResult> InsertUpdateUser(User user);
        Task<IEnumerable<User>> GetUserByProjectID(int IDProject);
        Task<Login> GetUserLogin(Login user);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository UserRepository;
        private readonly ILoginRepository LoginRepository;

        public UserService(IUserRepository UserRepository , ILoginRepository LoginRepository)
        {
            this.UserRepository = UserRepository;
            this.LoginRepository = LoginRepository;
        }


        public async Task<IEnumerable<User>> GetUserData(int IDUser = 0)
        {
            return await UserRepository.ExecSPToListAsync("sp_GetUser " + "'" + IDUser + "'");
        }

        public async Task<ExecuteResult> InsertUpdateUser(User user)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> Data = new List<StoredProcedure>();

            Data.Add(new StoredProcedure
            {
                SPName = "sp_InsertUpdateUser "
                + " @UserID = '" + user.UserID
                + "', @Username = '" + user.UserName
                + "', @Userpassword = '" + user.UserPassword
                + "', @RolesID = '" + user.RolesID
                + "', @UserStatus = '" + user.UserStatus
                + "', @ImagePath = '" + user.ImagePath + "'"
            });

            ReturnValue = await UserRepository.ExecMultipleSPWithTransaction(Data);
            return ReturnValue;

        }

        public async Task<Login> GetUserLogin(Login user)
        {
            return await LoginRepository.ExecSPToSingleAsync("sp_UserLogin " + "'" + user.Username + "', " + "'" + user.Password + "'");
        }

        public async Task<IEnumerable<User>> GetUserByProjectID(int IDProject)
        {
            return await UserRepository.ExecSPToListAsync("sp_GetUserByProjectID " + "'" + IDProject + "'");
        }
    }
}
