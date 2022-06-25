using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuranEducation.Models
{
    public class Inbox
    {
        public int Id { get; set; }
        public string AUserName { get; set; }
        public string Message { get; set; }
        public DateTime Arrival { get; set; }
        public string Sender { get; set; }
        public string Link { get; set; }
    }
}