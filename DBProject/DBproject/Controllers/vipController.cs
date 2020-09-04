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
        [HttpGet("{uid}")]
        public dataReturnMessage Vip(string uid)
        {
            dataReturnMessage result = new dataReturnMessage();

            // 用户id不存在
            var sUid = from m in dbContext.user_data
                       where m.user_id == uid
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
                       where m.vip_id == uid
                       select m;
            if (sVipId.FirstOrDefault() == null)
            {
                result.code = 0;
                result.message = "用户不是vip";
                result.data = null;
                return result;
            }
            vip_data vip = sVipId.First();
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
            var sPer = from m in dbContext.vip_permission
                       where m.vip_level == vip.vip_level
                       select m;
            vip_permission vipPer = sPer.First();
            // 查询成功
            result.code = 1;
            result.message = "查询成功";
            vipReturnData data = new vipReturnData { vip_id = uid, end_time = vip.end_time, vip_level = vip.vip_level, permission = vipPer.permissions };
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

            // 之前不是vip
            double length = _in.length;
            if (sVip.FirstOrDefault() == null)
            {
                DateTime nowTime = DateTime.Now;
                vip_data newVal = new vip_data { vip_id = inUid, vip_level = inLevel };
                newVal.begin_time = string.Format("{0:yyyy-MM-dd}", nowTime);
                nowTime = nowTime.AddDays(length);
                newVal.end_time = string.Format("{0:yyyy-MM-dd}", nowTime);

                dbContext.vip_data.Add(newVal);
                dbContext.SaveChanges();

                result.code = 1;
                result.message = "充值成功";
                return result;
            }
            // 用户已经是vip
            else
            {
                vip_data nowUser = sVip.First();
                // 同等级续时长
                if (nowUser.vip_level == inLevel) {
                    DateTime endTime = Convert.ToDateTime(nowUser.end_time);
                    endTime = endTime.AddDays(length);
                    nowUser.end_time = string.Format("{0:yyyy-MM-dd}", endTime);

                    dbContext.vip_data.Update(nowUser);
                    dbContext.SaveChanges();

                    result.code = 1;
                    result.message = "充值成功";
                    return result;
                }
                // 充值等级比现等级低
                else if(nowUser.vip_level > inLevel)
                {
                    result.code = 0;
                    result.message = "充值等级过低";
                    return result;
                }
                // vip升级  原时间归零 时间重新算
                else
                {
                    // vip等级不存在
                    var sLevel = from m in dbContext.vip_permission
                                 where m.vip_level == inLevel
                                 select m;
                    if(sLevel.FirstOrDefault() == null)
                    {
                        result.code = 0;
                        result.message = "vip等级无效";
                        return result;
                    }
                    DateTime nowTime = DateTime.Now;
                    nowUser.vip_level = inLevel;
                    nowUser.begin_time = string.Format("{0:yyyy-MM-dd}", nowTime);
                    nowTime = nowTime.AddDays(length);
                    nowUser.end_time = string.Format("{0:yyyy-MM-dd}", nowTime);

                    dbContext.vip_data.Update(nowUser);
                    dbContext.SaveChanges();

                    result.code = 1;
                    result.message = "充值成功";
                    return result;
                }
            }
        }
    }
}