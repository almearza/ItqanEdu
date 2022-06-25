using QuranEducation.Models;
using QuranEducation.Models.VM;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web.Security;
using QuranEducation.Helpers;

namespace QuranEducation.Controllers
{

    public class StudentController : MyController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;
        private RoleManager<IdentityRole> roleManager;
        public StudentController()
        {
            _context = new ApplicationDbContext();
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
        }
        public StudentController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
                if (roleManager != null)
                {
                    roleManager.Dispose();
                    roleManager = null;
                }
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }

            base.Dispose(disposing);
        }
        [Authorize(Roles = RoleNames.StudentLevel + "," + RoleNames.AdminLevel)]
        public ActionResult Index(string Id = "")
        {
            var Profile = new StudentProfile();
            if (Id != null && User.IsInRole(RoleNames.AdminLevel))
            {
                Profile = _context.StudentProfiles.Include(m => m.AppUser).FirstOrDefault(m => m.AppUser.UserName == Id);
            }
            else
            {
                Profile = _context.StudentProfiles.Include(m => m.AppUser).FirstOrDefault(m => m.AppUser.UserName == User.Identity.Name);
            }
            if (Profile == null)
                return RedirectToAction("Login", "Account");
            return View(Profile);
        }
        [AllowAnonymous]
        public ActionResult HandleAccount()
        {
            var _student = new StudentVM();
            if (User.Identity.IsAuthenticated)
            {
                _student = _context.StudentProfiles.Include(m => m.AppUser).Where(m => m.AppUser.UserName == User.Identity.Name)
                    .Select(m => new StudentVM
                    {
                        UserId = m.AppUser.Id,
                        Email = m.AppUser.Email,
                        PhoneNumber = m.AppUser.PhoneNumber,
                        Name = m.AppUser.FullName,
                        ImgUrl = m.ImgUrl,
                        LangCode = m.AppUser.LangCode,
                        BOD = m.BOD,
                        CertUrl = m.CertUrl,
                        EduYear = m.EduYear,
                        EnName = m.EnName,
                        HomePhoneNumber = m.HomePhoneNumber,
                        Nationality = m.Nationality,
                        ResidentCountryCode = m.ResidentCountryCode,
                        StScore = m.StScore,
                        Gender=m.Gender
                    })
                    .FirstOrDefault();
                return View(_student);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> SaveAccountAsync(StudentVM model)
        {
            var _currentDate = DateTime.Now.Date;
            if (!ModelState.IsValid)
            {
                return View("HandleAccount", model);
            }

            var _student = new StudentProfile();
            if (!string.IsNullOrEmpty(model.UserId))
            {
                //edit user of student
                _student = _context.StudentProfiles.Include(m => m.AppUser).Where(m => m.AppUser.UserName == User.Identity.Name).FirstOrDefault();
                _student.AppUser.Email = model.Email;
                _student.AppUser.UserName = model.Email;
                _student.AppUser.LangCode = model.LangCode;
                _student.AppUser.FullName = model.Name;
                _student.AppUser.PhoneNumber = model.PhoneNumber;
                //edit student
                _student.BOD = model.BOD;
                _student.EduYear = model.EduYear;
                _student.EnName = model.EnName;
                _student.HomePhoneNumber = model.HomePhoneNumber;
                _student.Nationality = model.Nationality;
                _student.ResidentCountryCode = model.ResidentCountryCode;
                _student.StScore = model.StScore;
                _student.Gender = model.Gender;
                //edit files if sent
                var StudentProfileImg = model.ImgFile;
                if (StudentProfileImg != null && StudentProfileImg.ContentLength > 0)
                {
                    var imgExt = StudentProfileImg.FileName.Substring(StudentProfileImg.FileName.LastIndexOf(".")).Replace("\"", "");

                    var StudentProfileImgFullPath = "StudentImg_" + _student.AppUser.Id + imgExt;
                    var StudentProfileImg_ServerSavePath = Path.Combine(Server.MapPath("~/images/StudentsProfiles/") + StudentProfileImgFullPath);

                    //Save file to server folder  
                    StudentProfileImg.SaveAs(StudentProfileImg_ServerSavePath);
                    _student.ImgUrl = StudentProfileImgFullPath;

                }
                var StudentCertFile = model.CertFile;
                if (StudentCertFile != null && StudentCertFile.ContentLength > 0)
                {
                    var certExt = StudentCertFile.FileName.Substring(StudentCertFile.FileName.LastIndexOf(".")).Replace("\"", "");

                    var StudentCertFileFullPath = "StudentCert_" + _student.AppUser.Id + certExt;
                    var StudentCertFile_ServerSavePath = Path.Combine(Server.MapPath("~/images/StudentCerts/") + StudentCertFileFullPath);

                    //Save file to server folder  
                    StudentCertFile.SaveAs(StudentCertFile_ServerSavePath);
                    _student.CertUrl = StudentCertFileFullPath;
                }
                try
                {
                    _context.SaveChanges();
                    //TempData["message"] = "تم قبولك للانضمام للتواصل مالاشتراك باحد الدورات اذهب " + GeneralHelper.GetInstFullName(_student.AppUser.FullName);

                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", QuranRes.EmailAlreadyTaken);
                    return View("HandleAccount", model);
                }
            }
            else
            {
                //add new user account

                var StudentProfileImg = model.ImgFile;

                //if (StudentProfileImg == null || StudentProfileImg.ContentLength <= 0)
                //{
                //    ModelState.AddModelError("ImgFile", QuranRes.InstImgError);
                //    return View("HandleAccount", model);
                //}

                var StudentCertFile = model.CertFile;
                if (StudentCertFile == null || StudentCertFile.ContentLength <= 0)
                {
                    ModelState.AddModelError("CertFile", QuranRes.StCertRequired);
                    return View("HandleAccount", model);
                }
                var user = new ApplicationUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    FullName = model.Name,
                    LangCode = model.LangCode
                };
                var _password = model.PhoneNumber != null ? "0" + model.PhoneNumber.Substring(model.PhoneNumber.Length - 9, 9) : "P@$$w0rd";
                var result = await UserManager.CreateAsync(user, _password);
                if (result.Succeeded)
                {
                    if (StudentProfileImg != null && StudentProfileImg.ContentLength > 0)
                    {
                        var imgExt = StudentProfileImg.FileName.Substring(StudentProfileImg.FileName.LastIndexOf(".")).Replace("\"", "");

                    var StudentProfileImgFullPath = "StudentImg_" + user.Id + imgExt;
                    var StudentProfileImg_ServerSavePath = Path.Combine(Server.MapPath("~/images/StudentsProfiles/") + StudentProfileImgFullPath);

                    //Save file to server folder  
                    StudentProfileImg.SaveAs(StudentProfileImg_ServerSavePath);
                    _student.ImgUrl = StudentProfileImgFullPath;
                    }

                    var certExt = StudentCertFile.FileName.Substring(StudentCertFile.FileName.LastIndexOf(".")).Replace("\"", "");

                    var StudentCertFileFullPath = "StudentCert_" + user.Id + certExt;
                    var StudentCertFile_ServerSavePath = Path.Combine(Server.MapPath("~/images/StudentCerts/") + StudentCertFileFullPath);

                    //Save file to server folder  
                    StudentCertFile.SaveAs(StudentCertFile_ServerSavePath);
                    _student.CertUrl = StudentCertFileFullPath;


                    _student.AppUserId = user.Id;
                    _student.BOD = model.BOD;
                    _student.EduYear = model.EduYear;
                    _student.EnName = model.EnName;
                    _student.HomePhoneNumber = model.HomePhoneNumber;
                    _student.Nationality = model.Nationality;
                    _student.ResidentCountryCode = model.ResidentCountryCode;
                    _student.StScore = model.StScore;
                    _student.Gender = model.Gender;
                    _context.StudentProfiles.Add(_student);


                    //add user to the role
                    if (!await roleManager.RoleExistsAsync(RoleNames.StudentLevel))
                        await roleManager.CreateAsync(new IdentityRole(RoleNames.StudentLevel));
                    await UserManager.AddToRolesAsync(user.Id, RoleNames.StudentLevel);
                    _context.SaveChanges();
                    //TempData["message"] = "تم قبولك للانضمام للتواصل مالاشتراك باحد الدورات اذهب " + GeneralHelper.GetInstFullName();
                    //send sms:
                    var usernameText = QuranRes.UserNameMsg;
                    var passText = QuranRes.PassMsg;
                    var fullSms = usernameText + "  " + model.Email + "\n " + passText + "  " + _password + "\n " + QuranRes.ChangePassLater ;
                    //SMSHelper.Send(fullSms, model.PhoneNumber);
                    EmailSender sender = new EmailSender();
                    sender.SendMail(model.Email, fullSms);

                    await SignInManager.SignInAsync(user, isPersistent: true, rememberBrowser: true);

                    return RedirectToAction("Index");

                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                    return View("HandleAccount", model);
                }
            }
            _context.SaveChanges();
            return RedirectToAction("Index");

        }

        [Authorize(Roles = RoleNames.StudentLevel)]
        public ActionResult MyTutorials()
        {
            var MyTutorials = _context.StudentTutorials.Include(m => m.Tutorial)
                .Where(m => m.UserName == User.Identity.Name && m.ManagemetAccept)
                .Select(m => m.Tutorial)
                .ToList();
            return View(MyTutorials);
        }
        [Authorize(Roles = RoleNames.StudentLevel)]
        public ActionResult Subscribe(int Id)
        {

            var tut = _context.Tutorials.FirstOrDefault(t => t.Id == Id);
            if (tut == null) return HttpNotFound();

            if (GeneralHelper.IsRequested(Id, User.Identity.Name))
            {
                return RedirectToAction("MyTutorials");
            }
            var sub = new StudentTutorial
            {
                RequestDate = DateTime.Now,
                TutorialId = Id,
                UserName = User.Identity.Name
            };
            //send noti to admins
            var adminRole = _context.Roles.FirstOrDefault(r => r.Name == RoleNames.AdminLevel).Id;
            var AdminUsers = _context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(adminRole));
            var _currentUserName = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).FullName;
            var tutName = tut.Title;
            foreach (var admin in AdminUsers)
            {
                var message = new Inbox
                {
                    Arrival = DateTime.Now,
                    AUserName = admin.UserName,
                    Message = " قام الطالب " + _currentUserName + " بإرسال طلب إشتراك في الدورة " + tutName,
                    Sender = _currentUserName,
                    Link = Url.Action("StudentRequest", "Tutorial", null, Request.Url.Scheme)
                };
                _context.Inbox.Add(message);

            }
            _context.StudentTutorials.Add(sub);
            _context.SaveChanges();

            return RedirectToAction("MyTutorials");
        }
        [Authorize(Roles = RoleNames.StudentLevel + "," + RoleNames.AdminLevel)]
        public ActionResult EvalInst()
        {
            return View();
        }
        [Authorize(Roles = RoleNames.AdminLevel)]
        public ActionResult InstGrantedEval(string Id)
        {
            var model = _context.InstEval.Where(m => m.InstUserName == Id).ToList();
            return View(model);

        }

        [Authorize(Roles = RoleNames.StudentLevel + "," + RoleNames.AdminLevel)]
        [HttpPost]
        public ActionResult EvalInst(InstEval model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var ExistEval = _context.InstEval.FirstOrDefault(m => m.EvalUserName == User.Identity.Name 
            && m.InstUserName == model.InstUserName && m.TutorialId == model.TutorialId);
            if (ExistEval != null)
            {
                ModelState.AddModelError("", QuranRes.Alreadyeval);
                return View(model);
            }
            var instHasThisTut = _context.Tutorials.Where(m => m.Id == model.TutorialId && m.InstUName == model.InstUserName).FirstOrDefault() != null;
            if (!instHasThisTut)
            {
                ModelState.AddModelError("", QuranRes.instHasNotThisTut);
                return View(model);
            }
            model.EvalUserName = User.Identity.Name;
            _context.InstEval.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Done","Home");
        }
        [Authorize(Roles = RoleNames.InstructorLevel)]
        [HttpGet]
        public ActionResult Certify(string UserName, int tutId)
        {
            var stTut = _context.StudentTutorials.FirstOrDefault(m => m.UserName == UserName && m.TutorialId==tutId);
            if (stTut == null)
                return HttpNotFound();
            var FullName = _context.Users.Where(m=>m.UserName==UserName ).Select(m=>m.FullName).FirstOrDefault();
            return View(new EvaluateVM { TutId = tutId, UserName = UserName, FullName = FullName });
        }
        [Authorize(Roles = RoleNames.InstructorLevel)]
        [HttpPost]
        public ActionResult Certify(EvaluateVM model)
        {
            if(!ModelState.IsValid|| model.Degree == CertifiedDegree.NotCertified)
            {
                ModelState.AddModelError("", QuranRes.CertifyError);
                return View(model);
            }
            var tutst = _context.StudentTutorials.Where(m => m.TutorialId == model.TutId && m.UserName == model.UserName).FirstOrDefault();
            if (tutst != null)
            {
                tutst.CertifiedDegree = model.Degree;
                tutst.Certified = true;
                tutst.CertifiedDate = DateTime.Now;
                _context.SaveChanges();
            }
            return RedirectToAction("GetStudents", "Report", new { tutId = model.TutId });
            
        }
        [Authorize(Roles = RoleNames.StudentLevel)]
        public ActionResult MyEvaluate()
        {
            var StTuts = _context.StudentTutorials.Include(m => m.Tutorial).Where(m => m.UserName == User.Identity.Name && m.CertifiedDegree!=CertifiedDegree.NotCertified).ToList();
            return View(StTuts);
        }
        }
}