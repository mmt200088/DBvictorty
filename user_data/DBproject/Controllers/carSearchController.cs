using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

using DBproject.Controllers.Utils;
using DBproject.context;
using DBproject.Model;


namespace DBproject.Controllers
{
    public class CarBuyForShow
    {
        public string buy_id { get; set; }
        public string car_name { get; set; }
        public string color { get; set; }
        public string car_condition { get; set; }
        public int price { get; set; }
        public string phone { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
    }

    public class CarSellForShow
    {
        public string car_id { get; set; }
        public string car_condition { get; set; }
        public string date_buyin { get; set; }
        public string accident { get; set; }
        public int price { get; set; }
        public string phone { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string car_vin { get; set; }
        public string car_name { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class carSearchController : ControllerBase
    {
        private readonly coreDbContext _context;

        public carSearchController(coreDbContext context)
        {
            _context = context;
        }

        //POST:api/carSearch/getSell
        [HttpPost("getSell")]
        public IActionResult GetSell(string searchkey)
        {
            try
            {
                //在数据库搜索表单
                var sellList = from sell in _context.car_data
                               join vin in _context.car_vin_data
                               on sell.car_vin equals vin.car_vin
                               join user in _context.user_data
                               on sell.user_id equals user.user_id
                               where vin.brand == searchkey
                               select new { sell, vin ,user};
                var showList = new List<CarSellForShow>();
                //将表单转为显示形式
                foreach(var row in sellList)
                {
                    var temp = new CarSellForShow
                    {
                        car_id = row.sell.car_id,
                        user_id = row.sell.user_id,
                        user_name = row.user.user_name,
                        phone = row.sell.phone,
                        accident = row.sell.accident,
                        car_condition = row.sell.car_condition,
                        date_buyin = row.sell.date_buyin.ToString(),
                        price = row.sell.price,
                        car_name = row.vin.car_name,
                        car_vin = row.sell.car_vin
                    };
                    showList.Add(temp);
                }
                //排序
                //
                var rr= new RestfulResult.RestfulArray<CarSellForShow>();
                rr.code = 1;
                rr.message = "成功查询";
                rr.data = showList.ToArray();
                return new JsonResult(rr);
            }
            catch (Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(0, exc.Message);
                return new JsonResult(rr);
            }
        }

        //POST:api/carSearch/getBuy
        [HttpPost("getBuy")]
        public IActionResult GetBuy(string searchkey)
        {
            try
            {
                //在数据库搜索表单
                var buyList = from buy in _context.buying_leads
                              join user in _context.user_data
                              on buy.user_id equals user.user_id
                              where buy.brand == searchkey
                              select new { buy, user };

                var showList = new List<CarBuyForShow>();
                //将表单转为显示形式
                foreach (var row in buyList)
                {
                    var temp = new CarBuyForShow
                    {
                        buy_id=row.buy.buy_id,
                        car_name= row.buy.car_name,
                        car_condition= row.buy.car_condition,
                        color= row.buy.color,
                        phone= row.buy.phone,
                        price= row.buy.price,
                        user_id= row.buy.user_id,
                        user_name= row.user.user_name
                    };
                    showList.Add(temp);
                }
                //排序
                //
                var rr = new RestfulResult.RestfulArray<CarBuyForShow>();
                rr.code = 1;
                rr.message = "成功查询";
                rr.data = showList.ToArray();
                return new JsonResult(rr);
            }
            catch (Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(0, exc.Message);
                return new JsonResult(rr);
            }
        }
    }
}