using AutoMapper;
using AutoMapper.QueryableExtensions;
using QuranEducation.Models;
using QuranEducation.Models.VM;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using QuranEducation.Helpers;

namespace QuranEducation.Controllers
{
    [Authorize(Roles = RoleNames.AdminLevel)]
    public class TutorialController : MyController
    {
        private ApplicationDbContext _context;
        private DateTime _currentDate = DateTime.Now;

        public TutorialController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult Index()
        {
            //var Tutorials = _context.Tutorials.ProjectTo<TutorialVM>().ToList();
            var Tutorials = _context.Tutorials.ProjectTo<TutorialVM>().ToList();

            return View(Tutorials);
        }
        public ActionResult StudentRequest()
        {
            var requests = _context.StudentTutorials.Include(t => t.Tutorial).Where(m => m.AcceptDate == null).ToList();
            var SubList = new List<SubVM>();
            if (requests != null)
            {
                foreach (var req in requests)
                {
                    ApplicationUser student = _context.Users.Where(u => u.UserName == req.UserName).FirstOrDefault();
                    // var student = _context.Users.Where(u => u.UserName == req.UserName).FirstOrDefault();
                    if (student != null)
                    {
                        SubList.Add(new SubVM
                        {
                            Id = req.Id,
                            RequestDate = req.RequestDate,
                            StudentName = student.FullName,
                            StudentPhone = student.PhoneNumber,
                            StudentUserName = student.UserName,
                            StudentEmial = student.Email,
                            TutorialSubCount = _context.StudentTutorials.Where(t => t.Id == req.TutorialId && t.ManagemetAccept).Count(),
                            TutorialTitle = req.Tutorial.Title
                        });

                    }

                }
            }
            return View(SubList);
        }
        [HttpPost]
        public bool AcceptSub(int Id)
        {
            try
            {
                var sub = _context.StudentTutorials.Include(t => t.Tutorial).FirstOrDefault(m => m.Id == Id);
                if (sub != null)
                {
                    sub.ManagemetAccept = true;
                    sub.AcceptDate = DateTime.Now;
                    //send sms 
                    var lang = _context.Users.Where(u => u.UserName == sub.UserName).Select(u => u.LangCode).FirstOrDefault();
                    var message = new Inbox
                    {
                        Arrival = DateTime.Now,
                        AUserName = sub.UserName,
                        Message = getStringFromRes("ManagementAccept", lang),
                        Sender = getStringFromRes("ManagementTitle", lang),
                        Link = Url.Action("MyTutorials", "Student", null, Request.Url.Scheme)
                    };
                    _context.Inbox.Add(message);
                    EmailSender sender = new EmailSender();
                    var fullSms = message.Message + "  - " + sub.Tutorial?.Title + "  - " + message.Link;
                    sender.SendMail(sub.UserName, fullSms);
                    _context.SaveChanges();
                    Response.StatusCode = 200;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
                return false;
            }
            Response.StatusCode = 403;
            return false;
        }
        [HttpPost]
        public bool RejectSub(int Id)
        {
            try
            {
                var sub = _context.StudentTutorials.Include(t => t.Tutorial).FirstOrDefault(m => m.Id == Id);
                if (sub != null)
                {
                    sub.ManagemetAccept = false;
                    sub.AcceptDate = DateTime.Now;
                    //send sms 
                    var lang = _context.Users.Where(u => u.UserName == sub.UserName).Select(u => u.LangCode).FirstOrDefault();
                    var message = new Inbox
                    {
                        Arrival = DateTime.Now,
                        AUserName = sub.UserName,
                        Message = getStringFromRes("ManagementReject", lang),
                        Sender = getStringFromRes("ManagementTitle", lang),
                        Link = Url.Action("GetAllTutorials", "Report", null, Request.Url.Scheme)
                    };
                    _context.Inbox.Add(message);
                    EmailSender sender = new EmailSender();
                    var fullSms = message.Message + "  - " + sub.Tutorial?.Title + "  - " + message.Link;
                    sender.SendMail(sub.UserName, fullSms);
                    _context.SaveChanges();
                    Response.StatusCode = 200;
                    return true;
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = 403;
                return false;
            }
            Response.StatusCode = 403;
            return false;
        }
       
        public ActionResult HandlTutorial(int? Id)
        {
            if (Id.HasValue)
            {
                var TutorialVM = _context.Tutorials.ProjectTo<TutorialVM>().FirstOrDefault(m => m.Id == Id);
                return View(TutorialVM);
            }
            var _currentDate = DateTime.Now;
            return View(new TutorialVM
            {
                OpenDate = _currentDate,
                CloseDate = _currentDate
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTutorial(TutorialVM model)
        {
            var _currentDate = DateTime.Now.Date;
            if (!ModelState.IsValid)
            {
                return View("HandlTutorial", model);
            }

            if (model.OpenDate >= model.CloseDate)
            {
                ModelState.AddModelError("CloseDate", "تاريخ قفل الدورة يجب ان يكون اكبر من تاريخ فتح الدورة");
                return View("HandlTutorial", model);
            }
            if (model.Id == 0)
            {
                if (model.OpenDate < _currentDate)
                {
                    ModelState.AddModelError("OpenDate", "تاريخ فتح الدورة يجب ان يكون اكبر من او يساوي تاريخ اليوم");
                    return View("HandlTutorial", model);
                }
                model.DoneBy = User.Identity.Name;
                model.StDate = DateTime.Now;
                model.Active = true;

                //save image of tut
                var tutorialImg = model.Image;
                if (tutorialImg == null || tutorialImg.ContentLength <= 0)
                {
                    ModelState.AddModelError("Image", "الرجاء اختيار الصورة اولا");
                    return View("HandlTutorial", model);
                }
                var imgExt = tutorialImg.FileName.Substring(tutorialImg.FileName.LastIndexOf(".")).Replace("\"", "");

                var tutImg = "TutImg_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + imgExt;
                var tutImg_ServerSavePath = Path.Combine(Server.MapPath("~/images/tutorial/") + tutImg);

                //Save file to server folder  
                tutorialImg.SaveAs(tutImg_ServerSavePath);
                model.ImageUrl = tutImg;

                _context.Tutorials.Add(Mapper.Map<Tutorial>(model));
            }
            else
            {
                var DbTutorial = _context.Tutorials.FirstOrDefault(m => m.Id == model.Id);
                if (DbTutorial == null) return HttpNotFound();
                //save image of tut
                var tutorialImg = model.Image;
                if (tutorialImg != null && tutorialImg.ContentLength > 0)
                {

                    var imgExt = tutorialImg.FileName.Substring(tutorialImg.FileName.LastIndexOf(".")).Replace("\"", "");

                    var tutImg = "TutImg_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + imgExt;
                    var tutImg_ServerSavePath = Path.Combine(Server.MapPath("~/images/tutorial/") + tutImg);

                    tutorialImg.SaveAs(tutImg_ServerSavePath);
                    DbTutorial.ImageUrl = tutImg;
                }

                DbTutorial.Title = model.Title;
                DbTutorial.Descr = model.Descr;
                DbTutorial.OpenDate = model.OpenDate;
                DbTutorial.CloseDate = model.CloseDate;
                DbTutorial.LangCode = model.LangCode;
                DbTutorial.InstUName = model.InstUName;
                DbTutorial.DoneBy = User.Identity.Name;
                DbTutorial.StDate = DateTime.Now;
            }
            var message = new Inbox
            {
                Arrival = _currentDate,
                AUserName = model.InstUName,
                Message = getStringFromRes("TutAssignToInst", model.LangCode) + " : " + model.Title,
                Sender = getStringFromRes("ManagementTitle", model.LangCode),
                Link = Url.Action("GetTutorials", "Report", null, Request.Url.Scheme)
            };
            _context.Inbox.Add(message);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public bool Activate(int Id, bool active)
        {
            var Tutorial = _context.Tutorials.FirstOrDefault(m => m.Id == Id);
            if (Tutorial == null)
            {
                return false;
            }
            else
            {
                Tutorial.Active = !active;
                Tutorial.DoneBy = User.Identity.Name;
                Tutorial.StDate = DateTime.Now;

                _context.SaveChanges();
                return true;
            }
        }
        private string getStringFromRes(string keyVal, string LangKey)
        {
            var strVal = QuranRes.ResourceManager.GetString(keyVal, CultureInfo.GetCultureInfo(LangKey));
            strVal = strVal == null ? QuranRes.ResourceManager.GetString(keyVal, CultureInfo.DefaultThreadCurrentCulture) : strVal;
            return strVal;
        }
        //add
        public ActionResult Getstudent1(int Id)
        {
            //var tutorials = _context.StudentTutorials.Where(m => m.TutorialId == Id);

            var studentRoleId = _context.Roles.FirstOrDefault(m => m.Name == RoleNames.StudentLevel).Id;
            if (User.IsInRole(RoleNames.AdminLevel))
            {
                var subStudents = _context.StudentTutorials.Where(m => m.TutorialId == Id && m.ManagemetAccept).Select(m => m.UserName).ToList();
                // var subStudents = _context.StudentTutorials.Where(m => m.TutorialId == Id).Select(m => m.UserName).ToList();
                var users = _context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(studentRoleId) && subStudents.Contains(u.UserName)).ToList();
                ViewBag.tutId = Id;
                return View(users);
            }
            else
            {
                var users = _context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(studentRoleId)).ToList();
                return View(users);
            }
        }


    }
}