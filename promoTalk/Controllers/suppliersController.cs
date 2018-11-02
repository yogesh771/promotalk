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
using System.Web.Script.Serialization;

namespace promoTalk.Controllers
{
    public class suppliersController : BaseClass
    {
        private promotalkEntities db = new promotalkEntities();

        // GET: suppliers
        public  ActionResult Index(int id)
        {
            ViewBag.supplierFor = id;
            var suppliers =  db.suppliers.Where(e=>e.SupplierFor==id && e.isActive == true).ToList();
            return View(suppliers);
        }

        // GET: suppliers/Create
        public ActionResult Create(int id)
        {
            ViewBag.StateID = db.states.Select(e => new { e.StateID, e.StateName });
            ViewBag.countryID = db.countries.Select(e => new { e.countryID, e.countryName });
            supplier osupplier = new supplier();
            osupplier.SupplierFor = id;
            return View(osupplier);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(supplier supplier)
        {
            ViewBag.result = "f";
            if (ModelState.IsValid)
            {
                supplier.isActive = true;
                db.suppliers.Add(supplier);
                await db.SaveChangesAsync();
                ViewBag.result = "s";
                ViewBag.StateID = db.states.Select(e => new { e.StateID, e.StateName });
                ViewBag.countryID = db.countries.Select(e => new { e.countryID, e.countryName });
                return View(supplier);
               // return RedirectToAction("Index", new { id = supplier.SupplierFor });
            }
            ViewBag.StateID = db.states.Select(e => new { e.StateID, e.StateName });
            ViewBag.countryID = db.countries.Select(e => new { e.countryID, e.countryName });
            return View(supplier);
        }

        // GET: suppliers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplier supplier = await db.suppliers.FindAsync(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
                  return View(supplier);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(supplier supplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(supplier).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new{ id = supplier.SupplierFor });
           
            }
           // ViewBag.StateID = new SelectList(db.states, "StateID", "StateName", supplier.StateID);
            return View(supplier);
        }
        public ActionResult fillState(Int32 countryID)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            string result = javaScriptSerializer.Serialize(db.states.Where(e => e.countryID == countryID).Select(e => new { e.StateID, e.StateName }));
            return Json(result, JsonRequestBehavior.AllowGet);
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
