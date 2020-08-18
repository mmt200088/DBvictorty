using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplication10.Models
{
    public class User_data
    {
       // [Key]
        public string Id { get; set; }
      //  [MaxLength(32)]
        public string Name { get; set; }
    //    [MaxLength(32)]
        public string gender { get; set; }
   //     [MaxLength(10)]
        public string city { get; set; }
    //    [MaxLength(32)]
        public string password { get; set; }

  
    }
}
