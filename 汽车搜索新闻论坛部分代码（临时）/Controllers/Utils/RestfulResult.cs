using System;
using System.Collections.Generic;

namespace WebApplication10.Controllers.Utils
{
    static public class RestfulResult
    {
        public class RestfulData
        {
            public RestfulData()
            {
            }

            public RestfulData(int v1, string v2)
            {
                this.code = v1;
                this.message = v2;
            }

            public int code { get; set; }

            public string message { get; set; }

        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class RestfulData<T> : RestfulData
        {
            /// <summary>
            /// <![CDATA[数据]]>
            /// </summary>
            public virtual T data { get; set; }
        }

        /// <summary>
        /// <![CDATA[返回数组]]>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public class RestfulArray<T> : RestfulData<IEnumerable<T>>
        {

        }
    }
}
