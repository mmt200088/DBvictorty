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
    public class uidController : ControllerBase
    {
        private coreDbContext dbContext;
        public uidController(coreDbContext _context)
        {
            dbContext = _context;
        }

        // GET uid
        // api/uid/123456
        [HttpGet("{uid}")]
        public dataReturnMessage Uid(string uid)
        {
            dataReturnMessage result = new dataReturnMessage();

            // 用户id不存在
            var sUid = from m in dbContext.user_data
                       where m.user_id == uid
                       select m;
            if(sUid.FirstOrDefault() == null)
            {
                result.code = 0;
                result.message = "用户id不存在";
                result.data = null;
                return result;
            }

            // 查询成功
            result.data = sUid.First();
            result.code = 1;
            result.message = "查询成功";
            return result;
        }

        // PUT uid
        // api/uid
        [HttpPut]
        public ReturnMessage UidPut(dynamic _in)
        {
            ReturnMessage result = new ReturnMessage();
            string inUid = _in.user_id;
            string inName = _in.user_name;

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

            // 用户昵称重复
            var sName = from m in dbContext.user_data
                        where m.user_name == inName
                        select m;
            if(sName.FirstOrDefault() != null)
            {
                if(sName.First().user_id != inUid)
                {
                    result.code = 0;
                    result.message = "用户昵称重复";
                    return result;
                }
            }

            // 修改成功
            user_data newData = sUid.First();
            newData.password = _in.password;
            newData.user_name = inName;
            newData.gender = _in.gender;
            newData.city = _in.city;
            dbContext.user_data.Update(newData);
            dbContext.SaveChanges();

            result.code = 1;
            result.message = "修改成功";
            return result;
        }

        // DELETE uid
        // api/uid/123456
        [HttpDelete("{uid}")]
        public dataReturnMessage UidDelete(string uid)
        {
            dataReturnMessage result = new dataReturnMessage();
            var sUid = from m in dbContext.user_data
                       where m.user_id == uid
                       select m;
            //用户不存在
            if(sUid.FirstOrDefault() == null)
            {
                result.code = 0;
                result.message = "用户不存在";
                result.data = null;
                return result;
            }

            var oldData = sUid.First();
            //删除用户
            dbContext.user_data.Remove(oldData);
            dbContext.SaveChanges();
            result.code = 1;
            result.message = "删除成功";
            result.data = null;
            return result;
        }
    }
}