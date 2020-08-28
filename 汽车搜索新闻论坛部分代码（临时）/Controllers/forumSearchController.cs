using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using WebApplication10.Controllers.Utils;
using Microsoft.EntityFrameworkCore;
using WebApplication10.DataAccess.Base;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    public class PostOrReplyForShow
    {
        public string post_id { get; set; }   
        public string title { get; set; }
        //摘要
        public string content { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string date { get; set; }
        public bool isPost { get; set; }
    }
    public class PostForShow
    {
        public string post_id { get; set; }
        public string title { get; set; }
        //完整
        public string content { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string post_date { get; set; }
    }
    public class ReplysForShow
    {
        public string reply_id { get; set; }
        //完整
        public string content { get; set; }
        public string user_id { get; set; }
        public string user_name { get; set; }
        public string reply_date { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class forumSearchController : ControllerBase
    {
        private readonly CoreDbContext _context;

        public forumSearchController(CoreDbContext context)
        {
            _context = context;
        }

        //POST:api/forumSearch/get
        [HttpPost("get")]
        public IActionResult Get(dynamic _in)
        {
            try
            {
                //搜索条件
                string keyword = _in.keyword;
                bool isOnlyPost = _in.isOnlyPost;
                string user_id = _in.user_id;
                //获得表单数据
                var postList = from post in _context.Post_data
                               join user in _context.User_data
                                 on post.user_id equals user.user_id
                               where (post.content.Contains(keyword)) &
                                     (post.user_id == user_id)
                               orderby post.post_date descending
                               select new { post, user };

                var replyList = from reply in _context.Reply_data
                                join user in _context.User_data
                                  on reply.user_id equals user.user_id
                                where (!isOnlyPost) &
                                      (reply.content.Contains(keyword)) &
                                      (reply.user_id == user_id)
                                orderby reply.reply_date descending
                                select new { reply, user };
                //没找到数据的异常
                if (postList.Count() + replyList.Count() == 0)
                {
                    throw (new Exception("没找到满足要求的贴"));
                }
                //将表单数据转化为展示数据
                var showList = new List<PostOrReplyForShow>();
                var contract_content = new string("");
                foreach (var postRow in postList)
                {
                    if (postRow.post.content.Count() > 50)
                        contract_content = postRow.post.content.Substring(0, 50) + "...";
                    else
                        contract_content = postRow.post.content;
                    var temp = new PostOrReplyForShow
                    {
                        content = contract_content,
                        date = postRow.post.post_date.ToString(),
                        isPost = true,
                        post_id = postRow.post.post_id,
                        title = postRow.post.title,
                        user_id = postRow.post.user_id,
                        user_name = postRow.user.user_name
                    };
                    showList.Add(temp);
                }
                foreach (var replyRow in replyList)
                {
                    if (replyRow.reply.content.Count() > 50)
                        contract_content = replyRow.reply.content.Substring(0, 50) + "...";
                    else
                        contract_content = replyRow.reply.content;
                    var title = _context.Post_data.Find(replyRow.reply.post_id).title;
                    var temp = new PostOrReplyForShow
                    {
                        content = contract_content,
                        date = replyRow.reply.reply_date.ToString(),
                        isPost = true,
                        post_id = replyRow.reply.post_id,
                        title = title,
                        user_id = replyRow.reply.user_id,
                        user_name = replyRow.user.user_name
                    };
                    showList.Add(temp);
                }
                //整理数据
                showList.Sort((x, y) =>
                {
                    if (Convert.ToDateTime(x.date) > Convert.ToDateTime(x.date))
                        return -1;
                    else
                        return 1;
                });
                //返回正确结果
                RestfulResult.RestfulArray < PostOrReplyForShow > rr= new RestfulResult.RestfulArray<PostOrReplyForShow>();
                rr.Code = 1;
                rr.Message = "成功搜索到目标贴";
                rr.Data = showList.ToArray();
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

        //POST:api/forumSearch/getPost
        [HttpPost("getPost")]
        public IActionResult GetPost([FromRoute] string post_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:" + ModelState.ToString()));
                }

                var post = _context.Post_data.Find(post_id);

                if (post == null)
                {
                    throw (new Exception("没有找到满足条件的数据"));
                }
                //
                var user = _context.User_data.Find(post.user_id);
                var postshow = new PostForShow
                {
                    content = post.content,
                    post_date = post.post_date.ToString(),
                    post_id = post.post_id,
                    title = post.title,
                    user_id = post.user_id,
                    user_name = user.user_name
                };

                RestfulResult.RestfulData<PostForShow> rr = new RestfulResult.RestfulData<PostForShow>();
                rr.Code = 1;
                rr.Message = "成功查找新闻";
                rr.Data = postshow;
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

        //POST:api/forumSearch/get
        [HttpPost("getReplys")]
        public IActionResult GetReplys([FromRoute] string post_id)
        {
            try
            {
                var replyList = from reply in _context.Reply_data
                                join user in _context.User_data
                                  on reply.user_id equals user.user_id
                                where reply.post_id == post_id
                                  orderby reply.reply_date ascending
                                select new { reply, user };
                if (replyList.Count() == 0)
                {
                    throw (new Exception("没找到回复贴"));
                }
                var showList = new List<ReplysForShow>();
                foreach(var replyRow in replyList)
                {
                    var temp = new ReplysForShow
                    {
                        content = replyRow.reply.content,
                        reply_date = replyRow.reply.reply_date.ToString(),
                        reply_id = replyRow.reply.reply_id,
                        user_id = replyRow.reply.user_id,
                        user_name = replyRow.user.user_name
                    };
                    showList.Add(temp);
                }

                RestfulResult.RestfulArray < ReplysForShow > rr = new RestfulResult.RestfulArray<ReplysForShow>();
                rr.Code = 1;
                rr.Message = "成功查询到结果";
                rr.Data = showList.ToArray();
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