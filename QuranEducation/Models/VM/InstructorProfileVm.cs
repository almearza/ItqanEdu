using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuranEducation.Models.VM
{
    public class InstructorProfileVm
    {

        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "InstTitleError")]
        [Display(ResourceType =typeof(QuranRes),Name = "JobTitle")]
        public string Title { get; set; }
        [Required( ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "InstDescError")]
        [AllowHtml]
        public string Desc { get; set; }
        public HttpPostedFileBase Image { get; set; }
        public string ImageUrl { get; set; }
        public ApplicationUser AppUser { get; set; }
        public string UserName { get; set; }
    }
}