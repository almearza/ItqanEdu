using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace QuranEducation.Models
{
    public class Tutorial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Descr { get; set; }
        public DateTime OpenDate { get; set; }
        public DateTime CloseDate { get; set; }
        public string ImageUrl { get; set; }
        public string LangCode { get; set; }
        public bool Active { get; set; }
        public string DoneBy { get; set; }
        public DateTime StDate { get; set; }
        public string InstUName { get; set; }
        public int CountOfLec { get; set; }
    }
}