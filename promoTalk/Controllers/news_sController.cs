using System;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using promoTalk.Models;

namespace promoTalk.Controllers
{
    public class news_sController : BaseClass
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
               
                if (file != null)
                {                   
                    news.seo_OGImage = UploadFile(file, "/Content/marketing");
                }              

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
            if (file != null)
            {
                news.seo_OGImage = UploadFile(file, "/Content/marketing");
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
