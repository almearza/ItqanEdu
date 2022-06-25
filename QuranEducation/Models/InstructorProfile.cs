using QuranEducation.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuranEducation.Models
{
    public class InstructorProfile
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string  Desc { get; set; }
        public string ImageUrl { get; set; }
        public string UserName { get; set; }

    }
}