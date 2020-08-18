using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace WebApplication10.Models
{
    [Table("person")]
    public class person
    {
        [Column("id")]
       
        public string id { get; set; }

        [Column("name")]

        public string name { get; set; }
    }
}
