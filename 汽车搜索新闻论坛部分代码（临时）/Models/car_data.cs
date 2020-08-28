using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication10.DataAccess.Base;

namespace WebApplication10.Models
{
    [Table("car_data")]
    public class car_data
    {
        [Column("car_id")]
        [Key]
        public string car_id { get; set; }

        [Column("car_vin")]
        public string car_vin { get; set; }

        [Column("condition")]
        public string condition { get; set; }

        //String?
        [Column("date_buyin")]
        public DateTime date_buyin { get; set; }

        [Column("accident")]
        public string accident { get; set; }

        [Column("price")]
        public int price { get; set; }

        [Column("phone")]
        public string phone { get; set; }

        //应该有此属性，但目前没有
        [Column("user_id")]
        public string user_id { get; set; }
    }
}
