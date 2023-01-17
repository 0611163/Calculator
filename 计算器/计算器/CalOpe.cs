////////////////////////////////
//创建日期：2010年12月19日
////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 计算器
{
    /// <summary>
    /// 运算符类
    /// 包括运算符、函数
    /// 、常数(看作参数个数为0的函数)、变量(看作参数个数为0的函数)
    /// 、其他符号(括号、分隔符等)
    /// </summary>
    class CalOpe : IComparable
    {
        /// <summary>
        /// 运算符的标识符
        /// </summary>
        public string tag;
        /// <summary>
        /// 运算符的优先级
        /// </summary>
        public int level;
        /// <summary>
        /// 运算符参数个数，值为-1表示参数个数不确定，值为-2表示该运算因子不是运算符
        /// </summary>
        public int parameterNumber;
        /// <summary>
        /// 运算符类型
        /// </summary>
        public OpeType opeType;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tag">标识符</param>
        /// <param name="level">优先级</param>
        /// <param name="parameterNumber">参数个数
        /// ，值为-1表示参数个数不确定，值为-2表示该运算因子不是运算符</param>
        /// <param name="opeType">运算符类型</param>
        public CalOpe(string tag, int level, int parameterNumber, OpeType opeType)
        {
            this.tag = tag;
            this.level = level;
            this.parameterNumber = parameterNumber;
            this.opeType = opeType;
        }

        /// <summary>
        /// 实现IComparable接口
        /// </summary>
        public int CompareTo(object obj)
        {
            int result;
            CalOpe calOpe = (CalOpe)obj;
            if (this.tag.Length > calOpe.tag.Length)
            {
                result = 1;
            }
            else if (this.tag.Length == calOpe.tag.Length)
            {
                result = 0;
            }
            else
            {
                result = -1;
            }
            return -result;//取负数，可以实现按tag长度从大到小排序
        }
    }
}
