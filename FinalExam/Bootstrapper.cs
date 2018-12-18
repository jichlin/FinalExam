using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using Data.Repositories;
using FinalExamService;
using Data.Infrastructure;

namespace FinalExam
{
    public static class Bootstrapper
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            //Init DB
            container.RegisterType<IDbFactory, DBFactory>();
            //Init Service
            container.RegisterType<IUserRepository, UserRepository>();
            container.RegisterType<IUserService, UserService>();

            container.RegisterType<IRolesRepository, RolesRepositories>();
            container.RegisterType<IRolesService, RolesService>();

            container.RegisterType<IProjectRepository, ProjectRepositories>();
            container.RegisterType<IProjectService, ProjectService>();

            container.RegisterType<ISprintRepository, SprintRepositories>();
            container.RegisterType<ISprintService, SprintService>();

            container.RegisterType<ITaskRepository, TaskRepositories>();
            container.RegisterType<ITaskService, TaskService>();

            container.RegisterType<IWorkItemRepository, WorkItemRepositories>();
            container.RegisterType<IWorkItemService, WorkItemService>();

            container.RegisterType<IStateRepository, StateRepositories>();
            container.RegisterType<IStateService, StateService>();



            container.RegisterType<ILoginRepository, LoginRepositories>();


            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();    
            RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }
    }
}