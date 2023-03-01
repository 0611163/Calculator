/////////////////////////////////////////////////////////////////
//2012年7月20日
//修改BUG：输入-3E-3时'-'的发音不正确，没有考虑到科学计数法的情况
//2012年7月21日
//增加功能：
//1、计算结果转换为人民币大写
//2、存储算式
//3、算式注释功能
/////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tool;

namespace 计算器
{
    /// <summary>
    /// 公共方法与公式变量类
    /// </summary>
    class Common
    {
        /// <summary>
        /// 数字分组长度
        /// </summary>
        public static int groupLength = 3;
        /// <summary>
        /// 出错字符串表达式
        /// </summary>
        public static string strWrongExp = null;
        /// <summary>
        /// 锁
        /// </summary>
        public static bool _lock = false;
        /// <summary>
        /// 软件试用时间的天数
        /// </summary>
        public static int probationalTime = 60;

        #region 密钥
        /// <summary>
        /// 密钥
        /// </summary>
        public static string DESKey = DESEncrypt.Decrypt("3B7463A00F460B35DD70BF694258198EBFEC870BA792E509886CAE92A0873C3EA21CE1B2DAD9D430471D80A08B402C45939ECE4DDE155BD614773F9DC8BF35A03522B4482A9BD767", "KTMCK");
        #endregion

        #region 锁
        /// <summary>
        /// 加锁，用途：如果调用了Common.SetWrongOpeColor将不再调用Common.SetColor
        /// </summary>
        public static void Lock()
        {
            _lock = true;
        }

        /// <summary>
        /// 解锁，用途：如果调用了Common.SetWrongOpeColor将不再调用Common.SetColor
        /// </summary>
        public static void UnLock()
        {
            _lock = false;
        }
        #endregion

        #region 求阶乘
        /// <summary>
        /// 求阶乘，这里不使用递归算法，因为递归算法当n比较大时，会使程序崩溃
        /// </summary>
        public static double Fac(int n)
        {
            if (n < 0)
            {
                throw (new Exception("无法对负数求阶乘"));
            }
            else if (n == 0)
            {
                return 1;
            }
            else if (n == 1)
            {
                return 1;
            }
            else if (n <= 1E5)
            {
                double result = 1;

                for (int i = 1; i <= n; i++)
                {
                    result *= i;
                }

                return result;
            }
            else //若输入的数字过大
            {
                return double.PositiveInfinity;//返回正无穷大
            }
        }
        #endregion

        #region 运算结果处理
        /// <summary>
        /// 运算结果处理，如3333.5555处理成3 333.555 5以便于阅读
        /// </summary>
        public static string ResultProcess(string str)
        {
            StringBuilder sb = new StringBuilder(str);

            int pointPos = sb.ToString().IndexOf('.');
            if (pointPos != -1)//有小数点的情况
            {
                //处理小数点的前面
                for (int i = pointPos - groupLength; i > 0; i -= groupLength)
                {
                    sb.Insert(i, " ");
                }

                pointPos = sb.ToString().IndexOf('.');//注意，重新计算
                //处理小数点后的情况
                for (int i = pointPos + 4; i <= sb.Length - 1; i += 4)
                {
                    sb.Insert(i, " ");
                }
            }
            else//没有小数点的情况
            {
                for (int i = sb.Length - groupLength; i > 0; i -= groupLength)
                {
                    sb.Insert(i, " ");
                }
            }

            //处理科学计数法的情况
            int EPos = sb.ToString().IndexOf('E');
            if (EPos != -1)
            {
                string sLast = sb.ToString().Substring(EPos);
                sb.Replace(sLast, sLast.Replace(" ", "").Insert(1, " ").Insert(0, " "));
            }

            sb.Replace("  ", " ");//如果产生两个空格，转换成一个

            return sb.ToString();
        }
        #endregion

        #region 设置文本框中的文字颜色
        /// <summary>
        /// 设置文本框中的文字颜色
        /// </summary>
        public static void SetColor(string longStr, RichTextBox richTextBox)
        {
            CalOpeList opeList = new CalOpeList();

            //重置字体和颜色
            richTextBox.SelectAll();
            richTextBox.SelectionColor = Color.Black;
            richTextBox.SelectionFont = new Font(Form1.font.FontFamily, Form1.font.Size);
            richTextBox.SelectionLength = 0;

            //设置运算符的颜色，处理运算符列表的每一个运算符，注意，从后往前扫描
            for (int i = opeList.count - 1; i >= 0; i--)
            {
                //扫描字符串
                for (int k = 0; k < longStr.Length; k++)
                {
                    //找到运算符
                    if (longStr.Substring(k).IndexOf(opeList.opeList[i].tag) == 0)
                    {
                        if (opeList.opeList[i].opeType == OpeType.leftParenthesis
                            || opeList.opeList[i].opeType == OpeType.rightParenthesis)//括号
                        {
                            richTextBox.Select(k, opeList.opeList[i].tag.Length);
                            richTextBox.SelectionColor = Color.FromArgb(200, 0, 0);
                            richTextBox.SelectionStart = richTextBox.Text.Length;
                            richTextBox.SelectionColor = Color.Black;
                        }
                        else if (opeList.opeList[i].opeType == OpeType.noParameter)//常数或变量
                        {
                            richTextBox.Select(k, opeList.opeList[i].tag.Length);
                            richTextBox.SelectionColor = Color.FromArgb(128, 0, 128);
                            richTextBox.SelectionStart = richTextBox.Text.Length;
                            richTextBox.SelectionColor = Color.Black;
                        }
                        else//运算符或函数
                        {
                            if (opeList.opeList[i].tag.Length == 1
                                && opeList.opeList[i].opeType != OpeType.left
                                && opeList.opeList[i].opeType != OpeType.right)
                            {
                                richTextBox.Select(k, opeList.opeList[i].tag.Length);
                                richTextBox.SelectionColor = Color.FromArgb(0, 64, 128);
                                richTextBox.SelectionStart = richTextBox.Text.Length;
                                richTextBox.SelectionColor = Color.Black;
                            }
                            else
                            {
                                richTextBox.Select(k, opeList.opeList[i].tag.Length);
                                richTextBox.SelectionColor = Color.FromArgb(0, 128, 128);
                                richTextBox.SelectionStart = richTextBox.Text.Length;
                                richTextBox.SelectionColor = Color.Black;
                            }
                        }
                    }
                }
            }

            //重置字体
            richTextBox.SelectAll();
            richTextBox.SelectionFont = Form1.font;
            richTextBox.SelectionStart = richTextBox.Text.Length;
        }//end SetColor
        #endregion

        #region 设置出错运算符的字体颜色等
        /// <summary>
        /// 设置出错运算符的字体颜色等
        /// </summary>
        /// <param name="opeTag">出错运算符标识符</param>
        /// <param name="pos">出错位置</param>
        /// <param name="richTextBox">富文本框</param>
        public static void SetWrongOpeColor(string opeTag, int pos, RichTextBox richTextBox)
        {
            if (richTextBox.Text != strWrongExp)
            {
                UnLock();//解锁
                if (strWrongExp != null)
                {
                    richTextBox.Text = strWrongExp;
                }
                Lock();//加锁
            }

            richTextBox.Select(pos, opeTag.Length);
            richTextBox.SelectionColor = Color.FromArgb(255, 0, 0);
            richTextBox.SelectionFont = new Font(richTextBox.SelectionFont, FontStyle.Bold);
            richTextBox.SelectionStart = richTextBox.Text.Length;
            richTextBox.SelectionLength = 0;
            richTextBox.SelectionColor = Color.Black;

            strWrongExp = null;//注意，重新设置回null
        }
        #endregion

        #region 将double值转换成科学计算法表示
        /// <summary>
        /// 将double值转换成科学计算法表示
        /// </summary>
        public static string ToScientific(double d)
        {
            //解决诸如99999999.99999999转换错误的问题
            d = Convert.ToDouble(d.ToString());

            int count = 0;
            string result = null;

            while (Math.Abs(d) / Math.Pow(10, count) >= 10)
            {
                count++;
            }

            while (Math.Abs(d) / Math.Pow(10, count) > 0
                && Math.Abs(d) / Math.Pow(10, count) < 1)
            {
                count--;
            }

            result = (d / Math.Pow(10, count)).ToString() + "E" + count.ToString();

            return ResultProcess(result);
        }
        #endregion

        #region 发音
        /// <summary>
        /// 发音
        /// </summary>
        /// <param name="tag">标识</param>
        public static async Task Speech(string tag)
        {
            if (tag == "")
            {
                return;
            }

            await Task.Run(() => Play(tag));
        }
        public static void Play(object tag)
        {
            try
            {
                using (Stream stream = Properties.Resources.ResourceManager.GetStream(tag.ToString()))
                {
                    using (System.Media.SoundPlayer soundPlayer = new System.Media.SoundPlayer(stream))
                    {
                        soundPlayer.PlaySync();
                    }
                }
            }
            catch { }
        }
        #endregion

        #region 是否负号
        /// <summary>
        /// 是否负号
        /// </summary>
        /// <param name="strExp">已输入的表达式</param>
        /// <param name="tag">当前符号</param>
        public static bool isSubtractive(string strExp, string tag, int selectionStart)
        {
            bool result = false;//默认不是负号

            if (tag == "-")
            {
                strExp = strExp.Substring(0, selectionStart).Replace(" ", "");
                if (strExp.Length == 1)//只有一个符号
                {
                    result = true;
                }
                else
                {
                    //科学计数法的情况
                    if (strExp[strExp.Length - 2] == 'E')
                    {
                        result = true;
                    }
                    //前面是'='号的情况
                    if (strExp[strExp.Length - 2] == '=')
                    {
                        result = true;
                    }

                    CalOpeList opeList = new CalOpeList();
                    CalOpe ope = null;
                    //检测tag前面有没有运算符
                    for (int i = 0; i < strExp.Length; i++)
                    {
                        //从长字符串到短字符串检测
                        string subStr = strExp.Substring(i, strExp.Length - i - 1);
                        ope = opeList.GetOpe(subStr);//查找运算符
                        if (ope != null)//找到运算符
                        {
                            break;
                        }
                    }

                    if (ope != null)//找到运算符
                    {
                        //检测运算符种类
                        if (ope.opeType == OpeType.bothSides
                            || ope.opeType == OpeType.leftParenthesis
                            || ope.opeType == OpeType.PlusOrMinus
                            || ope.opeType == OpeType.right
                            || ope.opeType == OpeType.Separator)
                        {
                            result = true; ;
                        }
                    }
                }
            }

            return result;
        }
        #endregion

        #region 转换为可读的字符串，如：110转换为1百1十
        /// <summary>
        /// 转换为可读的字符串，如：110转换为1百1十
        /// </summary>
        /// <param name="doubleValue"></param>
        /// <returns></returns>
        public static string ToNaturalSpeech(double doubleValue)
        {
            StringBuilder result = new StringBuilder();
            string negativeTag = "";//负数标识
            string strDecimalPart = "";//小数部分

            try
            {
                //是否为负数
                bool isNegative = false;
                if (doubleValue < 0)
                {
                    isNegative = true;
                    negativeTag = "负";
                }

                string[] str = doubleValue.ToString().Split('.');

                StringBuilder sbIntegerPart = null;
                //整数部分
                if (isNegative)
                {
                    sbIntegerPart = new StringBuilder(str[0].Substring(1));
                }
                else
                {
                    sbIntegerPart = new StringBuilder(str[0]);
                }
                //小数部分
                if (str.Length > 1)
                {
                    strDecimalPart = "点" + str[1];
                }

                List<string> unitsList = new List<string>();
                unitsList.Add("");
                unitsList.Add("十");
                unitsList.Add("百");
                unitsList.Add("千");
                unitsList.Add("万");
                unitsList.Add("十");
                unitsList.Add("百");
                unitsList.Add("千");
                unitsList.Add("亿");
                unitsList.Add("十");
                unitsList.Add("百");
                unitsList.Add("千");
                unitsList.Add("万");
                unitsList.Add("十");
                unitsList.Add("百");
                unitsList.Add("千");
                unitsList.Add("亿");

                //先作一般处理
                for (int i = 0; i < sbIntegerPart.Length; i++)
                {
                    result.Append(sbIntegerPart[i]);
                    result.Append(unitsList[sbIntegerPart.Length - i - 1]);
                }

                //下面语句的先后顺序不可随意更改
                //下面五条语句处理后，不存在0十、0百、0千的情况
                //但仍可能存在0亿、0万的情况
                result.Replace("0十", "0");
                result.Replace("0百", "0");
                result.Replace("0千", "0");
                result.Replace("0亿", "亿0");
                //例：101000读作十万零1千，而不是十万一千
                //所以0万的0应该置后，上面0亿的情况同理
                result.Replace("0万", "万0");

                //while循环处理后，不存在连续两个0的情况
                while (result.ToString().IndexOf("00") != -1)
                {
                    result.Replace("00", "0");//又可能会产生如：0万
                }

                //由于已经不存在连续两个0的情况
                //下面两条语句处理后，不存在0亿、0万的情况
                //也不会导致连续两个0的情况
                result.Replace("0亿", "亿");
                result.Replace("0万", "万");

                //处理末尾
                if (result.ToString()[result.Length - 1] == '0'
                    && result.Length > 1)
                {
                    result.Remove(result.Length - 1, 1);
                }

                //特殊情况
                result.Replace("亿万", "亿");

                //特殊情况
                if (result.ToString().IndexOf("1十万") == 0)
                {
                    result.Replace("1十万", "十万");
                }
                if (result.ToString().IndexOf("1十亿") == 0)
                {
                    result.Replace("1十亿", "十亿");
                }

                //特殊情况
                if (result.ToString().IndexOf("1十") == 0)
                {
                    result.Replace("1十", "十", 0, 2);
                }
            }
            catch { }

            return negativeTag + result.ToString() + strDecimalPart;
        }
        #endregion

        #region 删除表达式的注释
        /// <summary>
        /// 删除表达式的注释
        /// </summary>
        /// <param name="strExp">表达式</param>
        /// /// <param name="hasAnnotate">表达式是否有注释</param>
        public static string DeleteAnnotate(string strExp, out bool hasAnnotate)
        {
            //默认没有注释
            hasAnnotate = false;
            //计算注释起止符号的个数
            int count = 0;
            for (int i = 0; i < strExp.Length; i++)
            {
                if (strExp[i] == '"'
                    || strExp[i] == '（'
                    || strExp[i] == '）'
                    || strExp[i] == '〔'
                    || strExp[i] == '〕')
                {
                    count++;
                }
            }

            if (count == 0)//没有注释起止符号
            {
                return strExp;
            }
            else if (count % 2 != 0)//注释起止符号不成对
            {
                throw (new Exception("注释符号不成对"));
            }

            hasAnnotate = true;//表达式有注释
            StringBuilder result = new StringBuilder();
            bool flag = false;//值为true表示注释开始，值为false表示注释结束

            //逐个扫描
            for (int i = 0; i < strExp.Length; i++)
            {
                if (!flag && (strExp[i] == '"'
                    || strExp[i] == '（'//中文全角
                    || strExp[i] == '）'
                    || strExp[i] == '〔'//英文全角
                    || strExp[i] == '〕'))
                {
                    flag = true;//注释开始
                    continue;
                }
                else if (flag && (strExp[i] == '"'
                    || strExp[i] == '（'//注意，不可删除，否则如9+9（注释（+9计算有误
                    || strExp[i] == '）'//中文全角
                    || strExp[i] == '〔'
                    || strExp[i] == '〕'))//英文全角
                {
                    flag = false;//注释结束
                    continue;
                }

                //注释结束后继续输入到result
                if (flag == false)
                {
                    result.Append(strExp[i].ToString());
                }
            }

            return result.ToString();
        }
        #endregion

        #region 转换为中文大写
        /// <summary>
        /// 转换为中文大写
        /// </summary>
        public static string ToChineseCapital(string stringValue)
        {
            //科学计数法不转换
            if (stringValue.IndexOf("E") != -1)
            {
                throw new Exception("有非法字符无法转换");
            }
            double doubleValue;
            //无法转换成数字则不转换
            if (!double.TryParse(stringValue.Replace(" ", ""), out doubleValue))
            {
                throw new Exception("有非法字符无法转换");
            }
            //负数不能转换
            if (doubleValue < 0)
            {
                throw new Exception("负数转换无意义");
            }

            StringBuilder result = new StringBuilder();

            string strTemp = ToNaturalSpeech(doubleValue);
            string[] strs = strTemp.Split('点');
            //逐个扫描整数部分
            for (int i = 0; i < strs[0].Length; i++)
            {
                string tag = strs[0][i].ToString();
                result.Append(ToCapital(tag));
            }

            if (strs.Length == 1)//没有小数
            {
                result.Append("元整");
            }
            else//有小数
            {
                bool isIntPartZero = false;//整数部分是否为0
                if (result.Length == 1
                    && result[0].ToString() == "零")//整数部分为0
                {
                    isIntPartZero = true;
                    result.Remove(0, 1);
                }
                else//整数部分不为零
                {
                    result.Append("元");
                }
                //处理小数部分
                string jiao = "";
                string cent = "";
                //获取小数部分保留两位小数
                double d = double.Parse("0." + strs[1]);
                string s = d.ToString("0.00");
                jiao = s[2].ToString();
                cent = s[3].ToString();
                result.Append(ToCapital(jiao) + "角" + ToCapital(cent) + "分");
                if (isIntPartZero)//整数部分为0
                {
                    result.Replace("零角", "").Replace("零分", "");
                }
                else//整数部分不为0
                {
                    if (cent == "0")//分为0
                    {
                        if (jiao == "0")//角为0
                        {
                            result.Replace("零角", "").Replace("零分", "");
                            result.Append("整");
                        }
                        else
                        {
                            string doublePart = doubleValue.ToString().Split('.')[0];
                            if (doublePart[doublePart.Length - 1] == '0')
                            {
                                result.Insert(result.ToString().IndexOf("元") + 1, "零");
                            }
                            result.Replace("零角", "").Replace("零分", "");
                        }
                    }
                    else
                    {
                        if (jiao == "0")
                        {
                            result.Replace("零角", "零");
                        }
                        else
                        {
                            string doublePart = doubleValue.ToString().Split('.')[0];
                            if (doublePart[doublePart.Length - 1] == '0')
                            {
                                result.Insert(result.ToString().IndexOf("元") + 1, "零");
                            }
                        }
                    }
                }
            }

            return result.ToString();
        }
        #endregion

        #region 单个字符转换成大写
        /// <summary>
        /// 单个字符转换成大写
        /// </summary>
        private static string ToCapital(string tag)
        {
            string result = "";
            switch (tag)
            {
                case "0":
                    result = "零";
                    break;
                case "1":
                    result = "壹";
                    break;
                case "2":
                    result = "贰";
                    break;
                case "3":
                    result = "叁";
                    break;
                case "4":
                    result = "肆";
                    break;
                case "5":
                    result = "伍";
                    break;
                case "6":
                    result = "陆";
                    break;
                case "7":
                    result = "柒";
                    break;
                case "8":
                    result = "捌";
                    break;
                case "9":
                    result = "玖";
                    break;
                case "亿":
                    result = "亿";
                    break;
                case "万":
                    result = "万";
                    break;
                case "千":
                    result = "仟";
                    break;
                case "百":
                    result = "佰";
                    break;
                case "十":
                    result = "拾";
                    break;
            }
            return result;
        }
        #endregion

        #region 获取函数的表达式
        /// <summary>
        /// 获取函数的表达式
        /// </summary>
        /// <param name="funName">函数名称</param>
        public static string GetFunExpr(string funName)
        {
            if (Properties.Settings.Default.funList != null)
            {
                for (int i = 0; i < Properties.Settings.Default.funList.Count; i++)
                {
                    if (Properties.Settings.Default.funList[i].Split('|')[0] == funName)
                    {
                        return Properties.Settings.Default.funList[i].Split('|')[2];
                    }
                }
            }
            return "";
        }
        #endregion

        #region 根据函数名获取函数全部信息
        public static string GetFunFull(string funName)
        {
            StringCollection funList = Properties.Settings.Default.funList;
            for (int i = 0; i < funList.Count; i++)
            {
                if (funList[i].Split('|')[0] == funName)
                {
                    return funList[i];
                }
            }
            return "";
        }
        #endregion

        #region 表达式输入框退格
        /// <summary>
        /// 退格
        /// </summary>
        public static void BackSpace(RichTextBox txtExp)
        {
            if (txtExp.SelectionLength > 0)//选择了字符串
            {
            }
            else//没有选择字符串
            {
                //要删除的是运算符，处理过后选择该运算符
                CalOpeList opeList = new CalOpeList();
                for (int i = 0; i < opeList.count; i++)
                {
                    if (txtExp.SelectionStart >= opeList.opeList[i].tag.Length
                        && opeList.opeList[i].tag == txtExp.Text.Substring(txtExp.SelectionStart - opeList.opeList[i].tag.Length, opeList.opeList[i].tag.Length))
                    {
                        txtExp.SelectionStart = txtExp.SelectionStart - opeList.opeList[i].tag.Length;
                        txtExp.SelectionLength = opeList.opeList[i].tag.Length;
                        break;
                    }
                }
            }

            int _cursorPos = txtExp.SelectionStart;
            if (txtExp.SelectionLength > 0)//选择了字符串
            {
                txtExp.Text = txtExp.Text.Remove(_cursorPos, txtExp.SelectionLength);
                txtExp.Focus();
                txtExp.SelectionStart = _cursorPos;
            }
            else//没有选择字符串
            {
                if (_cursorPos - 1 >= 0)
                {
                    txtExp.Text = txtExp.Text.Remove(_cursorPos - 1, 1);
                    txtExp.Focus();
                    txtExp.SelectionStart = _cursorPos - 1;
                }
                else
                {
                    txtExp.Focus();
                }
            }
        }
        #endregion

        #region 获取函数的搜索结果，且按照匹配程度从大到小排列
        /// <summary>
        /// 获取函数的搜索结果，且按照匹配程度从大到小排列
        /// </summary>
        /// <param name="funList">函数列表</param>
        /// <param name="searchKey">搜索关键字</param>
        /// <param name="searchFunSort">要搜索的函数类别</param>
        public static StringCollection GetFunSearchResult(StringCollection funList, string searchKey, string searchFunSort)
        {
            StringCollection result = new StringCollection();
            List<double> resultMatchList = new List<double>();//与result对应的匹配程度列表

            try
            {
                //逐个扫描函数列表，从后往前扫描
                for (int i = funList.Count - 1; i >= 0; i--)
                {
                    string[] fun = funList[i].Split('|');
                    string funName = fun[0].ToLower();//函数名称
                    string funDesc = fun[1].ToLower();//函数描述
                    string funParameter = fun[3].ToLower();//函数参数
                    string funSort = fun[4].ToLower();//函数类别
                    double howMatch = 0;//匹配程度

                    searchKey = searchKey.Trim().ToLower();
                    searchFunSort = searchFunSort.Trim().ToLower();
                    string funInfo = funName + funDesc + funParameter;//函数信息，包括名称、描述、参数
                    //根据函数类别和搜索关键字判断
                    if (funInfo.IndexOf(searchKey) != -1 //函数信息中存在搜索关键字
                        && (searchFunSort == "" //函数类别为空
                        || funSort == searchFunSort)) //函数类别等于要搜索的函数类别
                    {
                        if (funName.IndexOf(searchKey) != -1)//函数名称中存在搜索关键字
                        {
                            double d = searchKey.Length / (double)funName.Length;
                            if (d > howMatch)
                            {
                                howMatch = d + 1E-5;//使得函数名称、函数描述和参数列表在相同howMatch情况下，以函数名称的匹配为准
                            }
                        }
                        if (funDesc.IndexOf(searchKey) != -1)//函数描述中存在搜索关键字
                        {
                            double d = searchKey.Length / (double)funDesc.Length;
                            if (d > howMatch)
                            {
                                howMatch = d;
                            }
                        }
                        if (funParameter.IndexOf(searchKey) != -1)//函数参数列表中存在搜索关键字
                        {
                            double d = searchKey.Length / (double)funParameter.Length;
                            if (d > howMatch)
                            {
                                howMatch = d;
                            }
                        }

                        //添加到result中
                        if (result.Count == 0)
                        {
                            result.Add(funList[i]);
                            resultMatchList.Add(howMatch);
                        }
                        else
                        {
                            //插入到合适位置
                            int k = 0;
                            for (; k < result.Count; k++)
                            {
                                //如果比当前位置的函数的匹配程度高
                                if (howMatch > resultMatchList[k])
                                {
                                    result.Insert(k, funList[i]);
                                    resultMatchList.Insert(k, howMatch);
                                    break;
                                }
                            }
                            if (k == result.Count)//前面没有找到对应的插入位置
                            {
                                result.Add(funList[i]);
                                resultMatchList.Add(howMatch);
                            }
                        }
                    }//end if
                }//end for
            }
            catch
            {
            }

            return result;
        }
        #endregion

        #region 判断程序能否继续使用
        /// <summary>
        /// 判断程序能否继续使用
        /// </summary>
        public static bool IsProgramPassValidate()
        {
            try
            {
                //如果程序没有注册
                if (!IsRegister())
                {
                    #region 判断程序第一次运行的标志是否存在
                    if (!FirstRunFlagExists())//标志不存在
                    {
                        return false;
                    }
                    #endregion

                    #region 判断用户有没有修改系统日期
                    //获取最后一次运行的日期
                    DateTime lastRunDate = Convert.ToDateTime(FileOperator.GetValue("LastRunDate"));
                    DateTime lastRunDate2 = Convert.ToDateTime(DESEncrypt.Decrypt(RegisterOperator.GetRegData(DESEncrypt.Encrypt("LastRunDate", Common.DESKey)), Common.DESKey));
                    if (!lastRunDate.Equals(lastRunDate2))
                    {
                        return false;
                    }

                    //如果当前日期早于程序最后一次运行的日期，说明用户修改了系统日期
                    if (DateTime.Now.Date.CompareTo(lastRunDate) < 0)
                    {
                        //如果用户修改了系统日期，则回返false
                        return false;
                    }
                    #endregion

                    #region 判断有没有超过试用期
                    //读取程序第一次运行的日期
                    DateTime dtFirstRunDate = Convert.ToDateTime(FileOperator.GetValue("FirstRunDate"));
                    DateTime dtFirstRunDate2 = Convert.ToDateTime(DESEncrypt.Decrypt(RegisterOperator.GetRegData(DESEncrypt.Encrypt("FirstRunDate", Common.DESKey)), Common.DESKey));
                    if (!dtFirstRunDate.Equals(dtFirstRunDate2))
                    {
                        return false;
                    }
                    //如果超过试用日期
                    if (DateTime.Now.Date.Subtract(dtFirstRunDate).Days > Common.probationalTime)
                    {
                        return false;
                    }
                    #endregion

                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region 判断程序有没有注册
        /// <summary>
        /// 判断程序有没有注册
        /// </summary>
        public static bool IsRegister()
        {
            try
            {
                //判断注册表中的注册码是否与根据本机机器码生成的注册码相同
                if (GetRegisterCodeFromRegTable() == CreateRegisterCodeFromMachineCode())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 获取注册表中的注册码
        /// <summary>
        /// 获取注册表中的注册码
        /// </summary>
        public static string GetRegisterCodeFromRegTable()
        {
            try
            {
                //判断注册信息是否存在
                if (RegisterOperator.IsRegDataExists(DESEncrypt.Encrypt("RegisterInfo", Common.DESKey)))
                {
                    //读取注册信息，并解密
                    return DESEncrypt.Decrypt(RegisterOperator.GetRegData(DESEncrypt.Encrypt("RegisterInfo", Common.DESKey)), Common.DESKey);
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 根据本机机器码，生成注册码
        /// <summary>
        /// 根据本机机器码，生成注册码
        /// </summary>
        public static string CreateRegisterCodeFromMachineCode()
        {
            //获取本机机器码，对机器码加密
            string str = DESEncrypt.Encrypt(HardwareInfoClass.GetMachineCode(), Common.DESKey);
            //计算其MD5值
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.Default.GetBytes(str));

            StringBuilder registerCode = new StringBuilder();
            foreach (byte b in md5.Hash)
            {
                registerCode.AppendFormat("{0:X2}", b);
            }

            return registerCode.ToString();
        }
        #endregion

        #region  退出程序，并关闭所有窗体
        /// <summary>
        /// 退出程序，并关闭所有窗体
        /// </summary>
        public static void ExitProgram()
        {
            try
            {
                for (int i = 0; i < Application.OpenForms.Count; i++)
                {
                    Application.OpenForms[i].Close();
                    Application.OpenForms[i].Dispose();
                }
                Application.Exit();
            }
            catch
            {
            }
        }
        #endregion

        #region 记录程序第一次运行的标志
        /// <summary>
        /// 记录程序第一次运行的标志
        /// </summary>
        public static void MemorizeFirstRunFlag()
        {

            #region 使用文件夹记录
            //创建文件夹
            string path = DESEncrypt.Decrypt("4C88E64EAF09C20619D20AFBDDB8DE3847C5EB972C6103A3C4C2B2B829CF0CFF", Common.DESKey);
            Directory.CreateDirectory(path);
            //创建子文件夹
            string subPath = path + "\\" + "ã";
            Directory.CreateDirectory(subPath);
            //创建文件
            string filePathName = subPath + "\\" + "abc.dat";
            FileStream fileStream = File.Create(filePathName);
            fileStream.Close();
            fileStream.Dispose();
            //写入文件
            StreamWriter streamWriter = new StreamWriter(filePathName);
            streamWriter.Write("abc");
            streamWriter.Close();
            streamWriter.Dispose();
            //伪装文件属性
            DateTime dt = DateTime.Now.AddYears(-3);
            File.SetCreationTime(filePathName, dt);
            File.SetLastAccessTime(filePathName, dt);
            File.SetLastWriteTime(filePathName, dt);
            //伪装子文件夹属性
            dt = DateTime.Now.AddYears(-3);
            Directory.SetCreationTime(subPath, dt);
            Directory.SetLastAccessTime(subPath, dt);
            Directory.SetLastWriteTime(subPath, dt);
            //伪装文件夹属性
            dt = DateTime.Now.AddYears(-3);
            Directory.SetCreationTime(path, dt);
            Directory.SetLastAccessTime(path, dt);
            Directory.SetLastWriteTime(path, dt);
            //隐藏子文件夹
            DirectoryInfo dirInfo = new DirectoryInfo(subPath);
            dirInfo.Attributes = FileAttributes.Hidden | FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Archive;
            //隐藏文件夹
            dirInfo = new DirectoryInfo(path);
            dirInfo.Attributes = FileAttributes.Hidden | FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Archive;
            //设置子文件夹权限
            DirectorySecurity dirSecurity = Directory.GetAccessControl(subPath);
            FileSystemAccessRule fsAccessRule = new FileSystemAccessRule("Everyone",
                FileSystemRights.FullControl,
                AccessControlType.Deny);
            dirSecurity.SetAccessRule(fsAccessRule);
            Directory.SetAccessControl(subPath, dirSecurity);
            //设置文件夹权限
            dirSecurity = Directory.GetAccessControl(path);
            fsAccessRule = new FileSystemAccessRule("Everyone",
              FileSystemRights.FullControl,
              AccessControlType.Deny);
            dirSecurity.SetAccessRule(fsAccessRule);
            Directory.SetAccessControl(path, dirSecurity);
            #endregion

        }
        #endregion

        #region 判断程序第一次运行的标志是否存在
        /// <summary>
        /// 判断程序第一次运行的标志是否存在
        /// </summary>
        public static bool FirstRunFlagExists()
        {

            #region 读取文件夹标志
            string path = DESEncrypt.Decrypt("4C88E64EAF09C20619D20AFBDDB8DE3847C5EB972C6103A3C4C2B2B829CF0CFF", Common.DESKey);
            if (Directory.Exists(path))//文件夹标志存在
            {
                return true;
            }
            else//文件夹标志不存在
            {
                return false;
            }
            #endregion

        }
        #endregion

        #region 软件自动注册
        /// <summary>
        /// 先判断软件有没有注册，如果没有注册，则自动注册该软件
        /// 如果软件已注册，则不会执行注册信息写入操作
        /// </summary>
        public static void AutoRegister()
        {
            if (!Common.IsRegister())
            {
                //注册信息写入注册表
                RegisterOperator.SetRegData(DESEncrypt.Encrypt("RegisterInfo", Common.DESKey), DESEncrypt.Encrypt(Common.CreateRegisterCodeFromMachineCode(), Common.DESKey));
            }
        }
        #endregion

    }
}
