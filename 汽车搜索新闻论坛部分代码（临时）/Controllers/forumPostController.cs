using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApplication10.Controllers.Utils;
using WebApplication10.DataAccess.Base;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class forumPostController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public forumPostController(CoreDbContext context)
        {
            _context = context;
        }

        //POST:api/forumPost/add
        [HttpPost("add")]
        public async Task<IActionResult> Add(dynamic _in)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:" + ModelState.ToString()));
                }

                var newpost = new post_data()
                {
                    content=_in.content,
                    title=_in.title,
                    post_date=_in.post_date,
                    post_id=_in.post_id,
                    user_id=_in.user_id
                };

                _context.Post_data.Add(newpost);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("插入到数据库失败"));
                }

                RestfulResult.RestfulData rr = new RestfulResult.RestfulData();
                rr.Code = 1;
                rr.Message = "成功发布主题贴";
                return new JsonResult(rr);
            }
            catch (Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData();
                rr.Code = 0;
                rr.Message = exc.Message;
                return new JsonResult(rr);
            }
        }

        //POST:api/forumPost/delete
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromRoute]string post_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:" + ModelState.ToString()));
                }

                var post = await _context.Post_data.FindAsync(post_id);
                if (post == null)
                {
                    throw (new Exception("未找到待删除的对象"));
                }

                _context.Post_data.Remove(post);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("数据库删除操作失败"));
                }

                //还应该删除所有回复贴
                //数据库级联删除自动完成

                RestfulResult.RestfulData rr = new RestfulResult.RestfulData();
                rr.Code = 1;
                rr.Message = "成功删除主题贴";
                return new JsonResult(rr);
            }
            catch (Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData();
                rr.Code = 0;
                rr.Message = exc.Message;
                return new JsonResult(rr);
            }
        }
    }
}