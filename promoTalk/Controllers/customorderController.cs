using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using promoTalk.Models;
using System.IO;

namespace promoTalk.Controllers
{
    public class customorderController : Controller
    {
        private promotalkEntities db = new promotalkEntities();

        // GET: customorder
        public async Task<ActionResult> Index()
        {
            return View(await db.tbl_customOrder.OrderByDescending(e=>e.createdDate).ToListAsync());
        }

        // GET: customorder/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_customOrder tbl_customOrder = await db.tbl_customOrder.FindAsync(id);
            if (tbl_customOrder == null)
            {
                return HttpNotFound();
            }
            return View(tbl_customOrder);
        }

        // GET: customorder/Create
        [HttpGet]
        public ActionResult Requestform(int? id, string category)
        {
            if (id == null)
            { return RedirectToAction("PageNotFound", "Home"); }
            ViewBag.title = "PROMOTALKS";
            ViewBag.desc = "PROMOTALKS";
            ViewBag.keyword = "PROMOTALKS";
            ViewBag.imgsrc = "";

            var modl = (dynamic)null;
            switch (category)
            {
                case "offer":
                    modl = db.productCatalogs.Where(u => u.catalogID == id).FirstOrDefault();
                    ViewBag.title = modl.productTitle;
                    ViewBag.slugURL = modl.slugURL;
                    ViewBag.category = category;
                    ViewBag.pagetitle = "Order Custom Offer";
                    break;
                case "catalog":
                    modl = db.productCatalogs.Where(u => u.catalogID == id).FirstOrDefault();
                    ViewBag.title = modl.productTitle;
                    ViewBag.slugURL = modl.slugURL;
                    ViewBag.category = category;
                    ViewBag.pagetitle = "Order Custom Catalog";
                    break;
                default:
                    return RedirectToAction("PageNotFound", "Home");
                    break;

            }
            ViewBag.catalogID = id;
            return View();
        }
        // POST: customorder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]       
        public ActionResult Requestform(tbl_customOrder tbl_customOrder, HttpPostedFileBase file)
        {
            TempData["result"] = "f";
            tbl_customOrder.isActive = true;
            tbl_customOrder.createdDate = BaseUtil.GetCurrentDateTime();
            String fileName = "";
            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                file.SaveAs(path);
                tbl_customOrder.logo = "/Content/img/" + fileName;
            }
            if (ModelState.IsValid)
            {
                db.tbl_customOrder.Add(tbl_customOrder);
                db.SaveChanges();
                TempData["result"] = "s";
                return RedirectToAction("Requestform", "customorder", new { id = tbl_customOrder.catalogID, category = tbl_customOrder.category });
            }
            return View();
        }

        // GET: customorder/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_customOrder tbl_customOrder = await db.tbl_customOrder.FindAsync(id);
            if (tbl_customOrder == null)
            {
                return HttpNotFound();
            }
            return View(tbl_customOrder);
        }

        // POST: customorder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(tbl_customOrder tbl_customOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tbl_customOrder).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbl_customOrder);
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
