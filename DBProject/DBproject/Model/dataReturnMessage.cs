using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBproject.Model
{
    public class dataReturnMessage
    {
        public int code { get; set; }
        public string message { get; set; }
        public dynamic data { get; set; }
    }
}
