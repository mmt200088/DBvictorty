using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApplication10.DataAccess.Base;
using WebApplication10.DataAccess.Interface;
using WebApplication10.DataAccess.Implement;
using WebApplication10.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication10.Controllers
{
    [Route("api/person")]
    
    [ApiController]
    public class personController:ControllerBase
    {


        //注入接口(服务)
        public  readonly PersonClass myclass; //只读

        public personController (PersonClass myClass)
        {
            myclass = myClass;
        }


        //插入数据


        [HttpGet]
        public ActionResult<string> GetAllPerson()
        {
            var names = "meiyou数据";
            var students = myclass.GetPersons();

            if (students != null)
            {
                names = "";
                foreach (var s in students)
                {
                    names += $"{s.name} \r\n";
                }

            }

            return "kkk";
        }

        [HttpPost("{id}/{name}")]
        public ActionResult<string> CreatePerson(string id,string name)
        {
            if (string.IsNullOrEmpty(name.Trim()))
            {
                return "姓名不能为空";
            }

            var peo = new person()
            {
                id = id,
                name = name

            };

            var result = myclass.CreatePerson(peo);

            if (result)
            {
                return "学生插入成功";
            }

            else
            {
                return "学生插入失败";
            }



        }



        [HttpGet("{id}")]
        public ActionResult<string> Getxxxson(string id)
        {

            var name = "hahah 数据";
            var student = myclass.GetPersonByID(id);

            if (student != null)
            {
                name = student.name;
            }

            Console.WriteLine("sss");
            return name;
        } 






    }
}
