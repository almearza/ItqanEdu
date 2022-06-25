using QuranEducation.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuranEducation.Controllers
{
    public class MyController : Controller
    {
            protected override IAsyncResult BeginExecuteCore(AsyncCallback callback, object state)
            {
                HttpCookie langCookie = Request.Cookies["culture"];
                if (langCookie != null)
                {
                    LanguageMang.SetLanguage("",langCookie.Value);
                }
                return base.BeginExecuteCore(callback, state);
            }
        
    }
}