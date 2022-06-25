using System;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace QuranEducation.Models.VM
{
    public class TutorialVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال عنوان الدورة")]
        [Display(Name = "عنوان الدورة")]
        public string Title { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال وصف الدورة")]
        [Display(Name = "وصف الدورة")]
        [AllowHtml]
        public string Descr { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال تاريخ فتح الدورة")]
        [Display(Name = "تاريخ فتح الدورة")]
        [DataType(DataType.Date, ErrorMessage = "الرجاء إدخال تاريخ")]
        public DateTime OpenDate { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال تاريخ قفل الدورة")]
        [Display(Name = "تاريخ قفل الدورة")]
        [DataType(DataType.Date, ErrorMessage = "الرجاء إدخال تاريخ")]
        public DateTime CloseDate { get; set; }
        [Display(Name = "صورة الدورة")]
        public HttpPostedFileBase Image { get; set; }
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "الرجاء إدخال لغة الدورة")]
        [Display(Name = "لغة الدورة")]
        public string LangCode { get; set; }
        [Required(ErrorMessage = "الرجاء إدخال معلم الدورة")]
        [Display(Name = "معلم الدورة")]
        public string InstUName { get; set; }
        public bool Active { get; set; }
        public string DoneBy { get; set; }
        public DateTime StDate { get; set; }
    }
}