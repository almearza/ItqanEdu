using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuranEducation.Models.VM
{
    public class LectureVM
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType =typeof(QuranRes),ErrorMessageResourceName ="FieldRequired")]
        public string Title { get; set; }
        [Required(ErrorMessageResourceType =typeof(QuranRes),ErrorMessageResourceName ="FieldRequired")]
        [AllowHtml]
        public string Descr { get; set; }
        [Required(ErrorMessageResourceType =typeof(QuranRes),ErrorMessageResourceName ="FieldRequired")]
        public string LecDate { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string LecStartTimeStr { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string LecEndTimeStr { get; set; }
        [Required(ErrorMessageResourceType =typeof(QuranRes),ErrorMessageResourceName ="FieldRequired")]
        public string VirtualRoomUrl { get; set; }
        public string VideoUrl { get; set; }
        public string AttachmentUrl { get; set; }
        public HttpPostedFileBase Attachment { get; set; }
        public Tutorial Tutorial { get; set; }
        [Required(ErrorMessageResourceType =typeof(QuranRes),ErrorMessageResourceName ="FieldRequired")]
        public int TutorialId { get; set; }
        public DateTime StDate { get; set; }
    }
}