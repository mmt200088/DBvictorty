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
    public class loginController : ControllerBase
    {
        private coreDbContext dbContext;
        public loginController(coreDbContext _context)
        {
            dbContext = _context;
        }

        // POST login
        // api/login
        [HttpPost]
        public ReturnMessage Login(dynamic _in)
        {
            ReturnMessage result = new ReturnMessage();
            int flag = 1;
            string inUid = _in.user_id;
            string inPwd = _in.password;

            var sUid = from m in dbContext.user_data
                       where m.user_id == inUid
                       select m;
            // 用户id不存在
            if (sUid.FirstOrDefault() == null) flag = 0;
            if(flag == 0)
            {
                result.code = 0;
                result.message = "用户id不存在";
                return result;
            }
            // 用户id存在 校验密码
            // flag: 1 密码错误
            //       2 正确
            foreach(var p in sUid)
            {
                if (p.password == inPwd) flag = 2;
            }
            if(flag == 1)
            {
                result.code = 0;
                result.message = "密码错误";
                return result;
            }
            else if(flag == 2)
            {
                result.code = 1;
                result.message = "登录成功";
                return result;
            }
            else
            {
                result.code = 0;
                result.message = "未知错误";
                return result;
            }
        }
    }
}