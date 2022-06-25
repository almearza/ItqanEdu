using AutoMapper;
using QuranEducation.Helpers;
using QuranEducation.Models;
using QuranEducation.Models.VM;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.IO;
using AutoMapper.QueryableExtensions;

namespace QuranEducation.Controllers
{
    public class AssigmentController : MyController
    {
        private ApplicationDbContext _context;
        DateTime _currentDate = DateTime.Now;
        public AssigmentController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: InstructorProfile
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        public ActionResult GetAssigments(int Id)
        {
            var tut = _context.Tutorials.FirstOrDefault(m => m.Id == Id && m.InstUName == User.Identity.Name);
            if (tut == null) return HttpNotFound();
            var ass = _context.Assigments.Where(m => m.TutorialId == tut.Id && m.Tutorial.InstUName == User.Identity.Name)
                .ToList();
            ViewBag.TutId = tut.Id;
            ViewBag.TutTitle = tut.Title;
            return View(ass);
        }
        public ActionResult CreateAssignment(int Id)
        {
            var tut = _context.Tutorials.FirstOrDefault(m => m.Id == Id && m.InstUName == User.Identity.Name && m.Active);
            if (tut == null) return HttpNotFound();
            var dateTime = string.Format("{0:hh:mm tt}", _currentDate);
            return View("HandlAssigment", new AssigmentVM { Tutorial = tut, Id = 0, TutorialId = Id, StartTime = dateTime, EndTime = dateTime });
        }
        public ActionResult HandlAssigment(int Id = 0)
        {
            HttpCookie langCookie = Request.Cookies["culture"];
            var langCode = "En";
            if (langCookie != null)
            {
                langCode = langCookie.Value;
            }
            LanguageMang.SetLanguage("", "En");


            var Assigment = _context.Assigments.Include(m => m.AssigmentAttachments).Include(m => m.Tutorial).FirstOrDefault(m => m.Id == Id && m.Tutorial.InstUName == User.Identity.Name);
            if (Assigment == null) return HttpNotFound();
            var AssigmentVM = Mapper.Map<AssigmentVM>(Assigment);
            //DateTime.ParseExact(lecDateTime.Date, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            var startDateTime = Assigment.StartTime;
            var endDateTime = Assigment.EndTime;

            AssigmentVM.StartDate = startDateTime.ToString("MM/dd/yyyy");
            AssigmentVM.StartTime = string.Format("{0:hh:mm tt}", startDateTime);

            AssigmentVM.EndDate = endDateTime.ToString("MM/dd/yyyy");
            AssigmentVM.EndTime = string.Format("{0:hh:mm tt}", endDateTime);

            LanguageMang.SetLanguage("", langCode);

            return View(AssigmentVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SaveAssignment(AssigmentVM model)
        {
            var _currentDate = DateTime.Now.Date;
            if (!ModelState.IsValid)
            {
                return View("HandlAssigment", model);
            }
            HttpCookie langCookie = Request.Cookies["culture"];
            var langCode = "En";
            if (langCookie != null)
            {
                langCode = langCookie.Value;
            }
            LanguageMang.SetLanguage("", "En");
            var startTime = model.StartDate + " " + model.StartTime;
            var startDateTime = DateTime.ParseExact(startTime, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            var endTime = model.EndDate + " " + model.EndTime;
            var endDateTime = DateTime.ParseExact(endTime, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            LanguageMang.SetLanguage("", langCode);
            var assTut = _context.Tutorials.FirstOrDefault(t => t.Id == model.TutorialId && t.Active);
            if (assTut == null)
            {
                ModelState.AddModelError("", QuranRes.AssTutNotFound);
                return View("HandlAssigment", model);
            }
            var lecLang = assTut.LangCode;

            if (startDateTime >= endDateTime)
            {
                ModelState.AddModelError("", QuranRes.assEndBeggerThanLecStart);
                return View("HandlAssigment", model);
            }
            if (model.Id == 0)
            {
                if (startDateTime < _currentDate)
                {
                    ModelState.AddModelError("", QuranRes.assTimeError);
                    return View("HandlAssigment", model);
                }
                //save atta of lec
                var assAttachments = model.AFiles;
                if (assAttachments != null && assAttachments.Count() > 0)
                {
                    var toBeAddedAtts = new List<AssigmentAttachment>();
                    foreach (var assFile in assAttachments)
                    {
                        if (assFile != null)
                        {
                            var attExt = assFile.FileName.Substring(assFile.FileName.LastIndexOf(".")).Replace("\"", "");

                            var assatt = "Assatt_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + attExt;
                            var assatt_ServerSavePath = Path.Combine(Server.MapPath("~/Attachments/Assigments/") + assatt);

                            assFile.SaveAs(assatt_ServerSavePath);
                            toBeAddedAtts.Add(new AssigmentAttachment { FileUrl = assatt });
                        }
                    }
                    model.AssigmentAttachments = toBeAddedAtts;
                }
                var ass = Mapper.Map<Assigment>(model);
                ass.StartTime = startDateTime;
                ass.EndTime = endDateTime;
                ass.StDate = _currentDate;
                ass.Tutorial = null;
                _context.Assigments.Add(ass);
            }
            else
            {
                var DbAssignment = _context.Assigments.FirstOrDefault(m => m.Id == model.Id);
                if (DbAssignment == null) return HttpNotFound();
                //save image of tut
                var assAttachments = model.AFiles;
                if (assAttachments != null && assAttachments.Count() > 0)
                {
                    var toBeAddedAtts = new List<AssigmentAttachment>();
                    foreach (var assFile in assAttachments)
                    {
                        if (assFile != null)
                        {
                            var attExt = assFile.FileName.Substring(assFile.FileName.LastIndexOf(".")).Replace("\"", "");

                            var assatt = "Assatt_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + attExt;
                            var assatt_ServerSavePath = Path.Combine(Server.MapPath("~/Attachments/Assigments/") + assatt);

                            assFile.SaveAs(assatt_ServerSavePath);
                            toBeAddedAtts.Add(new AssigmentAttachment { FileUrl = assatt });
                        }
                    }
                    model.AssigmentAttachments = toBeAddedAtts;
                }

                DbAssignment.Title = model.Title;
                DbAssignment.Descr = model.Descr;
                DbAssignment.StartTime = startDateTime;
                DbAssignment.EndTime = endDateTime;
                DbAssignment.TutorialId = model.TutorialId;
                DbAssignment.Degree = model.Degree;
                DbAssignment.AssType = model.AssType;
                DbAssignment.StDate = _currentDate;
            }
            var adminRole = _context.Roles.FirstOrDefault(r => r.Name == RoleNames.AdminLevel).Id;
            var users = _context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(adminRole)).ToList();

            //send to student also:
            var studentRole = _context.Roles.FirstOrDefault(r => r.Name == RoleNames.StudentLevel).Id;
            var subUsers = _context.StudentTutorials.Where(m => m.TutorialId == model.TutorialId).Select(m => m.UserName).ToList();
            var subStudentUsers = _context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(studentRole) && subUsers.Contains(u.UserName));
            users.AddRange(subStudentUsers);

            var _currentUserName = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).FullName;
            var tutName = _context.Tutorials.FirstOrDefault(t => t.Id == model.TutorialId).Title;
            foreach (var admin in users)
            {
                var message = new Inbox
                {
                    Arrival = _currentDate,
                    AUserName = admin.UserName,
                    Message = getStringFromRes("AddLecMsg1", admin.LangCode) + "  " + _currentUserName +
                    getStringFromRes("AddAssMsg2", admin.LangCode) + "  " + model.Title + "  "
                    + getStringFromRes("AddLecMsg3", admin.LangCode) + "  " + tutName,
                    Sender = _currentUserName,
                    Link = Url.Action("GetAssigments", "Report", new { Id = model.TutorialId }, Request.Url.Scheme)
                };
                _context.Inbox.Add(message);

            }
            _context.SaveChanges();
            return RedirectToAction("GetAssigments", new { Id = model.TutorialId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool RemoveAtt(int Id)
        {
            var assAtt = _context.AssigmentAttachment.FirstOrDefault(m => m.Id == Id);
            if (assAtt == null) return false;
            try
            {
                var attPath = Path.Combine(Server.MapPath("~/Attachments/Assigments/") + assAtt.FileUrl);
                System.IO.File.Delete(attPath);
            }
            catch (Exception ex)
            {
                return false;
            }
            _context.AssigmentAttachment.Remove(assAtt);
            _context.SaveChanges();
            return true;
        }
        public ActionResult Solution(int Id)
        {
            var existSolVM = _context.AssSolutions.Include(m => m.Assignment).ProjectTo<AssSolutionVM>().FirstOrDefault(m => m.AssignmentId == Id && m.UserName == User.Identity.Name);
            if (existSolVM != null)
            {
                return View(existSolVM);
            }
            else
            {
                var Assigment = _context.Assigments.FirstOrDefault(m => m.Id == Id && m.EndTime >= _currentDate);
                if (Assigment == null) return HttpNotFound();
                var issub = GeneralHelper.IsSubscribe(Assigment.TutorialId, User.Identity.Name);
                if (!issub)
                {
                    return RedirectToAction("MyTutorials", "Student");
                }
                var solVM = new AssSolutionVM
                {
                    Assignment = Assigment,
                    AssignmentId = Assigment.Id
                };
            return View(solVM);
            }

        }
        public ActionResult SaveSolution (AssSolutionVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Assignment = _context.Assigments.FirstOrDefault(m => m.Id == model.AssignmentId);
                return View("Solution", model);
            }
            if (model.Id == 0)
            {
                var solAttachments = model.AFiles;
                if (solAttachments != null && solAttachments.Count() > 0)
                {
                    var toBeAddedAtts = new List<SolutionAttachment>();
                    foreach (var solFile in solAttachments)
                    {
                        if (solFile != null)
                        {
                            var attExt = solFile.FileName.Substring(solFile.FileName.LastIndexOf(".")).Replace("\"", "");

                            var solatt = "solatt_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + attExt;
                            var solatt_ServerSavePath = Path.Combine(Server.MapPath("~/Attachments/Solutions/") + solatt);

                            solFile.SaveAs(solatt_ServerSavePath);
                            toBeAddedAtts.Add(new SolutionAttachment { FileUrl = solatt });
                        }
                    }
                    model.SolutionAttachments = toBeAddedAtts;
                }
                var assSol = Mapper.Map<AssSolution>(model);
                assSol.UserName = User.Identity.Name;
                _context.AssSolutions.Add(assSol);
            }
            else
            {
                var dbSol = _context.AssSolutions.FirstOrDefault(m => m.Id == model.Id && m.UserName==User.Identity.Name);
                if (dbSol == null) return HttpNotFound();
                dbSol.Descr = model.Descr;

                var solAttachments = model.AFiles;
                if (solAttachments != null && solAttachments.Count() > 0)
                {
                    var toBeAddedAtts = new List<SolutionAttachment>();
                    foreach (var solFile in solAttachments)
                    {
                        if (solFile != null)
                        {
                            var attExt = solFile.FileName.Substring(solFile.FileName.LastIndexOf(".")).Replace("\"", "");

                            var solatt = "solatt_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + attExt;
                            var solatt_ServerSavePath = Path.Combine(Server.MapPath("~/Attachments/Solutions/") + solatt);

                            solFile.SaveAs(solatt_ServerSavePath);
                            toBeAddedAtts.Add(new SolutionAttachment {SolutionId=dbSol.Id,
                                FileUrl = solatt });
                        }
                    }
                    _context.SolutionAttachments.AddRange(toBeAddedAtts);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("GetStAssignments","Report");
        }
        private string getStringFromRes(string keyVal, string LangKey)
        {
            var strVal = QuranRes.ResourceManager.GetString(keyVal, CultureInfo.GetCultureInfo(LangKey));
            strVal = strVal == null ? QuranRes.ResourceManager.GetString(keyVal, CultureInfo.DefaultThreadCurrentCulture) : strVal;
            return strVal;
        }
    }

}