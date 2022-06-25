using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuranEducation.Models.VM
{
    public class InstEvalReportVM
    {
        public string InstName { get; set; }
        public string TutName { get; set; }
        public string EvalName { get; set; }
        public string ArriveInATime { get; set; }
        public string GoodInStudy { get; set; }
        public string GoodCommunications { get; set; }
        public string GoodInVoice { get; set; }
        public string TechProblemInTutorial { get; set; }
        public string GoodInUsingBoard { get; set; }
    }
}