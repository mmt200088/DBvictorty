using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using DBproject.context;


namespace DBproject.Model
{
    [Table("encyclopedia")]
    public class encyclopedia
    {
        [Column("ID")]
        [Key]
        public string ID { get; set; }

        [Column("title")]
        public string title { get; set; }

        //是否应该改为string？
        [Column("post_date")]
        public DateTime post_date { get; set; }

        [Column("author_id")]
        public string author_id { get; set; }

        [Column("author_name")]
        public string author_name { get; set; }

        [Column("content")]
        [Required]
        [StringLength(4096)]
        public string content { get; set; }

        [Column("part")]
        public string part { get; set; }

        [Column("reader_num")]
        public int reader_num { get; set; }
    }
}
