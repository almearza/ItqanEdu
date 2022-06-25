using QuranEducation.Models;
using QuranEducation.Models.VM;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.Net;

using QuranEducation.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace QuranEducation.Controllers
{
    public partial class ReportController : MyController
    {
        string langCode = "En";
        private DateTime _currentDate = DateTime.Now;
        ApplicationDbContext _context;
        public ReportController()
        {
            _context = new ApplicationDbContext();

        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [Authorize(Roles = RoleNames.AdminLevel)]
        public ActionResult GetWeeklyTimeTable()
        {
            var startDate = DateTime.Now.StartOfWeek(DayOfWeek.Saturday);
            var endDate = startDate.AddDays(6);

            var lecsBetweenTwoDates = _context.Lectures
                .Where(m => m.LecStrartTime >= startDate && m.LecEndTime <= endDate)
                .Include(m => m.Tutorial)
                .OrderBy(m => m.LecStrartTime)
                .ToList();

            return View(lecsBetweenTwoDates);
        }
        [Authorize(Roles = RoleNames.InstructorLevel)]
        //[Authorize(Roles = RoleNames.AdminLevel)]

        public ActionResult GetTutorials()
        {
            var tutorials = _context.Tutorials.Where(m => m.InstUName == User.Identity.Name &&
            DbFunctions.TruncateTime(m.CloseDate) >= DbFunctions.TruncateTime(_currentDate) && m.Active).OrderByDescending(m => m.Id).ToList();
            return View(tutorials);
        }
        //[Authorize(Roles = RoleNames.StudentLevel)]
        [AllowAnonymous]
        public ActionResult GetAllTutorials()
        {
            HttpCookie langCookie = Request.Cookies["culture"];

            if (langCookie != null)
            {
                langCode = langCookie.Value;
            }
            var tutorials = _context.Tutorials
                .Where(m => DbFunctions.TruncateTime(m.CloseDate) >= DbFunctions.TruncateTime(_currentDate) && m.Active && m.LangCode == langCode)
                .OrderByDescending(m => m.Id).ToList();
            return View(tutorials);
        }

        [Authorize(Roles = RoleNames.StudentLevel)]
        public ActionResult GetStTutorial(int Id)
        {
            var TutorialVM = _context.StudentTutorials.Include(m => m.Tutorial)
                .Where(m => m.UserName == User.Identity.Name && m.TutorialId == Id && m.ManagemetAccept)
                .Select(m => m.Tutorial)
                .ProjectTo<TutorialVM>().FirstOrDefault();
            if (TutorialVM == null) return HttpNotFound();

            return View(TutorialVM);
        }
        //[Authorize(Roles = RoleNames.InstructorLevel + "," + RoleNames.StudentLevel)]
        [AllowAnonymous]
        public ActionResult GetTutorial(int Id)
        {
            var TutorialVM = _context.Tutorials.ProjectTo<TutorialVM>().FirstOrDefault(m => m.Id == Id && m.Active);
            if (TutorialVM == null) return HttpNotFound();

            return View(TutorialVM);
        }
        [Authorize(Roles = RoleNames.InstructorLevel)]
        public ActionResult GetWeeklyTimeTableInst()
        {
            var tutsIds = _context.Tutorials.Where(t => t.InstUName == User.Identity.Name && t.Active).Select(t => t.Id).ToList();

            var startDate = DateTime.Now.StartOfWeek(DayOfWeek.Saturday);
            var endDate = startDate.AddDays(6);

            var lecsBetweenTwoDates = _context.Lectures
                .Where(m => (m.LecStrartTime >= startDate && m.LecEndTime <= endDate) && tutsIds.Contains(m.TutorialId))
                .Include(m => m.Tutorial)
                .OrderBy(m => m.LecStrartTime)
                .ToList();

            return View(lecsBetweenTwoDates);
        }
        [Authorize(Roles = RoleNames.StudentLevel)]
        public ActionResult GetWeeklyTimeTableSt()
        {
            //get subscribed tuts
            var tutsIds = _context.StudentTutorials
                .Where(t => t.Tutorial.Active && t.UserName==User.Identity.Name && t.ManagemetAccept)
                .Select(t => t.TutorialId).ToList();

            var startDate = DateTime.Now.StartOfWeek(DayOfWeek.Saturday);
            var endDate = startDate.AddDays(6);

            var lecsBetweenTwoDates = _context.Lectures
                .Where(m => (m.LecStrartTime >= startDate && m.LecEndTime <= endDate) && tutsIds.Contains(m.TutorialId))
                .Include(m => m.Tutorial)
                .OrderBy(m => m.LecStrartTime)
                .ToList();

            return View(lecsBetweenTwoDates);
        }
        [Authorize(Roles = RoleNames.AdminLevel)]
        public ActionResult GetInsts()
        {
            var instRoleId = _context.Roles.FirstOrDefault(m => m.Name == RoleNames.InstructorLevel).Id;
            var users = _context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(instRoleId)).ToList();
            return View(users);
        }
        [Authorize(Roles = RoleNames.AdminLevel)]
        public ActionResult GetInstsEval()
        {
            var InstEval = _context.InstEval.OrderBy(m => m.InstUserName).ToList();
            var convertedEvals = new List<InstEvalReportVM>();
            if (InstEval != null)
            {
                foreach (var eval in InstEval)
                {
                    convertedEvals.Add(new InstEvalReportVM
                    {
                        InstName = _context.Users.Where(m => m.UserName == eval.InstUserName).Select(m => m.FullName).FirstOrDefault(),
                        TutName = _context.Tutorials.Where(m => m.Id == eval.TutorialId).Select(m => m.Title).FirstOrDefault(),
                        EvalName = _context.Users.Where(m => m.UserName == eval.EvalUserName).Select(m => m.FullName).FirstOrDefault(),
                        ArriveInATime = eval.ArriveInATime ? QuranRes.yes : QuranRes.no,
                        GoodCommunications = eval.GoodCommunications ? QuranRes.yes : QuranRes.no,
                        GoodInStudy = eval.GoodInStudy ? QuranRes.yes : QuranRes.no,
                        GoodInUsingBoard = eval.GoodInUsingBoard ? QuranRes.yes : QuranRes.no,
                        GoodInVoice = eval.GoodInVoice ? QuranRes.yes : QuranRes.no,
                        TechProblemInTutorial = eval.TechProblemInTutorial ? QuranRes.yes : QuranRes.no
                    });
                }
            }
            return View(convertedEvals);
        }
        [Authorize(Roles = RoleNames.AdminLevel+","+RoleNames.InstructorLevel)]
        public ActionResult GetStudents(int? tutId)
        {
            var studentRoleId = _context.Roles.FirstOrDefault(m => m.Name == RoleNames.StudentLevel).Id;
            if (User.IsInRole(RoleNames.InstructorLevel) && tutId.HasValue)
            {
                var subStudents = _context.StudentTutorials.Where(m => m.TutorialId==tutId).Select(m => m.UserName).ToList();
                var users = _context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(studentRoleId) && subStudents.Contains(u.UserName)).ToList();
                ViewBag.tutId = tutId;
                return View(users);
            }
            else
            {
                var users = _context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(studentRoleId)).ToList();
                return View(users);
            }
        }

        [Authorize(Roles = RoleNames.InstructorLevel)]
        public ActionResult GetStudentAssignments(string Id,string StName)
        {
            var instTuts = _context.Tutorials.Where(m => m.InstUName == User.Identity.Name).Select(m => m.Id).ToList();
            var assignmentsSol = _context.AssSolutions.Include(m => m.Assignment)
                .Include(m => m.SolutionAttachments).Where(m => m.UserName == Id && instTuts.Contains(m.Assignment.TutorialId)).ToList();
            ViewBag.StName = StName;
            return View(assignmentsSol);
        }
        [Authorize(Roles = RoleNames.AdminLevel + "," + RoleNames.StudentLevel)]
        public ActionResult GetLecture(int Id)
        {
            HttpCookie langCookie = Request.Cookies["culture"];

            if (langCookie != null)
            {
                langCode = langCookie.Value;
            }
            LanguageMang.SetLanguage("", "En");

            var Lecture = _context.Lectures.Include(m => m.Tutorial).FirstOrDefault(m => m.Id == Id);
            if (Lecture == null) return HttpNotFound();
            if (User.IsInRole(RoleNames.StudentLevel))
            {
                var issub = GeneralHelper.IsSubscribe(Lecture.TutorialId, User.Identity.Name);
                if (!issub)
                {
                    return RedirectToAction("GetAllTutorials");
                }
            }
            var LectureVM = Mapper.Map<LectureVM>(Lecture);
            var lecDateTime = Lecture.LecStrartTime;
            LectureVM.LecDate = lecDateTime.ToString("MM/dd/yyyy");
            LectureVM.LecStartTimeStr = string.Format("{0:hh:mm tt}", lecDateTime);
            LectureVM.LecEndTimeStr = string.Format("{0:hh:mm tt}", Lecture.LecEndTime);

            LanguageMang.SetLanguage("", langCode);
            return View(LectureVM);
        }
        [Authorize(Roles = RoleNames.AdminLevel + "," + RoleNames.StudentLevel)]
        public ActionResult GetAssignment(int Id)
        {
            HttpCookie langCookie = Request.Cookies["culture"];

            if (langCookie != null)
            {
                langCode = langCookie.Value;
            }
            LanguageMang.SetLanguage("", "En");

            var Assignment = _context.Assigments.Include(m => m.Tutorial).Include(m => m.AssigmentAttachments).ProjectTo<AssigmentVM>().FirstOrDefault(m => m.Id == Id);
            if (Assignment == null) return HttpNotFound();
            if (User.IsInRole(RoleNames.StudentLevel))
            {
                var issub = GeneralHelper.IsSubscribe(Assignment.TutorialId, User.Identity.Name);
                if (!issub)
                {
                    return RedirectToAction("GetAllTutorials");
                }
            }
            LanguageMang.SetLanguage("", langCode);
            return View(Assignment);
        }
        [Authorize(Roles = RoleNames.AdminLevel + "," + RoleNames.StudentLevel)]
        public ActionResult GetLectures(int Id)
        {
            var tut = _context.Tutorials.FirstOrDefault(m => m.Id == Id);
            if (User.IsInRole(RoleNames.StudentLevel))
            {
                var issub = GeneralHelper.IsSubscribe(Id, User.Identity.Name);
                if (!issub)
                {
                    return RedirectToAction("GetAllTutorials");
                }
            }
            if (tut == null) return HttpNotFound();
            var lecs = _context.Lectures.Where(m => m.TutorialId == tut.Id)
                .ToList();
            ViewBag.TutId = tut.Id;
            ViewBag.TutTitle = tut.Title;
            return View(lecs);
        }
        [Authorize(Roles = RoleNames.AdminLevel + "," + RoleNames.StudentLevel)]
        public ActionResult GetAssignments(int Id)
        {
            if (User.IsInRole(RoleNames.StudentLevel))
            {
                var issub = GeneralHelper.IsSubscribe(Id, User.Identity.Name);
                if (!issub)
                {
                    return RedirectToAction("GetAllTutorials");
                }
            }
            var tut = _context.Tutorials.FirstOrDefault(m => m.Id == Id);
            if (tut == null) return HttpNotFound();
            var ass = _context.Assigments.Where(m => m.TutorialId == tut.Id && m.EndTime>=_currentDate)
                .ToList();
            ViewBag.TutId = tut.Id;
            ViewBag.TutTitle = tut.Title;
            return View(ass);
        }
        [Authorize(Roles = RoleNames.StudentLevel)]
        public ActionResult GetStAssignments()
        {
            var tut = _context.StudentTutorials.Where(m => m.UserName==User.Identity.Name).Select(m=>m.TutorialId).ToList();
            var ass = _context.Assigments.Where(m => tut.Contains(m.TutorialId) && m.EndTime >= _currentDate)
                .ToList();
            return View("GetAssignments", ass);
        }
        [Authorize(Roles =RoleNames.AdminLevel)]
        public ActionResult Contactus()
        {
            var contactus = _context.ContactUs.ToList();
            return View(contactus);
        }
        // GET: Card_Feture/Delete/5
        public ActionResult Delete(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContactUs contact = _context.ContactUs.Find(Id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int Id)
        {
            ContactUs contact = _context.ContactUs.Find(Id);
            _context.ContactUs.Remove(contact);
            _context.SaveChanges();
            return RedirectToAction("Contactus");
        }

    }
}