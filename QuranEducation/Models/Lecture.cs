using System;

namespace QuranEducation.Models
{
    public class Lecture
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public DateTime LecStrartTime { get; set; }
        public DateTime LecEndTime { get; set; }
        public string VirtualRoomUrl { get; set; }
        public string VideoUrl { get; set; }
        public string AttachmentUrl { get; set; }
        public Tutorial Tutorial { get; set; }
        public int TutorialId { get; set; }
        public DateTime StDate { get; set; }
    }
}