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
    public class carbuyController : ControllerBase
    {
        private coreDbContext dbContext;
        public carbuyController(coreDbContext _context)
        {
            dbContext = _context;
        }

        // POST carbuy
        // api/carbuy
        [HttpPost]
        public ReturnMessage Carbuy(dynamic _in)
        {
            ReturnMessage result = new ReturnMessage();
            string inBuyId = _in.buy_id;
            string inUid = _in.user_id;

            // 求购id已存在
            var sBuyId = from m in dbContext.buying_leads
                         where m.buy_id == inBuyId
                         select m;
            if(sBuyId.FirstOrDefault() != null)
            {
                result.code = 0;
                result.message = "求购id已存在";
                return result;
            }

            // 用户id不存在
            var sUid = from m in dbContext.user_data
                       where m.user_id == inUid
                       select m;
            if(sUid.FirstOrDefault() == null)
            {
                result.code = 0;
                result.message = "用户id不存在";
                return result;
            }

            // 发布求购信息成功
            buying_leads newBuyingLeads = new buying_leads
            {
                buy_id = _in.buy_id,
                user_id = _in.user_id,
                brand = _in.brand,
                car_name = _in.car_name,
                color = _in.color,
                displacement = _in.displacement,
                car_condition = _in.car_condition,
                date_produce = _in.date_produce,
                accident = _in.accident,
                price = _in.price,
                phone = _in.phone
            };
            dbContext.buying_leads.Add(newBuyingLeads);
            dbContext.SaveChanges();

            result.code = 1;
            result.message = "发布成功";
            return result;

        }
    }
}