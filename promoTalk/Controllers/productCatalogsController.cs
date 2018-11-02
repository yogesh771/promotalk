using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using promoTalk.Models;
using System.IO;

namespace promoTalk.Controllers
{
    public class productCatalogsController : BaseClass
    {
        private promotalkEntities db = new promotalkEntities();

        // GET: productCatalogs
        public ActionResult Index(int id)
        {
            ViewBag.productCategoryID =new SelectList( db.productCategories.Where(e=>e.SupplierFor== id && e.isActive == true).Select(e => new { e.productCategoryID, e.productCategory1 }), "productCategoryID", "productCategory1");
            ViewBag.supplierID = new SelectList(db.suppliers.Where(e => e.SupplierFor == id && e.isActive==true).Select(e => new { e.supplierID, e.supplierName }), "supplierID", "supplierName");
            ViewBag.isoffers = id;
            return View();
        }
        [HttpPost]
        public ActionResult _partialProductCatalogs(int productCategoryID, int supplierID,bool isPremium , bool isOffer )
        {
            int isOffer_ = 0;
            if (isOffer == true)
                isOffer_ = 1;


            var productCatalogs = db.getProductCatalog_sp(productCategoryID, supplierID, isOffer_,1).ToList();
            ViewBag.isOffer = isOffer;
            return PartialView("_partialProductCatalogs", productCatalogs.ToList());
        }
        // GET: productCatalogs/Create
        public ActionResult Create(int id )
        {
            ViewBag.productCategoryID = new SelectList(db.productCategories.Where(e => e.SupplierFor == id && e.isActive == true).Select(e => new { e.productCategoryID, e.productCategory1 }).OrderBy(e => e.productCategory1), "productCategoryID", "productCategory1");
            ViewBag.supplierID = db.suppliers.Where(e => e.SupplierFor == id && e.isActive == true).Select(e => new { e.supplierID, e.supplierName });
            productCatalog productCatalog = new productCatalog();
            productCatalog.isOffer = id==2? true:false;
            return View(productCatalog);
        }

        // POST: productCatalogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(productCatalog productCatalog, IEnumerable< HttpPostedFileBase> files , FormCollection frm)
        {
            var id1_ = productCatalog.isOffer == true ? 1 : 0;
            ViewBag.productCategoryID = new SelectList(db.productCategories.Where(e => e.SupplierFor == id1_ && e.isActive == true).Select(e => new { e.productCategoryID, e.productCategory1 }).OrderBy(e => e.productCategory1), "productCategoryID", "productCategory1");
            ViewBag.supplierID = db.suppliers.Where(e => e.SupplierFor == id1_ && e.isActive == true).Select(e => new { e.supplierID, e.supplierName });

            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(productCatalog.slugURL, "productCatalogs", 0, id1_))
            {
                ViewBag.result1 = "EXISTS";
               return View(productCatalog);
               
            }
            var productCategoryies = frm["hdnproductCategoryID"].ToString();
            ViewBag.result = "f";
            String fileName = ""; 
            try
            {
                if (files != null)
                {
                    int i = 1;
                    foreach (var file in files)
                    {
                        if (i == 1)
                        {
                            if (file != null && (file.ContentType == "image/png" || file.ContentType == "image/jpeg"))
                                {
                                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                                file.SaveAs(path);
                                productCatalog.thumbImageURL =  "/Content/img/" + fileName;
                            }
                            else
                            {
                                productCatalog.thumbImageURL =  "/Content/img/" + "emptyLogo.jpg";
                            }
                        }
                        if (i == 2)
                        {
                            if (file != null && file.ContentType == "application/pdf")
                            {
                                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                                var path = Path.Combine(Server.MapPath("~/Content/PDF"), fileName);
                                file.SaveAs(path);
                                productCatalog.pdfURL =  "/Content/PDF/" + fileName;
                                productCatalog.pdfName = fileName;
                            }
                        }
                        i = i + 1;

                    }
                }
            }
            catch (Exception e)
            {
                return View(productCatalog);
            }
            productCatalog.isDeleted = false;
            productCatalog.seo_OGImage = productCatalog.thumbImageURL;
            productCatalog.createdDate = BaseUtil.GetCurrentDateTime();
           

            if (ModelState.IsValid)
            {
                db.productCatalogs.Add(productCatalog);
                db.SaveChanges();

                if (productCategoryies != "")
                {
                    var nums = productCategoryies.Split(',').ToArray();
                    catalogCatagory ocatalogCatagory;
                    foreach (var i in nums)
                    {
                        if (i != "")
                        {
                            ocatalogCatagory = new catalogCatagory();
                            ocatalogCatagory.catalogID = productCatalog.catalogID;
                            ocatalogCatagory.productCategoryID = Convert.ToInt32(i);
                            db.catalogCatagories.Add(ocatalogCatagory);
                            db.SaveChanges();
                        }
                    }
                }

                    ViewBag.result = "s";
                 var oproductCatalog = new productCatalog();
                oproductCatalog.isOffer = productCatalog.isOffer;
                return View(productCatalog);
             
            }
           
            return View(productCatalog);
        }
        public ActionResult catalogcategory(int id)
        {
            var serviceList = db.catalogCatagories.Include(e => e.productCategory).Where(e => e.catalogID == id).OrderBy(e => e.productCategory.productCategory1).ToList();
            return PartialView("catalogcategory", serviceList);
        }
        // GET: productCatalogs/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            productCatalog productCatalog = db.productCatalogs.Find(id);
            if (productCatalog == null)
            {
                return HttpNotFound();
            }
            var id1 = productCatalog.isOffer == true ? 2 : 1;
            ViewBag.productCategoryID = new SelectList(db.productCategories.Where(e => e.SupplierFor == id1 && e.isActive == true).Select(e => new { e.productCategoryID, e.productCategory1 }).OrderBy(e => e.productCategory1), "productCategoryID", "productCategory1");
            var selectedCategory = db.catalogCatagories.Where(e => e.catalogID == id).Select(e=>new {e.productCategory.productCategory1, e.productCategory.productCategoryID}).ToList();

            var selectedCategoriesString = "";
            var selectedCategoriesID = new List<Int32>();
            foreach (var ss in selectedCategory)
            {
                selectedCategoriesID.Add(ss.productCategoryID);
                selectedCategoriesString = selectedCategoriesString + ss.productCategory1+",";
            }
            if (selectedCategory.Count > 0)
            {
                int index = selectedCategoriesString.LastIndexOf(',');
                ViewBag.catalogcatagory = selectedCategoriesString.Substring(0, index);
            }
            ViewBag.selectedCategoriesID = String.Join(",", selectedCategoriesID);
            ViewBag.supplierID = new SelectList(db.suppliers.Where(e => e.SupplierFor == id1 && e.isActive == true).Select(e => new { e.supplierID, e.supplierName }), "supplierID", "supplierName", productCatalog.supplierID);
            //ViewBag.supplierID = db.suppliers.Where(e => e.SupplierFor == id1 && e.isActive == true).Select(e => new { e.supplierID, e.supplierName });
            return View(productCatalog);
        }

        // POST: productCatalogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( productCatalog productCatalog, IEnumerable<HttpPostedFileBase> files, FormCollection frm)
        {
            var id1 = productCatalog.isOffer == true ? 1 : 0;
            ViewBag.productCategoryID = new SelectList(db.productCategories.Where(e => e.SupplierFor == id1 && e.isActive == true).Select(e => new { e.productCategoryID, e.productCategory1 }).OrderBy(e => e.productCategory1), "productCategoryID", "productCategory1");
            ViewBag.supplierID = db.suppliers.Where(e => e.SupplierFor == id1 && e.isActive == true).Select(e => new { e.supplierID, e.supplierName });

            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(productCatalog.slugURL, "productCatalogs", productCatalog.catalogID, id1))
            {
                ViewBag.result1 = "EXISTS";
                 return View(productCatalog);

            }

            var productCategoryies = frm["hdnproductCategoryID"].ToString();
            String fileName = ""; 
          
            if (files!=null)
            {
                int i = 1;
                foreach (var file in files)
                {
                    if (i == 1 && file!=null)
                    {
                        if (file.ContentType == "image/png" || file.ContentType == "image/jpeg")
                        {
                            fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                            file.SaveAs(path);
                            productCatalog.thumbImageURL = "/Content/img/" + fileName;
                        }
                        
                    }
                    if (i == 2)
                    {
                        if(file !=null && file.ContentType == "application/pdf")
                        {
                            fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                            var path = Path.Combine(Server.MapPath("~/Content/PDF"), fileName);
                            file.SaveAs(path);
                            productCatalog.pdfURL =  "/Content/PDF/" + fileName;
                        }
                    }
                    i = i + 1;

                }
            }
            
            productCatalog.createdDate = BaseUtil.GetCurrentDateTime();
            if (ModelState.IsValid)
            {
                db.Entry(productCatalog).State = EntityState.Modified;
                db.SaveChanges();

                if (productCategoryies != "")
                {
                    var nums = productCategoryies.Split(',').ToArray();
                    catalogCatagory ocatalogCatagory;
                    var ss = db.catalogCatagories.Where(e => e.catalogID == productCatalog.catalogID).ToList();
                    db.catalogCatagories.RemoveRange(ss);
                    db.SaveChanges();
                    foreach (var i in nums)
                    {
                        if (i != "")
                        {
                            ocatalogCatagory = new catalogCatagory();
                            ocatalogCatagory.catalogID = productCatalog.catalogID;
                            ocatalogCatagory.productCategoryID = Convert.ToInt32(i);
                            db.catalogCatagories.Add(ocatalogCatagory);
                            db.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("Index", new { id = productCatalog.isOffer == true ? 2 : 1 });
            }
            return View(productCatalog);
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
