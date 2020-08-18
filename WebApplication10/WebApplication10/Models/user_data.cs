using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

using WebApplication10.DataAccess.Base;

namespace WebApplication10.Models
{
    public class User_data
    {
        [Key]
        public string user_id { get; set; }
        // [MaxLength(20)]
        public string user_name { get; set; }
        // [MaxLength(3)]
        public string gender { get; set; }

        public string city { get; set; }

        public string password { get; set; }
    }
}