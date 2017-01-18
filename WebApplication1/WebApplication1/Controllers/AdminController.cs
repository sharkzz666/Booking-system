using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models.DB;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;

namespace WebApplication1.Controllers
{
    public class AdminController : Controller
    {
        private pooktehlurosEntities db = new pooktehlurosEntities();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Admin/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Admin/Create
        public ActionResult Create()
        {


            ViewBag.RoleID = new SelectList(db.Ref_Role, "ID", "Description");
            ViewBag.StatusID = new SelectList(db.Ref_Status, "ID", "Description");

            var DetailUpdate = new User
            {
                Created_Date = System.DateTime.Now,
                Created_By = Convert.ToInt64(Session["UserID"])


            };

            return View(DetailUpdate);

        }

        // POST: Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,RoleID,ParentID,Username,Password,Name,Email,Phone,IC,RID,Address,Epoint,Rpoint,BonusStar,StatusID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                user.Created_Date = System.DateTime.Now;
                user.Created_By = Convert.ToInt64(Session["UserID"]);
                db.SaveChanges();
                return RedirectToAction("Index", new { @id = Session["UserID"] });
            }

            ViewBag.RoleID = new SelectList(db.Ref_Role, "ID", "Description", user.RoleID);
            ViewBag.StatusID = new SelectList(db.Ref_Status, "ID", "Description", user.StatusID);
            return View(user);
        }
         
        public ActionResult Exportfile()
        { 
            var purchases =
                from p in db.Purchases
                join pd in db.Products on p.ProductID equals pd.ID
                join u in db.Users on p.UserID equals u.ID
                select new
                            {
                                UserID = u.RID, // or pc.ProdId
                                DatePurchased = p.Datecreate,
                                UserName = u.Name,
                                ProductName = pd.NameProduct,
                                TotalQuantity = p.Quantity,
                                TotalPrice = (p.Quantity * pd.Price)
                                // other assignments
                            };


            Random rnd = new Random();
            int myRandomNo = rnd.Next(1000, 9999); // creates a 4 digit random no.

            GridView gv = new GridView();

            gv.DataSource = purchases.ToList();
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=SummaryReport_" + myRandomNo + ".xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Admin");
        }

        // GET: Admin/Edit/5
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
            user.Updated_Date = System.DateTime.Now;
            user.Updated_By = Convert.ToInt64(Session["UserID"]);
            ViewBag.RoleID = new SelectList(db.Ref_Role, "ID", "Description", user.RoleID);
            ViewBag.StatusID = new SelectList(db.Ref_Status, "ID", "Description", user.StatusID);
            return View(user);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,RoleID,ParentID,Username,Password,Name,Email,Phone,IC,RID,Address,Epoint,Rpoint,BonusStar,StatusID")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                user.Updated_Date = System.DateTime.Now;
                user.Updated_By = Convert.ToInt64(Session["UserID"]);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Ref_Role, "ID", "Description", user.RoleID);
            ViewBag.StatusID = new SelectList(db.Ref_Status, "ID", "Description", user.StatusID);
            return View(user);
        }

        // GET: Admin/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            User user = await db.Users.FindAsync(id);
            db.Users.Remove(user);
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
