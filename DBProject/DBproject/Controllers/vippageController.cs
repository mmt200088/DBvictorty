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
    public class vippageController : ControllerBase
    {
        private coreDbContext dbContext;
        public vippageController(coreDbContext _context)
        {
            dbContext = _context;
        }

        // POST vippage
        // api/vippage
        [HttpPost]
        public dataReturnMessage VipPage(dynamic _in)
        {
            dataReturnMessage result = new dataReturnMessage();

            int pageNum = _in.page_num;
            int pageSize = _in.page_size;

            var sAll = from m in dbContext.vip_data
                       select m;
            int num = sAll.Count();

            sAll = sAll.Skip((pageNum - 1) * pageSize).Take(pageSize);
            result.code = 1;
            result.message = "查询成功";
            result.data = new totalVip();
            result.data.total = num;
            result.data.page_num = pageNum;
            result.data.vip_data = new vip_data[pageSize];
            int i = 0;
            foreach(var p in sAll)
            {
                result.data.vip_data[i++] = p;
            }
            return result;
        }
    }
}