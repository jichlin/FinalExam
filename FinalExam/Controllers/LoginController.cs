using FinalExamModel.FinalExamModels.View_Model;
using FinalExamModels;
using FinalExamService;
using FinalExamService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace FinalExam.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IUserService UserService;

        public LoginController(IUserService UserService) : base(){
            this.UserService = UserService;
        }

        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Login(Login login)
        {
            Login x = await UserService.GetUserLogin(login);
            if(x != null && ModelState.IsValid)
            {
                
                IEnumerable<User> user = await UserService.GetUserData(x.UserID);
                Session[SessionEnum.IDUSER] = user.First().UserID;
                Session[SessionEnum.NAMEUSER] = user.First().UserName;
                Session[SessionEnum.ROLEUSER] = user.First().RolesID;

                return RedirectToAction("Index", "Project");
            }
            else
            {
                if(x == null && ModelState.IsValid)
                    ViewBag.Error = "Wrong Credentials";
                return View("Index");
            }

        }

        public async Task<ActionResult> Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

    }
}