using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuranEducation.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        [Display(ResourceType = typeof(QuranRes), Name = "PhoneNumber")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string ProfilePic { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "الرمز")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessage = "أدخل البريد الإلكتروني")]
        [Display(Name = "البريد الإلكتروني")]
        [StringLength(50, ErrorMessage = "طول البريد الالكتروني يجب ان لا يتجاوز 50 حرفا")]
        [EmailAddress(ErrorMessage ="الرجاء ادخال بريد الكتروني صحيح")]
        public string Email { get; set; }

        [Required(ErrorMessage = "أدخل كلمة السر")]
        [DataType(DataType.Password)]
        [Display(Name = "كلمة السر")]
        public string Password { get; set; }

        [Display(Name = "تذكرني؟")]
        public bool RememberMe { get; set; }
        public string CaptchaText { get; set; }
    }


    public class UserPlusRoleNameVM
    {
        public ApplicationUser User { get; set; }
        public string RoleName { get; set; }
        public string ActivityName { get; set; }
    }
    public class UsersViewModel
    {
        public ICollection<UserPlusRoleNameVM> Users { get; set; }
    }
    public class RegisterViewModel
    {
        public string UserId { get; set; }
        [Required(ErrorMessage = "أدخل البريد الإلكتروني و بالصورة الصحيحة")]
        [EmailAddress(ErrorMessage = "البريد المدخل غير صالح")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "البريد الإلكتروني")]
        [StringLength(50, ErrorMessage = "طول البريد الالكتروني يجب ان لا يتجاوز 50 حرفا")]
        public string Email { get; set; }


        [Display(Name = "الاسم كاملا")]
        [Required(ErrorMessage = "الرجاء كتابة الاسم")]
        [StringLength(100, ErrorMessage = "طول الاسم يجب ان لا يقل عن 6 ولا يزيد عن 100 حرفا", MinimumLength = 6)]
        public string Name { get; set; }
        [Display(Name = "رقم الجوال")]
        [Required(ErrorMessage = "الرجاء كتابة رقم الجوال")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="الرجاء إدخال رقم جوال صحيح")]
        public string PhoneNumber { get; set; }
        //[Display(Name = "رقم الهوية")]
        //public string IdentityNo { get; set; }
        [Required(ErrorMessage = "حدد صلاحيات المستخدم")]
        [Display(Name = "الصلاحية")]
        public string RoleName { get; set; }
        [Required(ErrorMessage = "حدد لغة المستخدم")]
        [Display(Name = "اللغة")]
        public string LangCode { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "OldPassErrorText")]
        [Display(Name = "OldPassText", ResourceType = typeof(QuranRes))]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "NewPassErrorText")]
        [Display(Name = "NewPassText", ResourceType = typeof(QuranRes))]
        //[StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [StringLength(50, ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "NewPassLength", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassText",ResourceType =typeof(QuranRes))]
        [Compare("NewPassword",ErrorMessageResourceType =typeof(QuranRes), ErrorMessageResourceName ="ConfirmPassErrorText")]
        public string ConfirmPassword { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
