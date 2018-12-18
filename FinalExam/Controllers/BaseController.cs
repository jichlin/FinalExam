using FinalExamModels;
using FinalExamService.Helper;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace FinalExam.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {

        }

        // GET: Base

        protected async override void OnActionExecuting(ActionExecutingContext filterContext)
        {

          //  filterContext.Result = new RedirectToRouteResult(
          //     new RouteValueDictionary
          //{
          //                             { "action", "Page404" },
          //                             { "controller", "Auth" }
          //});
            //HttpSessionStateBase session = filterContext.HttpContext.Session;
            //if (session.IsNewSession
            //  || Session[SessionEnum.IDUSER] == null
            //  || Session[SessionEnum.NAMEUSER] == null
            //  || Session[SessionEnum.ROLEUSER] == null)
            //{

            //    //Riderect to Action Login
            //    filterContext.Result = new RedirectToRouteResult(
            //                        new RouteValueDictionary
            //                   {
            //                           { "action", "Index_Login" },
            //                           { "controller", "Login" }
            //                   });

            //}
            //else
            //{
            //    var result = string.Join("/", new[]{
            //        "",
            //        RouteData.Values["controller"]
            //    });
            //    var memberList = MemberPrivilegesService.CheckIsPageAccessableByMember(Convert.ToInt32(Session[SessionEnum.JENISANGGOTA]), result.ToString());
            //    var member = memberList.FirstOrDefault();

            //    //khusus kasus regis/upgrade anggota
            //    if (memberList.Count() > 1)
            //    {
            //        if (member.Lihat == false)
            //        {
            //            member = memberList.LastOrDefault();
            //        }
            //    }

            //    MemberPrivileges adminManyMenu = new MemberPrivileges();

            //    if (member == null)
            //    {
            //        result = string.Join("/", new[]{
            //            "",
            //            RouteData.Values["controller"],
            //            RouteData.Values["action"]
            //        });
            //        memberList = MemberPrivilegesService.CheckIsPageAccessableByMember(Convert.ToInt32(Session[SessionEnum.JENISANGGOTA]), result.ToString());

            //        if (memberList.Count() > 1)
            //        {
            //            var IDMenu = (int)Session["IDMenu"];
            //            member = (MemberPrivileges)memberList.Where(q => q.IDMenu == IDMenu).FirstOrDefault();

            //        }
            //        else
            //        {
            //            member = (MemberPrivileges)memberList.FirstOrDefault();
            //        }
            //    }


            //    TempData["Lihat"] = member.Lihat;
            //    TempData["Simpan"] = member.Simpan;
            //    TempData["Hapus"] = member.Hapus;
            //    TempData["Download"] = member.Download;
            //    if (!member.Lihat)
            //    {
            //filterContext.Result = new RedirectToRouteResult(
            //               new RouteValueDictionary
            //          {
            //                           { "action", "Page404" },
            //                           { "controller", "Auth" }
            //          });
            //    }

            //}

        }

        protected void CreateSessions(User user)
        {
            int PrevUserID = 0;
            if (Session[SessionEnum.IDUSER] != null)
                PrevUserID = Convert.ToInt32(Session[SessionEnum.IDUSER]);

            if (PrevUserID == 0 || PrevUserID != user.UserID)
            {
                Session[SessionEnum.IDUSER] = user.UserID;
                Session[SessionEnum.NAMEUSER] = user.UserName;
                Session[SessionEnum.ROLEUSER] = user.RolesID;


            }
        }


        protected void ResetSessions()
        {
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            Session.Clear();
            Session.Abandon();
        }

    }
}