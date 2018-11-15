using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using promoTalk.Models;

namespace promoTalk.Controllers
{
    public class productCategoriesController : BaseClass
    {
        private promotalkEntities db = new promotalkEntities();
      
        public async Task<ActionResult> Index(int id)
        {
            ViewBag.supplierFor = id;
            return View(await db.productCategories.Where(e=>e.SupplierFor==id && e.isActive==true).ToListAsync());
        }  
        public ActionResult Create(int id)
        {
            productCategory oproductCategory = new productCategory();
            oproductCategory.SupplierFor = id;
            return View(oproductCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( productCategory productCategory)
        {
            ViewBag.result = "f";
            if (ModelState.IsValid)
            {
                productCategory.isActive = true;
                db.productCategories.Add(productCategory);
                await db.SaveChangesAsync();
                ViewBag.result = "s";
                return View(productCategory);              
            }

            return View(productCategory);
        }
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productCategories = await db.productCategories.FindAsync(id);
            if (productCategories == null)
            {
                return HttpNotFound();
            }
        
            return View(productCategories);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(productCategory productCategories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productCategories).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = productCategories.SupplierFor });
            }        
            return View(productCategories);
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
