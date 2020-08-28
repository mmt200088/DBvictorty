using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApplication10.Models;
using WebApplication10.DataAccess.Base;

namespace WebApplication10.DataAccess.Interface
{
    public interface PersonInterface
    {

        //插入数据 对实体类进行操作
        bool CreatePerson(person person);

        //取全部记录
        IEnumerable<person> GetPersons();

        //取某id记录
        person GetPersonByID(string id);

        //根据id更新整条记录
        bool UpdatePerson(person person);

        //根据id更新名称
        bool UpdateNameByID(string id, string name);

        //根据id删掉记录
        bool DeletePersonByID(string id);
    }
}
