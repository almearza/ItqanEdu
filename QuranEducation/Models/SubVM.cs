using System;

namespace QuranEducation.Models
{
    public class SubVM
    {
        public int Id { get; set; }
        public string TutorialTitle { get; set; }
        public int TutorialSubCount { get; set; }
        public string StudentName { get; set; }
        public string StudentUserName { get; set; }
        public string StudentEmial { get; set; }
        public string StudentPhone { get; set; }
        public DateTime RequestDate { get; set; }
    }
}