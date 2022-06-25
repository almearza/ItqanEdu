using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using QuranEducation.Models;
using QuranEducation.Models.VM;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.DataProtection;
using QuranEducation.Helpers;
using System.Web.Security;
using System.Globalization;
using Facebook;
using System.IO;

namespace QuranEducation.Controllers
{
    [ExceptionLogger]
    public class AccountController : MyController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;
        private RoleManager<IdentityRole> roleManager;

        public AccountController()
        {
            _context = new ApplicationDbContext();
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        [Authorize(Roles = RoleNames.AdminLevel)]
        public ActionResult Index()
        {
            var adminRoleId = _context.Roles.FirstOrDefault(m => m.Name == RoleNames.AdminLevel).Id;
            var users = _context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(adminRoleId)).Select(u => new UserPlusRoleNameVM
            {
                User = u,
                RoleName = _context.Roles.FirstOrDefault(r => r.Id == u.Roles.FirstOrDefault().RoleId).Name,
            }).ToList();
            var vm = new UsersViewModel { Users = users };
            return View(vm);
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        //[CaptchaValidator(ErrorMessage = "الرجاء التحقق من العلامة", RequiredMessage = "الرجاء الضغط على علامة التحقق")]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //var capture = Session["__captcha"] as string;
            //if (capture != model.CaptchaText)
            //{
            //    ModelState.AddModelError("", "الرجاء إدخال رمز التحقق بصورة صحيحة");
            //    return View(model);
            //}
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: true);

            switch (result)
            {
                case SignInStatus.Success:
                    LanguageMang.SetLanguage(model.Email);
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", QuranRes.AccountWasLocked);
                    return View(model);
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", QuranRes.LoginFailedMsg);
                    return View(model);
            }
        }



        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            //var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            //var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            //bez i use sms only not email:
            var factorOptions = "Phone Code";
            //return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(factorOptions))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", QuranRes.AccountWasLocked);
                    return View(model);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", QuranRes.LoginFailedMsg);
                    return View(model);
            }
        }

        // GET: /Account/Register
        [Authorize(Roles = RoleNames.AdminLevel)]
        [HttpGet]
        //[AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterViewModel();
            return View(model);
        }
        [Authorize(Roles = RoleNames.AdminLevel)]
        [HttpGet]
        public ActionResult Edit(string Id)
        {
            var dbUser = UserManager.FindById(Id);
            var userRoleName = roleManager.FindById(dbUser.Roles.FirstOrDefault().RoleId).Name;
            var model = new RegisterViewModel()
            {
                UserId = dbUser.Id,
                Name = dbUser.FullName,
                Email = dbUser.Email,
                RoleName = userRoleName,
                LangCode = dbUser.LangCode,
                PhoneNumber = dbUser.PhoneNumber,
            };
            return View("Register", model);
        }
        [HttpGet]
        [Authorize(Roles = RoleNames.AdminLevel)]
        public ActionResult Eval(string Id)
        {
            var _instProfile = _context.InstructorProfiles.FirstOrDefault(m => m.UserName == Id);
            if (_instProfile == null) return HttpNotFound();
            return View(_instProfile);
        }
        [Authorize(Roles = RoleNames.AdminLevel)]
        //[AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                //check if new or edit
                if (String.IsNullOrEmpty(model.UserId) || String.IsNullOrWhiteSpace(model.UserId))
                {
                    //new user
                    var user = new ApplicationUser
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        //IdentityNo=model.IdentityNo,
                        EmailConfirmed = true,
                        FullName = model.Name,
                        PhoneNumberConfirmed = true,
                        PhoneNumber = model.PhoneNumber,
                        LangCode = model.LangCode
                        //TwoFactorEnabled = true,

                    };
                    
                    var _password = model.PhoneNumber!=null?"0"+model.PhoneNumber.Substring(model.PhoneNumber.Length- 9, 9):"P@$$w0rd";
                    var result = await UserManager.CreateAsync(user, _password);
                    if (result.Succeeded)
                    {
                        //add user to the role
                        if (!await roleManager.RoleExistsAsync(model.RoleName))
                            await roleManager.CreateAsync(new IdentityRole(model.RoleName));
                        await UserManager.AddToRolesAsync(user.Id, model.RoleName);
                        //automatic sign in:
                        //await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);

                        // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                        // Send an email with this link
                        // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                        // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                        // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");
                        //send sms to inst:
                        //if (model.RoleName == RoleNames.InstructorLevel)
                        //{
                        //send sms with email and pass
                        var usernameText = getStringFromRes("UserNameMsg", model.LangCode);
                        var passText = getStringFromRes("PassMsg", model.LangCode);
                        var fullSms = usernameText + "  " + model.Email + "\n " + passText + "  " + _password + "\n " + getStringFromRes("ChangePassLater", model.LangCode);
                        EmailSender sender = new EmailSender();
                        sender.SendMail(model.Email, fullSms);
                        //}
                        return RedirectToAction("Index", "Account");
                    }
                    AddErrors(result);
                }
                else
                {
                    //edit user
                    var dbUser = UserManager.FindById(model.UserId);
                    dbUser.FullName = model.Name;
                    dbUser.Email = model.Email;
                    //dbUser.IdentityNo = model.IdentityNo;
                    dbUser.PhoneNumberConfirmed = true;
                    dbUser.EmailConfirmed = true;
                    dbUser.PhoneNumber = model.PhoneNumber;
                    dbUser.LangCode = model.LangCode;
                    //dbUser.TwoFactorEnabled = true;
                    //1- remove old role
                    var dbUserRole = UserManager.GetRoles(dbUser.Id).FirstOrDefault();
                    if (dbUserRole != null)
                        await UserManager.RemoveFromRoleAsync(dbUser.Id, dbUserRole);
                    //2- add user to the new role
                    if (!await roleManager.RoleExistsAsync(model.RoleName))
                        await roleManager.CreateAsync(new IdentityRole(model.RoleName));
                    await UserManager.AddToRolesAsync(dbUser.Id, model.RoleName);

                    await UserManager.UpdateAsync(dbUser);
                    //send sms to inst:
                    if (model.RoleName == RoleNames.InstructorLevel)
                    {
                        //send sms with email and pass
                        var usernameText = getStringFromRes("UserNameMsg", model.LangCode);
                        //var passText = getStringFromRes("PassMsg", model.LangCode);
                        var fullSms = usernameText + "  " + model.Email;
                        EmailSender sender = new EmailSender();
                        sender.SendMail(model.Email, fullSms);
                    }
                    return RedirectToAction("Index", "Account");

                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult ForgetPass()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult ForgetPass(string Email)
        {
            var isExsit = _context.Users.FirstOrDefault(m => m.Email == Email);
            if (isExsit == null)
            {
                ModelState.AddModelError("", QuranRes.EnterCorrectPhone);
                return View();
            }
            else
            {
                var code = new Random().Next(0, 1000).ToString("0000");
                Session["Code"] = code;
                Session["UserName"] = isExsit.UserName;
                EmailSender sender = new EmailSender();
                sender.SendMail(Email, QuranRes.Code +":"+code);
                return View("VerifyCodePass");
            }
        }

        [AllowAnonymous]
        public ActionResult VerifyCodePass()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCodePass(string Code)
        {
            var _code = Session["Code"];
            if (!_code.Equals(Code))
            {
                ModelState.AddModelError("", QuranRes.codeNotValid);
                return View();
            }
            var _username = Session["UserName"] as string;
            var user = _context.Users.FirstOrDefault(m => m.UserName == _username);
            var _password = Membership.GeneratePassword(10, 1);
            string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await UserManager.ResetPasswordAsync(user.Id, code, _password);
            if (result.Succeeded)
            {
                var fullSms = QuranRes.UserNameMsg + "  " + user.Email + "\n " + QuranRes.PassMsg + "  " + _password + "\n " + QuranRes.ChangePassLater;
                //SMSHelper.Send(fullSms, user.PhoneNumber);
                EmailSender sender = new EmailSender();
                sender.SendMail(user.Email, fullSms);
                return View("PasswordChanged");
            }
            else
            {
                ModelState.AddModelError("", QuranRes.ErrorWhileResetPass);
                return View();
            }
            
        }
        public ActionResult PasswordChanged()
        {
            return View();
        }
        //change pass by any auth user
        [Authorize(Roles = RoleNames.AdminLevel + "," + RoleNames.InstructorLevel + "," + RoleNames.StudentLevel)]
        public ActionResult ChangePass()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.AdminLevel + "," + RoleNames.InstructorLevel + "," + RoleNames.StudentLevel)]
        public async Task<ActionResult> ChangePass(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var dbUser = await UserManager.FindAsync(User.Identity.Name, model.OldPassword);
            if (dbUser == null)
            {
                ModelState.AddModelError("", QuranRes.ChangePassFailed);
                return View(model);
            }
            var provider = new DpapiDataProtectionProvider("DefaultToken");
            UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));
            string code = await UserManager.GeneratePasswordResetTokenAsync(dbUser.Id);
            var result = await UserManager.ResetPasswordAsync(dbUser.Id, code, model.NewPassword);
            if (result.Succeeded)
            {
                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                return RedirectToAction("Login");
            }
            else
            {
                AddErrors(result);
                return View(model);
            }
        }
        //reset by admin to default
        // POST: /Account/ResetPassword
        [Authorize(Roles = RoleNames.AdminLevel)]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> ResetPassword(string Id)
        {

            var DbUser = await UserManager.FindByIdAsync(Id);
            if (DbUser == null)
            {
                // Don't reveal that the user does not exist
                Response.StatusCode = 404;
                return RedirectToAction("Index", "Account");
            }
            var provider = new DpapiDataProtectionProvider("DefaultToken");
            UserManager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(provider.Create("EmailConfirmation"));
            string code = await UserManager.GeneratePasswordResetTokenAsync(DbUser.Id);
            var _password = Membership.GeneratePassword(10, 1);
            var result = await UserManager.ResetPasswordAsync(DbUser.Id, code, _password);
            if (result.Succeeded)
            {
                Response.StatusCode = 200;
                return RedirectToAction("Index", "Account");
            }
            //AddErrors(result);
            return RedirectToAction("Index", "Account");
        }
        [Authorize(Roles = RoleNames.AdminLevel)]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult> Activate(string Id)
        {

            var DbUser = await UserManager.FindByIdAsync(Id);
            if (DbUser == null)
            {
                // Don't reveal that the user does not exist
                Response.StatusCode = 404;
                return RedirectToAction("Index", "Account");
            }
            //check status:enabled or disabled
            if (DbUser.LockoutEndDateUtc != null && DbUser.LockoutEndDateUtc > DateTime.Now)//locked
                DbUser.LockoutEndDateUtc = DateTime.Now.AddDays(-1);//unlock
            else
            {
                var newDate = DbUser.LockoutEndDateUtc = DateTime.Now.AddYears(200);
                DbUser.LockoutEndDateUtc = newDate;
            }

            var result = await UserManager.UpdateAsync(DbUser);
            if (result.Succeeded)
            {
                Response.StatusCode = 200;
                return RedirectToAction("Index", "Account");
            }
            //AddErrors(result);
            return RedirectToAction("Index", "Account");
        }


        // POST: /Account/LogOff
        [Authorize(Roles = RoleNames.AllLevels)]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        //[Authorize(Roles = RoleNames.AllLevels)]
        public JsonResult UserProfileJson()
        {
            if (!User.Identity.IsAuthenticated) return Json("", JsonRequestBehavior.AllowGet);
            var DbUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            //return Json("فواز فضل المولى المقدم", JsonRequestBehavior.AllowGet);
            return Json(DbUser.FullName, JsonRequestBehavior.AllowGet);
        }

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

        #region Helpers
        // Used for XSRF protection when adding external logins
        private string getStringFromRes(string keyVal, string LangKey)
        {
            var strVal = QuranRes.ResourceManager.GetString(keyVal, CultureInfo.GetCultureInfo(LangKey));
            strVal = strVal == null ? QuranRes.ResourceManager.GetString(keyVal, CultureInfo.DefaultThreadCurrentCulture) : strVal;
            return strVal;
        }


        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}