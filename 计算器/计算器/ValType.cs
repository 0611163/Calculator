using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 计算器
{
    /// <summary>
    /// 运算因子(数字及运算符)类型
    /// </summary>
    enum ValType
    {
        /// <summary>
        /// 数字
        /// </summary>
        Number,

        /// <summary>
        /// 运算符、函数、常数等
        /// </summary>
        Operator
    }
}
