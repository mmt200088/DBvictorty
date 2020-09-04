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
    public class usersController : ControllerBase
    {
        private coreDbContext dbContext;
        public usersController(coreDbContext _context)
        {
            dbContext = _context;
        }

        // POST users
        // api/users
        [HttpPost]
        public dataReturnMessage Users(dynamic _in)
        {
            dataReturnMessage result = new dataReturnMessage();

            var sAll = from m in dbContext.user_data
                       select m;
            int num = sAll.Count();
            int pageNum = _in.page_num;
            int pageSize = _in.page_size;

            sAll = sAll.Skip((pageNum - 1) * pageSize).Take(pageSize);
            result.code = 1;
            result.message = "查询成功";
            result.data = new totalUsers();
            result.data.total = num;
            result.data.page_num = pageNum;
            result.data.users = new user_data[pageSize];
            int i = 0;
            foreach(var p in sAll)
            {
                result.data.users[i++] = p;
            }
            return result;
        }
    }
}