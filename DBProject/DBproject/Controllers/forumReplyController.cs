using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using DBproject.Controllers.Utils;
using DBproject.context;
using DBproject.Model;

namespace DBproject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class forumReplyController : ControllerBase
    {
        private readonly coreDbContext _context;

        public forumReplyController(coreDbContext context)
        {
            _context = context;
        }

        //POST:api/forumReply/add
        [HttpPost("add")]
        public async Task<IActionResult> Add(dynamic _in)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:" + ModelState.ToString()));
                }

                string reply_id = _in.reply_id;
                var replys = from row in _context.Reply_data
                             where row.reply_id == reply_id
                             select row;
                var reply = replys.FirstOrDefault();
                if (reply != null)
                {
                    throw (new Exception("该对象ID已存在"));
                }

                var newreply = new reply_data()
                {
                    content = _in.content,
                    reply_date= _in.reply_date,
                    reply_id=_in.reply_id,
                    post_id = _in.post_id,
                    user_id = _in.user_id
                };

                _context.Reply_data.Add(newreply);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("插入到数据库失败"));
                }

                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(1, "成功发布回复贴");
                return new JsonResult(rr);
            }
            catch (Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(0, exc.Message);
                return new JsonResult(rr);
            }
        }

        //POST:api/forumReply/delete
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(string reply_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:" + ModelState.ToString()));
                }

                var replys = from row in _context.Reply_data
                            where row.reply_id == reply_id
                            select row;
                var reply = replys.FirstOrDefault();
                if (reply == null)
                {
                    throw (new Exception("未找到此对象"));
                }

                _context.Reply_data.Remove(reply);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("数据库删除操作失败"));
                }

                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(1, "成功删除回复贴");
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