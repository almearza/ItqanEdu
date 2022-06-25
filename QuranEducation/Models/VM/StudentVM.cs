using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace QuranEducation.Models.VM
{
    public class StudentVM
    {
        public string UserId { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string EnName { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string Nationality { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        [Column(TypeName = "date")]
        public string BOD { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string PhoneNumber { get; set; }
        public string HomePhoneNumber { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string LangCode { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public int ResidentCountryCode { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string EduYear { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string StScore { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string Gender { get; set; }

        public HttpPostedFileBase ImgFile { get; set; }
        public string ImgUrl { get; set; }
        public HttpPostedFileBase CertFile { get; set; }
        public string CertUrl { get; set; }
    }
}