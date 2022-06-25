using QuranEducation.Helpers;
using QuranEducation.Models;
using QuranEducation.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuranEducation.Controllers
{
    public class HomeController:MyController
    {
        
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public bool ChangeLanguage(string Id)
        {
            try {
                LanguageMang.SetLanguage("", Id);
                return true;
            }
            catch { return false; }
        }
        public ActionResult Done()
        {
            return View();
        }


    }

}