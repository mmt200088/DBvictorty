using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace DBproject.Model
{
    public class vip_data
    {
        [Key]
        public string vip_id { get; set; }
        public int vip_level { get; set; }
        public string begin_time { get; set; }
        public string end_time { get; set; }
    }
}
