using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApplication10.Models;
using WebApplication10.DataAccess.Base;

namespace WebApplication10.DataAccess.Interface
{
    public interface UserDataInterface
    {

        //插入数据 对person实体类进行操作
        bool CreateUser(User_data user);

        //取全部记录
        IEnumerable<User_data> GetUsers();

        //取某id记录
        User_data GetUserByID(string id);

        //根据id更新整条记录
        bool UpdateUser(User_data user);

        //根据id更新名称
        bool UpdateNameByID(string id, string name);

        //根据id删掉记录
        bool DeleteUserByID(string id);
    }
}
