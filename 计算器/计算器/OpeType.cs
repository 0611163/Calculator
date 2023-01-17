using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 计算器
{
    /// <summary>
    /// 运算符类型
    /// </summary>
    enum OpeType
    {
        /// <summary>
        /// 该运算因子不是运算符
        /// </summary>
        notOperator,
        /// <summary>
        /// 常数或变量(没有参数的运算符)
        /// </summary>
        noParameter,
        /// <summary>
        /// '+'或'-'
        /// 这两个运算符即可以当正负号使用又可以当加减运算符使用
        /// </summary>
        PlusOrMinus,
        /// <summary>
        /// 参数全在左边的运算符
        /// </summary>
        left,
        /// <summary>
        /// 参数全在右边的运算符
        /// </summary>
        right,
        /// <summary>
        /// 左边和右边都有参数的运算符
        /// </summary>
        bothSides,
        /// <summary>
        /// 左括号
        /// </summary>
        leftParenthesis,
        /// <summary>
        /// 右括号
        /// </summary>
        rightParenthesis,
        /// <summary>
        /// 分隔符，这里指逗号
        /// </summary>
        Separator
    }
}
