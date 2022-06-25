using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuranEducation.Models
{
    public class Assigment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Tutorial Tutorial { get; set; }
        public int TutorialId { get; set; }
        public AssType AssType { get; set; }
        public double Degree { get; set; }
        public DateTime StDate { get; set; }
        public List<AssigmentAttachment> AssigmentAttachments { get; set; }
    }
    public enum AssType
    {
        Assigment=1,Homework=2
    }
    public class AssigmentAttachment
    {
        public int Id { get; set; }
        public Assigment Assigment { get; set; }
        public int AssigmentId { get; set; }
        public string FileUrl { get; set; }
    }
    

}