using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace DBproject.Model
{
    public class car_data
    {
        [Key]
        public string car_id { get; set; }
        public string car_vin { get; set; }
        public string car_condition { get; set; }
        public string date_buyin { get; set; }
        public string accident { get; set; }
        public int price { get; set; }
        public string phone { get; set; }
    }
}
