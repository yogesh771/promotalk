
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace promoTalk.Models
{
    public class BaseClass : Controller
    {
     
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            String ControllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToUpper();
            String ActionName = filterContext.ActionDescriptor.ActionName.ToUpper();
            String Action = string.Format("{0}Controller{1}", ControllerName, ActionName).ToUpper();

            if (BaseUtil.ListControllerExcluded().Contains(ControllerName))
            {
                if (ControllerName == "ACCOUNT" || ControllerName == "HOME" || (ControllerName == "marketings" && ActionName=="INDEX")
                      || (ControllerName == "technologies" && ActionName == "INDEX") || (ControllerName == "news" && ActionName == "INDEX")
                       || (ControllerName == "productCatalogs" && ActionName == "INDEX") )
                {
                    return;
                }
            }
                if (BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()) == "")
                {
                    filterContext.Result = null;
                    filterContext.Result = new RedirectResult("/Account/login");
                    return;
                }
                return;
           
        }
        protected override void OnException(ExceptionContext exceptionContext)
        {
             promotalkEntities db = new promotalkEntities();
            if (!exceptionContext.ExceptionHandled)
            {
                AppErrorLog oAppErrorLogs = new AppErrorLog();
                oAppErrorLogs.ErrorMsg = exceptionContext.Exception.Message;
                 ViewBag.msg= exceptionContext.Exception.Message;
                oAppErrorLogs.datelog = BaseUtil.GetCurrentDateTime();
                db.AppErrorLogs.Add(oAppErrorLogs);
                db.SaveChanges();
                exceptionContext.ExceptionHandled = true;               
                Response.Redirect("~/Views/Shared/Error.cshtml");
            }
        }

    }
        
}