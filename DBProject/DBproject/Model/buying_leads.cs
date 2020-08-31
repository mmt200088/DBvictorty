using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;

namespace DBproject.Model
{
    public class buying_leads
    {
        [Key]
        public string buy_id { get; set; }
        public string user_id { get; set; }
        public string brand { get; set; }
        public string car_name { get; set; }
        public string color { get; set; }
        public int displacement { get; set; }
        public string car_condition { get; set; }
        public string date_produce { get; set; }
        public string accident { get; set; }
        public int price { get; set; }
        public string phone { get; set; }
    }
}
