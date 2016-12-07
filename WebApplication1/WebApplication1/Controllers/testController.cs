using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DB;

namespace WebApplication1.Controllers
{
    public class testController : Controller
    {
        private pooktehlurosEntities db = new pooktehlurosEntities();
        // GET: test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(long? id)
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
    }
}