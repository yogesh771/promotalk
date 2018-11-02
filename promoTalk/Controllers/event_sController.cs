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
    public class event_sController : Controller
    {
        private promotalkEntities db = new promotalkEntities();

        // GET: events
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _partialEvents(DateTime txtFromDate, DateTime txtToDate, string txtEventName)
        {
            if(txtEventName!="")
            return PartialView("_partialEvents", db.tbl_events.Where(e=>e.dateTime>= txtFromDate && e.dateTime <= txtToDate && e.name.StartsWith(txtEventName)).OrderByDescending(e=>e.createdDate).ToList());

            return PartialView("_partialEvents", db.tbl_events.Where(e => e.dateTime >= txtFromDate && e.dateTime <= txtToDate).OrderByDescending(e => e.createdDate).ToList());
        }
        // GET: events/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_events tbl_events = await db.tbl_events.FindAsync(id);
            if (tbl_events == null)
            {
                return HttpNotFound();
            }
            return View(tbl_events);
        }

        // GET: events/Create
        public ActionResult Create()
        {
            return View();
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(tbl_events tbl_events, HttpPostedFileBase file)
        {
            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(tbl_events.slugURL, "events", 0, 0))
            {
                ViewBag.result1 = "EXISTS";
                return View(tbl_events);
            }
            ViewBag.result = "f";
            String fileName = ""; 
           
            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/marketing"), fileName);
                file.SaveAs(path);
                tbl_events.featuredImag = "/Content/marketing/" + fileName;
            }
           
            tbl_events.createdDate = BaseUtil.GetCurrentDateTime();
           
            if (ModelState.IsValid)
            {
                db.tbl_events.Add(tbl_events);
                await db.SaveChangesAsync();
                ViewBag.result = "s";
                return View();
            }

            return View(tbl_events);
        }

        // GET: events/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_events tbl_events = await db.tbl_events.FindAsync(id);
            if (tbl_events == null)
            {
                return HttpNotFound();
            }
            return View(tbl_events);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( tbl_events tbl_events, HttpPostedFileBase file)
        {
            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(tbl_events.slugURL, "events", tbl_events.eventID, 0))
            {
                ViewBag.result1 = "EXISTS";
                return View(tbl_events);
            }
            String fileName = ""; 
            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/marketing"), fileName);
                file.SaveAs(path);
                tbl_events.featuredImag = "/Content/marketing/" + fileName;
            }
            tbl_events.createdDate = BaseUtil.GetCurrentDateTime();
            if (ModelState.IsValid)
            {
                db.Entry(tbl_events).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tbl_events);
        }

        // GET: events/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbl_events tbl_events = await db.tbl_events.FindAsync(id);
            if (tbl_events == null)
            {
                return HttpNotFound();
            }
            return View(tbl_events);
        }

        // POST: events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            tbl_events tbl_events = await db.tbl_events.FindAsync(id);
            db.tbl_events.Remove(tbl_events);
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
