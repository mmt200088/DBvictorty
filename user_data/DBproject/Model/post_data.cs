using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using DBproject.context;

namespace DBproject.Model
{
    [Table("post_data")]
    public class post_data
    {
        [Column("post_id")]
        [Key]
        public string post_id { get; set; }

        [Column("user_id")]
        public string user_id { get; set; }

        [Column("post_date")]
        public DateTime post_date { get; set; }

        [Column("title")]
        public string title { get; set; }

        [Column("content")]
        public string content { get; set; }
    }
}
