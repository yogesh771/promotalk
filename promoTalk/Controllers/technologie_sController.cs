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
    public class technologie_sController : Controller
    {
        private promotalkEntities db = new promotalkEntities();

        // GET: technologies
        public async Task<ActionResult> Index()
        {
            return View(await db.technologies.ToListAsync());
        }

        // GET: technologies/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: technologies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(technology technology, HttpPostedFileBase file)
        {
            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(technology.slugURL, "technologies", technology.technologyID, 0))
            {
                ViewBag.result1 = "EXISTS";
                return View(technology);
            }
            ViewBag.result = "f";
            String fileName = ""; 
            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/marketing"), fileName);
                file.SaveAs(path);
                technology.seo_OGImage =  "/Content/marketing/" + fileName;
            }
          


            technology.createdDate = BaseUtil.GetCurrentDateTime();
            if (ModelState.IsValid)
            {
                db.technologies.Add(technology);
                await db.SaveChangesAsync();
                ViewBag.result = "f";
                return View();
            }

            return View(technology);
        }

        // GET: technologies/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            technology technology = await db.technologies.FindAsync(id);
            if (technology == null)
            {
                return HttpNotFound();
            }
            return View(technology);
        }

        // POST: technologies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(technology technology, HttpPostedFileBase file)
        {
            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(technology.slugURL, "technologies", technology.technologyID, 0))
            {
                ViewBag.result1 = "EXISTS";
                return View(technology);
            }
            String fileName = ""; 
            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/marketing"), fileName);
                file.SaveAs(path);
                technology.seo_OGImage =  "/Content/marketing/" + fileName;
            }           


            technology.createdDate = BaseUtil.GetCurrentDateTime();
            if (ModelState.IsValid)
            {
                db.Entry(technology).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(technology);
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
