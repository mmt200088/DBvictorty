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
    public class permissionController : ControllerBase
    {
        private coreDbContext dbContext;
        public permissionController(coreDbContext _context)
        {
            dbContext = _context;
        }

        // GET permission
        // api/permission/1  (1为会员等级 暂定1 2 3 三挡
        [HttpGet("{vip_level}")]
        public dataReturnMessage Permission(int vip_level)
        {
            dataReturnMessage result = new dataReturnMessage();

            // 查询成功
            var sPms = from m in dbContext.vip_permission
                       where m.vip_level <= vip_level
                       orderby m.vip_level descending   // 从大到小排序
                       select m;
            vip_permission[] right = new vip_permission[sPms.Count()];
            int i = 0;
            foreach(var p in sPms)
            {
                right[i++] = p;
            }
            result.code = 1;
            result.message = "查询成功";
            result.data = right;
            return result;
        }
    }
}