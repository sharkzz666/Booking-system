using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.DB;

namespace WebApplication1.Controllers
{
    public class adminlistviewsController : Controller
    {
        private pooktehlurosEntities db = new pooktehlurosEntities();

        // GET: adminlistviews
        public async Task<ActionResult> Index()
        {
            return View(await db.adminlistviews.ToListAsync());
        }

        // GET: adminlistviews/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            adminlistview adminlistview = await db.adminlistviews.FindAsync(id);
            if (adminlistview == null)
            {
                return HttpNotFound();
            }
            return View(adminlistview);
        }

        // GET: adminlistviews/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: adminlistviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,RoleID,ParentID,Username,Password,Name,Email,Phone,IC,RID,Address,Epoint,Rpoint,BonusStar")] adminlistview adminlistview)
        {
            if (ModelState.IsValid)
            {
                db.adminlistviews.Add(adminlistview);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(adminlistview);
        }

        // GET: adminlistviews/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            adminlistview adminlistview = await db.adminlistviews.FindAsync(id);
            if (adminlistview == null)
            {
                return HttpNotFound();
            }
            return View(adminlistview);
        }

        // POST: adminlistviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,RoleID,ParentID,Username,Password,Name,Email,Phone,IC,RID,Address,Epoint,Rpoint,BonusStar")] adminlistview adminlistview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(adminlistview).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(adminlistview);
        }

        // GET: adminlistviews/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            adminlistview adminlistview = await db.adminlistviews.FindAsync(id);
            if (adminlistview == null)
            {
                return HttpNotFound();
            }
            return View(adminlistview);
        }

        // POST: adminlistviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            adminlistview adminlistview = await db.adminlistviews.FindAsync(id);
            db.adminlistviews.Remove(adminlistview);
            await db.SaveChangesAsync();
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
