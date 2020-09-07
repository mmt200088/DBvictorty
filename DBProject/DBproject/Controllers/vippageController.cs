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
            string str = _in.query;

            var sAll = from m in dbContext.vip_data
                       join n in dbContext.user_data on m.vip_id equals n.user_id
                       where n.user_name.Contains(str)
                       select new
                       {
                           vip_id = m.vip_id,
                           user_name = n.user_name,
                           vip_level = m.vip_level,
                           begin_time = m.begin_time,
                           end_time = m.end_time
                       };
            int num = sAll.Count();

            sAll = sAll.Skip((pageNum - 1) * pageSize).Take(pageSize);
            result.code = 1;
            result.message = "查询成功";
            result.data = new totalVip();
            result.data.total = num;
            result.data.page_num = pageNum;
            result.data.vip_data = new vipReturnData[pageSize];
            vipReturnData vip_data = new vipReturnData();
            int i = 0;
            foreach(var p in sAll)
            {
                result.data.vip_data[i] = new vipReturnData { user_name = p.user_name, vip_id = p.vip_id, vip_level = p.vip_level, start_time = p.begin_time, end_time = p.end_time };
                ++i;
            }
            return result;
        }
    }
}