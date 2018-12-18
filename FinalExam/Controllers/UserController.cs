using FinalExamModel.FinalExamModels;
using FinalExamModels;
using FinalExamService;
using Model.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FinalExam.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService UserService;
        private readonly IRolesService RolesService;

        public UserController(IUserService UserService, IRolesService RolesService) : base()
        {
            this.UserService = UserService;
            this.RolesService = RolesService;
        }



        public async Task<ActionResult> Index()
        {

            IEnumerable<User> Users = await UserService.GetUserData();
            foreach (User x in Users)
            {
                if (x.ImagePath != "")
                {
                    string a = "/" + x.ImagePath.Substring(x.ImagePath.IndexOf("ProfilePic"));
                    a.Replace('\\', '/');
                    x.ImagePath = a;
                }
            }

            return View(Users);
        }


        [HttpGet]
        public async Task<ActionResult> addUser(int UserID = 0)
        {
            IEnumerable<Roles> listofRoles = await RolesService.getRoles();

            List<SelectListItem> rolesdll = new List<SelectListItem>();
            foreach (Roles x in listofRoles)
            {
                rolesdll.Add(new SelectListItem() { Text = x.RolesName, Value = x.RolesID.ToString() });
            }

            ViewBag.roles = rolesdll;
            if (UserID > 0)
            {
                IEnumerable<User> user = await UserService.GetUserData(UserID);
                User x = user.First();
                ViewBag.Edit = 1;

                string a = "/" + x.ImagePath.Substring(x.ImagePath.IndexOf("ProfilePic"));
                a.Replace('\\', '/');
                x.ImagePath = a;

                return PartialView("_AddUser", x);
            }
            else
            {
                return PartialView("_AddUser");
            }
        }

        [HttpPost]
        public async Task<ActionResult> addUser(User user)
        {
            if (user.ImageFile != null)
            {
                string pic = System.IO.Path.GetFileName(user.ImageFile.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/ProfilePic"), pic);
                // file is uploaded
                user.ImagePath = path;
                user.ImageFile.SaveAs(path);
            }

            ExecuteResult r = await UserService.InsertUpdateUser(user);

            return RedirectToAction("Index");
        }

    }
}