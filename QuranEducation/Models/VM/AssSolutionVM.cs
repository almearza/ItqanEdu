using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuranEducation.Models.VM
{
    public class AssSolutionVM
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        [AllowHtml]
        public string Descr { get; set; }
        public IEnumerable<HttpPostedFileBase> AFiles { get; set; }
        public List<SolutionAttachment> SolutionAttachments { get; set; }
        public Assigment Assignment { get; set; }
        public int AssignmentId { get; set; }
        public string UserName { get; set; }
    }
}