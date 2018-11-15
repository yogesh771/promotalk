using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using promoTalk.Models;


namespace promoTalk.Controllers
{
    public class customorderController : BaseClass
    {
        private promotalkEntities db = new promotalkEntities();
     
        public async Task<ActionResult> Index()
        {
            return View(await db.tbl_customOrder.OrderByDescending(e=>e.createdDate).ToListAsync());
        }

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
       
        [HttpPost]
        [ValidateAntiForgeryToken]       
        public ActionResult Requestform(tbl_customOrder tbl_customOrder, HttpPostedFileBase file)
        {
            TempData["result"] = "f";
            tbl_customOrder.isActive = true;
            tbl_customOrder.createdDate = BaseUtil.GetCurrentDateTime();
           
            if (file != null)
            {               
                tbl_customOrder.logo = UploadFile(file,"/Content/img");
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
