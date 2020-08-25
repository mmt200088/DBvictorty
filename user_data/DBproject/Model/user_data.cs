using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace DBproject.Model
{
    public class user_data
    {
        [Key]
        public string user_id { get; set; }
        [MaxLength(32)]
        public string password { get; set; }
        [MaxLength(32)]
        public string user_name { get; set; }
        [MaxLength(32)]
        public string gender { get; set; }
        [MaxLength(32)]
        public string city { get; set; }
    }
}
