using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 计算器
{
    class CalResult
    {
        /// <summary>
        /// 数字列表
        /// </summary>
        public List<CalNum> numList = new List<CalNum>();
        /// <summary>
        /// 结果个数
        /// </summary>
        public int resultCount
        {
            get
            {
                return this.numList.Count;
            }
        }

        /// <summary>
        /// 用来显示的普通结果
        /// </summary>
        public string GeneralResultToShow
        {
            get
            {
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < resultCount; i++)
                {
                    str.Append(numList[i].GeneralResultToShow);
                    if (i != resultCount - 1)
                    {
                        str.Append(',');
                    }
                }
                return str.ToString();
            }
        }

        /// <summary>
        /// 用来显示的科学计数法结果
        /// </summary>
        public string ScientificResultToShow
        {
            get
            {
                StringBuilder str = new StringBuilder();
                for (int i = 0; i < resultCount; i++)
                {
                    str.Append(numList[i].ScientificResultToShow);
                    if (i != resultCount - 1)
                    {
                        str.Append(',');
                    }
                }
                return str.ToString();
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CalResult()
        {
        }
    }
}
