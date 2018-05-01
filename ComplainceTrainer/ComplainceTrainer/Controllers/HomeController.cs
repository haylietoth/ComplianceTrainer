using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ComplianceTrainer.Models;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using ComplianceTrainer.helpers;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ComplianceTrainer.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string error = "")
        {
            if (error != "")
            {
                ViewBag.errorMessage = error;
            }

            sessionStorage session = new sessionStorage();
            if (session.getSessionUser() != null)
            {
                return View(session.getSessionUser());
            }

            validation val = new validation();

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Active Directory Code

            //if (val.getUserCredentials(Request))
            //{
            //    return View(session.getSessionUser());
            //}
            //return RedirectToAction("Login");

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////            
            /// Non-Active Directory Code

            User myUser = new User();
            myUser = val.validate();
            UserViewModel vmUser = myUser.userToModel(myUser);
            vmUser.isHR = true;
            session.setSessionUser(vmUser);
            return View(session.getSessionUser());

         
        }

        public ActionResult Login()
        {
            return View();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// Active Directory Code
        //public ActionResult Login(string username = "", string password = "")
        //{
        //    if (username != "" && password != "")
        //    {
        //        using (var context = new PrincipalContext(ContextType.Domain))
        //        {
        //            ADSearcher ad = new ADSearcher();
        //            if (ad.IsAuthenticated(username, password))
        //            {
        //                UserPrincipal user = ad.findSearchedUserName(username);
        //                TempData["logonUser"] = ad.findByUserName(user);
        //                return RedirectToAction("Index", new { logonUser = "logonUser" });
        //            }
        //        }
        //        ViewBag.userMessage = "Access Denied Invalid Login Please Try Again";
        //        return View();
        //    }
        //    ViewBag.userMessage = "Please Log In";
        //    return View();
        //}
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

}