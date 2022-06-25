using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuranEducation.Models
{
    public class ContactUs
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "EmailType")]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string Message { get; set; }
        public string ArrivalDate { get; set; }
    }
}