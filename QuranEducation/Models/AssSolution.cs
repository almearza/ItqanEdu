using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuranEducation.Models
{
    public class AssSolution
    {
        public int Id { get; set; }
        public Assigment Assignment { get; set; }
        public int AssignmentId { get; set; }
        public string UserName { get; set; }
        public string Descr { get; set; }
        public List<SolutionAttachment> SolutionAttachments { get; set; }

    }
    public class SolutionAttachment
    {
        public int Id { get; set; }
        public AssSolution Solution { get; set; }
        public int SolutionId { get; set; }
        public string FileUrl { get; set; }
    }
}