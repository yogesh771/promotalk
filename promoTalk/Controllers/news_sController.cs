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
    public class news_sController : Controller
    {
        private promotalkEntities db = new promotalkEntities();

        // GET: news
        public async Task<ActionResult> Index()
        {
            return View(await db.news.ToListAsync());
        }
            
        // GET: news/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: news/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(news news, HttpPostedFileBase file)
        {
            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(news.slugURL, "news", news.newsID, 0))
            {
                ViewBag.result1 = "EXISTS";
                return View(news);
            }
            ViewBag.result = "f";
            try
            {
                String fileName = ""; 
                if (file != null)
                {
                    fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/marketing"), fileName);
                    file.SaveAs(path);
                    news.seo_OGImage =  "/Content/marketing/" + fileName;
                }
                //else
                //{
                //    news.seo_OGImage = domainName + "/Content/img/" + "emptyLogo.jpg";
                //}


                news.createdDate = BaseUtil.GetCurrentDateTime();
                if (ModelState.IsValid)
                {
                    db.news.Add(news);
                    await db.SaveChangesAsync();
                    ViewBag.result = "s";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }

            return View(news);
        }

        // GET: news/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            news news = await db.news.FindAsync(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: news/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( news news, HttpPostedFileBase file)
        {

            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(news.slugURL, "news", news.newsID, 0))
            {
                ViewBag.result1 = "EXISTS";
                return View(news);
            }
            String fileName = "";
            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/marketing"), fileName);
                file.SaveAs(path);
                news.seo_OGImage = "/Content/marketing/" + fileName;
            }
           
            news.createdDate = BaseUtil.GetCurrentDateTime();
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    ViewBag.Error = e.Message;
                }
                return RedirectToAction("Index");
            }
            return View(news);
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
