using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBproject.Model
{
    public class totalUsers
    {
        public int total { get; set; }
        public int page_num { get; set; }
        public object[] users { get; set; }
    }
}
