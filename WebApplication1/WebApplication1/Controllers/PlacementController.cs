using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DB;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PlacementController : Controller
    {
        private pooktehlurosEntities db = new pooktehlurosEntities();
        // GET: Placement
        public ActionResult Index(long? id)
        {
                   

            User UserTemp = db.Users.Find(Convert.ToInt64(id));
            var sample = db.Users.Where(t => t.ParentID == UserTemp.RID);

            return View(sample);

       
    }

        // GET: Placement/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Placement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Placement/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Placement/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Placement/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Placement/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Placement/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
