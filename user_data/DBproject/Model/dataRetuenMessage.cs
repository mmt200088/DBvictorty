using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBproject.Model
{
    public class dataRetuenMessage
    {
        public int code { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
