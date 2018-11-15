
using System;
using System.IO;
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

            if (BaseUtil.ListControllerTocheckLogin().Contains(ControllerName))
            {
                if (BaseUtil.GetSessionValue(AdminInfo.LoginID.ToString()) == "")
                {
                    filterContext.Result = null;
                    filterContext.Result = new RedirectResult("/Account/login");
                    return;
                }
                return;
            }
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
        public string UploadFile(HttpPostedFileBase file, string path)
        {
            String fileName = "";
            fileName = Guid.NewGuid() + "_" + Path.GetFileName(file.FileName);
            var savedToPath = Path.Combine(Server.MapPath("~" + path), fileName);
            file.SaveAs(savedToPath);
            return path + "/" + fileName;
        }
    }
        
}