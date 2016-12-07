using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DB;

namespace WebApplication1.Controllers
{
    public class RedeemController : Controller
    {
        private pooktehlurosEntities db = new pooktehlurosEntities();
        // GET: /Redeem/
        public async Task<ActionResult> PurchaseReedem(string id)
        {
            int temp = Convert.ToInt32(id);
            // ProductTypeID = 1 for purchases, not redeem item 
            var purchases = db.Purchases.Include(p => p.Product).Include(p => p.User).Where(p => p.UserID == temp).Where(p => p.Product.ProductTypeID == 2);
            return View(await purchases.ToListAsync());
        }


        public ActionResult Home()
        {
            var products = db.Products.Include(p => p.Ref_ProductType).Where(p => p.ProductTypeID == 2);
            return View(products.ToList());
        }
        // GET: Purchases/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // GET: Purchases/Create
        public ActionResult Create(long? id)
        {
            var DetailUpdate = new Purchase
            {
                Datecreate = System.DateTime.Now

            };
            ViewBag.ProductID = new SelectList(db.Products.Where(p => p.ProductTypeID == 2), "ID", "NameProduct", id);
            ViewBag.StatusID = new SelectList(db.Ref_Status, "ID", "Description");
            ViewBag.UserID = Convert.ToString(Session["UserID"]);
            return View(DetailUpdate);
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,UserID,Description,Quantity")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                purchase.UserID = Convert.ToInt64(Session["UserID"]);
                purchase.Datecreate = System.DateTime.Now;
                purchase.StatusID = 1;
                db.Purchases.Add(purchase);
                db.SaveChanges();
                return RedirectToAction("PurchaseReedem", new { @id = Session["UserID"] });
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "NameProduct", purchase.ProductID);
            ViewBag.UserID = Convert.ToString(Session["UserID"]);
            ViewBag.StatusID = new SelectList(db.Ref_Status, "ID", "Description", purchase.StatusID);
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);

            if (purchase == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "NameProduct", purchase.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "Username", purchase.UserID);
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,UserID,Description,Quantity,StatusID")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {

                db.Entry(purchase).State = EntityState.Modified;
                purchase.Datecreate = System.DateTime.Now;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "NameProduct", purchase.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "Username", purchase.UserID);
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Purchase purchase = db.Purchases.Find(id);
            if (purchase == null)
            {
                return HttpNotFound();
            }
            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Purchase purchase = db.Purchases.Find(id);
            db.Purchases.Remove(purchase);
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
    