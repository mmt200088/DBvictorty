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
    public class PersonClass:PersonInterface
    {
        public CoreDbContext Context;

        //构造函数 注入dbcontext
        public PersonClass(CoreDbContext context)
        {
            Context = context;
        }

        //插入数据
        public bool CreatePerson(person peo)
        {
            Context.Person.Add(peo);
            return Context.SaveChanges() > 0;
        }

        //取全部记录
        public IEnumerable<person> GetPersons()
        {
            return Context.Person.ToList();
        }

        //取某id记录
        public person GetPersonByID(string id)
        {
            return Context.Person.SingleOrDefault(s => s.id == id);
        }

        //根据id更新整条记录
        public bool UpdatePerson(person peo)
        {
            Context.Person.Update(peo);
            return Context.SaveChanges() > 0;
        }

        //根据id更新名称
        public bool UpdateNameByID(string id, string name)
        {
            var state = false;
            var peo = Context.Person.SingleOrDefault(s => s.id == id);

            if (peo != null)
            {
                peo.name = name;
                state = Context.SaveChanges() > 0;
            }

            return state;
        }

        //根据id删掉记录
        public bool DeletePersonByID(string id)
        {
            var peo  = Context.Person.SingleOrDefault(s => s.id == id);
            Context.Person.Remove(peo);
            return Context.SaveChanges() > 0;
        }

    }
}
