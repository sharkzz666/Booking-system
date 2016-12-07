using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DB;
using PagedList;

namespace WebApplication1.Controllers
{
    public class ManagePurchaseController : Controller
    {
        private pooktehlurosEntities db = new pooktehlurosEntities();
        private pooktehlurosEntities dbSecond = new pooktehlurosEntities();

        // GET: ManagePurchase
        public ActionResult Index()
        {
            return View(db.Purchases.ToList());

        }

        // GET: ManagePurchase/Details/5
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

        // GET: ManagePurchase/Create
        public ActionResult Create()
        {
            ViewBag.ProductID = new SelectList(db.Products, "ID", "NameProduct");
            ViewBag.UserID = new SelectList(db.Users, "ID", "ParentID");
            ViewBag.StatusID = new SelectList(db.Ref_Status, "ID", "Description");
            return View();
        }

        // POST: ManagePurchase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ProductID,UserID,Description,Quantity,Status")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                db.Purchases.Add(purchase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductID = new SelectList(db.Products, "ID", "NameProduct", purchase.ProductID);
            ViewBag.UserID = new SelectList(db.Purchases, "ID", "ParentID", purchase.UserID);
            return View(purchase);
        }

        // GET: ManagePurchase/Edit/5
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
            ViewBag.UserID = new SelectList(db.Users, "ID", "ParentID", purchase.UserID);
            ViewBag.StatusID = new SelectList(db.Ref_Status, "ID", "Description", purchase.StatusID);
            return View(purchase);
        }

        // POST: ManagePurchase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ProductID,UserID,Description,Quantity,StatusID")] Purchase purchase)
        {
            if (ModelState.IsValid)
            { 
                var Product = db.Products.FirstOrDefault(userDB => userDB.ID == purchase.ProductID);

                Int64? StatusID = purchase.StatusID;
                //check last status ID
                //if current changes from the last, !2 -> 2 , 2 -> !2 , calculate EPoint, Rpoint, Bpoint

                var UserLastDetails = dbSecond.Purchases.FirstOrDefault(userDB => userDB.ID == purchase.ID);
                User user = db.Users.Find(purchase.UserID);
                Product product = db.Products.Find(purchase.ProductID);
                Int32? RPoint = (product.RPoint * purchase.Quantity);
                Int32? BStar = (Convert.ToInt32(product.Price) * purchase.Quantity);

                if (Product.ProductTypeID == 2)
                {

                    if (UserLastDetails.StatusID == 2 && StatusID != 2)
                    {
                        //change status to active, add point 
                        user.Epoint = user.Epoint + RPoint;
                        user.Rpoint = user.Rpoint + RPoint;

                        user.BonusStar = user.BonusStar + BStar;
                        db.Entry(user).State = EntityState.Modified;
                    }
                    else if (UserLastDetails.StatusID != 2 && StatusID == 2)
                    {
                        //change status to !active, remove point 

                        if (user.Epoint >= RPoint)
                        {
                            user.Epoint = user.Epoint - RPoint;
                        }
                        else
                            user.Epoint = 0;

                        if (user.Rpoint >= RPoint)
                        {
                            user.Rpoint = user.Rpoint - RPoint;
                        }
                        else
                            user.Rpoint = 0;

                        if (user.BonusStar >= BStar)
                        {
                            user.BonusStar = user.BonusStar - BStar;
                        }
                        else
                            user.BonusStar = 0;
                    }
                    else if (UserLastDetails.Quantity != purchase.Quantity)
                    {
                        if (purchase.Quantity > UserLastDetails.Quantity)
                        {
                            RPoint = purchase.Quantity - UserLastDetails.Quantity;
                            //add point 
                            user.Epoint = user.Epoint + (product.RPoint * RPoint);
                            user.Rpoint = user.Rpoint + (product.RPoint * RPoint);

                            user.BonusStar = user.BonusStar + (Convert.ToInt32(product.Price) * RPoint);
                            db.Entry(user).State = EntityState.Modified;
                        }
                        else
                        {
                            //change status to !active, remove point 
                            RPoint = UserLastDetails.Quantity - purchase.Quantity;
                            if (RPoint > 0)
                            {
                                user.Epoint = user.Epoint - (product.RPoint * RPoint);
                            }
                            else
                                user.Epoint = 0;

                            if (user.Rpoint >= RPoint)
                            {
                                user.Rpoint = user.Rpoint - (product.RPoint * RPoint);
                            }
                            else
                                user.Rpoint = 0;

                            if (user.BonusStar >= BStar)
                            {
                                user.BonusStar = user.BonusStar - (Convert.ToInt32(product.Price) * RPoint);
                            }
                            else
                                user.BonusStar = 0;
                        }
                        db.Entry(user).State = EntityState.Modified;
                    } 
                }
                else if (Product.ProductTypeID == 1)
                {  
                    if (UserLastDetails.StatusID != 2 && StatusID == 2)
                    {
                        //change status to active, add point 
                        user.Epoint = user.Epoint + RPoint;
                        user.Rpoint = user.Rpoint + RPoint;

                        user.BonusStar = user.BonusStar + BStar;
                        db.Entry(user).State = EntityState.Modified;
                    }
                    else if (UserLastDetails.StatusID == 2 && StatusID != 2)
                    {
                        //change status to !active, remove point 

                        if (user.Epoint >= RPoint)
                        {
                            user.Epoint = user.Epoint - RPoint;
                        }
                        else
                            user.Epoint = 0;

                        if (user.Rpoint >= RPoint)
                        {
                            user.Rpoint = user.Rpoint - RPoint;
                        }
                        else
                            user.Rpoint = 0;

                        if (user.BonusStar >= BStar)
                        {
                            user.BonusStar = user.BonusStar - BStar;
                        }
                        else
                            user.BonusStar = 0;
                    }
                    else if (UserLastDetails.Quantity != purchase.Quantity)
                    {
                        if (purchase.Quantity > UserLastDetails.Quantity)
                        {
                            RPoint = purchase.Quantity - UserLastDetails.Quantity;
                            //add point 
                            user.Epoint = user.Epoint + (product.RPoint * RPoint);
                            user.Rpoint = user.Rpoint + (product.RPoint * RPoint);

                            user.BonusStar = user.BonusStar + (Convert.ToInt32(product.Price) * RPoint);
                            db.Entry(user).State = EntityState.Modified;
                        }
                        else
                        {
                            //change status to !active, remove point 
                            RPoint = UserLastDetails.Quantity - purchase.Quantity;
                            if (RPoint > 0)
                            {
                                user.Epoint = user.Epoint - (product.RPoint * RPoint);
                            }
                            else
                                user.Epoint = 0;

                            if (user.Rpoint >= RPoint)
                            {
                                user.Rpoint = user.Rpoint - (product.RPoint * RPoint);
                            }
                            else
                                user.Rpoint = 0;

                            if (user.BonusStar >= BStar)
                            {
                                user.BonusStar = user.BonusStar - (Convert.ToInt32(product.Price) * RPoint);
                            }
                            else
                                user.BonusStar = 0;
                        }
                        db.Entry(user).State = EntityState.Modified;
                    } 
                }
                db.Entry(purchase).State = EntityState.Modified;
                 
                db.SaveChanges();
                return RedirectToAction("Index", new { @id = Session["UserID"] });
            }
            ViewBag.ProductID = new SelectList(db.Products, "ID", "NameProduct", purchase.ProductID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "ParentID", purchase.UserID);
            ViewBag.StatusID = new SelectList(db.Ref_Status, "ID", "Description", purchase.StatusID);
            return View(purchase);
        }

        // GET: ManagePurchase/Delete/5
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

        // POST: ManagePurchase/Delete/5
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
