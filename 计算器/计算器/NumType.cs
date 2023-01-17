using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 计算器
{
    enum NumType
    {
        /// <summary>
        /// 主类型为double型
        /// </summary>
        doubleValue,
        /// <summary>
        /// 主类型string型
        /// </summary>
        stringValue,
        /// <summary>
        /// 同时存储了double值和string值
        /// ，但存储的double值和string值不可相互转换
        /// </summary>
        both
    }
}
