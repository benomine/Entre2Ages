using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorEntre2Ages.Models
{
    public class Message
    {
        public long TimeStamp { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public bool Mine { get; set; }

        public bool IsNotice => Body.StartsWith("[Notice]");
        public string Css => Mine ? "sent" : "received";
    }
}
