using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace DBproject.Model
{
    public class car_vin_data
    {
        [Key]
        public string car_vin { get; set; }
        public string brand { get; set; }
        public string car_name { get; set; }
        public string color { get; set; }
        public float displacement { get; set; }
        public string date_produce { get; set; }
    }
}
