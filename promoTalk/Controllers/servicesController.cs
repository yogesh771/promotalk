using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using promoTalk.Models;

namespace promoTalk.Controllers
{
    public class servicesController : BaseClass
    {
        private promotalkEntities db = new promotalkEntities();

        public async Task<ActionResult> Index()
        {
            return View(await db.services.Where(e=>e.isActive==true).ToListAsync());
        }       

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( service service)
        {
            ViewBag.result = "f";
            service.createdDate = BaseUtil.GetCurrentDateTime();
            if (ModelState.IsValid)
            {
                service.isActive = true;
                db.services.Add(service);
                await db.SaveChangesAsync();
                ViewBag.result = "s";
                return View();
            }

            return View(service);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            service service = await db.services.FindAsync(id);
            if (service == null)
            {
                return HttpNotFound();
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit( service service)
        {
            service.createdDate = BaseUtil.GetCurrentDateTime();
            if (ModelState.IsValid)
            {
                db.Entry(service).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(service);
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
