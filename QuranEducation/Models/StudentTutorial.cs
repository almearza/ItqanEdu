using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuranEducation.Models
{
    public class StudentTutorial
    {
        public int Id { get; set; }
        public Tutorial Tutorial { get; set; }
        public int TutorialId { get; set; }
        public string UserName { get; set; }
        public bool ManagemetAccept { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? AcceptDate { get; set; }
        public bool Certified { get; set; }
        public DateTime? CertifiedDate { get; set; }
        public CertifiedDegree CertifiedDegree { get; set; }
    }
    public enum CertifiedDegree
    {
        NotCertified = 0, Excellent = 1, vGood = 2, Good = 3, Accepted = 4, Poor = 5
    }
}