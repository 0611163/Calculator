/////////////////////////////////////////////////
//说明：请谨慎修改
/////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 计算器
{
    public partial class Form1 : Form
    {
        #region 变量
        private int _selectionStartPos = 0;
        private string ANS = null;
        private CalResult result;
        public static Font font;
        private bool isScientificShow = false;
        private bool isKeyDown = false;
        public static bool refreshRichTextBoxContextMenu = false;
        private AddFun addFun = null;
        private int completeSteps = 0;//单步计算已完成的步数
        private string memoryExp = null;
        private bool hasAnnotate = false;//表达式是否有注释，默认没有注释
        private Help help = null;
        private List<string> memoryExprs = new List<string>();
        private int memoryExprsCursor = -1;
        private int memoryExprsMaxCount = 100;
        private List<string> memoryExprs_Cal = new List<string>();
        private int memoryExprsCursor_Cal = -1;
        private int memoryExprsMaxCount_Cal = 100;
        #endregion

        public Form1()
        {
            InitializeComponent();

            #region 初始化
            //记录表达式
            MemoryExpr("");
            //记录font
            font = txtExp.Font;

            //调整Form1的高度
            this.Height -= 195;

            //设置文本框背景色
            txtExp.BackColor = Color.FromArgb(204, 232, 207);
            txtResult.BackColor = Color.FromArgb(204, 232, 207);
            txtSearchKey.BackColor = Color.FromArgb(204, 232, 207);

            //为控件添加事件
            foreach (Control ctl in this.panel1.Controls)
            {
                ctl.Click += new System.EventHandler(this.panel1Controls_Click);
            }
            foreach (Control ctl in this.panel2.Controls)
            {
                ctl.Click += new System.EventHandler(this.panel2Controls_Click);
            }
            this.btnEqual.Click += new System.EventHandler(this.btnEqual_Click);

            //插入变量菜单
            InsertVarsItem();
            //更新算式菜单
            UpdateExpItems();
            //更新算式输入框右键菜单
            UpdateMenuRichTextBox();

            //发音相关设置
            this.语音ToolStripMenuItem1.Checked = Properties.Settings.Default.isSpeech;
            this.自然读音ToolStripMenuItem.Checked = Properties.Settings.Default.isNaturalSpeech;
            if (this.自然读音ToolStripMenuItem.Checked)
            {
                Common.groupLength = 4;
            }
            #endregion

        }

        #region Form1_Load
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region 等于
        /// <summary>
        /// 等于
        /// </summary>
        private void btnEqual_Click(object sender, EventArgs e)
        {
            //判断程序能否继续使用
            if (!Common.IsProgramPassValidate())
            {
                try
                {
                    RegisterForm regForm = new RegisterForm();
                    regForm.ShowDialog(this);
                    return;
                }
                catch
                {
                    return;
                }
            }

            try
            {
                if (this.语音ToolStripMenuItem1.Checked)
                {
                    //中止之前的计算结果发音
                }

                if (txtExp.Text.IndexOf("=") != -1)//存储变量
                {
                    memoryVars();
                }
                else//计算
                {
                    result = Calculate();//计算
                    ANS = result.GeneralResultToShow;//保存计算结果
                    txtResult.Text = ANS;//显示计算结果

                    //计算结果发音
                    if (this.语音ToolStripMenuItem1.Checked)
                    {
                        //起动线程发音
                        Task.Run(() => SpeechResult());
                    }
                }
                txtExp.Focus();
                txtExp.SelectionStart = txtExp.Text.Length;
                isScientificShow = false;
                //记录表达式
                MemoryExpr_Cal();
            }
            catch (Exception ex)
            {
                //计算结果框中显示出错信息
                ShowException(ex);
            }
        }
        #endregion

        #region 计算结果框中显示出错信息
        /// <summary>
        /// 计算结果框中显示出错信息
        /// </summary>
        private void ShowException(Exception ex)
        {
            try
            {
                if (hasAnnotate)
                {
                    //删除注释
                    txtExp.Text = Common.DeleteAnnotate(txtExp.Text, out hasAnnotate);
                    hasAnnotate = false;//重置
                }

                if (txtExp.Text.IndexOf("=") != -1)
                {
                    txtExp.Text = txtExp.Text.Substring(txtExp.Text.IndexOf("=") + 1);
                }

                string[] exSplit = ex.Message.Split('|');
                if (exSplit.Length >= 3)
                {
                    //如果调用了Common.SetWrongOpeColor将不再调用Common.SetColor
                    Common.Lock();
                    //设置出错运算符的颜色
                    Common.SetWrongOpeColor(exSplit[exSplit.Length - 2].Split(':')[1],
                        int.Parse(exSplit[exSplit.Length - 1].Split(':')[1]),
                        txtExp);
                    Common.UnLock();
                }

                StringBuilder exMessage = new StringBuilder();
                for (int i = 0; i < exSplit.Length; i++)
                {
                    if (exSplit[i].IndexOf(':') != 0)//开头没有':'号
                    {
                        exMessage.Append(exSplit[i]);
                    }
                }
                exMessage.Append(" （错误提示仅供参考）");
                txtResult.Text = exMessage.ToString();
                txtExp.Focus();
                txtExp.SelectionStart = txtExp.Text.Length;


            }
            catch { }
        }
        #endregion

        #region 计算表达式
        /// <summary>
        /// 计算表达式
        /// </summary>
        private CalResult Calculate()
        {
            CalValList valList = null;
            //记录表达式
            memoryExp = txtExp.Text;
            //删除注释
            txtExp.Text = Common.DeleteAnnotate(txtExp.Text, out hasAnnotate);
            //生成运算因子列表
            valList = new CalValList(txtExp.Text);
            //重新显示表达式
            if (hasAnnotate)//有注释
            {
                txtExp.Text = memoryExp;
            }
            else//没有注释
            {
                txtExp.Text = valList.nomalStringExpression;
            }

            CalResult result = null;
            if (this.单步计算ToolStripMenuItem.Checked)//单步计算
            {
                completeSteps++;
                for (int i = 0; i < completeSteps; i++)
                {
                    //优先级列表为空的情况
                    if (valList.levelList.Count == 0)
                    {
                        result = valList.CalculateOneStep();
                    }

                    if (valList.levelList.Count > 0)//如果优先级尚未处理完毕
                    {
                        result = valList.CalculateOneStep();
                    }
                }
            }
            else//一次性计算最终结果
            {
                result = valList.Calculate();//计算
            }

            return result;
        }
        #endregion

        #region 计算结果发音
        /// <summary>
        /// 计算结果发音
        /// </summary>
        public async Task SpeechResult()
        {
            //等于号发音
            await Common.Speech("等于");

            double doubleResult;
            string strResult = txtResult.Text.Replace(" ", "");
            //如果结果不可转换成数字
            if (!double.TryParse(strResult, out doubleResult))
            {
                return;
            }

            if (this.自然读音ToolStripMenuItem.Checked
                && strResult.IndexOf(",") == -1//只有一个计算结果
                && doubleResult.ToString().IndexOf("E") == -1)//不是科学计数法的情况
            {
                strResult = Common.ToNaturalSpeech(doubleResult);
            }

            //逐个扫描
            for (int i = 0; i < strResult.Length; i++)
            {
                string tag = strResult[i].ToString();

                int a;
                //是数字
                if (int.TryParse(tag, out a))
                {
                    tag = "_" + tag;
                }

                //发音
                await Common.Speech(tag);
            }//end for
        }
        #endregion

        #region txtExp_TextChanged
        /// <summary>
        /// txtExp文本框事件
        /// </summary>
        private void txtExp_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _selectionStartPos = txtExp.SelectionStart;
                //如果调用了Common.SetWrongOpeColor将不再调用Common.SetColor
                if (!Common._lock)
                {
                    Common.SetColor(txtExp.Text, txtExp);
                }
                txtExp.SelectionStart = _selectionStartPos;

                //清空计算结果
                if (txtExp.Text.Trim() == "")
                {
                    result = null;
                }

                //发音
                if (this.语音ToolStripMenuItem1.Checked)
                {
                    //中止计算结果发音

                    if (this.isKeyDown)
                    {
                        string tag = txtExp.Text[txtExp.SelectionStart - 1].ToString();
                        switch (tag)
                        {
                            case "0":
                            case "1":
                            case "2":
                            case "3":
                            case "4":
                            case "5":
                            case "6":
                            case "7":
                            case "8":
                            case "9":
                                tag = "_" + tag;
                                break;
                            case ".":
                                tag = "点";
                                break;
                            case "+":
                                tag = "加上";
                                break;
                            case "-":
                                //是负号
                                if (Common.isSubtractive(txtExp.Text, tag, txtExp.SelectionStart))
                                {
                                    tag = "负";
                                }
                                else
                                {
                                    tag = "减去";
                                }
                                break;
                            case "*":
                                tag = "乘以";
                                break;
                            case "/":
                                tag = "除以";
                                break;
                        }
                        Common.Speech(tag);
                        this.isKeyDown = false;
                    }
                }
            }
            catch { }
        }
        #endregion

        #region 科学计数法ToolStripMenuItem_Click
        /// <summary>
        /// 转换为科学计数法
        /// </summary>
        private void 科学计数法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (!isScientificShow)
                {
                    txtResult.Text = result.ScientificResultToShow;
                }
                else
                {
                    txtResult.Text = result.GeneralResultToShow;
                }
                isScientificShow = !isScientificShow;
            }
            catch { }
        }
        #endregion

        #region txtExp_KeyDown
        /// <summary>
        /// 键盘事件
        /// </summary>
        private void txtExp_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyData)
                {
                    case Keys.Enter:
                        btnEqual_Click(sender, e);
                        break;
                    case Keys.F4:
                        txtExp.Text = "";
                        txtResult.Text = "";
                        completeSteps = 0;//重置completeSteps
                        break;
                    case Keys.Back:
                        completeSteps = 0;//重置completeSteps
                        Common.BackSpace(txtExp);
                        e.Handled = true;//屏蔽键盘的回退键
                        break;
                    case Keys.Up:
                        if (memoryExprs.Count > 0)
                        {
                            memoryExprsCursor = memoryExprsCursor - 1 >= 0 ? memoryExprsCursor - 1 : 0;
                            txtExp.Text = memoryExprs[memoryExprsCursor];
                            txtExp.SelectionStart = txtExp.Text.Length;
                        }
                        break;
                    case Keys.Down:
                        if (memoryExprs.Count > 0)
                        {
                            memoryExprsCursor = memoryExprsCursor + 1 <= memoryExprs.Count - 1 ? memoryExprsCursor + 1 : memoryExprs.Count - 1;
                            txtExp.Text = memoryExprs[memoryExprsCursor];
                            txtExp.SelectionStart = txtExp.Text.Length;
                        }
                        break;
                    default:
                        if (!e.Control)
                        {
                            this.isKeyDown = true;//按键发音标识
                        }
                        completeSteps = 0;//重置completeSteps
                        break;
                }

                if (e.Control && (e.KeyCode == Keys.Up))
                {
                    if (memoryExprs_Cal.Count > 0)
                    {
                        memoryExprsCursor_Cal = memoryExprsCursor_Cal - 1 >= 0 ? memoryExprsCursor_Cal - 1 : 0;
                        txtExp.Text = memoryExprs_Cal[memoryExprsCursor_Cal];
                        txtExp.SelectionStart = txtExp.Text.Length;
                    }
                    e.Handled = true;
                }

                if (e.Control && (e.KeyCode == Keys.Down))
                {
                    if (memoryExprs_Cal.Count > 0)
                    {
                        if (memoryExprsCursor_Cal == memoryExprs_Cal.Count)
                        {
                            memoryExprsCursor_Cal = -1;
                        }
                        memoryExprsCursor_Cal = memoryExprsCursor_Cal + 1 <= memoryExprs_Cal.Count - 1 ? memoryExprsCursor_Cal + 1 : memoryExprs_Cal.Count - 1;
                        txtExp.Text = memoryExprs_Cal[memoryExprsCursor_Cal];
                        txtExp.SelectionStart = txtExp.Text.Length;
                    }
                    e.Handled = true;
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 对话框
        /// <summary>
        /// 帮助
        /// </summary>
        private void 帮助ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (help == null || help.IsDisposed)
            {
                help = new Help();
                help.Show();
            }
            else
            {
                help.Focus();
            }
        }

        /// <summary>
        /// 关于对话框
        /// </summary>
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog();
        }

        /// <summary>
        /// 存储函数
        /// </summary>
        private void 存储函数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //判断程序能否继续使用
            if (Common.IsProgramPassValidate())
            {
                if (addFun == null || addFun.IsDisposed)
                {
                    addFun = new AddFun();
                    addFun.Show();
                }
                else
                {
                    addFun.Focus();
                }
            }
            else
            {
                try
                {
                    RegisterForm regForm = new RegisterForm();
                    regForm.ShowDialog(this);
                    return;
                }
                catch
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        private void 注册ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RegisterForm regForm = new RegisterForm();
            regForm.ShowDialog(this);
        }
        #endregion

        #region btnBackspace_Click
        /// <summary>
        /// 退格
        /// </summary>
        private void btnBackspace_Click(object sender, EventArgs e)
        {
            completeSteps = 0;//重置completeSteps
            Common.BackSpace(txtExp);
            //记录表达式
            MemoryExpr(txtExp.Text);
            //更新计算过的表达式游标
            memoryExprsCursor_Cal = memoryExprs_Cal.Count;
        }
        #endregion

        #region 清除
        /// <summary>
        /// 清除
        /// </summary>
        private void btnC_Click(object sender, EventArgs e)
        {
            txtResult.Text = "";
            txtExp.Text = "";
            txtExp.Focus();
            //记录表达式
            MemoryExpr(txtExp.Text);
            //更新计算过的表达式游标
            memoryExprsCursor_Cal = memoryExprs_Cal.Count;
        }
        #endregion

        #region Input
        /// <summary>
        /// 输入字符串到文本框中
        /// </summary>
        private void Input(string str)
        {
            int _cursorPos = txtExp.SelectionStart;
            if (txtExp.SelectionLength > 0)
            {
                txtExp.Text = txtExp.Text.Remove(_cursorPos, txtExp.SelectionLength);
            }
            txtExp.Text = txtExp.Text.Insert(_cursorPos, str);
            txtExp.Focus();
            txtExp.SelectionStart = _cursorPos + str.Length;

            completeSteps = 0;//重置completeSteps

            //记录表达式
            MemoryExpr(txtExp.Text);
            //更新计算过的表达式游标
            memoryExprsCursor_Cal = memoryExprs_Cal.Count;
        }
        #endregion

        #region 输入上次运算结果
        /// <summary>
        /// 输入上次运算结果
        /// </summary>
        private void btnANS_Click(object sender, EventArgs e)
        {
            Input(ANS);
        }
        #endregion

        #region 菜单事件
        /// <summary>
        /// 移动光标
        /// </summary>
        private void 移动光标ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtExp.SelectionStart != 0)
            {
                txtExp.SelectionStart = 0;
            }
            else
            {
                txtExp.SelectionStart = txtExp.Text.Length;
            }
        }

        //人民币大写形式
        private void 人民币大写ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtResult.Text.Trim() != "")
                {
                    txtResult.Text = Common.ToChineseCapital(txtResult.Text);
                }
            }
            catch (Exception exp)
            {
                txtResult.Text = exp.Message;
            }
        }

        //人民币阿拉伯数字形式
        private void 人民币阿拉伯数字形式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtResult.Text.Trim() != "")
                {
                    txtResult.Text = "￥" + double.Parse(txtResult.Text.Replace(" ", "")).ToString("0.00");
                }
            }
            catch
            {
                txtResult.Text = "存在非法字符，无法转换";
            }
        }

        /// <summary>
        /// 科学型
        /// </summary>
        private void 科学型ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.科学型ToolStripMenuItem.Text == "科学型")
            {
                this.科学型ToolStripMenuItem.Text = "标准型";
                panel1.Location = new Point(panel1.Left, panel1.Top + 157);
                panel2.Location = new Point(panel2.Left, panel2.Top - 245);
                this.Height += 155;
            }
            else
            {
                this.科学型ToolStripMenuItem.Text = "科学型";
                panel1.Location = new Point(panel1.Left, panel1.Top - 157);
                panel2.Location = new Point(panel2.Left, panel2.Top + 245);
                this.Height -= 155;
            }
        }

        /// <summary>
        /// 复制计算结果到剪切板
        /// </summary>
        private void 复制计算结果ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtResult.Text.Trim() != "")
            {
                Clipboard.SetText(txtResult.Text.Replace(" ", ""));
                txtResult.Text += " 结果已复制到剪贴板";
            }
        }

        //存储算式
        private void 存储计算公式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Calculate();//计算，若表达式不错则抛出异常
                //存储算式
                MemoryExps();
            }
            catch (Exception ex)
            {
                //计算结果框中显示出错信息
                ShowException(new Exception("存储失败，失败原因：" + ex.Message));
            }
        }

        /// <summary>
        /// 在表达式两端输入括号
        /// </summary>
        private void 双括号ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtExp.Text.Trim() != "")
            {
                txtExp.Text = txtExp.Text.Insert(0, "(");
                txtExp.Text += ")";
                txtExp.Focus();
                txtExp.SelectionStart = txtExp.Text.Length;
            }
        }

        //获取剪切板内容
        private void 获取剪切板内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input(Clipboard.GetText());
        }

        private void 求和ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("sum()");
            txtExp.SelectionStart--;
        }
        private void 求平均ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("avg()");
            txtExp.SelectionStart--;
        }
        #endregion

        #region 为panel1和panel2中的控件添加Click事件
        /// <summary>
        /// 为panel1中的控件添加Click事件
        /// </summary>
        private void panel1Controls_Click(object sender, EventArgs e)
        {
            InputOpe(sender);
            AddSpeech(sender);
        }

        /// <summary>
        /// 为panel2中的控件添加Click事件
        /// </summary>
        private void panel2Controls_Click(object sender, EventArgs e)
        {
            InputOpe(sender);
            AddSpeech(sender);
        }

        /// <summary>
        /// 输入运算符
        /// </summary>
        /// <param name="sender"></param>
        private void InputOpe(object sender)
        {
            string[] tagList = ((Control)sender).Tag.ToString().Split('|');

            //输入运算符
            if (tagList[0] != "")
            {
                string str = tagList[0];

                //对于函数，输入左括号
                CalOpeList opeList = new CalOpeList();
                if (opeList.GetOpe(((Control)sender).Tag.ToString()) != null
                    && opeList.GetOpe(((Control)sender).Tag.ToString()).opeType == OpeType.right)
                {
                    str += "(";
                }

                //输入运算符
                Input(str);
            }
        }
        #endregion

        #region 按键发音
        /// <summary>
        /// 添加按键发音
        /// </summary>
        private void AddSpeech(object sender)
        {
            try
            {
                if (this.语音ToolStripMenuItem1.Checked == true)
                {
                    string[] tagList = ((Control)sender).Tag.ToString().Split('|');

                    //中止计算结果发音

                    //存在发音标识则发音
                    if (tagList.Length == 2)
                    {
                        //是负号
                        if (Common.isSubtractive(txtExp.Text, tagList[0], txtExp.SelectionStart))
                        {
                            Common.Speech("负");
                        }
                        else//不是负号
                        {
                            Common.Speech(tagList[1]);
                        }
                    }
                }
            }
            catch { }
        }
        #endregion

        #region 单位换算菜单事件
        private void 海里公里ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("conv(海里,公里,)");
            txtExp.SelectionStart--;
        }
        private void 光年公里ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("conv(光年,公里,)");
            txtExp.SelectionStart--;
        }
        private void 平方公里亩ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("conv(平方公里,亩,)");
            txtExp.SelectionStart--;
        }
        private void 亩平方公里ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("conv(亩,平方公里,)");
            txtExp.SelectionStart--;
        }
        private void 千米每小时米每秒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("conv(千米每小时,米每秒,)");
            txtExp.SelectionStart--;
        }
        private void 米每秒千米每小时ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("conv(米每秒,千米每小时,)");
            txtExp.SelectionStart--;
        }
        private void 英里公里ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("conv(英里,公里,)");
            txtExp.SelectionStart--;
        }
        private void 英寸厘米ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("conv(英寸,厘米,)");
            txtExp.SelectionStart--;
        }

        private void 亩平方米ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("conv(亩,平方米,)");
            txtExp.SelectionStart--;
        }

        private void 桶立方米ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("conv(桶,立方米,)");
            txtExp.SelectionStart--;
        }

        private void 马赫米每秒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input("conv(马赫,米每秒,)");
            txtExp.SelectionStart--;
        }
        #endregion

        #region 读取变量
        private void 读取变量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Input(((ToolStripMenuItem)sender).Text.Split(':')[0]);
        }
        #endregion

        #region 存储变量
        /// <summary>
        /// 存储变量
        /// </summary>
        private void memoryVars()
        {
            string[] variables = Properties.Settings.Default.strVariables.Split('|');
            //进入该函数说明'='存在，下面两条语句不会出错
            string leftValue = txtExp.Text.Split('=')[0];
            string rightValue = txtExp.Text.Split('=')[1];
            //等号右边计算结果
            string rightResult = "";
            //支持存储15个变量
            int maxVarLength = 30;

            #region 赋值语句不合法的情况
            //确保只有一个'='
            if (txtExp.Text.Split('=').Length > 2)
            {
                txtResult.Text = "变量赋值语句不合法";
                return;
            }
            //左右值存在
            if (txtExp.Text.Split('=')[0].Trim() == ""
                || txtExp.Text.Split('=')[1].Trim() == "")
            {
                txtResult.Text = "变量赋值语句不合法";
                return;
            }
            //变量名称不合法的情况
            //变量名称中不能存在单引号
            if (leftValue.IndexOf("'") != -1)
            {
                txtResult.Text = "变量赋值语句不合法";
                return;
            }
            double d;
            //变量名称不能全为数字
            if (double.TryParse(leftValue, out d)
                || double.TryParse(leftValue.Substring(0, 1), out d))
            {
                txtResult.Text = "变量名称不合法";
                return;
            }
            //单引号必须成对出现，且最多只能有一对
            if (rightValue.Split('\'').Length != 1
                && rightValue.Split('\'').Length != 3)
            {
                txtResult.Text = "变量赋值语句不合法";
                return;
            }
            #endregion

            //计算等号右边
            if (rightValue[0] == '\'' && rightValue[rightValue.Length - 1] == '\'')
            {
                rightResult = rightValue.Replace("'", "");
            }
            else
            {
                CalValList valList = new CalValList(rightValue);
                rightResult = valList.Calculate().GeneralResultToShow.Replace(" ", "");
                txtExp.Text = leftValue + "=" + valList.nomalStringExpression;
            }

            if (variables.Length == 1)//特殊情况
            {
                Properties.Settings.Default.strVariables = leftValue + "|" + rightResult;
                Properties.Settings.Default.Save();
                txtResult.Text = "变量'" + leftValue + "'已赋值为'" + rightResult + "'";
            }
            else
            {

                #region 查找是否存在同名变量或同名运算符或常数
                bool homonymyOpe = false;
                bool varExists = false;
                int existsVarPos = -1;//同名变量位置

                //是否存在同名变量
                for (int i = 0; i < variables.Length; i += 2)
                {
                    //找到同名变量
                    if (variables[i] == leftValue)
                    {
                        existsVarPos = i;
                        varExists = true;
                        break;
                    }
                }

                //是否存在同名运算符 
                CalOpeList opeList = new CalOpeList();
                for (int i = 0; i < opeList.count; i++)
                {
                    //找到同名运算符
                    if (opeList.opeList[i].tag == leftValue)
                    {
                        homonymyOpe = true;
                        break;
                    }
                }
                #endregion

                #region 存在同名变量，替换变量
                if (varExists)
                {
                    if (MessageBox.Show("变量'" + leftValue + "'已存在，且它的值为'"
                        + variables[existsVarPos + 1] + "'，是否替换？", "提示", MessageBoxButtons.OKCancel)
                        == DialogResult.OK)
                    {
                        variables[existsVarPos + 1] = rightResult;
                        string s = variables[0];
                        for (int i = 1; i < variables.Length; i++)
                        {
                            s += "|" + variables[i];
                        }
                        Properties.Settings.Default.strVariables = s;
                        Properties.Settings.Default.Save();
                        txtResult.Text = "变量'" + leftValue + "'的值已替换为'" + rightResult + "'";
                    }
                }
                #endregion

                #region 存在同名常数或运算符
                else if (homonymyOpe)
                {
                    txtResult.Text = "存储变量失败！存在同名常数或运算符";
                }
                #endregion

                #region 不存在同名变量、常数或运算符，插入变量
                else
                {
                    //列表variables的开始读取位置
                    int startPos = -1;
                    //是否超出支持的最大变量个数
                    if (variables.Length < maxVarLength)
                    {
                        startPos = 0;
                    }
                    else
                    {
                        startPos = 2;
                        if (MessageBox.Show("存储的变量已达到最大个数\n\n若要添加新的变量，则需要删除旧变量'"
                            + variables[0] + "'(它的值为" + variables[1] + ")\n\n是否删除？",
                            "提示", MessageBoxButtons.OKCancel)
                            == DialogResult.Cancel)
                        {
                            txtResult.Text = "变量未存储！";
                            return;
                        }

                    }

                    string s = variables[startPos];
                    for (int i = startPos + 1; i < variables.Length; i++)
                    {
                        s += "|" + variables[i];
                    }

                    Properties.Settings.Default.strVariables = s
                            + "|" + leftValue + "|" + rightResult;
                    Properties.Settings.Default.Save();
                    txtResult.Text = "变量'" + leftValue + "'已赋值为'" + rightResult + "'";
                }
                #endregion

            }

            //更新变量菜单
            this.变量ToolStripMenuItem.DropDownItems.Clear();
            InsertVarsItem();
            //更新算式输入框右键菜单
            UpdateMenuRichTextBox();
            //更新文本框
            txtExp.Text = txtExp.Text;
        }
        #endregion

        #region 删除所有变量
        /// <summary>
        /// 删除所有变量
        /// </summary>
        private void 删除所有变量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("删除所有变量？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Properties.Settings.Default.strVariables = "";
                Properties.Settings.Default.Save();
                RemoveVarsItem();
                //更新算式输入框右键菜单
                UpdateMenuRichTextBox();
                txtResult.Text = "已删除所有变量";
            }
        }
        #endregion

        #region 插入变量菜单
        /// <summary>
        /// 插入变量菜单
        /// </summary>
        private void InsertVarsItem()
        {
            string[] variables = Properties.Settings.Default.strVariables.Split('|');
            if (variables.Length < 2)//没有变量
            {
                ToolStripMenuItem noVar = new ToolStripMenuItem("没有变量");
                this.变量ToolStripMenuItem.DropDownItems.Add(noVar);
                return;
            }
            ToolStripMenuItem[] items = new ToolStripMenuItem[variables.Length / 2];

            for (int i = 0; i < variables.Length / 2; i++)
            {
                items[i] = new ToolStripMenuItem(variables[i * 2] + ":" + variables[i * 2 + 1]);
                items[i].Click += new System.EventHandler(this.读取变量ToolStripMenuItem_Click);
                items[i].MouseDown += new MouseEventHandler(this.VarsItem_MouseDown);
                this.变量ToolStripMenuItem.DropDownItems.Insert(0, items[i]);
            }

            this.变量ToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            ToolStripMenuItem removeVars = new ToolStripMenuItem("删除所有变量");
            removeVars.Click += new System.EventHandler(this.删除所有变量ToolStripMenuItem_Click);
            this.变量ToolStripMenuItem.DropDownItems.Add(removeVars);
        }
        #endregion

        #region 存储算式
        /// <summary>
        /// 存储算式
        /// </summary>
        private void MemoryExps()
        {
            //没有输入任何表达式
            if (txtExp.Text.Trim() == "")
            {
                return;
            }

            //获取已存在的算式
            string exps = Properties.Settings.Default.strExps;

            //若算式已存在
            if (exps.IndexOf(txtExp.Text) == 0)
            {
                txtResult.Text = "算式已存在！";
                return;
            }

            if (exps == "")//没有算式
            {
                Properties.Settings.Default.strExps = txtExp.Text;
                txtResult.Text = "算式已存储！";
            }
            else if (exps.Split('|').Length >= 15)//最多存储15个
            {
                if (MessageBox.Show("存储的算式已达到最大个数\n\n若要添加新的算式，则需要删除旧算式'"
                    + exps.Split('|')[exps.Split('|').Length - 1] + "\n\n是否删除？",
                    "提示", MessageBoxButtons.OKCancel)
                    == DialogResult.Cancel)
                {
                    txtResult.Text = "算式未存储！";
                    return;
                }

                Properties.Settings.Default.strExps = txtExp.Text + "|"
                    + exps.Substring(0, exps.LastIndexOf("|"));
                txtResult.Text = "算式已存储！";
            }
            else//已存在一些算式
            {
                Properties.Settings.Default.strExps = txtExp.Text + "|" + exps;
                txtResult.Text = "算式已存储！";
            }

            Properties.Settings.Default.Save();//保存修改
            //更新算式菜单
            UpdateExpItems();
            //更新算式输入框右键菜单
            UpdateMenuRichTextBox();
        }
        #endregion

        #region 更新算式菜单
        /// <summary>
        /// 更新算式菜单
        /// </summary>
        private void UpdateExpItems()
        {
            //先删除所有菜单项
            this.算式ToolStripMenuItem.DropDownItems.Clear();
            //没有算式的情况
            if (Properties.Settings.Default.strExps == "")
            {
                this.算式ToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem("没有算式"));
                return;
            }
            //获取算式列表
            string[] exps = Properties.Settings.Default.strExps.Split('|');

            ToolStripMenuItem[] items = new ToolStripMenuItem[exps.Length];

            //逐个扫描已存在的算式
            for (int i = 0; i < exps.Length; i++)
            {
                items[i] = new ToolStripMenuItem(exps[i]);
                items[i].Click += new System.EventHandler(this.ExpsItem_Click);
                items[i].MouseDown += new MouseEventHandler(this.ExpsItem_MouseDown);
                this.算式ToolStripMenuItem.DropDownItems.Add(items[i]);
            }

            ToolStripMenuItem deleteExpItem = new ToolStripMenuItem("删除所有算式");
            deleteExpItem.Click += new System.EventHandler(this.删除所有算式ToolStripMenuItem_Click);
            this.算式ToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
            this.算式ToolStripMenuItem.DropDownItems.Add(deleteExpItem);
        }
        #endregion

        #region 删除一个算式
        private void ExpsItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string exp = ((ToolStripMenuItem)sender).Text;

                this.输入ToolStripMenuItem.HideDropDown();//防止弹出框被菜单覆盖
                this.menuRichTextBox.Hide();
                if (MessageBox.Show("删除算式'" + exp + "'？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    //替换
                    string str = Properties.Settings.Default.strExps.Replace(exp, "");

                    if (str.Length > 0)
                    {
                        //开头是'|'
                        if (str[0] == '|')
                        {
                            str = str.Substring(1);
                        }
                        //结尾是'|'
                        if (str[str.Length - 1] == '|')
                        {
                            str = str.Substring(0, str.Length - 1);
                        }
                    }
                    //两个'|'的情况
                    Properties.Settings.Default.strExps = str.Replace("||", "|");

                    Properties.Settings.Default.Save();//保存修改
                    //更新算式菜单
                    UpdateExpItems();
                    //更新算式输入框右键菜单
                    UpdateMenuRichTextBox();
                    txtResult.Text = "算式'" + exp + "'已删除";
                }
            }
        }
        #endregion

        #region 删除所有算式
        private void 删除所有算式ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("删除所有算式？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                //删除存在的算式
                Properties.Settings.Default.strExps = "";
                Properties.Settings.Default.Save();
                //更新算式菜单
                this.算式ToolStripMenuItem.DropDownItems.Clear();
                this.算式ToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem("没有算式"));
                //更新算式输入框右键菜单
                UpdateMenuRichTextBox();
                //显示删除结果
                txtResult.Text = "已删除所有算式";
            }
        }
        #endregion

        #region 输入算式
        private void ExpsItem_Click(object sender, EventArgs e)
        {
            Input(((ToolStripMenuItem)sender).Text);
        }
        #endregion

        #region 删除变量菜单
        private void RemoveVarsItem()
        {
            this.变量ToolStripMenuItem.DropDownItems.Clear();
            ToolStripMenuItem noVar = new ToolStripMenuItem("没有变量");
            this.变量ToolStripMenuItem.DropDownItems.Add(noVar);
        }
        #endregion

        #region 删除一个变量
        private void VarsItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                string[] exp = ((ToolStripMenuItem)sender).Text.Split(':');
                this.输入ToolStripMenuItem.HideDropDown();//防止弹出框被菜单覆盖
                this.menuRichTextBox.Hide();
                if (MessageBox.Show("删除变量'" + exp[0] + "'？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    //替换
                    string str = Properties.Settings.Default.strVariables.Replace(exp[0] + "|" + exp[1], "");

                    if (str.Length > 0)
                    {
                        //开头是'|'
                        if (str[0] == '|')
                        {
                            str = str.Substring(1);
                        }
                        //结尾是'|'
                        if (str[str.Length - 1] == '|')
                        {
                            str = str.Substring(0, str.Length - 1);
                        }
                    }
                    //两个'|'的情况
                    Properties.Settings.Default.strVariables = str.Replace("||", "|");

                    Properties.Settings.Default.Save();//保存修改
                    //更新变量菜单
                    this.变量ToolStripMenuItem.DropDownItems.Clear();
                    InsertVarsItem();
                    //更新算式输入框右键菜单
                    UpdateMenuRichTextBox();
                    txtResult.Text = "变量'" + exp[0] + "'已删除";
                }
            }
        }
        #endregion

        #region 计算器托盘
        //点击托盘事件
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.TopMost = true;
                this.Visible = true;
                this.Focus();
                this.WindowState = FormWindowState.Normal;
                this.TopMost = false;
            }
        }

        //显示计算器
        private void 显示计算器ToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Visible = true;
                this.Focus();
                this.WindowState = FormWindowState.Normal;
            }
        }

        //关闭计算器
        private void 关闭计算器ToolStripMenuItem_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //中止计算结果发音
                this.Close();
                this.Dispose();
            }
        }
        #endregion

        #region Form1_FormClosing
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
        }
        #endregion

        #region 语音ToolStripMenuItem1_Click
        private void 语音ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.语音ToolStripMenuItem1.Checked)
            {
                this.语音ToolStripMenuItem1.Checked = false;
                this.自然读音ToolStripMenuItem.Checked = false;
                Common.groupLength = 3;
                Properties.Settings.Default.isSpeech = false;
                Properties.Settings.Default.isNaturalSpeech = false;
                Properties.Settings.Default.Save();

                //中止计算结果发音
            }
            else
            {
                this.语音ToolStripMenuItem1.Checked = true;
                Properties.Settings.Default.isSpeech = true;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region 自然读音ToolStripMenuItem_Click
        private void 自然读音ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.自然读音ToolStripMenuItem.Checked)
            {
                this.自然读音ToolStripMenuItem.Checked = false;
                Common.groupLength = 3;
                Properties.Settings.Default.isNaturalSpeech = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                this.自然读音ToolStripMenuItem.Checked = true;
                this.语音ToolStripMenuItem1.Checked = true;
                Common.groupLength = 4;
                Properties.Settings.Default.isSpeech = true;
                Properties.Settings.Default.isNaturalSpeech = true;
                Properties.Settings.Default.Save();
            }
        }
        #endregion

        #region 算式输入框右键菜单
        /// <summary>
        /// 添加算式输入框右键菜单
        /// </summary>
        private void UpdateMenuRichTextBox()
        {
            //变量菜单
            ContextMenuAddVars();
            //算式菜单
            ContextMenuExps();
            //文本框右键函数菜单
            ContextMenuFuns();
            //更新函数类别
            UpdateFunSort();
        }
        #endregion

        #region 文本框右键变量菜单
        /// <summary>
        /// 文本框右键变量菜单
        /// </summary>
        private void ContextMenuAddVars()
        {
            this.变量ToolStripMenuItem1.DropDownItems.Clear();
            string[] variables = Properties.Settings.Default.strVariables.Split('|');
            if (variables.Length < 2)//没有变量
            {
                ToolStripMenuItem noVar = new ToolStripMenuItem("没有变量");
                this.变量ToolStripMenuItem1.DropDownItems.Add(noVar);

                //下面语句不能删除
                ToolStripMenuItem item = new ToolStripMenuItem("item");
                item.Name = "item";
                this.变量ToolStripMenuItem1.DropDownItems.Add(item);
                this.变量ToolStripMenuItem1.DropDownItems.RemoveByKey("item");
                return;
            }
            ToolStripMenuItem[] items = new ToolStripMenuItem[variables.Length / 2];

            for (int i = 0; i < variables.Length / 2; i++)
            {
                items[i] = new ToolStripMenuItem(variables[i * 2] + ":" + variables[i * 2 + 1]);
                items[i].Click += new System.EventHandler(this.读取变量ToolStripMenuItem_Click);
                items[i].MouseDown += new MouseEventHandler(this.VarsItem_MouseDown);
                this.变量ToolStripMenuItem1.DropDownItems.Insert(0, items[i]);
            }

            this.变量ToolStripMenuItem1.DropDownItems.Add(new ToolStripSeparator());
            ToolStripMenuItem removeVars = new ToolStripMenuItem("删除所有变量");
            removeVars.Click += new System.EventHandler(this.删除所有变量ToolStripMenuItem_Click);
            this.变量ToolStripMenuItem1.DropDownItems.Add(removeVars);
        }
        #endregion

        #region 文本框右键算式菜单
        /// <summary>
        /// 文本框右键算式菜单
        /// </summary>
        private void ContextMenuExps()
        {
            //先删除所有菜单项
            this.算式ToolStripMenuItem1.DropDownItems.Clear();
            //没有算式的情况
            if (Properties.Settings.Default.strExps == "")
            {
                this.算式ToolStripMenuItem1.DropDownItems.Add(new ToolStripMenuItem("没有算式"));

                //下面语句不能删除
                ToolStripMenuItem item = new ToolStripMenuItem("item");
                item.Name = "item";
                this.算式ToolStripMenuItem1.DropDownItems.Add(item);
                this.算式ToolStripMenuItem1.DropDownItems.RemoveByKey("item");
                return;
            }
            //获取算式列表
            string[] exps = Properties.Settings.Default.strExps.Split('|');

            ToolStripMenuItem[] items = new ToolStripMenuItem[exps.Length];

            //逐个扫描已存在的算式
            for (int i = 0; i < exps.Length; i++)
            {
                items[i] = new ToolStripMenuItem(exps[i]);
                items[i].Click += new System.EventHandler(this.ExpsItem_Click);
                items[i].MouseDown += new MouseEventHandler(this.ExpsItem_MouseDown);
                this.算式ToolStripMenuItem1.DropDownItems.Add(items[i]);
            }

            ToolStripMenuItem deleteExpItem = new ToolStripMenuItem("删除所有算式");
            deleteExpItem.Click += new System.EventHandler(this.删除所有算式ToolStripMenuItem_Click);
            this.算式ToolStripMenuItem1.DropDownItems.Add(new ToolStripSeparator());
            this.算式ToolStripMenuItem1.DropDownItems.Add(deleteExpItem);
        }
        #endregion

        #region 文本框右键函数菜单
        /// <summary>
        /// 文本框右键函数菜单
        /// </summary>
        private void ContextMenuFuns()
        {
            //先删除所有菜单项
            for (int i = this.函数ToolStripMenuItem.DropDownItems.Count - 1; i > 1; i--)
            {
                this.函数ToolStripMenuItem.DropDownItems.RemoveAt(i);
            }
            //没有函数的情况
            if (Properties.Settings.Default.funList.Count == 0)
            {
                this.函数ToolStripMenuItem.DropDownItems.Add(new ToolStripMenuItem("没有函数"));
                return;
            }
            //获取函数列表
            StringCollection funList = Properties.Settings.Default.funList;

            //搜索
            funList = Common.GetFunSearchResult(funList, txtSearchKey.Text, cmbFunSort.Text);

            ToolStripMenuItem item;
            //只显示搜索到的前20个函数
            for (int i = 0; i < (funList.Count > 20 ? 20 : funList.Count); i++)
            {
                string[] str = funList[i].Split('|');
                item = new ToolStripMenuItem(str[0] + "(" + str[3] + ")");
                item.MouseDown += new MouseEventHandler(FunsItem_Click);
                item.MouseHover += new EventHandler(FunsItem_MouseHover);
                item.ToolTipText = str[1];
                this.函数ToolStripMenuItem.DropDownItems.Add(item);
            }
        }
        #endregion

        #region 点击函数菜单项
        private void FunsItem_Click(object sender, MouseEventArgs e)
        {
            Input(((ToolStripMenuItem)sender).Text.Split('(')[0] + "(");
        }
        #endregion

        #region txtExp_MouseDown
        private void txtExp_MouseDown(object sender, MouseEventArgs e)
        {
            if (Form1.refreshRichTextBoxContextMenu)
            {
                //更新算式输入框右键菜单
                UpdateMenuRichTextBox();
            }
        }
        #endregion

        #region 记录表达式
        /// <summary>
        /// 记录表达式
        /// </summary>
        private void MemoryExpr(string str)
        {
            try
            {
                if (memoryExprs.Count > 0 && str == "" && memoryExprs[memoryExprs.Count - 1] == "")
                {
                    return;
                }
                //添加表达式
                if (memoryExprsCursor == -1 || memoryExprsCursor == memoryExprs.Count - 1)
                {
                    memoryExprs.Add(str);
                    //更新游标
                    memoryExprsCursor++;
                }
                else
                {
                    memoryExprs.Insert(memoryExprsCursor + 1, str);
                    //更新游标
                    memoryExprsCursor++;
                    for (int i = memoryExprs.Count - 1; i > memoryExprsCursor; i--)
                    {
                        memoryExprs.RemoveAt(i);
                    }
                }
                //如果超过最大个数
                if (memoryExprs.Count > memoryExprsMaxCount - 1)
                {
                    memoryExprs.RemoveAt(0);
                    memoryExprsCursor = memoryExprsCursor - 1 < 0 ? 0 : memoryExprsCursor - 1;
                }
            }
            catch
            {
            }
        }
        #endregion

        #region 记录表达式
        /// <summary>
        /// 记录表达式
        /// </summary>
        private void MemoryExpr_Cal()
        {
            try
            {
                //如果与最后一个相同
                if (memoryExprs_Cal.Count > 0
                    && memoryExprs_Cal[memoryExprs_Cal.Count - 1] == txtExp.Text)
                {
                    return;
                }
                //添加表达式
                memoryExprs_Cal.Add(txtExp.Text);
                //更新游标
                memoryExprsCursor_Cal = memoryExprs_Cal.Count;
                //如果超过最大个数
                if (memoryExprs_Cal.Count > memoryExprsMaxCount_Cal - 1)
                {
                    memoryExprs_Cal.RemoveAt(0);
                    memoryExprsCursor_Cal = memoryExprs_Cal.Count;
                }
            }
            catch
            {
            }
        }
        #endregion

        private void 粘贴剪切板内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            获取剪切板内容ToolStripMenuItem_Click(sender, e);
        }

        private void 复制计算结果ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (txtResult.Text.Trim() != "")
            {
                复制计算结果ToolStripMenuItem_Click(sender, e);
            }
        }

        private void OpeMenuItem_Click(object sender, EventArgs e)
        {
            Input(((ToolStripMenuItem)sender).Text);
        }

        private void OpeMenuItem_Click2(object sender, EventArgs e)
        {
            Input(((ToolStripMenuItem)sender).Text + "(");
        }

        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            ContextMenuFuns();
        }

        private void FunsItem_MouseHover(object sender, EventArgs e)
        {
            if (txtSearchKey.Focused
                || cmbFunSort.Focused)
            {
                this.Focus();
            }
        }

        #region 更新函数类别
        /// <summary>
        /// 更新函数类别
        /// </summary>
        private void UpdateFunSort()
        {
            StringCollection funs = Properties.Settings.Default.funList;
            StringCollection sorts = new StringCollection();
            string selectionText = null;
            if (cmbFunSort.SelectedIndex != -1)
            {
                selectionText = cmbFunSort.SelectedItem.ToString();
            }
            else
            {
                selectionText = "";
            }

            for (int i = 0; i < funs.Count; i++)
            {
                string currentSort = funs[i].Split('|')[4];
                bool exists = false;
                for (int k = 0; k < sorts.Count; k++)
                {
                    if (currentSort == sorts[k])
                    {
                        exists = true;//该类别已添加
                    }
                }
                if (!exists)//如果该类型还没有添加到类别列表中
                {
                    sorts.Add(currentSort);
                }
            }

            //先清空
            cmbFunSort.Items.Clear();
            //重新添加类别
            cmbFunSort.Items.Add("");
            for (int i = 0; i < sorts.Count; i++)
            {
                cmbFunSort.Items.Add(sorts[i]);
            }
            cmbFunSort.SelectedItem = selectionText;
        }
        #endregion

        private void cmbFunSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            ContextMenuFuns();
        }

        private void txtExp_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //记录表达式
                if ((int)e.KeyChar > 32 && (int)e.KeyChar != 127)
                {
                    if (txtExp.SelectionLength != 0)
                    {
                        int currentPos = txtExp.SelectionStart;
                        txtExp.Text = txtExp.Text.Replace(txtExp.Text.Substring(txtExp.SelectionStart, txtExp.SelectionLength), "");
                        txtExp.SelectionStart = currentPos;
                    }
                    MemoryExpr(txtExp.Text.Insert(txtExp.SelectionStart, e.KeyChar.ToString()));
                }
                else
                {
                    MemoryExpr(txtExp.Text);
                }
                //更新计算过的表达式游标
                memoryExprsCursor_Cal = memoryExprs_Cal.Count;
            }
            catch
            {
            }
        }

        private void txtExp_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Delete:
                    //记录表达式
                    MemoryExpr(txtExp.Text);
                    break;
            }
        }

    }
}
