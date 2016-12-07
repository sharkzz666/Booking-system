using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models.DB;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request 
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Login(User model, string returnUrl)
        {
            using (pooktehlurosEntities entities = new pooktehlurosEntities())
            {
                string username = model.Username;
                string password = model.Password;

                // Now if our password was enctypted or hashed we would have done the
                // same operation on the user entered password here, But for now
                // since the password is in plain text lets just authenticate directly

                bool userValid = entities.Users.Any(user => user.Username == username && user.Password == password);
                // User found in the database
                if (userValid)
                {
                    var userID = entities.Users.First(user => user.Username == username && user.Password == password);
                    FormsAuthentication.SetAuthCookie(username, false);
                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        Session["UserID"] = userID.ID;
                        if (userID.RoleID == 1)
                        {
                            //for admin
                            return Redirect("/admin/Index/" + userID.ID);
                        }
                        else
                            return Redirect("/Users/Index/" + userID.ID);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
