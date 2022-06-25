using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuranEducation.Models.VM
{
    public class InboxVM
    {

        public int Id { get; set; }
        public ApplicationUser AUser { get; set; }
        public string AUserId { get; set; }
        public string Message { get; set; }
        public DateTime Arrival { get; set; }
    }
}