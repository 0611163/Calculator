////////////////////////////////////////////////////////
//该文件中有具体的运算符列表及它们的具体计算函数
//如果要添加新的运算符，请在该文件中修改
//2012年7月23日
//支持用户自定义函数计算功能
//2012年7月26日
//增加逻辑运算符
//2012年7月31日
//支持分段函数
////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace 计算器
{
    /// <summary>
    /// 运算符列表类，存储运算符(函数、常数、变量)列表，封装了具体运算符的计算功能
    /// </summary>
    class CalOpeList
    {
        /// <summary>
        /// 运算符个数
        /// </summary>
        public int count
        {
            get
            {
                return this.opeList.Count;
            }
        }

        /// <summary>
        /// 运算符列表
        /// </summary>
        public List<CalOpe> opeList;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CalOpeList()
        {
            #region 生成运算符列表
            opeList = new List<CalOpe>();

            //下面是运算符
            opeList.Add(new CalOpe("+", 97, 2, OpeType.PlusOrMinus));
            opeList.Add(new CalOpe("-", 97, 2, OpeType.PlusOrMinus));

            opeList.Add(new CalOpe("*", 98, 2, OpeType.bothSides));
            opeList.Add(new CalOpe("/", 98, 2, OpeType.bothSides));
            opeList.Add(new CalOpe("Mod", 98, 2, OpeType.bothSides));
            opeList.Add(new CalOpe("mod", 98, 2, OpeType.bothSides));//Mod首字母小写

            opeList.Add(new CalOpe("^", 99, 2, OpeType.bothSides));

            opeList.Add(new CalOpe("Exp", 100, 1, OpeType.right));//Exp首字母小写
            opeList.Add(new CalOpe("exp", 100, 1, OpeType.right));
            opeList.Add(new CalOpe("ln", 100, 1, OpeType.right));
            opeList.Add(new CalOpe("log", 100, -1, OpeType.right));
            opeList.Add(new CalOpe("sqrt", 100, -1, OpeType.right));

            opeList.Add(new CalOpe("sin", 100, 1, OpeType.right));
            opeList.Add(new CalOpe("cos", 100, 1, OpeType.right));
            opeList.Add(new CalOpe("tan", 100, 1, OpeType.right));
            opeList.Add(new CalOpe("asin", 100, 1, OpeType.right));
            opeList.Add(new CalOpe("acos", 100, 1, OpeType.right));
            opeList.Add(new CalOpe("atan", 100, 1, OpeType.right));
            opeList.Add(new CalOpe("sinh", 100, 1, OpeType.right));
            opeList.Add(new CalOpe("cosh", 100, 1, OpeType.right));
            opeList.Add(new CalOpe("tanh", 100, 1, OpeType.right));

            opeList.Add(new CalOpe("nCr", 100, 2, OpeType.right));
            opeList.Add(new CalOpe("ncr", 100, 2, OpeType.right));
            opeList.Add(new CalOpe("nAr", 100, 2, OpeType.right));
            opeList.Add(new CalOpe("nar", 100, 2, OpeType.right));

            opeList.Add(new CalOpe("!", 101, 1, OpeType.left));
            opeList.Add(new CalOpe("%", 101, 1, OpeType.left));
            opeList.Add(new CalOpe("度", 101, 1, OpeType.left));//角度转换成弧度
            opeList.Add(new CalOpe("toDegree", 100, 1, OpeType.right));//弧度转换成角度

            opeList.Add(new CalOpe("BIN", 102, 1, OpeType.left));//二进制转换成十进制
            opeList.Add(new CalOpe("OCT", 102, 1, OpeType.left));//八进制转换成十进制
            opeList.Add(new CalOpe("HEX", 102, 1, OpeType.left));//十六进制转换成十进制
            opeList.Add(new CalOpe("toB", 100, 1, OpeType.right));//十进制转换成二进制
            opeList.Add(new CalOpe("toO", 100, 1, OpeType.right));//十进制转换成八进制
            opeList.Add(new CalOpe("toH", 100, 1, OpeType.right));//十进制转换成十六进制

            opeList.Add(new CalOpe("(", 10000, 0, OpeType.leftParenthesis));
            opeList.Add(new CalOpe(")", -10000, 0, OpeType.rightParenthesis));
            opeList.Add(new CalOpe(",", 0, 0, OpeType.Separator));//分隔符
            opeList.Add(new CalOpe("，", 0, 0, OpeType.Separator));//分隔符

            //下面是按位与、或、异或、取反运算符
            opeList.Add(new CalOpe("And", 50, 2, OpeType.bothSides));
            opeList.Add(new CalOpe("and", 50, 2, OpeType.bothSides));
            opeList.Add(new CalOpe("Or", 50, 2, OpeType.bothSides));
            opeList.Add(new CalOpe("or", 50, 2, OpeType.bothSides));
            opeList.Add(new CalOpe("Xor", 50, 2, OpeType.bothSides));
            opeList.Add(new CalOpe("xor", 50, 2, OpeType.bothSides));
            opeList.Add(new CalOpe("Not", 100, 1, OpeType.right));
            opeList.Add(new CalOpe("not", 100, 1, OpeType.right));

            //下面是位移运算符
            opeList.Add(new CalOpe("<<", 50, 2, OpeType.bothSides));
            opeList.Add(new CalOpe("Lsh", 50, 2, OpeType.bothSides));
            opeList.Add(new CalOpe(">>", 50, 2, OpeType.bothSides));
            opeList.Add(new CalOpe("Rsh", 50, 2, OpeType.bothSides));

            //下面是常数
            opeList.Add(new CalOpe("PI", 120, 0, OpeType.noParameter));//圆周率
            opeList.Add(new CalOpe("pi", 120, 0, OpeType.noParameter));//圆周率
            opeList.Add(new CalOpe("e", 120, 0, OpeType.noParameter));//自然常数
            opeList.Add(new CalOpe("rnd", 120, 0, OpeType.noParameter));//随机数

            //下面是函数
            opeList.Add(new CalOpe("avg", 100, -1, OpeType.right));//求平均数
            opeList.Add(new CalOpe("sum", 100, -1, OpeType.right));//求和
            opeList.Add(new CalOpe("conv", 100, -1, OpeType.right));//单位转换
            opeList.Add(new CalOpe("s", 100, -1, OpeType.right));//样本标准方差
            opeList.Add(new CalOpe("dms", 100, -1, OpeType.right));//度分秒与小数形式相互转换

            //下面读取变量
            if (Properties.Settings.Default.strVariables != null)
            {
                string[] variables = Properties.Settings.Default.strVariables.Split('|');
                for (int i = 0; i < variables.Length / 2; i++)
                {
                    opeList.Add(new CalOpe(variables[i * 2], 120, 0, OpeType.noParameter));
                }
            }

            //支持用户自定义函数计算
            if (Properties.Settings.Default.funList != null)
            {
                for (int i = 0; i < Properties.Settings.Default.funList.Count; i++)
                {
                    string tag = Properties.Settings.Default.funList[i].Split('|')[0];
                    opeList.Add(new CalOpe(tag, 100, -1, OpeType.right));
                }
            }

            //其他运算符
            opeList.Add(new CalOpe("Sgn", 100, 1, OpeType.right));//取数字符号
            opeList.Add(new CalOpe("sgn", 100, 1, OpeType.right));//取数字符号
            opeList.Add(new CalOpe("Sign", 100, 1, OpeType.right));//取数字符号
            opeList.Add(new CalOpe("sign", 100, 1, OpeType.right));//取数字符号
            opeList.Add(new CalOpe("Int", 100, 1, OpeType.right));//取整
            opeList.Add(new CalOpe("int", 100, 1, OpeType.right));//取整
            opeList.Add(new CalOpe("abs", 100, 1, OpeType.right));//绝对值
            opeList.Add(new CalOpe("Abs", 100, 1, OpeType.right));//绝对值
            opeList.Add(new CalOpe("Rnd", 100, 2, OpeType.right));//产生随机数

            //分段函数，该函数只能用于用户自定义分段函数
            opeList.Add(new CalOpe("subfuns", 100, -1, OpeType.right));

            //按运算符的标识符字符串长度从大到小排序
            opeList.Sort();
            #endregion

        }

        #region 计算方法调用
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="ope">运算符</param>
        /// <param name="numList">参数列表</param>
        public static CalNum Calculate(CalOpe ope, List<CalNum> numList)
        {
            CalNum result = null;

            try
            {
                switch (ope.tag)
                {
                    //下面是运算符
                    case "+":
                        result = Cal_Plus(numList);
                        break;
                    case "-":
                        result = Cal_Minus(numList);
                        break;

                    case "*":
                        result = Cal_Multiply(numList);
                        break;
                    case "/":
                        result = Cal_Divide(numList);
                        break;
                    case "Mod":
                    case "mod":
                        result = Cal_Mod(numList);
                        break;

                    case "^":
                        result = Cal_Pow(numList);
                        break;

                    case "Exp":
                    case "exp":
                        result = Cal_Exp(numList);
                        break;
                    case "ln":
                        result = Cal_Ln(numList);
                        break;
                    case "log":
                        result = Cal_Log(numList);
                        break;
                    case "sqrt":
                        result = Cal_Sqrt(numList);
                        break;

                    case "sin":
                        result = Cal_Sin(numList);
                        break;
                    case "cos":
                        result = Cal_Cos(numList);
                        break;
                    case "tan":
                        result = Cal_Tan(numList);
                        break;
                    case "asin":
                        result = Cal_Asin(numList);
                        break;
                    case "acos":
                        result = Cal_Acos(numList);
                        break;
                    case "atan":
                        result = Cal_Atan(numList);
                        break;
                    case "sinh":
                        result = Cal_Sinh(numList);
                        break;
                    case "cosh":
                        result = Cal_Cosh(numList);
                        break;
                    case "tanh":
                        result = Cal_Tanh(numList);
                        break;

                    case "nCr": //组合
                    case "ncr":
                        result = Cal_nCr(numList);
                        break;
                    case "nAr": //排列
                    case "nar":
                        result = Cal_nAr(numList);
                        break;

                    case "!":
                        result = Cal_Fac(numList);
                        break;
                    case "%":
                        result = Cal_Per(numList);
                        break;
                    case "度":
                        result = Cal_Du(numList);
                        break;
                    case "toDegree":
                        result = Cal_Degree(numList);
                        break;

                    case "BIN": //二进制
                        result = Cal_B(numList);
                        break;
                    case "OCT": //八进制
                        result = Cal_O(numList);
                        break;
                    case "HEX": //十六进制
                        result = Cal_H(numList);
                        break;
                    case "toB": //二进制
                        result = Cal_ToB(numList);
                        break;
                    case "toO": //八进制
                        result = Cal_ToO(numList);
                        break;
                    case "toH": //十六进制
                        result = Cal_ToH(numList);
                        break;

                    //下面是常数
                    case "PI":
                    case "pi":
                        result = new CalNum(Math.PI);
                        break;
                    case "e":
                        result = new CalNum(Math.E);
                        break;
                    case "rnd":
                        Random rnd = new Random();
                        result = new CalNum(rnd.NextDouble());
                        break;

                    //下面是函数
                    case "avg":
                        result = average(numList);
                        break;
                    case "sum":
                        result = sum(numList);
                        break;
                    case "conv":
                        result = UnitConversion(numList);
                        break;
                    case "s":
                        result = SwatchStandardVariance(numList);
                        break;
                    case "dms":
                        result = dms(numList);
                        break;

                    //下面是逻辑运算
                    case "and":
                    case "And":
                        result = and(numList);
                        break;
                    case "or":
                    case "Or":
                        result = or(numList);
                        break;
                    case "xor":
                    case "Xor":
                        result = xor(numList);
                        break;
                    case "not":
                    case "Not":
                        result = not(numList);
                        break;

                    //下面是位移运算符
                    case ">>":
                    case "Rsh":
                        result = Rsh(numList);
                        break;
                    case "<<":
                    case "Lsh":
                        result = Lsh(numList);
                        break;

                    //其他运算符
                    case "sgn":
                    case "Sgn":
                    case "Sign":
                    case "sign":
                        result = sgn(numList);
                        break;
                    case "int":
                    case "Int":
                        result = Int(numList);
                        break;
                    case "Abs":
                    case "abs":
                        result = abs(numList);
                        break;
                    case "Rnd":
                        result = Rnd(numList);
                        break;

                    //分段函数
                    case "subfuns":
                        MakeException("该函数不能这样使用");
                        break;

                    //变量或用户自定义函数
                    default:
                        result = GetVariableOrCustomFunResultValue(ope.tag, numList);
                        break;

                }
            }
            catch (Exception ex)
            {
                throw (new Exception("在'" + ope.tag + "'附近可能存在错误：" + ex.Message));
            }

            if (result == null)
            {
                throw (new Exception("在'" + ope.tag + "'附近可能存在错误"));
            }

            if (result.stringValue == "非数字")
            {
                throw (new Exception("'" + ope.tag + "'运算的计算结果非数字"));
            }

            return result;
        }
        #endregion

        #region 获取变量或用户自定义函数的计算结果
        /// <summary>
        /// 获取变量值或用户自定义函数的计算结果
        /// </summary>
        /// <param name="tag">变量标识符</param>
        private static CalNum GetVariableOrCustomFunResultValue(string tag, List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 0)//变量
            {
                string[] variables = Properties.Settings.Default.strVariables.Split('|');

                //扫描变量键值列表
                for (int i = 0; i < variables.Length; i++)
                {
                    //找到变量
                    if (variables[i] == tag)
                    {
                        double d;
                        if (double.TryParse(variables[i + 1], out d))
                        {
                            result = new CalNum(d);
                        }
                        else
                        {
                            result = new CalNum(variables[i + 1]);
                        }
                    }
                }
            }
            else//用户自定义函数
            {
                try
                {
                    StringCollection funList = Properties.Settings.Default.funList;
                    if (funList != null)
                    {
                        //逐个扫描存在的函数，查找函数
                        for (int i = 0; i < funList.Count; i++)
                        {
                            if (funList[i].Split('|')[0] == tag)//找到函数
                            {
                                //参数个数不正确
                                if (numList.Count != funList[i].Split('|')[3].Split(',').Length)
                                {
                                    throw new Exception("|参数个数不正确");
                                }

                                string[] funParts = funList[i].Split('|');//分隔
                                StringBuilder funExpr = new StringBuilder(funParts[2]);//取出表达式

                                //替换表达式中的变量
                                for (int k = 0; k < numList.Count; k++)
                                {
                                    bool bl;
                                    //替换，用参数的stringValue替换且两边加上括号
                                    funExpr.Replace(Common.DeleteAnnotate(funParts[3].Split(',')[k], out bl), "(" + numList[k].stringValue + ")");
                                }

                                //分段函数
                                if (funExpr.ToString().IndexOf("subfuns") != -1)
                                {
                                    return subfuns(funExpr, numList);//不再进行后面的计算，返回
                                }

                                //计算
                                bool bl1;
                                string str = Common.DeleteAnnotate(funExpr.ToString(), out bl1);//删除注释
                                CalValList valList = new CalValList(str);
                                CalResult calResult = valList.Calculate();
                                if (calResult.numList.Count == 1)//一个计算结果
                                {
                                    result = calResult.numList[0];
                                }
                                else//多个计算结果
                                {
                                    result = new CalNum(calResult.GeneralResultToShow.Trim());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + "|（函数'" + tag + "'的表达式是'" + Common.GetFunExpr(tag) + "'）");
                }
            }

            return result;
        }
        #endregion

        #region 根据标识符取运算符
        /// <summary>
        /// 根据标识符取运算符
        /// </summary>
        public CalOpe GetOpe(string tag)
        {
            CalOpe result = null;

            for (int i = 0; i < count; i++)
            {
                if (opeList[i].tag == tag)
                {
                    result = opeList[i];
                    break;
                }
            }

            return result;
        }
        #endregion

        #region 获取两个数的小数部分的最大长度
        /// <summary>
        /// 获取两个数的小数部分的最大长度
        /// </summary>
        private static int GetDecimalLength(double leftValue, double rightValue)
        {
            int result = 0;

            if (leftValue.ToString().IndexOf("E") == -1
                && rightValue.ToString().IndexOf("E") == -1)
            {
                int m = leftValue.ToString().IndexOf(".") == -1 ? 0 : leftValue.ToString().Length - leftValue.ToString().IndexOf(".") - 1;
                int n = rightValue.ToString().IndexOf(".") == -1 ? 0 : rightValue.ToString().Length - rightValue.ToString().IndexOf(".") - 1;

                if (m > n)
                {
                    result = m;
                }
                else
                {
                    result = n;
                }
            }

            return result;
        }
        #endregion

        #region 具体计算方法
        #region 普通计算
        /// <summary>
        /// 加
        /// </summary>
        private static CalNum Cal_Plus(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(numList[0].DoubleValue);
            }
            else if (numList.Count == 2)
            {
                //转换成整数再运算，结果再转换成小数
                int decimalLength = GetDecimalLength(numList[0].DoubleValue, numList[1].DoubleValue);
                result = new CalNum((numList[0].DoubleValue * Math.Pow(10, decimalLength) + numList[1].DoubleValue * Math.Pow(10, decimalLength))
                    / Math.Pow(10, decimalLength));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 减
        /// </summary>
        private static CalNum Cal_Minus(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(0 - numList[0].DoubleValue);
            }
            else if (numList.Count == 2)
            {
                //转换成整数再运算，结果再转换成小数
                int decimalLength = GetDecimalLength(numList[0].DoubleValue, numList[1].DoubleValue);
                result = new CalNum((numList[0].DoubleValue * Math.Pow(10, decimalLength) - numList[1].DoubleValue * Math.Pow(10, decimalLength))
                    / Math.Pow(10, decimalLength));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 乘
        /// </summary>
        private static CalNum Cal_Multiply(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                result = new CalNum(numList[0].DoubleValue * numList[1].DoubleValue);
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 除
        /// </summary>
        private static CalNum Cal_Divide(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                result = new CalNum(numList[0].DoubleValue / numList[1].DoubleValue);
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// Mod(取余)
        /// </summary>
        private static CalNum Cal_Mod(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                result = new CalNum(numList[0].DoubleValue % numList[1].DoubleValue);
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 乘方
        /// </summary>
        private static CalNum Cal_Pow(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                result = new CalNum(Math.Pow(numList[0].DoubleValue, numList[1].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            if (double.IsInfinity(result.DoubleValue))
            {
                MessageBox.Show("指数运算'^'的运算结果为无穷大，可能会导致'log'等对数运算出现错误", "警告");
            }

            return result;
        }

        /// <summary>
        /// sin
        /// </summary>
        private static CalNum Cal_Sin(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Sin(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// cos
        /// </summary>
        private static CalNum Cal_Cos(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Cos(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// tan
        /// </summary>
        private static CalNum Cal_Tan(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Tan(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// Exp
        /// </summary>
        private static CalNum Cal_Exp(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Exp(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            if (double.IsInfinity(result.DoubleValue))
            {
                MessageBox.Show("指数运算'Exp'的运算结果为无穷大，可能会导致ln等对数运算出现错误", "警告");
            }

            return result;
        }

        /// <summary>
        /// ln
        /// </summary>
        private static CalNum Cal_Ln(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Log(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// log
        /// </summary>
        private static CalNum Cal_Log(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Log10(numList[0].DoubleValue));
            }
            //两个参数的情况
            else if (numList.Count == 2)
            {
                result = new CalNum(Math.Log(numList[0].DoubleValue, numList[1].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// sqrt
        /// </summary>
        private static CalNum Cal_Sqrt(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Sqrt(numList[0].DoubleValue));
            }
            //两个参数的情况
            else if (numList.Count == 2)
            {
                result = new CalNum(Math.Pow(numList[0].DoubleValue, 1 / numList[1].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// !(阶乘)
        /// </summary>
        private static CalNum Cal_Fac(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                int n;

                try
                {
                    n = int.Parse(numList[0].DoubleValue.ToString());
                }
                catch
                {
                    throw (new Exception("阶乘的操作数必须是整数"));
                }

                result = new CalNum(Common.Fac(n));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// %(百分比)
        /// </summary>
        private static CalNum Cal_Per(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(numList[0].DoubleValue / 100);
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 角度转换成弧度
        /// </summary>
        private static CalNum Cal_Du(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(numList[0].DoubleValue * Math.PI / 180);
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 弧度转换成角度
        /// </summary>
        private static CalNum Cal_Degree(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(numList[0].DoubleValue * 180 / Math.PI);
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 二进制转换成十进制
        /// </summary>
        private static CalNum Cal_B(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Convert.ToDouble(Convert.ToInt64(numList[0].stringValue, 2)));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 十进制转换成二进制
        /// </summary>
        private static CalNum Cal_ToB(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                if (numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && numList[0].DoubleValue.ToString().IndexOf(".") != -1)
                {
                    throw (new Exception("不支持小数"));
                }
                result = new CalNum(Convert.ToString((long)numList[0].DoubleValue, 2));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 八进制转换成十进制
        /// </summary>
        private static CalNum Cal_O(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Convert.ToDouble(Convert.ToInt64(numList[0].stringValue, 8)));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 十进制转换成八进制
        /// </summary>
        private static CalNum Cal_ToO(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                if (numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && numList[0].DoubleValue.ToString().IndexOf(".") != -1)
                {
                    throw (new Exception("不支持小数"));
                }
                result = new CalNum(Convert.ToString((long)numList[0].DoubleValue, 8));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 十六进制转换成十进制
        /// </summary>
        private static CalNum Cal_H(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Convert.ToDouble(Convert.ToInt64(numList[0].stringValue, 16)));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 十进制转换成十六进制
        /// </summary>
        private static CalNum Cal_ToH(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                if (numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && numList[0].DoubleValue.ToString().IndexOf(".") != -1)
                {
                    throw (new Exception("不支持小数"));
                }
                result = new CalNum(Convert.ToString((long)numList[0].DoubleValue, 16));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 组合
        /// </summary>
        private static CalNum Cal_nCr(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                result = new CalNum(Common.Fac((int)numList[0].DoubleValue)
                    / (Common.Fac((int)numList[0].DoubleValue - (int)numList[1].DoubleValue)
                    * Common.Fac((int)numList[1].DoubleValue)));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 排列
        /// </summary>
        private static CalNum Cal_nAr(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                result = new CalNum(Common.Fac((int)numList[0].DoubleValue)
                    / Common.Fac((int)numList[0].DoubleValue - (int)numList[1].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// asin
        /// </summary>
        private static CalNum Cal_Asin(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Asin(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// acos
        /// </summary>
        private static CalNum Cal_Acos(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Acos(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// atan
        /// </summary>
        private static CalNum Cal_Atan(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Atan(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// sinh
        /// </summary>
        private static CalNum Cal_Sinh(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Sinh(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// cosh
        /// </summary>
        private static CalNum Cal_Cosh(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Cosh(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// tanh
        /// </summary>
        private static CalNum Cal_Tanh(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Tanh(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 求平均数
        /// </summary>
        private static CalNum average(List<CalNum> numList)
        {
            CalNum result = null;

            double sum = 0;
            if (numList.Count != 0)
            {
                for (int i = 0; i < numList.Count; i++)
                {
                    sum += numList[i].DoubleValue;
                }
                result = new CalNum(sum / numList.Count);
            }

            return result;
        }

        /// <summary>
        /// 求和
        /// </summary>
        private static CalNum sum(List<CalNum> numList)
        {
            CalNum result = null;

            double sum = 0;
            if (numList.Count != 0)
            {
                for (int i = 0; i < numList.Count; i++)
                {
                    sum += numList[i].DoubleValue;
                }
                result = new CalNum(sum);
            }

            return result;
        }

        /// <summary>
        /// 样本标准方差
        /// </summary>
        private static CalNum SwatchStandardVariance(List<CalNum> numList)
        {
            CalNum result = null;

            //特殊情况
            if (numList.Count == 1)
            {
                return new CalNum(0);
            }

            double s = 0;
            double avg = average(numList).DoubleValue;
            for (int i = 0; i < numList.Count; i++)
            {
                s += Math.Pow(numList[i].DoubleValue - avg, 2);
            }
            s /= (numList.Count - 1);
            s = Math.Sqrt(s);

            result = new CalNum(s);

            return result;
        }

        /// <summary>
        /// 度分秒与小数形式相互转换
        /// </summary>
        private static CalNum dms(List<CalNum> numList)
        {
            CalNum result = null;

            double d = 0;
            double f = 0;
            double m = 0;
            if (numList.Count == 1)
            {
                d = Math.Floor(numList[0].DoubleValue);
                f = (numList[0].DoubleValue - d) * 60;
                m = (f - Math.Floor(f)) * 60;

                result = new CalNum(d.ToString() + "| 度 |"
                    + Math.Floor(f).ToString() + "| 分 |"
                    + m.ToString() + "| 秒");
            }
            else if (numList.Count == 2)
            {
                result = new CalNum(numList[0].DoubleValue + numList[1].DoubleValue / 60);
            }
            else if (numList.Count == 3)
            {
                result = new CalNum(numList[0].DoubleValue + numList[1].DoubleValue / 60 + numList[2].DoubleValue / 3600);
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }

        /// <summary>
        /// 产生随机数
        /// </summary>
        private static CalNum Rnd(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                Random rnd = new Random();
                result = new CalNum(rnd.Next((int)numList[0].DoubleValue, (int)(numList[1].DoubleValue + 1)));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }
        #endregion

        #region 单位换算
        /// <summary>
        /// 单位换算
        /// </summary>
        private static CalNum UnitConversion(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 3)
            {
                switch (numList[0].stringValue)
                {
                    case "海里":
                        switch (numList[1].stringValue)
                        {
                            case "公里":
                                result = Conversion(1.852, numList);
                                break;
                        }
                        break;
                    case "光年":
                        switch (numList[1].stringValue)
                        {
                            case "公里":
                                result = Conversion(9460528404879358.8126, numList);
                                break;
                        }
                        break;
                    case "平方公里":
                        switch (numList[1].stringValue)
                        {
                            case "亩":
                                result = Conversion(1500, numList);
                                break;
                        }
                        break;
                    case "亩":
                        switch (numList[1].stringValue)
                        {
                            case "平方公里":
                                result = Conversion(1.0 / 1500, numList);
                                break;
                            case "平方米":
                                //注意：1000*2/3计算结果是整数，1000.0*2/3才能得到正确的结果
                                result = Conversion(1000.0 * 2 / 3, numList);
                                break;
                        }
                        break;
                    case "千米每小时":
                        switch (numList[1].stringValue)
                        {
                            case "米每秒":
                                result = Conversion(1.0 / 3.6, numList);
                                break;
                        }
                        break;
                    case "米每秒":
                        switch (numList[1].stringValue)
                        {
                            case "千米每小时":
                                result = Conversion(3.6, numList);
                                break;
                        }
                        break;
                    case "英里":
                        switch (numList[1].stringValue)
                        {
                            case "公里":
                                result = Conversion(1.609344, numList);
                                break;
                        }
                        break;
                    case "英寸":
                        switch (numList[1].stringValue)
                        {
                            case "厘米":
                                result = Conversion(2.54, numList);
                                break;
                        }
                        break;
                    case "桶":
                        switch (numList[1].stringValue)
                        {
                            case "立方米":
                                result = Conversion(0.159, numList);
                                break;
                        }
                        break;
                    case "马赫":
                        switch (numList[1].stringValue)
                        {
                            case "米每秒":
                                result = Conversion(340.3, numList);
                                break;
                        }
                        break;
                }
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }
        #endregion

        #region 换算函数
        /// <summary>
        /// 换算函数
        /// </summary>
        /// <param name="coefficient">换算系数</param>
        private static CalNum Conversion(double coefficient, List<CalNum> numList)
        {
            CalNum result = null;

            double doubleResult = numList[2].DoubleValue * coefficient;
            string strResult = numList[2].DoubleValue.ToString()
                + "|" + numList[0].stringValue
                + " = |"
                + doubleResult.ToString()
                + "|" + numList[1].stringValue;
            result = new CalNum(doubleResult, strResult);

            return result;
        }
        #endregion

        #region 按位与、或、异或、取反运算
        //and
        private static CalNum and(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                if (numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && (numList[0].DoubleValue.ToString().IndexOf(".") != -1
                    || numList[1].DoubleValue.ToString().IndexOf(".") != -1))
                {
                    throw (new Exception("不支持小数"));
                }
                else
                {
                    result = new CalNum((long)numList[0].DoubleValue & (long)numList[1].DoubleValue);
                }
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }
        //or
        private static CalNum or(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                if (numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && (numList[0].DoubleValue.ToString().IndexOf(".") != -1
                    || numList[1].DoubleValue.ToString().IndexOf(".") != -1))
                {
                    throw (new Exception("不支持小数"));
                }
                else
                {
                    result = new CalNum((long)numList[0].DoubleValue | (long)numList[1].DoubleValue);
                }
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }
        //xor
        private static CalNum xor(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                if (numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && (numList[0].DoubleValue.ToString().IndexOf(".") != -1
                    || numList[1].DoubleValue.ToString().IndexOf(".") != -1))
                {
                    throw (new Exception("不支持小数"));
                }
                else
                {
                    result = new CalNum((long)numList[0].DoubleValue ^ (long)numList[1].DoubleValue);
                }
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }
        //not
        private static CalNum not(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                if (numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && numList[0].DoubleValue.ToString().IndexOf(".") != -1)
                {
                    throw (new Exception("不支持小数"));
                }
                else
                {
                    result = new CalNum(~(long)numList[0].DoubleValue);
                }
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }
        #endregion

        #region 位移运算
        //Lsh
        private static CalNum Lsh(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                if (numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && (numList[0].DoubleValue.ToString().IndexOf(".") != -1
                    || numList[1].DoubleValue.ToString().IndexOf(".") != -1))
                {
                    throw (new Exception("不支持小数"));
                }
                else
                {
                    int length1 = Convert.ToString((long)numList[0].DoubleValue, 2).Length;
                    int length2 = (int)numList[1].DoubleValue;
                    if (length1 + length2 > 63)
                    {
                        MakeException("溢出");
                    }
                    result = new CalNum((long)numList[0].DoubleValue << (int)numList[1].DoubleValue);
                }
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }
        //Rsh
        private static CalNum Rsh(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 2)
            {
                if (numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && numList[0].DoubleValue.ToString().IndexOf("E") == -1
                    && (numList[0].DoubleValue.ToString().IndexOf(".") != -1
                    || numList[1].DoubleValue.ToString().IndexOf(".") != -1))
                {
                    throw (new Exception("不支持小数"));
                }
                else
                {
                    int length1 = Convert.ToString((long)numList[0].DoubleValue, 2).Length;
                    int length2 = (int)numList[1].DoubleValue;
                    string str = null;
                    if (length2 > 63)
                    {
                        MakeException("溢出");
                    }
                    if (length2 <= length1)
                    {
                        str = Convert.ToString((long)numList[0].DoubleValue, 2).Substring(length1 - length2, length2);
                    }
                    else
                    {
                        str = Convert.ToString((long)numList[0].DoubleValue, 2);
                        for (int i = 0; i < length2 - length1; i++)
                        {
                            str = str.Insert(0, "0");
                        }
                    }
                    double d = 0;
                    for (int i = 0; i < str.Length; i++)
                    {
                        if (str[i] != '0')
                        {
                            d += 1 / Math.Pow(2, (i + 1));
                        }
                    }
                    result = new CalNum(((long)numList[0].DoubleValue >> (int)numList[1].DoubleValue) + d);
                }
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }
        #endregion

        #region 其他计算
        //sgn
        private static CalNum sgn(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Sign(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }
        //Int
        private static CalNum Int(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Truncate(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }
        //abs
        private static CalNum abs(List<CalNum> numList)
        {
            CalNum result = null;

            if (numList.Count == 1)
            {
                result = new CalNum(Math.Abs(numList[0].DoubleValue));
            }
            else
            {
                MakeException("参数个数不正确");
            }

            return result;
        }
        #endregion

        #region 分段函数
        /// <summary>
        /// 分段函数
        /// </summary>
        private static CalNum subfuns(StringBuilder funExpr, List<CalNum> numList)
        {
            CalNum result = null;

            if (funExpr.ToString().IndexOf("subfuns(") == -1)
            {
                MakeException("分段函数语法不正确");
            }
            if (funExpr[funExpr.Length - 1] != ')')
            {
                MakeException("分段函数语法不正确");
            }
            funExpr.Replace("subfuns(", "");
            funExpr.Remove(funExpr.Length - 1, 1);
            CalNum num = numList[0];//仅支持对第一个参数分段

            bool isBeyond = true;//是否越界
            string[] funs = funExpr.ToString().Split('，');

            if (funs.Length % 3 != 0)
            {
                MakeException("subfuns的参数个数不正确，必须三个为一组");
            }

            for (int i = 0; i < funs.Length; i += 3)//每三个一组：左边界、右边界、对应表达式
            {
                double leftBoundary = double.MinValue;//左边界
                double rightBoundary = double.MaxValue;//右边界
                bool leftHollow = false;//左边界是否空心，默认是实心
                bool rightHollow = false;//右边界是否空心，默认是实心
                bool isIn = false;//参数是否在该范围内，默认false

                if (funs[i].IndexOf(">") != -1)//左边界空心
                {
                    leftHollow = true;
                    leftBoundary = double.Parse(funs[i].Substring(1));
                }
                else
                {
                    leftBoundary = double.Parse(funs[i]);
                }
                if (funs[i + 1].IndexOf("<") != -1)//右边界空心
                {
                    rightHollow = true;
                    rightBoundary = double.Parse(funs[i + 1].Substring(1));
                }
                else
                {
                    rightBoundary = double.Parse(funs[i + 1]);
                }

                #region 判断参数是否在当前范围内
                if (leftHollow == true)//左边界空心
                {
                    if (rightHollow == true)//右边界空心
                    {
                        if (num.DoubleValue > leftBoundary
                            && num.DoubleValue < rightBoundary)
                        {
                            isIn = true;
                        }
                    }
                    else//右边界实心
                    {
                        if (num.DoubleValue > leftBoundary
                            && num.DoubleValue <= rightBoundary)
                        {
                            isIn = true;
                        }
                    }
                }
                else//左边界实心
                {
                    if (rightHollow == true)//右边界空心
                    {
                        if (num.DoubleValue >= leftBoundary
                            && num.DoubleValue < rightBoundary)
                        {
                            isIn = true;
                        }
                    }
                    else//右边界实心
                    {
                        if (num.DoubleValue >= leftBoundary
                            && num.DoubleValue <= rightBoundary)
                        {
                            isIn = true;
                        }
                    }
                }
                #endregion

                if (isIn)//如果参数在当前范围内
                {
                    isBeyond = false;//没有越界
                    //计算
                    bool bl1;
                    string str = Common.DeleteAnnotate(funs[i + 2], out bl1);//删除注释
                    CalValList valList = new CalValList(str);
                    CalResult calResult = valList.Calculate();
                    if (calResult.numList.Count == 1)//一个计算结果
                    {
                        result = calResult.numList[0];
                    }
                    else//多个计算结果
                    {
                        result = new CalNum(calResult.GeneralResultToShow.Trim());
                    }
                    //跳出循环
                    break;
                }
            }
            if (isBeyond)
            {
                MakeException("输入的参数超出定义的范围");
            }

            return result;
        }
        #endregion

        #endregion

        #region 产生异常
        private static void MakeException(string exMessage)
        {
            throw new Exception(exMessage);
        }
        #endregion

    }
}
