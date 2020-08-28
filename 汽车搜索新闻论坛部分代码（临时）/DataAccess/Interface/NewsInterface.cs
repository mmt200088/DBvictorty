using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebApplication10.Models;
using WebApplication10.DataAccess.Base;

namespace WebApplication10.DataAccess.Interface
{
    

    public interface NewsInterface
    {
        //插入一条新闻
        bool CreateNews(news nws);

        //根据新闻id删除一条新闻
        bool DeleteNewsByID(string nws_id);

        //根据一定条件搜索一些新闻
        IEnumerable<news> GetNews(newsSearchFilter filter);
    }
}
