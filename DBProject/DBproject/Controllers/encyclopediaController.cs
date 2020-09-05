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
    public class EncyclopediaForShow
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string post_date { get; set; }
        public string author_id { get; set; }
        public string author_name { get; set; }
        public string content { get; set; }
        public string partition { get; set; }
    }
    [Route("api/[controller]")]
    public class encyclopediaController : Controller
    {
        private readonly coreDbContext _context;

        public encyclopediaController(coreDbContext context)
        {
            _context = context;
        }

        //输入：json，添加的信息
        //输出：json，展示是否正确添加
        // POST: api/encyclopedia/add
        [HttpPost("add")]
        public async Task<IActionResult> Add(dynamic _in)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:" + ModelState.ToString()));
                }

                var pedia = new encyclopedia()
                {
                    author_name = _in.author_name,
                    author_id = _in.author_id,
                    content = _in.content,
                    ID = _in.ID,
                    partition = _in.partition,
                    post_date =_in.post_date,
                    title = _in.title,
                    reader_num = 0
                };

                _context.Encyclopedia.Add(pedia);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("插入到数据库失败"));
                }

                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(1, "成功发布百科");
                return new JsonResult(rr);
            }
            catch (Exception exc)
            {
                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(0, exc.Message);
                return new JsonResult(rr);
            }

        }

        //输入：json，删除的信息的主码
        //输出：json，展示是否正确删除
        // DELETE: api/encyclopedia/delete
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromRoute]string ID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:" + ModelState.ToString()));
                }

                var pedia = await _context.Encyclopedia.FindAsync(ID);
                if (pedia == null)
                {
                    throw (new Exception("未找到待删除的对象"));
                }

                _context.Encyclopedia.Remove(pedia);
                if (!(await _context.SaveChangesAsync() > 0))
                {
                    throw (new Exception("数据库删除操作失败"));
                }

                RestfulResult.RestfulData rr = new RestfulResult.RestfulData(1, "成功删除百科");
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
        // POST: api/encyclopedia/get
        [HttpPost("get")]
        public IActionResult Get([FromRoute]string keyword)
        {
            try
            {
                var pediaList = from pedia in _context.Encyclopedia
                               where KeywordSearch.ContainsKeywords(pedia.title+" "+ pedia.content, keyword)
                               orderby pedia.post_date descending
                               select pedia;

                if (pediaList.Count() == 0)
                {
                    throw (new Exception("没有找到满足条件的数据"));
                }
                List<EncyclopediaForShow> showList = new List<EncyclopediaForShow>();
                string contract_content = new string("");
                foreach (var nwsrow in pediaList)
                {
                    if (nwsrow.content.Count() > 50)
                        contract_content = nwsrow.content.Substring(0, 50) + "...";
                    else
                        contract_content = nwsrow.content;
                    EncyclopediaForShow temp = new EncyclopediaForShow()
                    {
                        author_name = nwsrow.author_name,
                        author_id = nwsrow.author_id,
                        content = contract_content,
                        ID = nwsrow.ID,
                        partition = nwsrow.partition,
                        post_date = nwsrow.post_date.ToString(),
                        title = nwsrow.title
                    };
                    showList.Add(temp);
                }

                var rr = new RestfulResult.RestfulArray<EncyclopediaForShow>();
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

        // GET: api/encyclopedia/getOne
        [HttpGet("getOne")]
        public async Task<IActionResult> GetOne([FromRoute] string ID)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw (new Exception("Bad Request,ModelState:" + ModelState.ToString()));
                }

                var pedia = await _context.Encyclopedia.FindAsync(ID);

                if (pedia == null)
                {
                    throw (new Exception("没有找到满足条件的数据"));
                }
                var pediashow = new EncyclopediaForShow()
                {
                    author_name = pedia.author_name,
                    author_id = pedia.author_id,
                    content = pedia.content,
                    ID = pedia.ID,
                    partition = pedia.partition,
                    post_date = pedia.post_date.ToString(),
                    title = pedia.title
                };
                var rr = new RestfulResult.RestfulData<EncyclopediaForShow>();
                rr.code = 1;
                rr.message = "成功查找新闻";
                rr.data = pediashow;
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
