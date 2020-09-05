using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBproject.Model
{
    public class totalVip
    {
        public int total { get; set; }
        public int page_num { get; set; }
        public vipReturnData[] vip_data { get; set; }
    }
}
