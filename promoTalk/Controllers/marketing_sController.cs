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
    public class marketing_sController : BaseClass
    {
        private promotalkEntities db = new promotalkEntities();

        // GET: marketings
        public async Task<ActionResult> Index()
        {
            return View(await db.marketings.ToListAsync());
        }
        
        public ActionResult Create()
        {
            return View();
        }

        // POST: marketings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(marketing marketing, HttpPostedFileBase file)
        {
            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(marketing.slugURL, "marketings", marketing.marketingID, 0))
            {
                ViewBag.result1 = "EXISTS";
                return View(marketing);
            }
            ViewBag.result = "f";
            String fileName = ""; 
            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/marketing"), fileName);
                file.SaveAs(path);
                marketing.seo_OGImage = "/Content/marketing/" + fileName;
            }
           
            marketing.createdDate = BaseUtil.GetCurrentDateTime();
            if (ModelState.IsValid)
            {
                db.marketings.Add(marketing);
                await db.SaveChangesAsync();
                ViewBag.result = "s";
                return View();
            }

            return View(marketing);
        }

        // GET: marketings/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            marketing marketing = await db.marketings.FindAsync(id);
            if (marketing == null)
            {
                return HttpNotFound();
            }
            return View(marketing);
        }

        // POST: marketings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(marketing marketing, HttpPostedFileBase file)
        {
            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(marketing.slugURL, "marketings", marketing.marketingID, 0))
            {
                ViewBag.result1 = "EXISTS";
                return View(marketing);
            }
            String fileName = ""; 
            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/marketing"), fileName);
                file.SaveAs(path);
                marketing.seo_OGImage = "/Content/marketing/" + fileName;
            }
            
            marketing.createdDate = BaseUtil.GetCurrentDateTime();
            if (ModelState.IsValid)
            {
                db.Entry(marketing).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(marketing);
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
