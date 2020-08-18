using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication10.DataAccess.Interface;
using WebApplication10.Models;
using WebApplication10.DataAccess.Base;


//类实现接口 
namespace WebApplication10.DataAccess.Implement
{
    public class UserDataClass : UserDataInterface
    {
        public CoreDbContext Context;

        //构造函数 注入dbcontext
        public UserDataClass(CoreDbContext context)
        {
            Context = context;
        }

        //插入数据
        public bool CreateUser(User_data user)
        {
            Context.user_data.Add(user);
            return Context.SaveChanges() > 0;
        }

        //取全部记录
        public IEnumerable<User_data> GetUsers()
        {
            return Context.user_data.ToList();
        }

        //取某id记录
        public User_data GetUserByID(string id)
        {
            return Context.user_data.SingleOrDefault(s => s.user_id == id);
        }

        //根据id更新整条记录
        public bool UpdateUser(User_data user)
        {
            Context.user_data.Update(user);
            return Context.SaveChanges() > 0;
        }

        //根据id更新名称
        public bool UpdateNameByID(string id, string name)
        {
            var state = false;
            var user = Context.user_data.SingleOrDefault(s => s.user_id == id);

            if (user != null)
            {
                user.user_name = name;
                state = Context.SaveChanges() > 0;
            }

            return state;
        }

        //根据id删掉记录
        public bool DeleteUserByID(string id)
        {
            var user = Context.user_data.SingleOrDefault(s => s.user_id == id);
            Context.user_data.Remove(user);
            return Context.SaveChanges() > 0;
        }

    }
}
