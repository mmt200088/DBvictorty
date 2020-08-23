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
    public class carsellController : ControllerBase
    {
        private coreDbContext dbContext;
        public carsellController(coreDbContext _context)
        {
            dbContext = _context;
        }

        // POST carsell
        // api/carsell
        [HttpPost]
        public ReturnMessage Carsell(dynamic _in)
        {
            ReturnMessage result = new ReturnMessage();
            string inId = _in.car_id;
            string inVin = _in.car_vin;

            // 二手车id已存在
            var sId = from m in dbContext.car_data
                      where m.car_id == inId
                      select m;
            if(sId.FirstOrDefault() != null)
            {
                result.code = 0;
                result.message = "二手车id已存在";
                return result;
            }

            // 车架号存在
            var sVin = from m in dbContext.car_vin_data
                       where m.car_vin == inVin
                       select m;
            if(sVin.FirstOrDefault() != null)
            {
                result.code = 0;
                result.message = "二手车车架号已存在";
                return result;
            }

            // 上架成功
            car_data newCarData = new car_data
            {
                car_id = _in.car_id,
                car_vin = _in.car_vin,
                car_condition = _in.car_condition,
                date_buyin = _in.date_buyin,
                accident = _in.accident,
                price = _in.price,
                phone = _in.phone
            };
            dbContext.car_data.Add(newCarData);
            car_vin_data newCarVinData = new car_vin_data
            {
                car_vin = _in.car_vin,
                brand = _in.brand,
                car_name = _in.car_name,
                color = _in.color,
                displacement = _in.displacement,
                date_produce = _in.date_produce
            };
            dbContext.car_vin_data.Add(newCarVinData);
            dbContext.SaveChanges();

            result.code = 1;
            result.message = "上架成功";
            return result;
        }
    }
}