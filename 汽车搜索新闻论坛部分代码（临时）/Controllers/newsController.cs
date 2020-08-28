﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using WebApplication10.Controllers.Utils;
using WebApplication10.DataAccess.Base;
using WebApplication10.DataAccess.Implement;
using WebApplication10.Models;

namespace WebApplication10.Controllers
{
    //用于展示的新闻信息
    public class NewsForShow
    {
        public string news_id { get; set; }

        public string title { get; set; }

        //是否应该改为string？
        public string post_date { get; set; }

        public string author_id { get; set; }

        public string author { get; set; }

        public string content { get; set; }

        public string partition { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class newsController : ControllerBase
    {
        private readonly CoreDbContext _context;

        private readonly NewsClass _myclass;

        public newsController(CoreDbContext context)
        {
            _context = context;
            _myclass = new NewsClass(context);
        }

        //输入：json，添加的信息
        //输出：json，展示是否正确添加
        // POST: api/news/add
        [HttpPost("add")]
        public async Task<IActionResult> Add(dynamic _in)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:"+ModelState.ToString()));
                }

                var nws = new news()
                {
                    author = _in.author,
                    author_id = _in.author_id,
                    content = _in.content,
                    news_id = _in.news_id,
                    partition = _in.partition,
                    post_date = Convert.ToDateTime(_in.post_data),
                    title = _in.title,
                    reader_num=0,
                    craze=0
                };

                _context.News.Add(nws);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("插入到数据库失败"));
                }

                RestfulResult.RestfulData rr = new RestfulResult.RestfulData();
                rr.Code = 1;
                rr.Message = "成功发布新闻";
                return new JsonResult(rr);
            }
            catch(Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData();
                rr.Code = 0;
                rr.Message = exc.Message;
                return new JsonResult(rr);
            }
            
        }

        //输入：json，删除的信息的主码
        //输出：json，展示是否正确删除
        // DELETE: api/news/delete
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromRoute]string news_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:" + ModelState.ToString()));
                }

                var news = await _context.News.FindAsync(news_id);
                if (news == null)
                {
                    throw (new Exception("未找到待删除的对象"));
                }

                _context.News.Remove(news);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("数据库删除操作失败"));
                }
               
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData();
                rr.Code = 1;
                rr.Message = "成功删除新闻";
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

        //输入：json，搜索条件
        //输出：json，展示新闻信息
        // POST: api/news/get
        [HttpPost("get")]
        public IActionResult Get([FromRoute]string keyword, [FromRoute]string author_id = "")
        {
            try
            {
                var filter = new newsSearchFilter()
                {
                    keyword = keyword
                };
                if (author_id != "")
                    filter.authorID = author_id;
                IEnumerable<news> newsList = _myclass.GetNews(filter);
                if (newsList.Count() == 0)
                {
                    throw (new Exception("没有找到满足条件的数据"));
                }
                List<NewsForShow> showList = new List<NewsForShow>();
                string contract_content = new string("");
                foreach (news nwsrow in newsList)
                {
                    if (nwsrow.content.Count() > 50)
                        contract_content = nwsrow.content.Substring(0, 50) + "...";
                    else
                        contract_content = nwsrow.content;
                    NewsForShow temp = new NewsForShow()
                    {
                        author = nwsrow.author,
                        author_id = nwsrow.author_id,
                        content = contract_content,
                        news_id = nwsrow.news_id,
                        partition = nwsrow.partition,
                        post_date = nwsrow.post_date.ToString(),
                        title = nwsrow.title
                    };
                    showList.Add(temp);
                }
                //将结果按照时间从近到远排序
                showList.Sort((x, y) =>
                {
                    if (Convert.ToDateTime(x.post_date) > Convert.ToDateTime(y.post_date))
                        return -1;
                    else
                        return 1;
                });

                RestfulResult.RestfulArray<NewsForShow> rr = new RestfulResult.RestfulArray<NewsForShow>();
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

        // GET: api/news/getNews
        [HttpGet("getNews")]
        public async Task<IActionResult> Getnews([FromRoute] string news_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:" + ModelState.ToString()));
                }

                var news = await _context.News.FindAsync(news_id);

                if (news == null)
                {
                    throw (new Exception("没有找到满足条件的数据"));
                }
                NewsForShow newsshow = new NewsForShow()
                {
                    author = news.author,
                    author_id = news.author_id,
                    content = news.content,
                    news_id = news.news_id,
                    partition = news.partition,
                    post_date = news.post_date.ToString(),
                    title = news.title
                };
                RestfulResult.RestfulData<NewsForShow> rr = new RestfulResult.RestfulData<NewsForShow>();
                rr.Code = 1;
                rr.Message = "成功查找新闻";
                rr.Data = newsshow;
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


        private bool IsNewsExists(string id)
        {
            return _context.News.Any(e => e.news_id == id);
        }
    }
}