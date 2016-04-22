using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Library_BL;

namespace MvcApplication.Controllers
{
    public class UserController : Controller
    {

        [HttpPost]
        public ActionResult LogIn(FormCollection collection)
        {
            string userName = collection["username"];
            string password = collection["password"];

            
            Library_BL.User user = Library_BL.User.getByusername(userName);
            if (user != null)
            {
                string passSalt = Settings.SecureString(password + user.Salt);
                if (user.Password == passSalt)
                {
                    ViewBag.Status = true;
                    Session["User"] = user;
                    if (user.isAdmin == 1)
                    {
                        return RedirectToAction("IndexAdmin", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("IndexUser", "Admin");
                    }
                }                  
            }
            ViewBag.Status = false;
            return RedirectToAction("Index", "Admin");
            
        }
        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Index", "Admin");

        }
        //
        // POST: /User/Create


    }


}
