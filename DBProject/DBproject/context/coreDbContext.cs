using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using DBproject.Model;

namespace DBproject.context
{
    public class coreDbContext : DbContext
    {
        public virtual DbSet<user_data> user_data { get; set; }
        public virtual DbSet<car_data> car_data { get; set; }
        public virtual DbSet<car_vin_data> car_vin_data { get; set; }   //wwww
        public virtual DbSet<buying_leads> buying_leads { get; set; }
        public virtual DbSet<vip_data> vip_data { get; set; }
        public virtual DbSet<vip_permission> vip_permission { get; set; }
        public virtual DbSet<news> News { get; set; }
        public virtual DbSet<post_data> Post_data { get; set; }
        public virtual DbSet<reply_data> Reply_data { get; set; }
        public virtual DbSet<encyclopedia> Encyclopedia { get; set; }
        public coreDbContext(DbContextOptions<coreDbContext> options) : base(options) { }
    }
}
