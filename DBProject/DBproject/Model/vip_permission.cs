using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace DBproject.Model
{
    public class vip_permission
    {
        [Key]
        public string permissions { get; set; }
        public int vip_level { get; set; }
    }
}
