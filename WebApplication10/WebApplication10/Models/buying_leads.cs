using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication10.DataAccess.Base;

namespace WebApplication10.Models
{
    [Table("buying_leads")]
    public class buying_leads
    {
        [Column("buy_id")]
        [Key]
        public string buy_id { get; set; }

        [Column("user_id")]
        public string user_id { get; set; }

        [Column("brand")]
        public string brand { get; set; }

        [Column("car_name")]
        public string car_name { get; set; }

        [Column("color")]
        public string color { get; set; }

        [Column("displacement")]
        public float displacement { get; set; }

        [Column("condition")]
        public string condition { get; set; }

        [Column("date_produce")]
        public float date_produce { get; set; }

        [Column("accident")]
        public string accident { get; set; }

        [Column("price")]
        public int price { get; set; }

        [Column("phone")]
        public string phone { get; set; }
    }
}
