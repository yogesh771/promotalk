using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using promoTalk.Models;

namespace promoTalk.Controllers
{
    public class technologie_sController : BaseClass
    {
        private promotalkEntities db = new promotalkEntities();

       
        public async Task<ActionResult> Index()
        {
            return View(await db.technologies.ToListAsync());
        }
      
        public ActionResult Create()
        {
            return View();
        }
       
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
            if (file != null)
            {
                technology.seo_OGImage = UploadFile(file, "/Content/marketing"); 
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
           
            if (file != null)
            {
                technology.seo_OGImage = UploadFile(file, "/Content/marketing"); 
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
