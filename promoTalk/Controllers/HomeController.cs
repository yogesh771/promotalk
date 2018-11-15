using promoTalk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Net;
using HtmlAgilityPack;

namespace promoTalk.Controllers
{

    public class HomeController : BaseClass
    {
        private promotalkEntities db = new promotalkEntities();
        public ActionResult Index()
        {
            ViewBag.pagetitle = "PromoTalks";
            ViewBag.title = "Search Latest Promo Products, Catalogs, Flyers, News and Events";
            ViewBag.desc = "Search Latest News, Events from Promo Industry, Marketing and Technology discussions for Promo Distributor and Suppliers";
            ViewBag.keyword = "PromoTalks";
            ViewBag.imgsrc = "";
            ViewBag.header = "no";
            return View();
        }
        [Route("catalog")]
        public ActionResult CatalogS()
        {
            var productCatalogs = db.productCatalogs.Where(e => e.isOffer == false && e.isDeleted == false);
            var catalogIDs = productCatalogs.Select(e => e.catalogID).Distinct();

            var productCategories = db.catalogCatagories.Where(e => catalogIDs.Contains(e.catalogID)).Select(e => e.productCategoryID).Distinct();
            ViewBag.productCategoryID = new SelectList(db.productCategories.Where(e => productCategories.Contains(e.productCategoryID) && e.SupplierFor == 1 && e.isActive == true).Select(e => new { e.productCategoryID, e.productCategory1 }), "productCategoryID", "productCategory1");

            var listsup = productCatalogs.Select(e => e.supplierID).Distinct();
            ViewBag.supplierID = new SelectList(db.suppliers.Where(e=>listsup.Contains(e.supplierID) && e.SupplierFor== 1 && e.isActive == true).Select(e => new { e.supplierID, e.supplierName }), "supplierID", "supplierName");
            ViewBag.title = "Latest Promotional Products Catalogs from Top Suppliers";
            ViewBag.desc = "Search latest Promotional Products Catalogs from the Top Suppliers of Promotional Product Industry";
            ViewBag.keyword = "catalogs";
            ViewBag.imgsrc = "";
            ViewBag.pagetitle = "Promo Products Catalogs";

            return View();
        }
        [HttpPost]
        public ActionResult _partialProductCatalogs(int productCategoryID, int supplierID, int isOffer, int isPrenium)
        {          
            return PartialView("_partialProductCatalogsExternal", isPrenium == 0? db.getProductCatalog_sp(productCategoryID, supplierID, 0, 1).ToList() : db.getProductCatalog_sp(productCategoryID, supplierID, 1, 0).ToList());
        }

        [HttpPost]
        public ActionResult _partialProductOffers(int productCategoryID, int supplierID, int isOffer, int isPrenium)
        {           
             var   productCatalogs = db.getProductCatalog_sp(productCategoryID, supplierID, 1, 0).ToList();           
            return PartialView("_partialProductOffers", productCatalogs);
        }

        [Route("flyer")]
        public ActionResult offers()
        {
            var productCatalogs = db.productCatalogs.Where(e => e.isOffer == true && e.isDeleted==false);
            var catalogIDs = productCatalogs.Select(e => e.catalogID).Distinct();

            var productCategories = db.catalogCatagories.Where(e=> catalogIDs.Contains(e.catalogID)).Select(e => e.productCategoryID).Distinct();
            ViewBag.productCategoryID = new SelectList(db.productCategories.Where(e => productCategories.Contains(e.productCategoryID) && e.SupplierFor == 2 && e.isActive == true).Select(e => new { e.productCategoryID, e.productCategory1 }), "productCategoryID", "productCategory1");

           var listsup = productCatalogs.Select(e => e.supplierID).Distinct();
            ViewBag.supplierID = new SelectList(db.suppliers.Where(e => listsup.Contains(e.supplierID) && e.SupplierFor == 2 && e.isActive==true).Select(e => new { e.supplierID, e.supplierName }), "supplierID", "supplierName");


            ViewBag.title = "Get Best Flyer From Promotional Product Industry Suppliers";
            ViewBag.desc = "Search for Latest Promotional Product Flyers from the Top Suppliers of Promotional Product Industry";
            ViewBag.keyword = "Flyers";
            ViewBag.imgsrc = "";
            ViewBag.pagetitle = "Promo Products Flyers";
            return View();
          
        }
        [Route("news")]
        public ActionResult News()
        {
            var news = db.news.Take(20).OrderByDescending(e => e.createdDate).ToList();

            ViewBag.title = "Get Latest News and Updates from Promo Products Industry";
            ViewBag.desc = "Get Latest News and Updates about Promotional Products, Promo Industry, Top Distributors and Suppliers";
            ViewBag.keyword = "PromoTalks";
            ViewBag.imgsrc = "";
            ViewBag.pagetitle = "Promo News";
            return View(news);
        }
        [Route("marketing")]
        public ActionResult Marketing()
        {
            //comment
            ViewBag.title = "Digital Marketing for Promo Distributor and Suppliers";
            ViewBag.desc = "Get Digital Marketing, Merchandising and Branding solutions to promote your business from the industry best experts";
            ViewBag.keyword = "PROMOTALKS";
            ViewBag.imgsrc = "";
            ViewBag.pagetitle = "Promo Marketing";
            var news = db.marketings.Take(20).OrderByDescending(e => e.createdDate).ToList();
            return View(news);
        }
        [Route("technology")]
        public ActionResult Technology()
        {
            ViewBag.title = "Latest Technology Tools, Topics and Discussions for Promo Distributor and Suppliers";
            ViewBag.desc = "Learn about latest Technology by worth tips, topics and tools provided by technology experts of Promo Product Industry";
            ViewBag.keyword = "PROMOTALKS";
            ViewBag.imgsrc = "";
            ViewBag.pagetitle = "Promo Technology";

            var news = db.technologies.OrderByDescending(e => e.createdDate).ToList();
            return View(news);
        }
        public ActionResult events_()
        {
            return View();
        }
        [Route("events")]
        public ActionResult events()
        {
            ViewBag.title = "Get updates from Promo Product Industry Events";
            ViewBag.desc = "Search and explore Promo Events, Expo and Roadshows across the country";
            ViewBag.keyword = "PROMOTALKS";
            ViewBag.imgsrc = "";
            ViewBag.pagetitle = "Promo Events";

            ViewBag.isFetureEvent = true;
            bool isFetureEvent = true;
            ViewBag.monthdate = new SelectList(completeEvent(isFetureEvent), "valueField", "textField");
            return View("events");
        }
        private List<ddl> completeEvent(bool isFetureEvent)
        {
            var GetCurrentDateTime = BaseUtil.GetCurrentDateTime();

            var mmyy = (dynamic)null;
            mmyy = db.getmonthyear(isFetureEvent).ToList();

            List<ddl> lisddl = new List<ddl>();
            ddl oddl;
            foreach (var j in mmyy)
            {
                oddl = new ddl();
                oddl.textField = j;
                oddl.valueField = j;
                lisddl.Add(oddl);
            }
            return lisddl;
        }
        [Route("events_archive")]
        public ActionResult events_archive()
        {
            ViewBag.title = "Get updates from Promo Product Industry Events";
            ViewBag.desc = "Search and explore Promo Events, Expo and Roadshows across the country";
            ViewBag.keyword = "PROMOTALKS";
            ViewBag.imgsrc = "";
            ViewBag.pagetitle = "Promo Events";
            ViewBag.isFetureEvent = false;
            bool isFetureEvent = false;
            ViewBag.monthdate = new SelectList(completeEvent(isFetureEvent), "valueField", "textField");
            return View("events");
        }
        public ActionResult _partialevents(string dtmonth, string eventType, bool isFetureEvent)
        {
            var dt = BaseUtil.GetCurrentDateTime();
            if (dtmonth == "All Months" && eventType == "All Events")
            {               
                return PartialView("_partialevents", isFetureEvent==true?  db.tbl_events.Where(e => e.dateTime >= dt).OrderBy(e => e.dateTime).ToList(): db.tbl_events.Where(e => e.dateTime < dt).OrderBy(e => e.dateTime).ToList());
            }
            else {
                IQueryable<tbl_events> query = db.Set<tbl_events>();               
                if (eventType != "All Events")
                {
                    switch (eventType) {
                         case "Association Event":
                            query=query.Where(e => e.associationEvent == true);
                         break;

                        case "Decorators Events":
                            query= query.Where(e => e.decorators == true);
                            break;

                        case "Embroiders Event":
                            query= query.Where(e => e.emroidersEvent == true);
                            break;

                        case "Industry Events":
                            query=query.Where(e => e.industrialEvent == true);
                            break;

                        case "Print Event":
                            query= query.Where(e => e.printerEvent == true);
                            break;
                    }
                    
                }
              
                if (dtmonth != "All Months")
                {
                    var monthint =0 ;
                    var k = dtmonth.Substring(0, dtmonth.IndexOf('-')-1);
                    monthint= BaseUtil.GetIntOfMonthName(k);                   

                    var year =Convert.ToInt16( dtmonth.Substring(dtmonth.IndexOf('-')+2,4));
                    query = query.Where(e => e.dateTime.Month == monthint && e.dateTime.Year == year);
                }
                var tblevents_ = query.ToList();
                return PartialView("_partialevents", tblevents_);
            }
        }
        

        [Route("news/{title}")]
        public ActionResult News_Description(string title)
        {
            if (title == "")
            {
                return RedirectToAction("PageNotFound");
            }
            var news = db.news;
            var modelview = news.Where(e => e.slugURL == title).FirstOrDefault();
            if (modelview == null)
            {
                return RedirectToAction("PageNotFound");
            }
            
            ViewBag.modelView = modelview;
            ViewBag.myList = news.Take(5).Where(e=>e.newsID!=modelview.newsID).OrderByDescending(e => e.createdDate).ToList();
            ViewBag.category = "news";

            ViewBag.title = modelview.title;
            ViewBag.desc = modelview.seo_OGDesription;
            ViewBag.keyword = modelview.seo_metaKeyword;
            ViewBag.imgsrc = modelview.seo_OGImage;
            ViewBag.pagetitle = modelview.title;
            ViewBag.lastUpdate = modelview.updateON;
            return View("Description");
        }
        [Route("marketing/{title}")]
        public ActionResult Marketing_Description(string title)
        {
            if (title == "")
            {
                return RedirectToAction("PageNotFound");
            }
            var Marketing = db.marketings;
            var modelview = Marketing.Where(e => e.slugURL == title).FirstOrDefault();
            if (modelview == null)
            {
                return RedirectToAction("PageNotFound");
            }
           
            ViewBag.modelView = modelview;

            ViewBag.title = modelview.title;
            ViewBag.desc = modelview.seo_OGDesription;
            ViewBag.keyword = modelview.seo_metaKeyword;
            ViewBag.imgsrc = modelview.seo_OGImage;

            ViewBag.myList = Marketing.Where(e => e.marketingID != modelview.marketingID).Take(5).OrderByDescending(e => e.createdDate).ToList();
            ViewBag.category = "Marketing";
            ViewBag.pagetitle = modelview.title;

            return View("Description");
        }
        [Route("technology/{title}")]
        public ActionResult technology_Description(string title)
        {
            if (title == "")
            {
                return RedirectToAction("PageNotFound");
            }
            var technologyID = db.technologies;
            var modelview = technologyID.Where(e => e.slugURL == title).FirstOrDefault();
            if (modelview == null)
            {
                return RedirectToAction("PageNotFound");
            }
           
            ViewBag.modelView = modelview; 


            ViewBag.title = modelview.title;
            ViewBag.desc = modelview.seo_OGDesription;
            ViewBag.keyword = modelview.seo_metaKeyword;
            ViewBag.imgsrc = modelview.seo_OGImage;
              ViewBag.pagetitle = modelview.title;
            ViewBag.myList = technologyID.Where(e => e.technologyID != modelview.technologyID).Take(5).OrderByDescending(e => e.createdDate).ToList();
            ViewBag.category = "technology";
            return View("Description");
        }

        [Route("promo-screen-printers")]
        public ActionResult promo_screen_printers()
        {
            ViewBag.title = "Get Latest News and Updates from Promo Products Industry";
            ViewBag.desc = "Get Latest news and updates about promotional products, art work and design as well promo events from promotional product industry";
            ViewBag.keyword = "PROMOTALKS";
            ViewBag.imgsrc = "";
            ViewBag.pagetitle = "Promo Screen Printers";
            ViewBag.type = "Promo Screen";

            ViewBag.HeadTitle = "Promo Screen Printers";
            return View("businessInformation");
        }
        [Route("promo-embroiders")]
        public ActionResult promo_embroiders()
        {
            ViewBag.title = "Get Latest News and Updates from Promo Products Industry";
            ViewBag.desc = "Get Latest news and updates about promotional products, art work and design as well promo events from promotional product industry";
            ViewBag.keyword = "PROMOTALKS";
            ViewBag.imgsrc = "";
            ViewBag.pagetitle = "Promo Embroiders";
            ViewBag.type = "Promo Embroiders";

            ViewBag.HeadTitle = "Promo Embroiders";
            return View("businessInformation");
        }
        [HttpGet]
        public ActionResult partial_subscription()
        {
            return PartialView("partial_subscription");
        }
        [HttpPost]
        public string partial_subscription(string txt_name, string txt_email)
        {
            var result = "";
            SubscriberInfo SubscriberInfo = new SubscriberInfo();
            SubscriberInfo.email = txt_email;
            SubscriberInfo.name = txt_name;
            SubscriberInfo.isActive = true;
            try
            {
                db.SubscriberInfoes.Add(SubscriberInfo);
                db.SaveChanges();

                
                    StreamReader sr = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/emailers/SubscriptionToClient.html"));
                    string HTML_Body = sr.ReadToEnd();
                    string final_Html_Body = HTML_Body.Replace("#name", txt_name);
                    sr.Close();
                     string To = txt_email;
                    string mail_Subject = "Thank you for subscribing PromoTalks.";
                result = BaseUtil.sendEmailer(To, mail_Subject, final_Html_Body);
                
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
            return result;
        }

        [HttpPost]
        public string SendOfferToEmail(string txt_email,  string url)
        {
            var result = "";
            try
            {
                using (WebClient client = new WebClient())
                {

                    string stringThatKeepsYourHtml = client.DownloadString(url);
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(stringThatKeepsYourHtml);
                    doc.GetElementbyId("productTitle").InnerHtml = "";
                    string whatUrLookingFor = doc.GetElementbyId("emailContentpickup").InnerHtml;
                    string To = txt_email;
                    string mail_Subject = "Offer send ";
                     result = BaseUtil.sendEmailer(To, mail_Subject, whatUrLookingFor);
                    return result;
                }
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
            return result;

        }
        [HttpPost]
        public Byte[] PdfSharpConvert(string url)
        {
            Byte[] res = null;
            using (WebClient client = new WebClient())
            {
                string stringThatKeepsYourHtml = client.DownloadString(url);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(stringThatKeepsYourHtml);
                string html = doc.GetElementbyId("emailContentpickup").InnerHtml;
                IronPdf.HtmlToPdf Renderer = new IronPdf.HtmlToPdf();
                var PDF = Renderer.RenderHtmlAsPdf(html);
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;filename=\"FileName.pdf\"");
                // edit this line to display ion browser and change the file name
                Response.BinaryWrite(PDF.BinaryData);
                Response.Flush();
                Response.End();
            }
            return res;
        }
        [HttpGet]
        public ActionResult partial_buisinessInformation(string type, string location, string name)
        {
            IQueryable<buisinessInformation>buisinessInformations = db.buisinessInformations.OrderByDescending(e => e.createdDate);

            if (type == "Promo Screen")
            {
                buisinessInformations = buisinessInformations.Where(e => e.isScreenPrinter == true);
            }
            else if (type == "Promo Embroiders")
            {
                buisinessInformations = buisinessInformations.Where(e => e.isEmbroiders == true);
            }
            if (name != "")
            {
                buisinessInformations = db.buisinessInformations.Where(e => e.bname.Contains(name));
            }
            if (location != "")
            {
                Int64 Zipcode;
                if (Int64.TryParse(location, out Zipcode))
                {
                    var zc = Zipcode.ToString();
                    buisinessInformations = db.buisinessInformations.Where(e => e.zipCode.Contains(zc));
                }
                else
                {
                    buisinessInformations = db.buisinessInformations.Where(e => e.City.Contains(location));
                }
            }
            var buisinessInformations_ = buisinessInformations.ToList();
            ViewBag.serviceList = db.buisinessServices.Include(e => e.service).OrderBy(e => e.service.ServiceName).ToList();
            return PartialView("partial_buisinessInformation", buisinessInformations_);
        }

        [Route("promo-screen-printers/{name}")]
        public ActionResult promo_screen_printersBIDescription(string name)
        {
            if (name == "")
            {
                return RedirectToAction("PageNotFound");
            }
            var modelview = db.buisinessInformations.Include(e=>e.state).Where(e => e.slugURL == name && e.isActive == true).FirstOrDefault();
            if (modelview == null)
            {
                return RedirectToAction("PageNotFound");
            }
            var buisinessInformations = modelview;
            ViewBag.serviceList = db.buisinessServices.Include(e => e.service).ToList();
            ViewBag.title = modelview.bname;
            ViewBag.desc = modelview.description;
            ViewBag.keyword = modelview.seo_metaKeyword;
            ViewBag.imgsrc = modelview.seo_OGImage;
            ViewBag.pagetitle = modelview.bname;
            return View("BIDescription",buisinessInformations);
        }
        [Route("promo-embroiders/{name}")]
        public ActionResult promoembroidersBIDescription(string name)
        {
            if (name == "")
            {
                return RedirectToAction("PageNotFound");
            }
            var modelview = db.buisinessInformations.Include(e => e.state).Where(e => e.slugURL == name && e.isActive == true).FirstOrDefault();
            if (modelview == null)
            {
                return RedirectToAction("PageNotFound");
            }
            var buisinessInformations = modelview;
            ViewBag.title = modelview.bname;
            ViewBag.desc = modelview.description;
            ViewBag.keyword = modelview.seo_metaKeyword;
            ViewBag.imgsrc = modelview.seo_OGImage;
            ViewBag.pagetitle = modelview.bname;
            ViewBag.serviceList = db.buisinessServices.Include(e => e.service).ToList();
            return View("BIDescription", buisinessInformations);
        }
        public ActionResult searchResult(string searchKeyWord)
        {
            var searchresult =(db.search_result(searchKeyWord)).ToList();
            if (searchresult.Count == 0)
            {
                List<search_result_Result> Listsearchresult = new List<search_result_Result>();

                search_result_Result osearchresult = new search_result_Result();
                osearchresult.label = "No Result Found";
                          
                Listsearchresult.Add(osearchresult);
                searchresult = Listsearchresult;
            }
            return Json(searchresult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult searchInBusinessTable(string searchKeyWord,String ColummName, string type)
        {
            IQueryable<buisinessInformation> searchresult = db.buisinessInformations;
            if (ColummName == "bname")
            {
                searchresult = searchresult.Where(e => e.bname.Contains(searchKeyWord));
            }
            if (ColummName == "location")
            {
                Int64 Zipcode;
                if (Int64.TryParse(searchKeyWord, out Zipcode))
                {
                    var zc = Zipcode.ToString();
                    searchresult = searchresult.Where(e => e.zipCode.Contains(zc));
                }
                else
                {
                    searchresult = searchresult.Where(e => e.City.Contains(searchKeyWord));
                }
            }
            var searchresult_ = searchresult.Select(e => new { e.bname, e.City }).ToList();
            return Json(searchresult_, JsonRequestBehavior.AllowGet);
        }

        [Route("events/{title}")]
        public ActionResult EventDescription(string title)
        {
            if (title == "")
            {
                return RedirectToAction("PageNotFound");
            }
            var tbl_events = db.tbl_events;
            var modelview = tbl_events.Where(e => e.slugURL == title).FirstOrDefault();
            if (modelview == null)
            {
                return RedirectToAction("PageNotFound");
            }
          
            var mod = modelview;
            var dt = mod.dateTime;
            var mon_ = BaseUtil.GetCurrentDateTime();
            var mon = dt.Month;
            ViewBag.monthName = dt.ToString("MMMM");
            //var day = dt.Day;           
            ViewBag.monthsEventsList = tbl_events.Where(e => e.dateTime.Month == mon && e.eventID != modelview.eventID).OrderBy(e => e.dateTime).ToList();

            ViewBag.title = modelview.seo_OGTitle;
            ViewBag.desc = modelview.seo_OGDesription;
            ViewBag.keyword = modelview.seo_metaKeyword;
            ViewBag.imgsrc = modelview.featuredImag;
            ViewBag.pagetitle = modelview.name;
            return View(mod);
        }
        
    
        [Route("promo-stores")]
        public ActionResult promostore()
        {
            ViewBag.title = "PROMOTALKS";
            ViewBag.desc = "PROMOTALKS";
            ViewBag.keyword = "PROMOTALKS";
            ViewBag.imgsrc = "";
            ViewBag.pagetitle = "Promo Stores";
            return View();
        }
        [Route("promo-artwork")]
        public ActionResult artwork()
        {
            ViewBag.title = "PROMOTALKS";
            ViewBag.desc = "PROMOTALKS";
            ViewBag.keyword = "PROMOTALKS";
            ViewBag.imgsrc = "";
            ViewBag.pagetitle = "Promo artwork";

            return View();
        }
       
        [Route("catalog/{title}")]
        public ActionResult catalogDescription(string title)
        {
            if (title == "")
            {
                return RedirectToAction("PageNotFound");
            }
            var catalog = catalogService(title);

            if (catalog == null)
            {
                return RedirectToAction("PageNotFound");
            }

            ViewBag.title = catalog.productTitle;
            ViewBag.desc = catalog.seo_OGDesription;
            ViewBag.keyword = catalog.seo_metaKeyword;
            ViewBag.imgsrc = catalog.thumbImageURL;
            ViewBag.pagetitle = catalog.productTitle;
            ViewBag.catalogcatagory = TempData["catalogcatagory"].ToString();
            ViewBag.TwoMoreCatalogs = (List<productCatalog>)TempData["TwoMoreCatalogs"];
            return View(catalog);
        }

        [Route("flyer/{title}")]
        public ActionResult offerDescription(string title)
        {
            if (title == "")
            {
                return RedirectToAction("PageNotFound");
            }
            var catalog = catalogService(title);

            if (catalog == null)
            {
                return RedirectToAction("PageNotFound");
            }

            ViewBag.title = catalog.productTitle;
            ViewBag.desc = catalog.seo_OGDesription;
            ViewBag.keyword = catalog.seo_metaKeyword;
            ViewBag.imgsrc = catalog.thumbImageURL;
            ViewBag.pagetitle = catalog.productTitle;
            ViewBag.catalogcatagory = TempData["catalogcatagory"].ToString();
            ViewBag.TwoMoreCatalogs = (List<productCatalog>)TempData["TwoMoreCatalogs"];
            return View(catalog);
        }
        private productCatalog catalogService(string title)
        {
            IQueryable<productCatalog> searchresult = db.productCatalogs.Include(e => e.supplier).Where(e => e.isDeleted == false);
            var catalog = searchresult.Where(e => e.slugURL == title && e.isDeleted == false).FirstOrDefault();
            if (catalog == null)
            {
                return null;
            }
            TempData["TwoMoreCatalogs"] = searchresult.Where(e => e.supplierID == catalog.supplierID && e.catalogID != catalog.catalogID).OrderByDescending(e => e.createdDate).Take(2).ToList();

            var catalogcatagory = "";
            var licat = db.catalogCatagories.Include(e => e.productCategory).Where(e => e.catalogID == catalog.catalogID).Select(e => new { e.productCategory.productCategory1 }).OrderBy(e => e.productCategory1).ToList();
            foreach (var a in licat)
            {
                catalogcatagory = catalogcatagory + a.productCategory1 + ", ";
            }
            TempData["catalogcatagory"] = "";
            if (licat.Count > 0)
            {
                int index = catalogcatagory.LastIndexOf(',');
                TempData["catalogcatagory"] = catalogcatagory.Substring(0, catalogcatagory.Length - 2);
            }
            return catalog;
        }
        public ActionResult PageNotFound()
        {
            Response.StatusCode = 404;
            return View();
        }
        [Route("viewpdf/{title}")]
        public ActionResult viewflipbook(string title)
        {
            if (title == "")
            {
                return RedirectToAction("PageNotFound");
            }
            var catalog = db.productCatalogs.Include(e => e.supplier).Where(e => e.slugURL == title).FirstOrDefault();
            if (catalog == null)
            {
                return RedirectToAction("PageNotFound");
            }
            ViewBag.title = catalog.productTitle;
            ViewBag.desc = catalog.seo_OGDesription;
            ViewBag.keyword = catalog.seo_metaKeyword;
            ViewBag.imgsrc = catalog.seo_OGImage;
            ViewBag.pagetitle = catalog.productTitle;
            ViewBag.pdfURL = catalog.pdfURL;
            return View();
        }
    }
   
   
    public class ddl {
        public string textField { get; set; }
        public string valueField { get; set; }
    }


}