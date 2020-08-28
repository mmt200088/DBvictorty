using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication10.DataAccess.Interface;
using WebApplication10.Models;
using WebApplication10.DataAccess.Base;

namespace WebApplication10.DataAccess.Implement
{
    //搜索
    public class newsSearchFilter
    {
        //标题关键词，需要用到模式匹配算法
        //如果字符串为空，就是不做要求，按照算法也是匹配任何字符串
        public string keyword = string.Empty;

        //作者id
        //如果字符串为空，就是不做要求
        public string authorID = string.Empty;

        //搜索时间范围，距今最久多少天
        //如果不大于0，就是不做要求
        //public int maxLastDays=0;
    }
    public class NewsClass
    {
        public CoreDbContext Context;

        //构造函数 注入dbcontext
        public NewsClass(CoreDbContext context)
        {
            Context = context;
        }

        public delegate bool FilterTitle(news nws);
        public delegate bool FilterAID(news nws);
        public delegate bool FilterTime(news nws);
        //根据一定条件搜索一些新闻
        public IEnumerable<news> GetNews(newsSearchFilter filter)
        {
           
            FilterTitle fltr_ttl =s=>true;
            FilterTitle fltr_aid = s => true;
            if (filter.keyword != string.Empty)
            {
                fltr_ttl = s => s.title.Contains(filter.keyword)||
                s.content.Contains(filter.keyword);
            }
            if (filter.authorID != string.Empty)
            {
                fltr_ttl = s => s.author_id==filter.authorID;
            }
            var nwslist = Context.News.Where(s => fltr_ttl(s) && fltr_aid(s));
            return nwslist.ToList();
        }
    }
}
