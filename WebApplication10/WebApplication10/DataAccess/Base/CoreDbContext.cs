using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using WebApplication10.Models;

namespace WebApplication10.DataAccess.Base
{
    //dbcontext是数据库上下文类 可以理解为数据库对象的实例 在ef中无需使用sql对数据库进行操作 而是通过dbcontext
   public class CoreDbContext : DbContext
    {
        //

            //通过构造函数注入数据库连接 必须依托于options
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {

        }

        //实体映射到数据库的两个表
       // public virtual DbSet<Order_data> order_data { get; set; } //创建实体类添加Context中

        public virtual DbSet<User_data> user_data { get; set; } //创建实体类添加Context中

        public virtual DbSet<person> Person { get; set; }//创建实体类添加到Context中


        //限制条码
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        //    modelBuilder.Entity<Order_data>()
         //       .Property(x => x.order_id).IsRequired().HasMaxLength(100);//order_id的长度属性必填 且最长不超过100

        }


    }
}
