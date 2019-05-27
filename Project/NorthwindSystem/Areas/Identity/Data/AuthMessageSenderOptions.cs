using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NorthwindSystem.Areas.Identity.Data
{
    public class AuthMessageSenderOptions
    {
        public string SendGridKey { get; set; }
        public string SendGridUser { get; set; }
        public string SendGridTitle { get; set; }
    }
}
