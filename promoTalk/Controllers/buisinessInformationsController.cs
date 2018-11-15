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
    public class buisinessInformationsController : BaseClass
    {
        private promotalkEntities db = new promotalkEntities();

        // GET: buisinessInformations
        public async Task<ActionResult> Index()
        { 
            return View(await db.buisinessInformations.Include(e=>e.state).OrderByDescending(e => e.createdDate).ToListAsync());
        }
      
        public ActionResult BuisinessServices(int id)
        {
            var serviceList = db.buisinessServices.Include(e => e.service).Where(e => e.buisinessID == id).OrderBy(e => e.service.ServiceName).ToList();
            return PartialView("_partialBuisinessServices", serviceList);
        }

        private void fillDropdown()
        {
            ViewBag.StateID = db.states.Select(e => new { e.StateID, e.StateName });
            ViewBag.countryID = db.countries.Select(e => new { e.countryID, e.countryName });
            ViewBag.serviceID = db.services.Where(e => e.isActive == true).OrderBy(e => e.ServiceName).ToList();
        }
        // GET: buisinessInformations/Create
        public ActionResult Create()
        {
            fillDropdown();
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(buisinessInformation buisinessInformation, HttpPostedFileBase file)
        {
            fillDropdown();

            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(buisinessInformation.slugURL, "buisinessInformations", buisinessInformation.buisinessID, 0))
            {
                ViewBag.result1 = "EXISTS";
                return View(buisinessInformation);
            }
            if (!oAccountController.isSlugExist(buisinessInformation.slugURL, "buisinessInformations", buisinessInformation.buisinessID, 1))
            {
                ViewBag.result2 = "EXISTS";
                return View(buisinessInformation);
            }
            ViewBag.result = "f";
            String fileName = "";
            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                file.SaveAs(path);
                buisinessInformation.logo = "/Content/img/" + fileName;
            }
            else
            {
                buisinessInformation.logo = "/Content/img/" + "emptyLogo.jpg";
            }

            buisinessInformation.createdDate = BaseUtil.GetCurrentDateTime();
            buisinessInformation.isActive = true;
            if (ModelState.IsValid)
            {
                db.buisinessInformations.Add(buisinessInformation);
                await db.SaveChangesAsync();
                if (buisinessInformation.servicesIDs != null)
                {
                    int[] nums = buisinessInformation.servicesIDs.Split(',').Select(int.Parse).ToArray();
                    buisinessService obuisinessService;
                    foreach (var i in nums)
                    {
                        obuisinessService = new buisinessService();
                        obuisinessService.buisinessID = buisinessInformation.buisinessID;
                        obuisinessService.serviceID = i;
                        db.buisinessServices.Add(obuisinessService);
                        await db.SaveChangesAsync();
                    }
                }

                ViewBag.result = "s";
                return View();
            }
            return View(buisinessInformation);
        }

        // GET: buisinessInformations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            buisinessInformation buisinessInformation = await db.buisinessInformations.FindAsync(id);
            if (buisinessInformation == null)
            {
                return HttpNotFound();
            }
            var serviceID = db.services.OrderBy(e => e.ServiceName).Where(e => e.isActive == true).ToList();
            var selectedServiceID = db.buisinessServices.Where(e=>e.buisinessID== id).ToList();
            List<selectedService> listselectedService = new List<selectedService>();
            selectedService oselectedService;
            var selectedCategoriesID = "";
            foreach (var serv in serviceID) {
                oselectedService = new selectedService();
                oselectedService.serviceID = serv.serviceID;
                oselectedService.ServiceName = serv.ServiceName;
                if (selectedServiceID.Any(e => e.serviceID == serv.serviceID))
                {
                    oselectedService.ischecked = true;
                    selectedCategoriesID = selectedCategoriesID + oselectedService.serviceID + ",";
                }
                else {
                    oselectedService.ischecked = false;
                }
                listselectedService.Add(oselectedService);
            }
            ViewBag.selectedCategoriesID = selectedCategoriesID;
            ViewBag.serviceIDs = listselectedService;
           
            ViewBag.StateID = new SelectList(db.states.Select(e => new { e.StateID, e.StateName }), "StateID", "StateName", buisinessInformation.StateID);            
            ViewBag.countryID = new SelectList(db.countries.Select(e => new { e.countryID, e.countryName }), "countryID", "countryName", buisinessInformation.countryID);

            int index = selectedCategoriesID.LastIndexOf(',');          
            ViewBag.catalogcatagory = selectedCategoriesID.Substring(0, index);

            buisinessInformation.servicesIDs = selectedCategoriesID.Substring(0, index); 
            return View(buisinessInformation);
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(buisinessInformation buisinessInformation, HttpPostedFileBase file)
        {
            fillDropdown();
            AccountController oAccountController = new AccountController();
            if (!oAccountController.isSlugExist(buisinessInformation.slugURL, "buisinessInformations", buisinessInformation.buisinessID, 0))
            {
                ViewBag.result1 = "EXISTS";
                return View(buisinessInformation);
            }
            if (!oAccountController.isSlugExist(buisinessInformation.slugURL, "buisinessInformations", buisinessInformation.buisinessID, 1))
            {
                ViewBag.result2 = "EXISTS";
                return View(buisinessInformation);
            }
            String fileName = "";

            if (file != null)
            {
                fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/img"), fileName);
                file.SaveAs(path);
                buisinessInformation.logo = "/Content/img/" + fileName;
            }
            buisinessInformation.createdDate = BaseUtil.GetCurrentDateTime();
            if (ModelState.IsValid)
            {
                db.Entry(buisinessInformation).State = EntityState.Modified;
                int[] nums = (dynamic)null;
                if (buisinessInformation.servicesIDs != null)
                {
                    try
                    {
                        var str = buisinessInformation.servicesIDs;
                        nums = str.Split(',').Select(int.Parse).ToArray();
                        db.buisinessServices.RemoveRange(db.buisinessServices.Where(x => x.buisinessID == buisinessInformation.buisinessID));
                        await db.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        var msg = e.Message;
                    }
                }
                buisinessService obuisinessService;
                foreach (var i in nums)
                {
                    obuisinessService = new buisinessService();
                    obuisinessService.buisinessID = buisinessInformation.buisinessID;
                    obuisinessService.serviceID = i;
                    db.buisinessServices.Add(obuisinessService);
                }

                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(buisinessInformation);
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
