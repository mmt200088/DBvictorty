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

        [Route("api/userdata")]

        [ApiController]
        public class userdataController : ControllerBase
        {


            //注入接口(服务)
            public readonly UserDataClass myclass; //只读

            public userdataController(UserDataClass myClass)
            {
                myclass = myClass;
            }


            //插入数据


            [HttpGet]
            public ActionResult<string> GetAllUsers()
            {
                var names = "meiyou数据";
                var users = myclass.GetUsers();

                if (users != null)
                {
                    names = "";
                    foreach (var s in users)
                    {
                        names += $"{s.user_name} \r\n";
                    }

                }

                return names;
            }

            [HttpPost("{id}/{name}/{gender}/{city}/{password}")]
            public ActionResult<string> CreatePerson(string id, string name,string gender,string city,string password)
            {

                var user = new User_data()
                {
                    user_id = id,
                    user_name = name,
                    gender = gender,
                    city = city,
                    password = password

                };

                var result = myclass.CreateUser(user);

                if (result)
                {
                    return "用户插入成功";
                }

                else
                {
                    return "用户插入失败";
                }



            }



            [HttpGet("{id}")]
            public ActionResult<string> GetUser(string id)
            {

                var name = "hahah 数据";
                var user = myclass.GetUserByID(id);

                if (user != null)
                {
                    name = user.user_name;
                }

                return name;
            }


        }
    }

