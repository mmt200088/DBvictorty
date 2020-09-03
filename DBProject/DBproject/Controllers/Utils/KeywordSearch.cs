using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBproject.Controllers.Utils
{
    public class KeywordSearch
    {
        static public bool ContainsKeywords(string content,string keywords)
        {
            //搜索字符串keywords按空格分为多个关键词
            var kws = keywords.Split(" ");
            foreach(var kw in kws)
            {
                if (content.Contains(kw))
                    return true;
            }
            return false;
        }
    }
}
