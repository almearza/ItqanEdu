using QuranEducation.Models;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
namespace QuranEducation.Helpers
{
    public class LanguageMang
    {

        public static List<Languages> AvailableLanguages = new List<Languages> {
            new Languages {
                Name = "العربية", Code = "ar"
            },
            new Languages {
                Name = "English", Code = "en"
            },
            new Languages {
                Name = "French", Code = "fr"
            },
            new Languages
            {
                Name = "Bosnian" , Code ="bo"
            },
            new Languages
            {
                Name = "Thai" , Code ="Ti"
            },
             new Languages
            {
                Name = "Russian" , Code ="os"
            },
              new Languages
            {
                Name = "Portuguese" , Code ="to"
            },
               new Languages
            {
                Name = "Spanish" , Code ="es"
            },
               new Languages
            {
                Name = "German" , Code ="de"
            },
               new Languages
            {
                Name = "Italian" , Code ="it"
            },
               new Languages
            {
                Name = "Philippines" , Code ="fil"
            },
               new Languages
            {
                Name = "أوردو" , Code ="ur"
            },
               new Languages
            {
                Name = "Vietnamese" , Code ="vi"
            },
               new Languages
            {
                Name = "Dutch" , Code ="nl"
            }
        };
        public static bool IsLanguageAvailable(string lang)
        {
            return AvailableLanguages.Where(a => a.Code.Equals(lang)).FirstOrDefault() != null ? true : false;
        }
        public static void SetLanguage(string UserName, string code = "")
        {
            try
            {
                var LangCode = "";
                if (UserName != "")
                {
                    using (var ctx = new ApplicationDbContext())
                    {
                       
                        var _dbCode = ctx.Users.Where(m => m.UserName == UserName).Select(m => m.LangCode).FirstOrDefault();
                        LangCode = !string.IsNullOrEmpty(_dbCode) ? _dbCode : code;
                    }
                }
                else
                {
                    LangCode = code;
                }
                if (!string.IsNullOrEmpty(LangCode) && IsLanguageAvailable(LangCode))
                {

                    var cultureInfo = new CultureInfo(LangCode);
                    Thread.CurrentThread.CurrentUICulture = cultureInfo;
                    HttpCookie langCookie = new HttpCookie("culture", LangCode);

                    langCookie.Expires = DateTime.Now.AddYears(100);
                    HttpContext.Current.Response.Cookies.Add(langCookie);
                }
            }
            catch (Exception)
            {
            }
        }
        public class Languages
        {
            public string Name
            {
                get;
                set;
            }
            public string Code
            {
                get;
                set;
            }
        }
    }

}