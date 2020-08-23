using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using DBproject.Model;
using DBproject.context;

namespace DBproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class registerController : ControllerBase
    {
        private coreDbContext dbContext;
        public registerController(coreDbContext _context)
        {
            dbContext = _context;
        }

        // POST register
        // api/register
        [HttpPost]
        public ReturnMessage register(dynamic _in)
        {
            ReturnMessage result = new ReturnMessage();
            string inUid = _in.user_id;
            string inName = _in.user_name;

            var sId = from m in dbContext.user_data
                      where m.user_id == inUid
                      select m;
            // 有重复id
            if (sId.FirstOrDefault() != null)
            {
                result.code = 0;
                result.message = "用户id重复";
                return result;
            }

            var sName = from m in dbContext.user_data
                        where m.user_name == inName
                        select m;
            // 有重复name
            if (sName.FirstOrDefault() != null)
            {
                result.code = 0;
                result.message = "用户名重复";
                return result;
            }

            user_data newData = new user_data { user_id = _in.user_id, password = _in.password, user_name = _in.user_name, gender = _in.gender, city = _in.city };
            dbContext.user_data.Add(newData);
            dbContext.SaveChanges();
            result.code = 1;
            result.message = "注册成功";
            return result;
        }
    }
}