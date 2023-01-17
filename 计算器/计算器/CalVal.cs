////////////////////////////////
//创建日期：2010年12月19日
//2012年7月27日
//增加字段primalPos
////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 计算器
{
    /// <summary>
    /// 运算因子
    /// 包括运算符、函数、常数、变量、其他符号、运算符的操作数、函数的参数等
    /// </summary>
    class CalVal
    {
        /// <summary>
        /// 数字
        /// 包括运算符的操作数或函数的参数
        /// </summary>
        public CalNum num;
        /// <summary>
        /// 运算符
        /// </summary>
        public CalOpe ope;
        /// <summary>
        /// 运算因子类型(数字、运算符)
        /// </summary>
        public ValType type;
        /// <summary>
        /// 运算因子优先级
        /// </summary>
        public int level;
        /// <summary>
        /// 运算因子在运算因子列表中的原始位置（规范化后）
        /// </summary>
        public int primalPos;//当计算出错时用于定位错误

        /// <summary>
        /// 运算因子的运算符类型
        /// 若运算因子为数字，则返回notOperator
        /// </summary>
        public OpeType OpeType
        {
            get
            {
                if (this.type == ValType.Operator)
                {
                    return ope.opeType;
                }
                else
                {
                    return OpeType.notOperator;
                }
            }
        }

        /// <summary>
        /// 运算因子的参数个数
        /// 若运算因子为数字，则返回－2
        /// </summary>
        public int ParameterNumber
        {
            get
            {
                if (type == ValType.Operator)
                {
                    return ope.parameterNumber;
                }
                else
                {
                    return -2;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CalVal(CalNum num)
        {
            this.num = num;
            this.type = ValType.Number;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CalVal(CalOpe ope)
        {
            this.ope = ope;
            this.type = ValType.Operator;
            this.level = ope.level;
        }
    }
}
