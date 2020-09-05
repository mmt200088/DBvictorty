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
    public class vipController : ControllerBase
    {
        private coreDbContext dbContext;
        public vipController(coreDbContext _context)
        {
            dbContext = _context;
        }

        // GET vip
        // api/vip/123456
        [HttpGet("{vid}")]
        public dataReturnMessage Vip(string vid)
        {
            dataReturnMessage result = new dataReturnMessage();

            // 用户id不存在
            var sUid = from m in dbContext.user_data
                       where m.user_id == vid
                       select m;
            if (sUid.FirstOrDefault() == null)
            {
                result.code = 0;
                result.message = "用户id不存在";
                result.data = null;
                return result;
            }

            // 用户不是vip
            var sVipId = from m in dbContext.vip_data
                         where m.vip_id == vid
                         select m;
            if (sVipId.FirstOrDefault() == null)
            {
                result.code = 0;
                result.message = "用户不是vip";
                result.data = null;
                return result;
            }
            vip_data vip = sVipId.First();
            user_data user = sUid.First();
            // 用户vip过期
            DateTime nowTime = DateTime.Now;
            if(Convert.ToDateTime(vip.end_time) < nowTime)
            {
                result.code = 0;
                result.message = "vip已过期";
                dbContext.vip_data.Remove(vip);
                dbContext.SaveChanges();
                return result;
            }
            // 查询成功
            result.code = 1;
            result.message = "查询成功";
            vipReturnData data = new vipReturnData { vip_id = vid, user_name = user.user_name, start_time = vip.begin_time, end_time = vip.end_time, vip_level = vip.vip_level};
            result.data = data;
            return result;
        }

        // POST vip
        // api/vip
        [HttpPost]
        public ReturnMessage Vip(dynamic _in)
        {
            ReturnMessage result = new ReturnMessage();

            string inUid = _in.vip_id;
            int inLevel = _in.vip_level;

            // 用户id不存在
            var sUid = from m in dbContext.user_data
                       where m.user_id == inUid
                       select m;
            if (sUid.FirstOrDefault() == null)
            {
                result.code = 0;
                result.message = "用户id不存在";
                return result;
            }

            var sVip = from m in dbContext.vip_data
                       where m.vip_id == inUid
                       select m;

            DateTime nowTime = DateTime.Now;
            // 之前不是vip
            if (sVip.FirstOrDefault() == null)
            {
                vip_data newVal = new vip_data { vip_id = inUid, vip_level = inLevel };
                newVal.begin_time = string.Format("{0:yyyy-MM-dd}", nowTime);
                nowTime = nowTime.AddDays(30);
                newVal.end_time = string.Format("{0:yyyy-MM-dd}", nowTime);

                dbContext.vip_data.Add(newVal);
                dbContext.SaveChanges();

                result.code = 1;
                result.message = "添加成功";
                return result;
            }
            // 用户已经是vip
            var nowUser = sVip.First();
            nowUser.vip_level = inLevel;
            nowUser.begin_time = string.Format("{0:yyyy-MM-dd}", nowTime);
            nowTime = nowTime.AddDays(30);
            nowUser.end_time = string.Format("{0:yyyy-MM-dd}", nowTime);

            dbContext.vip_data.Update(nowUser);
            dbContext.SaveChanges();

            result.code = 1;
            result.message = "添加成功";
            return result;
        }

        // DELETE vip
        // api/vip/123456
        [HttpDelete("{uid}")]
        public dataReturnMessage VipDelete(string uid)
        {
            dataReturnMessage result = new dataReturnMessage();
            var sVip = from m in dbContext.vip_data
                       where m.vip_id == uid
                       select m;
            // 用户id不存在
            if(sVip.FirstOrDefault() == null)
            {
                result.code = 0;
                result.message = "用户id不存在";
                return result;
            }
            // 删除成功
            var oldData = sVip.First();
            dbContext.vip_data.Remove(oldData);
            dbContext.SaveChanges();
            result.code = 1;
            result.message = "删除成功";
            result.data = null;
            return result;
        }

        // PUT vip
        // api/vip
        [HttpPut]
        public ReturnMessage VipPut(dynamic _in)
        {
            ReturnMessage result = new ReturnMessage();
            string inUid = _in.vip_id;
            int inLength = _in.surplus;
            int inLevel = _in.vip_level;

            var sVip = from m in dbContext.vip_data
                       where m.vip_id == inUid
                       select m;
            // 用户不是vip
            if(sVip.FirstOrDefault() == null)
            {
                result.code = 0;
                result.message = "用户不是vip";
                return result;
            }

            // 修改成功
            var newData = sVip.First();
            DateTime startTime = Convert.ToDateTime(newData.begin_time);
            startTime = startTime.AddDays(inLength);
            newData.end_time = string.Format("{0:yyyy-MM-dd}", startTime);
            newData.vip_level = inLevel;
            dbContext.vip_data.Update(newData);
            dbContext.SaveChanges();
            result.code = 1;
            result.message = "修改成功";
            return result;
        }
    }
}