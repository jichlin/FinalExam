using Data.Repositories;
using FinalExamModel.FinalExamModels.View_Model;
using FinalExamModels;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalExamService
{

    public interface IProjectService
    {
        Task<IEnumerable<Projects>> GetProjectsData(int IDProject = 0);
        Task<IEnumerable<Projects>> GetAllProjects();
        Task<ExecuteResult> InsertProject(Projects x);
        Task<ExecuteResult> InsertDeleteUserAuthorization(List<AuthorizeUser> x);
        Task<Projects> GetProjectByProjectID(int ProjectID);


    }

    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository ProjectRepository;

        public ProjectService(IProjectRepository ProjectRepository)
        {
            this.ProjectRepository = ProjectRepository;
        }

        public async Task<IEnumerable<Projects>> GetAllProjects()
        {
            return await ProjectRepository.ExecSPToListAsync("sp_getProjectsforAdmin");
        }

        public async Task<Projects> GetProjectByProjectID(int ProjectID)
        {
            return await ProjectRepository.ExecSPToSingleAsync("sp_getProjectByProjectID '" + ProjectID + "'");
        }

        public async Task<IEnumerable<Projects>> GetProjectsData(int UserID = 0)
        {
            return await ProjectRepository.ExecSPToListAsync("sp_getProjectByUserID " + "'" + UserID + "'");
        }

        public async Task<ExecuteResult> InsertDeleteUserAuthorization(List<AuthorizeUser> x)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> Data = new List<StoredProcedure>();

            foreach(AuthorizeUser user in x)
            {
                Data.Add(new StoredProcedure
                {
                    SPName = "AddDeleteAuthorization " +
                   "@ProjectID = '" + user.ProjectID
                   + "' ," + "@UserID = '" + user.User.UserID
                   + "' ," + "@Auth = '" + user.Authorize + "'"
                });
            }
           

            ReturnValue = await ProjectRepository.ExecMultipleSPWithTransaction(Data);
            return ReturnValue;

        }

        public async Task<ExecuteResult> InsertProject(Projects project)
        {
            ExecuteResult ReturnValue = new ExecuteResult();
            List<StoredProcedure> Data = new List<StoredProcedure>();

            Data.Add(new StoredProcedure
            {
                SPName = "sp_InsertProject "+
                    "@ProjectName = '" + project.ProjectName + 
                    "' ," + "@ProjectDesc = '" + project.ProjectDesc + "'"
                });

            ReturnValue = await ProjectRepository.ExecMultipleSPWithTransaction(Data);
            return ReturnValue;
        }
    }
}
