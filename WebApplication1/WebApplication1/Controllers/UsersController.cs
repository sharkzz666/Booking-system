using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DB;

namespace WebApplication1.Controllers
{
    public class UsersController : Controller
    {
        private pooktehlurosEntities db = new pooktehlurosEntities();

        // GET: Users
        public ActionResult Index(string id)
        {
            User user = db.Users.Find(Convert.ToInt64(id));
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find((Convert.ToInt64(id)));
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: Users/Create
        public ActionResult NewUser()
        {
            ViewBag.Message = "Welcome!";
            ViewBag.RoleID = new SelectList(db.Ref_Role.Where(p => p.ID > 1), "ID", "Description");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RoleID,ParentID,Username,Password,Name,Email,Phone,IC,RID,Address,Epoint,Rpoint,BonusStar")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewUser([Bind(Include = "ID,RoleID,ParentID,Username,Password,Name,Email,Phone,IC,RID,Address")] User user)
        {
            ViewBag.Message = "Welcome!";
            if (ModelState.IsValid)
            {
                pooktehlurosEntities entities = new pooktehlurosEntities();
                bool userValid = entities.Users.Any(userDB => userDB.Username == user.Username);
                bool ReferralIDValid = entities.Users.Any(userDB => userDB.RID == user.ParentID);
                bool IsValid = true;

                // User found in the database
                if (!ReferralIDValid)
                {
                    IsValid = false;
                    ModelState.AddModelError("", "Referral ID is not valid. Please re-enter."); 
                } 
                if (userValid)
                {
                    IsValid = false;
                    ModelState.AddModelError("", "Username are not available. Please choose another username.");
                } 

                if (IsValid)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    ViewBag.Message = "Thank You for register!";
                    return View(user);
                }
            }

            //repopulate ddl
            ViewBag.RoleID = new SelectList(db.Ref_Role.Where(p => p.ID > 1), "ID", "Description");

            return View(user);
        }

        [HttpPost]
        public JsonResult NewUserJSON(string Prefix)
        {
            Prefix = Prefix.ToUpper();
            var User = db.Users.Where(p => p.ID != 1);
            //Searching records from list using LINQ query  
            var UserReferralID = (from N in User
                                  where N.RID.Contains(Prefix)
                                  select new { N.RID });
            return Json(UserReferralID, JsonRequestBehavior.AllowGet);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID, Username,Password,Name,Email,Phone,IC,ParentID,RID,Address")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("/Details/"+ Session["UserID"]);

                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}",
                                                    validationError.PropertyName,
                                                    validationError.ErrorMessage);
                        }
                    }
                }
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
