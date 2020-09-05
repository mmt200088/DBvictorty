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
    public class forumPostController : ControllerBase
    {
        private readonly coreDbContext _context;

        public forumPostController(coreDbContext context)
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

                string post_id = _in.post_id;
                var posts = from row in _context.Post_data
                             where row.post_id == post_id
                             select row;
                var post = posts.FirstOrDefault();
                if (post != null)
                {
                    throw (new Exception("该对象ID已存在"));
                }

                var newpost = new post_data()
                {
                    content=_in.content,
                    title=_in.title,
                    post_date= _in.post_date,
                    post_id=_in.post_id,
                    user_id=_in.user_id
                };

                _context.Post_data.Add(newpost);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("插入到数据库失败"));
                }

                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(1, "成功发布主题贴");
                return new JsonResult(rr);
            }
            catch (Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(0, exc.Message);
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

                var posts = from row in _context.Post_data
                            where row.post_id == post_id
                            select row;
                var post = posts.FirstOrDefault();
                if (post == null)
                {
                    throw (new Exception("未找到此对象"));
                }

                _context.Post_data.Remove(post);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("数据库删除操作失败"));
                }

                //还应该删除所有回复贴
                //数据库级联删除自动完成

                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(1, "成功删除主题贴");
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