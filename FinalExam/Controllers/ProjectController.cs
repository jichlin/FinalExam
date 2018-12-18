using FinalExamModel.FinalExamModels;
using FinalExamModel.FinalExamModels.View_Model;
using FinalExamModels;
using FinalExamService;
using FinalExamService.Helper;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FinalExam.Controllers
{
    public class ProjectController : BaseController
    {

        private readonly IProjectService ProjectService;
        private readonly IUserService UserService;
        private readonly ISprintService SprintService;
        private readonly IWorkItemService WorkItemService;
        private readonly ITaskService TaskService;
        private readonly IStateService StateService;




        public ProjectController(IProjectService ProjectService , IUserService UserService , ISprintService SprintService , ITaskService TaskService,
            IWorkItemService WorkItemService , IStateService StateService)
        {
            this.SprintService = SprintService;
            this.ProjectService = ProjectService;
            this.UserService = UserService;
            this.TaskService = TaskService;
            this.WorkItemService = WorkItemService;
            this.StateService = StateService;
        }

        // GET: Project
        public async Task<ActionResult> Index()
        {
            string role = Session[SessionEnum.ROLEUSER].ToString();
            IEnumerable<Projects> projects;
            if (role == "2")
            {
                projects = await ProjectService.GetAllProjects();
            }
            else
            {
                string id = Session[SessionEnum.IDUSER].ToString();
                int userID = Int32.Parse(id);
                projects = await ProjectService.GetProjectsData(userID);
            }
            return View(projects);
        }

        [HttpGet]
        public async Task<ActionResult> addProject()
        {
            return PartialView("_AddProjects");
        }

        [HttpPost]
        public async Task<ActionResult> addProject(Projects project)
        {

            ExecuteResult r = await ProjectService.InsertProject(project);

            IEnumerable<Projects> projects;
            string role = Session[SessionEnum.ROLEUSER].ToString();
            if (role == "2")
            {
                projects = await ProjectService.GetAllProjects();
            }
            else
            {
                string id = Session[SessionEnum.IDUSER].ToString();
                int userID = Int32.Parse(id);
                projects = await ProjectService.GetProjectsData(userID);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> changeAuthorization(int projectID)
        {
            IEnumerable<User> Users = await UserService.GetUserByProjectID(projectID);
            IEnumerable<User> allusers = await UserService.GetUserData();
            List<AuthorizeUser> listOfAuthorizedUser = new List<AuthorizeUser>();
            foreach (User x in allusers)
            {
                if (x.ImagePath != "")
                {
                    string a = "/" + x.ImagePath.Substring(x.ImagePath.IndexOf("ProfilePic"));
                    a.Replace('\\', '/');
                    x.ImagePath = a;
                }

                AuthorizeUser auth = new AuthorizeUser();
                auth.User = x;
                auth.ProjectID = projectID;

                bool contains = Users.Any(l => l.UserID == x.UserID);

                if (contains)
                {
                    auth.Authorize = true;
                }
                else
                {
                    auth.Authorize = false;
                }
                listOfAuthorizedUser.Add(auth);
            }
            ViewBag.users = allusers;

            return PartialView("_ChangeAuthorization", listOfAuthorizedUser);
        }

        [HttpPost]
        public async Task<ActionResult> changeAuthorization(List<AuthorizeUser> user)
        {
            if (user.Count > 0 || user != null)
            {
                ExecuteResult r = await ProjectService.InsertDeleteUserAuthorization(user);
            }
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ProjectDetails(int ProjectID)
        {
            IEnumerable<Sprint> sprints = await SprintService.GetSprintDataByProjectID(ProjectID);
            Projects project = await ProjectService.GetProjectByProjectID(ProjectID);
            ViewBag.Name = project.ProjectName;
            return View(sprints);
        }

        public async Task<ActionResult> ShowWorkItems(int sprintID)
        {
            IEnumerable<WorkItem> items = await WorkItemService.getWorkItemBySprintID(sprintID);
            foreach(WorkItem x in items)
            {
                x.WorkTasks = await TaskService.getTasksByWorkItemID(x.WorkItemID);
            }
            Sprint s = await SprintService.GetSprintBySprintID(sprintID);
            ViewBag.SprintName = s.SprintName;
            ViewBag.SprintID = s.SprintID;
            return PartialView("_ListOfWorkItem",items);
        }

        [HttpGet]
        public async Task<ActionResult> UpdateSprint(int sprintID)
        {
            Sprint sprint = await SprintService.GetSprintBySprintID(sprintID);
            return PartialView("_UpdateSprint", sprint);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateSprint(Sprint sprint)
        {
            ExecuteResult r = await SprintService.UpdateSprint(sprint);
            return RedirectToAction("ProjectDetails", new {ProjectID = sprint.ProjectID });
        }

        [HttpGet]
        public async Task<ActionResult> InsertUpdateWorkItem(int sprintID , int workItemID = 0)
        {
            IEnumerable<States> listofStates = await StateService.getState();

            List<SelectListItem> statesddl = new List<SelectListItem>();
            foreach (States x in listofStates)
            {
                statesddl.Add(new SelectListItem() { Text = x.StateName, Value = x.StateID.ToString() });
            }

            ViewBag.states = statesddl;
            if (workItemID > 0)
            {
                WorkItem workItem = await WorkItemService.getWorkItemByWorkItemID(workItemID);
                ViewBag.Edit = 1;

                return PartialView("_InsertUpdateWorkItem", workItem);
            }
            else
            {
                return PartialView("_InsertUpdateWorkItem");
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertUpdateWorkItem(WorkItem works)
        {
            ExecuteResult r = await WorkItemService.InsertUpdateWorkItems(works);
            Sprint sprint = await SprintService.GetSprintBySprintID(works.SprintID);
            return RedirectToAction("ProjectDetails", new { ProjectID = sprint.ProjectID });
        }

        [HttpGet]
        public async Task<ActionResult> InsertUpdateWorkTask(int WorkID,int TaskID = 0)
        {
            IEnumerable<States> listofStates = await StateService.getState();
            IEnumerable<User> listofUser = await UserService.GetUserData();
            List<SelectListItem> statesddl = new List<SelectListItem>();
            List<SelectListItem> userddl = new List<SelectListItem>();

            foreach (States x in listofStates)
            {
                statesddl.Add(new SelectListItem() { Text = x.StateName, Value = x.StateID.ToString() });
            }

            foreach (User x in listofUser)
            {
                userddl.Add(new SelectListItem() { Text = x.UserName, Value = x.UserID.ToString() });
            }

            ViewBag.states = statesddl;
            ViewBag.users = userddl;
            ViewBag.workItemID = WorkID;
            if (TaskID > 0)
            {
                WorkTask tasks = await TaskService.getTasksByTaskID(TaskID);
                ViewBag.Edit = 1;

                return PartialView("_InsertUpdateWorkTask", tasks);
            }
            else
            {
                return PartialView("_InsertUpdateWorkTask");
            }
        }

        [HttpPost]
        public async Task<ActionResult> InsertUpdateWorkTask(WorkTask workTask)
        {
            ExecuteResult r = await TaskService.InsertUpdateWorkTask(workTask);
            WorkItem w = await WorkItemService.getWorkItemByWorkItemID(workTask.WorkItemID);
            Sprint s = await SprintService.GetSprintBySprintID(w.SprintID);

            return RedirectToAction("ProjectDetails", new { ProjectID = s.ProjectID });
        }

        public async Task<ActionResult> DeleteWorkTask(int TaskID)
        {

            WorkTask t = await TaskService.getTasksByTaskID(TaskID);
            WorkItem w = await WorkItemService.getWorkItemByWorkItemID(t.WorkItemID);
            Sprint s = await SprintService.GetSprintBySprintID(w.SprintID);


            ExecuteResult r = await TaskService.DeleteWorkTask(TaskID);

            return RedirectToAction("ProjectDetails", new { ProjectID = s.ProjectID });
        }

        public async Task<ActionResult> DeleteWorkItem(int WorkItemID)
        {
            WorkItem w = await WorkItemService.getWorkItemByWorkItemID(WorkItemID);
            Sprint s = await SprintService.GetSprintBySprintID(w.SprintID);
            ExecuteResult r = await WorkItemService.DeleteWorkItem(WorkItemID);
            return RedirectToAction("ProjectDetails", new { ProjectID = s.ProjectID });
        }

    }
}