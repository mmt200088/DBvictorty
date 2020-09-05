using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using DBproject.Controllers.Utils;
using DBproject.context;
using DBproject.Model;

namespace DBproject.Controllers
{
    //用于展示的新闻信息
    public class NewsForShow
    {
        public string news_id { get; set; }

        public string title { get; set; }

        public string post_date { get; set; }

        public string author_id { get; set; }

        public string author_name { get; set; }

        public string content { get; set; }

        public string part { get; set; }
    }
    //一页新闻
    public class NewsPage
    {
        public int total{ get; set; }
        public int page_num { get; set; }
        public NewsForShow[] newsSet { get; set; }
    }
    [Route("api/[controller]")]
    [ApiController]
    public class newsController : ControllerBase
    {
        private readonly coreDbContext _context;

        public newsController(coreDbContext context)
        {
            _context = context;
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

                string news_id = _in.news_id;
                var newss = from row in _context.News
                             where row.news_id == news_id
                             select row;
                var news = newss.FirstOrDefault();
                if (news != null)
                {
                    throw (new Exception("该对象ID已存在"));
                }

                var nws = new news()
                {
                    author_name = _in.author_name,
                    author_id = _in.author_id,
                    content = _in.content,
                    news_id = _in.news_id,
                    part = _in.part,
                    post_date = _in.post_date,
                    title = _in.title,
                    reader_num = 0,
                    craze = 0
                };

                _context.News.Add(nws);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("插入到数据库失败"));
                }

                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(1, "成功发布新闻");
                return new JsonResult(rr);
            }
            catch(Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(0, exc.Message);
                return new JsonResult(rr);
            }
            
        }

        //输入：json，删除的信息的主码
        //输出：json，展示是否正确删除
        // POST: api/news/delete
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(string news_id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:" + ModelState.ToString()));
                }

                var newss = from row in _context.News
                            where row.news_id == news_id
                            select row;
                var news = newss.FirstOrDefault();
                if (news == null)
                {
                    throw (new Exception("未找到该对象"));
                }

                _context.News.Remove(news);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("数据库删除操作失败"));
                }
               
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(1, "成功删除新闻");
                return new JsonResult(rr);
            }
            catch (Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(0, exc.Message);
                return new JsonResult(rr);
            }
        }

        //输入：json，搜索条件
        //输出：json，展示新闻信息
        // POST: api/news/get
        [HttpPost("get")]
        public IActionResult Get(string keyword, string author_id)
        {
            try
            {
                bool isAuthorId = true;
                if (author_id == "")
                    isAuthorId = false;
                var newsList = from news in _context.News
                               where (KeywordSearch.ContainsKeywords(news.title+" "+ news.content, keyword))&&
                                     ((!isAuthorId)||news.author_id==author_id)
                               orderby news.post_date descending
                               select news;

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
                        author_name = nwsrow.author_name,
                        author_id = nwsrow.author_id,
                        content = contract_content,
                        news_id = nwsrow.news_id,
                        part = nwsrow.part,
                        post_date = nwsrow.post_date.ToString(),
                        title = nwsrow.title
                    };
                    showList.Add(temp);
                }

                RestfulResult.RestfulArray<NewsForShow> rr = new RestfulResult.RestfulArray<NewsForShow>();
                rr.code = 1;
                rr.message = "成功查询到结果";
                rr.data = showList.ToArray();
                return new JsonResult(rr);
            }
            catch (Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(0, exc.Message);
                return new JsonResult(rr);
            }
        }

        // GET: api/news/getNews
        [HttpGet("getNews")]
        public async Task<IActionResult> Getnews(string news_id)
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
                    author_name = news.author_name,
                    author_id = news.author_id,
                    content = news.content,
                    news_id = news.news_id,
                    part = news.part,
                    post_date = news.post_date.ToString(),
                    title = news.title
                };
                RestfulResult.RestfulData<NewsForShow> rr = new RestfulResult.RestfulData<NewsForShow>();
                rr.code = 1;
                rr.message = "成功查找新闻";
                rr.data = newsshow;
                return new JsonResult(rr);
            }
            catch (Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(0, exc.Message);
                return new JsonResult(rr);
            }
           
        }

        // GET: api/news/get_pageNews
        [HttpGet("get_pageNews")]
        public IActionResult Get_pageNews(dynamic _in)
        {
            try
            {               
                List<NewsForShow> showList = NewsSearch(_in.keyword, _in.author_id);              
                int startIndex = (_in.page_num - 1) * _in.page_size;
                int endIndex = _in.page_num * _in.page_size;
                List<NewsForShow> pageList;
                if (showList.Count() < startIndex - 1)
                    throw (new Exception("超出范围"));
                else if (showList.Count() < endIndex)
                    pageList = showList.GetRange(startIndex, showList.Count() - startIndex);
                else
                    pageList = showList.GetRange(startIndex, _in.page_size);

                var resultData = new NewsPage
                {
                    page_num = _in.page_num,
                    total = showList.Count(),
                    newsSet=pageList.ToArray()
                };
                var rr=new RestfulResult.RestfulData<NewsPage>();
                rr.code = 1;
                rr.message = "成功获取新闻页";
                rr.data = resultData;
                return new JsonResult(rr);
            }
            catch (Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(0, exc.Message);
                return new JsonResult(rr);
            }
        }
        private bool IsNewsExists(string id)
        {
            return _context.News.Any(e => e.news_id == id);
        }
        private List<NewsForShow> NewsSearch(string keyword,string author_id)
        {
            bool isAuthorId = true;
            if (author_id == "")
                isAuthorId = false;
            var newsList = from news in _context.News
                           where (news.title.Contains(keyword) || news.content.Contains(keyword)) &&
                           ((!isAuthorId) || news.author_id == author_id)
                           orderby news.post_date descending
                           select news;

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
                    author_name = nwsrow.author_name,
                    author_id = nwsrow.author_id,
                    content = contract_content,
                    news_id = nwsrow.news_id,
                    part = nwsrow.part,
                    post_date = nwsrow.post_date.ToString(),
                    title = nwsrow.title
                };
                showList.Add(temp);
            }
            return showList;
        }
    }
}