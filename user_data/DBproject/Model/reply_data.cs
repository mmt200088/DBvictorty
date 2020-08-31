using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using DBproject.context;

namespace DBproject.Model
{
    [Table("reply_data")]
    public class reply_data
    {
        [Column("reply_id")]
        [Key]
        public string reply_id { get; set; }

        [Column("user_id")]
        public string user_id { get; set; }

        [Column("reply_date")]
        public DateTime reply_date { get; set; }

        [Column("post_id")]
        public string post_id { get; set; }

        [Column("content")]
        public string content { get; set; }
    }
}
