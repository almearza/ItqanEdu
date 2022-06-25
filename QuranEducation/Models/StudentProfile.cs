using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuranEducation.Models
{
    public class StudentProfile
    {
        public int Id { get; set; }
        public string ImgUrl { get; set; }
        public ApplicationUser AppUser { get; set; }
        public string AppUserId { get; set; }
        public string EnName { get; set; }
        public string Nationality { get; set; }
        public string BOD { get; set; }
        public string HomePhoneNumber { get; set; }
        public int ResidentCountryCode { get; set; }
        public string EduYear { get; set; }
        public string StScore { get; set; }
        public string CertUrl { get; set; }
        public string Gender { get; set; }
    }

}