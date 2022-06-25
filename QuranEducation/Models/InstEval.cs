using QuranEducation.Models.VM;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QuranEducation.Models
{
    public class InstEvalVM
    {
        public List<Tutorial> Tuts { get; set; }
        public List<InstVM> Insts { get; set; }
    }
        public class InstEval
    {
        public int Id { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public string InstUserName { get; set; }
        [Required(ErrorMessageResourceType = typeof(QuranRes), ErrorMessageResourceName = "FieldRequired")]
        public int TutorialId { get; set; }
        public string EvalUserName { get; set; }
        public bool ArriveInATime { get; set; }
        public bool GoodInStudy { get; set; }
        public bool GoodCommunications { get; set; }
        public bool GoodInVoice { get; set; }
        public bool TechProblemInTutorial { get; set; }
        public bool GoodInUsingBoard { get; set; }
    }
}