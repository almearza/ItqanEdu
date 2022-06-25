using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace QuranEducation.Helpers
{
    public class ExceptionLogger : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            string message = "\n" + "controller>>> " + filterContext.RouteData.Values["controller"].ToString();
            message += "\n" + "action>>> " + filterContext.RouteData.Values["action"].ToString();
            message += "\n" + "message>>> " + filterContext.Exception.Message;
            message += "\n" + "InnerException>>>" + filterContext.Exception.InnerException;
            message += "\n" + "TargetSite>>>" + filterContext.Exception.TargetSite;
            message += "\n" + "Requester>>>" + GetIPAddress();
            message += "\n" + "BrowserInfo>>>" + BrowserInfo();
            message += "\n" + "Time>>>" + DateTime.Now.ToString() + "\n";
            logException(message);
            logException("-------------------------------------------------------------------------------------------------------------------------------------");
        }
        private void logException(string data)
        {
            File.AppendAllText(HttpContext.Current.Server.MapPath("~/Exceptions.txt"), data);
        }
        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }
        private string BrowserInfo()
        {
            var context = HttpContext.Current;
            string browserInfo =
            "RemoteUser=" + context.Request.ServerVariables["REMOTE_USER"] + ";\n"
           + "RemoteHost=" + context.Request.ServerVariables["REMOTE_HOST"] + ";\n"
           + "Type=" + context.Request.Browser.Type + ";\n"
           + "Name=" + context.Request.Browser.Browser + ";\n"
           + "Version=" + context.Request.Browser.Version + ";\n"
           + "MajorVersion=" + context.Request.Browser.MajorVersion + ";\n"
           + "MinorVersion=" + context.Request.Browser.MinorVersion + ";\n"
           + "Platform=" + context.Request.Browser.Platform + ";\n"
           + "SupportsCookies=" + context.Request.Browser.Cookies + ";\n"
           + "SupportsJavaScript=" + context.Request.Browser.EcmaScriptVersion.ToString() + ";\n"
           + "SupportsActiveXControls=" + context.Request.Browser.ActiveXControls + ";\n"
           + "SupportsJavaScriptVersion=" + context.Request.Browser["JavaScriptVersion"] + "\n";
            return browserInfo;
        }

    }
}