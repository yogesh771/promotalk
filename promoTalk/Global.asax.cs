using promoTalk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace promoTalk
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static promotalkEntities db = new promotalkEntities();

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //HArdik
            RegisterRoutes(RouteTable.Routes);
        }
        //HArdik
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.MapPageRoute("GuestMenu","Schoogle-Introduction", "~/Schoogle_Introduction.aspx");

            //List<Page> objPageList = new List<Page>();
            //objPageList = (new PageFactory()).GetAll();
            //for (int i = 0; i < objPageList.Count; i++)
            //{
            //    routes.MapPageRoute(objPageList[i].PageName.ToLower(), objPageList[i].PageCustomUrl.ToLower(), "~/" + objPageList[i].PageName);
            //    //routes.MapPageRoute(objPageList[i].PageCustomUrl.ToLower(), objPageList[i].PageCustomUrl.ToLower(), "~/Default.aspx?" + objPageList[i].PageCustomUrl.ToLower());
            //}

            //List<Business.Core.Article> objArticleList = new List<Business.Core.Article>();
            //objArticleList = (new ArticleFactory()).GetAll();
            //foreach (var objArticle in objArticleList)
            //{
            //    routes.MapPageRoute(objArticle.ArticleTitle.Replace(' ', '_').ToLower(), objArticle.ArticleTitle.Replace(' ', '_').ToLower(), "~/Article_Detail.aspx");//?Id="+objArticle.ArticleId);
            //}

            //var objEvents = db.tbl_events.ToList();

            //foreach (var objEvent in objEvents)
            //{
            //    routes.MapPageRoute("event/title", "~/events/{title}", "~/events/"+objEvent.name.Replace(' ','_').ToLower());
            //}


            //routes.MapPageRoute("events","events")
        }
    }
}
