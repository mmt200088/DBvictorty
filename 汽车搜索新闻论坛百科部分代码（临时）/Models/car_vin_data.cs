using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApplication10.DataAccess.Base;

namespace WebApplication10.Models
{
    [Table("car_vin_data")]
    public class car_vin_data
    {
        [Column("car_vin")]
        [Key]
        public string car_vin { get; set; }

        [Column("brand")]
        public string brand { get; set; }

        [Column("car_name")]
        public string car_name { get; set; }

        [Column("color")]
        public string color { get; set; }

        [Column("displacement")]
        public float displacement { get; set; }

        [Column("date_produce")]
        public float date_produce { get; set; }
    }
}
