using QuranEducation.Models;
using QuranEducation.Models.VM;
using System.Web.Mvc;
using System.Data.Entity;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using AutoMapper;

namespace QuranEducation.Controllers
{
    [Authorize(Roles = RoleNames.InstructorLevel)]
    public class InstructorProfileController : MyController
    {


        private ApplicationDbContext _context;
        public InstructorProfileController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: InstructorProfile
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        [AllowAnonymous]
        public ActionResult Index(string UserName = "")
        {
            if (UserName != "")
            {
                var InstProfile = _context.InstructorProfiles
                               .Where(u => u.UserName == UserName).ProjectTo<InstructorProfileVm>().FirstOrDefault();
                if (InstProfile == null)
                    return HttpNotFound();
                InstProfile.AppUser = _context.Users.FirstOrDefault(u => u.UserName == UserName);
                return View(InstProfile);
            }
            var Profile = _context.InstructorProfiles
                .Where(u => u.UserName == User.Identity.Name).ProjectTo<InstructorProfileVm>().FirstOrDefault();
            if (Profile == null)
                return RedirectToAction("HandlProfile");
            Profile.AppUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            return View(Profile);
        }
        public ActionResult HandlProfile()
        {
            var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            var InstProfileVM = _context.InstructorProfiles.Where(u =>u.UserName == User.Identity.Name)
                .ProjectTo<InstructorProfileVm>().FirstOrDefault();
            if (InstProfileVM != null)
            {
                InstProfileVM.AppUser = user;
                return View(InstProfileVM);
            }
            return View(new InstructorProfileVm { AppUser=user});
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveInstProfile(InstructorProfileVm model)
        {
            var _currentDate = DateTime.Now.Date;
            if (!ModelState.IsValid)
            {
                return View("HandlProfile", model);
            }
            if (model.AppUser == null || string.IsNullOrEmpty(model.AppUser.FullName) || string.IsNullOrEmpty(model.AppUser.Email)
                    || string.IsNullOrEmpty(model.AppUser.PhoneNumber))
            {
                ModelState.AddModelError("", QuranRes.OtherInstDataError);
                return View("HandlProfile", model);
            }
            var Auser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            model.UserName = Auser.UserName;
            Auser.Email = model.AppUser.Email;
            Auser.LangCode = model.AppUser.LangCode;
            Auser.FullName = model.AppUser.FullName;
            Auser.PhoneNumber = model.AppUser.PhoneNumber;
            if (model.Id == 0)
            {
                //save image
                var InstProfileImg = model.Image;
                if (InstProfileImg == null || InstProfileImg.ContentLength <= 0)
                {
                    ModelState.AddModelError("Image", QuranRes.InstImgError);
                    return View("HandlProfile", model);
                }
                var imgExt = InstProfileImg.FileName.Substring(InstProfileImg.FileName.LastIndexOf(".")).Replace("\"", "");

                var instProfileImg = "InstImg_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + imgExt;
                var instProfileImg_ServerSavePath = Path.Combine(Server.MapPath("~/images/InstructorProfile/") + instProfileImg);

                //Save file to server folder  
                InstProfileImg.SaveAs(instProfileImg_ServerSavePath);
                model.ImageUrl = instProfileImg;
                var toBeSaveModel = Mapper.Map<InstructorProfile>(model);
                _context.InstructorProfiles.Add(toBeSaveModel);
            }
            else
            {
                var DbInstructor = _context.InstructorProfiles.FirstOrDefault(m => m.Id == model.Id);
                if (DbInstructor == null) return HttpNotFound();
                //save image
                var InstProfileImg = model.Image;
                if (InstProfileImg != null && InstProfileImg.ContentLength > 0)
                {

                    var imgExt = InstProfileImg.FileName.Substring(InstProfileImg.FileName.LastIndexOf(".")).Replace("\"", "");

                    var instProfileImg = "InstImg_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + imgExt;
                    var instProfileImg_ServerSavePath = Path.Combine(Server.MapPath("~/images/InstructorProfile/") + instProfileImg);

                    //Save file to server folder  
                    InstProfileImg.SaveAs(instProfileImg_ServerSavePath);
                    model.ImageUrl = instProfileImg;
                }

                DbInstructor.Title = model.Title;
                DbInstructor.Desc = model.Desc;
            }


            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}