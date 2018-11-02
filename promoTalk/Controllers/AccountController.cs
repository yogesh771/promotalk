using promoTalk.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace promoTalk.Controllers
{
    public class AccountController : BaseClass
    {
        promotalkEntities db = new promotalkEntities();
       
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel oLoginViewModel)
        {
            string password = BaseUtil.encrypt(oLoginViewModel.password);
            string userName = BaseUtil.encrypt(oLoginViewModel.userName);
            var oUserInfo = db.users.Where(e => e.username == userName && e.pwd == password).FirstOrDefault();
            if (oUserInfo != null)
            {
                BaseUtil.SetSessionValue(AdminInfo.LoginID.ToString(), oUserInfo.userID.ToString());             
                return RedirectToAction("Index", "suppliers", new { id= 1}); 
            }
            ViewBag.msg = "Invalid user credentials";
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public int RemoveImage(int id, string Modulename)
        {
            var v = db.RemoveImage(id, Modulename);
            return v;

        }
        public int daleteRecords(string tableName, int id)
        {
            try
            {
                var obj = (dynamic)null;
                switch (tableName)
                { 
                    case "buisinessInformation":
                        obj = db.buisinessInformations.Find(id);
                        db.buisinessInformations.Remove(obj);
                        db.SaveChanges();
                        break;                       
               
                    case "productCatalog":
                        obj = db.productCatalogs.Find(id);
                        obj.isDeleted = true;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        break;
                    case "productCategory":
                        obj = db.productCategories.Find(id);
                        obj.isActive = false;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();                       
                        break;

                    case "marketing":
                        obj = db.marketings.Find(id);
                        db.marketings.Remove(obj);
                        db.SaveChanges();
                        break;
                    case "services":
                        obj = db.services.Find(id);
                        obj.isActive = false;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        break;

                    case "supplier":
                        obj = db.suppliers.Find(id);
                        obj.isActive = false;
                        db.Entry(obj).State = EntityState.Modified;
                        db.SaveChanges();
                        break;
                    case "technology":
                        obj = db.technologies.Find(id);
                        db.technologies.Remove(obj);
                        db.SaveChanges();
                        break;
                    case "news":
                        obj = db.news.Find(id);
                        db.news.Remove(obj);
                        db.SaveChanges();
                        break;

                }
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
           
        }



      
        public bool isSlugExist(string slugURL,string tblname, Int64 ID, int isOffer)   //catalog=0 ,isOffer=1 //ID=0 new
        {

            var isExist = (dynamic)null;
            switch (tblname)
            {
                case "productCatalogs":  //0=new 1 edit
                    if (isOffer == 1)
                    {
                        isExist = db.productCatalogs.Where(u => u.slugURL == slugURL && u.isOffer == true && u.catalogID != ID && u.isDeleted == false).FirstOrDefault();
                    }
                    else
                    {
                        isExist = db.productCatalogs.Where(u => u.slugURL == slugURL && u.isOffer == false && u.catalogID != ID && u.isDeleted == false).FirstOrDefault();
                    }
                    break;
                case "news"://0=new 1 edit
                    isExist = db.news.Where(u => u.slugURL == slugURL && u.newsID != ID).FirstOrDefault();
                    break;
                case "technologies":
                    isExist = db.technologies.Where(u => u.slugURL == slugURL && u.technologyID != ID).FirstOrDefault();
                    break;
                case "marketings":
                    isExist = db.marketings.Where(u => u.slugURL == slugURL && u.marketingID != ID).FirstOrDefault();
                    break;
                case "buisinessInformations":  //isoffer = 0 for slugURL and 1 for website url
                    if (isOffer == 0)
                    {
                        isExist = db.buisinessInformations.Where(u => u.slugURL == slugURL && u.buisinessID != ID && u.isActive == true).FirstOrDefault();
                    }
                    if (isOffer == 1)
                    {
                        isExist = db.buisinessInformations.Where(u => u.website == slugURL && u.buisinessID != ID && u.isActive == true).FirstOrDefault();
                    }
                    break;
                case "events":
                    isExist = db.tbl_events.Where(u => u.slugURL == slugURL && u.eventID != ID).FirstOrDefault();
                    break;
            }
            
            if (isExist == null)
            {
                return true;
            }
            return false;
        }       
       
       
       
    }
}