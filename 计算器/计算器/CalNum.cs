using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 计算器
{
    /// <summary>
    /// 数字
    /// 包括运算符的操作数或函数的参数
    /// </summary>
    class CalNum
    {
        /// <summary>
        /// double值
        /// </summary>
        private double doubleValue = double.NaN;
        /// <summary>
        /// string值
        /// </summary>
        public string stringValue = string.Empty;
        /// <summary>
        /// 数字类型
        /// </summary>
        public NumType numType;

        /// <summary>
        /// double值
        /// </summary>
        public double DoubleValue
        {
            get
            {
                if (double.IsNaN(doubleValue))
                {
                    if (stringValue.IndexOf(',') == -1)//防止如9,9转换成99，逗号已作为参数分隔符和算式分隔符使用
                    {
                        return double.Parse(stringValue);
                    }
                    else
                    {
                        return doubleValue;
                    }
                }
                else
                {
                    return doubleValue;
                }
            }
        }

        /// <summary>
        /// 用来显示的普通结果
        /// </summary>
        public string GeneralResultToShow
        {
            get
            {
                if (this.numType == NumType.both
                    || double.IsNaN(this.doubleValue))
                {
                    string[] str = this.stringValue.Split('|');
                    StringBuilder result = new StringBuilder();
                    for (int i = 0; i < str.Length; i++)
                    {
                        double d;
                        if (double.TryParse(str[i], out d))
                        {
                            result.Append(Common.ResultProcess(str[i].Replace(" ", "")));
                        }
                        else
                        {
                            result.Append(str[i]);
                        }
                    }
                    return result.ToString();
                }
                else
                {
                    return Common.ResultProcess(this.stringValue.Replace(" ", ""));
                }
            }
        }

        /// <summary>
        /// 用来显示的科学计数法结果
        /// </summary>
        public string ScientificResultToShow
        {
            get
            {
                if (this.numType == NumType.both
                    || double.IsNaN(this.doubleValue))
                {
                    string[] str = this.stringValue.Split('|');
                    StringBuilder result = new StringBuilder();
                    for (int i = 0; i < str.Length; i++)
                    {
                        double d;
                        if (double.TryParse(str[i], out d))
                        {
                            result.Append(Common.ToScientific(d));
                        }
                        else
                        {
                            result.Append(str[i]);
                        }
                    }
                    return result.ToString();
                }
                else
                {
                    return Common.ToScientific(this.doubleValue);
                }
            }
        }

        #region 构造函数
        //执行了构造函数之后，doubleValue可能是不存在的，但stringValue一定存在
        /// <summary>
        /// 构造函数
        /// </summary>
        public CalNum(string stringValue)
        {
            this.stringValue = stringValue;
            if (stringValue.IndexOf(',') == -1)//防止如9,9转换成99，逗号已作为参数分隔符和算式分隔符使用
            {
                double d;
                if (double.TryParse(stringValue.Replace(" ", ""), out d))
                {
                    this.doubleValue = d;
                }
            }
            this.numType = NumType.stringValue;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CalNum(double doubleValue)
        {
            this.doubleValue = doubleValue;
            this.stringValue = doubleValue.ToString();
            this.numType = NumType.doubleValue;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CalNum(double doubleValue, string stringValue)
        {
            this.doubleValue = doubleValue;
            this.stringValue = stringValue;
            this.numType = NumType.both;
        }
        #endregion

    }
}
