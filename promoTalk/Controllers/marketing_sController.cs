using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using promoTalk.Models;

namespace promoTalk.Controllers
{
    public class marketing_sController : BaseClass
    {
        private promotalkEntities db = new promotalkEntities();
       
        public async Task<ActionResult> Index()
        {
            return View(await db.marketings.ToListAsync());
        }
        
        public ActionResult Create()
        {
            return View();
        }

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
            if (file != null)
            {               
                marketing.seo_OGImage = UploadFile(file, "/Content/marketing");
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
          
            if (file != null)
            {                
                marketing.seo_OGImage = UploadFile(file, "/Content/marketing");
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
