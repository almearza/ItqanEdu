using AutoMapper;
using QuranEducation.Helpers;
using QuranEducation.Models;
using QuranEducation.Models.VM;
using System;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuranEducation.Controllers
{
    [Authorize(Roles = RoleNames.InstructorLevel)]
    public class LectureController : MyController
    {
        private ApplicationDbContext _context;
        DateTime _currentDate = DateTime.Now;
        public LectureController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: InstructorProfile
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }


        public ActionResult GetLectures(int Id)
        {
            var tut = _context.Tutorials.FirstOrDefault(m => m.Id == Id && m.InstUName == User.Identity.Name);
            if (tut == null) return HttpNotFound();
            var lecs = _context.Lectures.Where(m => m.TutorialId == tut.Id && m.Tutorial.InstUName == User.Identity.Name)
                .ToList();
            ViewBag.TutId = tut.Id;
            ViewBag.TutTitle = tut.Title;
            return View(lecs);
        }

        public ActionResult CreateLecture(int Id)
        {
            var tut = _context.Tutorials.FirstOrDefault(m => m.Id == Id && m.InstUName == User.Identity.Name && m.Active);
            if (tut == null) return HttpNotFound();
            var dateTime = string.Format("{0:hh:mm tt}", _currentDate);
            return View("HandlLecture", new LectureVM { Tutorial = tut, Id = 0, TutorialId = Id, LecStartTimeStr = dateTime, LecEndTimeStr = dateTime });
        }
        public ActionResult HandlLecture(int Id = 0)
        {
            HttpCookie langCookie = Request.Cookies["culture"];
            var langCode = "En";
            if (langCookie != null)
            {
                langCode = langCookie.Value;
            }
            LanguageMang.SetLanguage("", "En");


            var Lecture = _context.Lectures.Include(m => m.Tutorial).FirstOrDefault(m => m.Id == Id && m.Tutorial.InstUName == User.Identity.Name);
            if (Lecture == null) return HttpNotFound();
            var LectureVM = Mapper.Map<LectureVM>(Lecture);
            //DateTime.ParseExact(lecDateTime.Date, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            var lecDateTime = Lecture.LecStrartTime;

            LectureVM.LecDate = lecDateTime.ToString("MM/dd/yyyy");
            LectureVM.LecStartTimeStr = string.Format("{0:hh:mm tt}", lecDateTime);
            LectureVM.LecEndTimeStr = string.Format("{0:hh:mm tt}", Lecture.LecEndTime);

            LanguageMang.SetLanguage("", langCode);

            return View(LectureVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult SaveLecture(LectureVM model)
        {
            var _currentDate = DateTime.Now.Date;
            if (!ModelState.IsValid)
            {
                return View("HandlLecture", model);
            }
            HttpCookie langCookie = Request.Cookies["culture"];
            var langCode = "En";
            if (langCookie != null)
            {
                langCode = langCookie.Value;
            }
            LanguageMang.SetLanguage("", "En");
            var lecStartTimeString = model.LecDate + " " + model.LecStartTimeStr;
            var lectureStartTime = DateTime.ParseExact(lecStartTimeString, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);

            var lecEndTimeString = model.LecDate + " " + model.LecEndTimeStr;
            var lectureEndTime = DateTime.ParseExact(lecEndTimeString, "MM/dd/yyyy hh:mm tt", CultureInfo.InvariantCulture);
            LanguageMang.SetLanguage("", langCode);
            var lecTut = _context.Tutorials.FirstOrDefault(t => t.Id == model.TutorialId && t.Active);
            if (lecTut == null)
            {
                ModelState.AddModelError("", QuranRes.LecTutNotFound);
                return View("HandlLecture", model);
            }
            var lecLang = lecTut.LangCode;
            //there is no lec start between these st time and end time of this comming lec
            var lecStartBetweenSETime = _context.Lectures.FirstOrDefault(l => l.LecStrartTime >= lectureStartTime && l.LecStrartTime <= lectureEndTime
              && l.Tutorial.LangCode == lecLang && l.Id != model.Id);
            if (lecStartBetweenSETime != null)
            {
                ModelState.AddModelError("", QuranRes.LecsInSameTimeError);
                return View("HandlLecture", model);
            }
            //there is no lec end within between st time and end time of this comming lec
            var lecEndBetweenSETime = _context.Lectures.FirstOrDefault(l => l.LecEndTime >= lectureStartTime && l.LecEndTime <= lectureEndTime
              && l.Tutorial.LangCode == lecLang && l.Id != model.Id);
            if (lecEndBetweenSETime != null)
            {
                ModelState.AddModelError("", QuranRes.LecsInSameTimeError);
                return View("HandlLecture", model);
            }
            if (lectureStartTime >= lectureEndTime)
            {
                ModelState.AddModelError("", QuranRes.LecEndBeggerThanLecStart);
                return View("HandlLecture", model);
            }
            if (model.Id == 0)
            {
                if (lectureStartTime < _currentDate)
                {
                    ModelState.AddModelError("", QuranRes.LecTimeError);
                    return View("HandlLecture", model);
                }
                model.StDate = DateTime.Now;
                //save atta of lec
                var LectureAtt = model.Attachment;
                if (LectureAtt != null && LectureAtt.ContentLength > 0)
                {
                    var attExt = LectureAtt.FileName.Substring(LectureAtt.FileName.LastIndexOf(".")).Replace("\"", "");

                    var tutatt = "Tutatt_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + attExt;
                    var tutatt_ServerSavePath = Path.Combine(Server.MapPath("~/Attachments/Lectures/") + tutatt);

                    LectureAtt.SaveAs(tutatt_ServerSavePath);
                    model.AttachmentUrl = tutatt;
                }
                var Lec = Mapper.Map<Lecture>(model);
                Lec.LecStrartTime = lectureStartTime;
                Lec.LecEndTime = lectureEndTime;
                _context.Lectures.Add(Lec);
                lecTut.CountOfLec += 1;
            }
            else
            {
                var DbLecture = _context.Lectures.FirstOrDefault(m => m.Id == model.Id);
                if (DbLecture == null) return HttpNotFound();
                //save image of tut
                var Lectureatt = model.Attachment;
                if (Lectureatt != null && Lectureatt.ContentLength > 0)
                {

                    var attExt = Lectureatt.FileName.Substring(Lectureatt.FileName.LastIndexOf(".")).Replace("\"", "");

                    var tutatt = "Tutatt_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + attExt;
                    var tutatt_ServerSavePath = Path.Combine(Server.MapPath("~/Attachments/Lectures/") + tutatt);

                    Lectureatt.SaveAs(tutatt_ServerSavePath);
                    DbLecture.AttachmentUrl = tutatt;
                }

                DbLecture.Title = model.Title;
                DbLecture.Descr = model.Descr;
                DbLecture.LecStrartTime = lectureStartTime;
                DbLecture.LecEndTime = lectureEndTime;
                DbLecture.TutorialId = model.TutorialId;
                DbLecture.VideoUrl = model.VideoUrl;
                DbLecture.VirtualRoomUrl = model.VirtualRoomUrl;
                DbLecture.StDate = DateTime.Now;
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
                    Message = getStringFromRes("AddLecMsg1", admin.LangCode) + "  " + _currentUserName + "  " +
                    getStringFromRes("AddLecMsg2", admin.LangCode) + "  " + model.Title + "  " + getStringFromRes("AddLecMsg3", admin.LangCode)
                    + "  " + tutName,
                    Sender = _currentUserName,
                    Link = Url.Action("GetLectures", "Report", new { Id = model.TutorialId }, Request.Url.Scheme)
                };
                _context.Inbox.Add(message);

            }

            _context.SaveChanges();
            return RedirectToAction("GetLectures", new { Id = model.TutorialId });
        }
        private string getStringFromRes(string keyVal, string LangKey)
        {
            var strVal = QuranRes.ResourceManager.GetString(keyVal, CultureInfo.GetCultureInfo(LangKey));
            strVal = strVal == null ? QuranRes.ResourceManager.GetString(keyVal, CultureInfo.DefaultThreadCurrentCulture) : strVal;
            return strVal;
        }

    }
}