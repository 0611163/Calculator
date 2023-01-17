///////////////////////////////////////////////////////
//创建日期：2012年7月23日
//功能：函数存储与管理
///////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.IO;

namespace 计算器
{
    public partial class AddFun : Form
    {
        private int _selectionStartPos = 0;
        private int _exampleIndex = 0;
        private int _exampleCountMax = 10;

        private int ExampleIndex
        {
            get
            {
                if (_exampleIndex > _exampleCountMax)
                {
                    return _exampleIndex - 1;
                }
                else
                {
                    _exampleIndex++;
                    return _exampleIndex - 1;
                }
            }
            set
            {
                _exampleIndex = value;
            }
        }

        public AddFun()
        {
            InitializeComponent();

            txtFunName.SelectionStart = txtFunName.Text.Length;
            txtFunName.SelectionLength = 0;

            //更新函数列表框
            UpdateListBox();

            //更新函数类别
            UpdateFunSort();
        }

        /// <summary>
        /// 更新函数类别
        /// </summary>
        private void UpdateFunSort()
        {
            StringCollection funs = Properties.Settings.Default.funList;
            StringCollection sorts = new StringCollection();

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

            string selectionText = null;
            if (cmbFunSearchSort.SelectedIndex != -1)
            {
                selectionText = cmbFunSearchSort.SelectedItem.ToString();
            }
            //先清空
            cmbFunSort.Items.Clear();
            cmbFunSearchSort.Items.Clear();
            //重新添加类别
            cmbFunSort.Items.Add("");
            cmbFunSearchSort.Items.Add("");
            for (int i = 0; i < sorts.Count; i++)
            {
                cmbFunSort.Items.Add(sorts[i]);
                cmbFunSearchSort.Items.Add(sorts[i]);
            }
            cmbFunSearchSort.SelectedItem = selectionText;
        }

        private void funExpr_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (!Common._lock)
                {
                    _selectionStartPos = funExpr.SelectionStart;
                    Common.SetColor(funExpr.Text, funExpr);
                    funExpr.SelectionStart = _selectionStartPos;
                }
            }
            catch
            {
            }
        }

        //存储
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                txtFunName.Text = txtFunName.Text.Replace(" ", "");
                //函数名称不能为空
                if (txtFunName.Text.Trim() == "")
                {
                    MessageBox.Show("函数名称不能为空", "提示");
                    return;
                }

                //参数变量是否合法
                string desc = "";
                if (!isVarsLegit(out desc))
                {
                    MessageBox.Show(desc, "提示");
                    return;
                }

                //判断是否存在同名函数
                for (int i = 0; i < Properties.Settings.Default.funList.Count; i++)
                {
                    string[] strFuns = Properties.Settings.Default.funList[i].Split('|');
                    if (txtFunName.Text.Trim() == strFuns[0])//存在同名函数
                    {
                        if (MessageBox.Show("存在同名函数'" + strFuns[0] + "：" + strFuns[2] + "'，\n\n是否替换？", "提示"
                            , MessageBoxButtons.OKCancel) == DialogResult.OK)//确认替换
                        {
                            //删除原有函数
                            Properties.Settings.Default.funList.RemoveAt(i--);
                            //存储函数
                            StoreFun("替换");
                            //更新函数列表框
                            UpdateListBox();
                            listBox_FunList.SelectedItem = txtFunName.Text.Trim()
                                + "(" + txtFunVars.Text.Trim() + "): "
                                + funExpr.Text.Trim();
                            //更新函数类别
                            UpdateFunSort();
                            return;
                        }
                        else//取消替换
                        {
                            return;
                        }
                    }
                }

                //不存在同名函数
                //存储函数
                StoreFun("存储");
                //更新函数列表框
                UpdateListBox();
                listBox_FunList.SelectedItem = txtFunName.Text.Trim()
                                + "(" + txtFunVars.Text.Trim() + "): "
                                + funExpr.Text.Trim();
                //更新函数类别
                UpdateFunSort();
            }
            catch (Exception ex)
            {
                //显示表达式出错信息
                ShowException(ex);
            }
        }

        #region 显示表达式出错信息
        /// <summary>
        /// 显示表达式出错信息
        /// </summary>
        private void ShowException(Exception ex)
        {
            try
            {
                string[] exSplit = ex.Message.Split('|');
                if (exSplit.Length >= 3)
                {
                    //如果调用了Common.SetWrongOpeColor将不再调用Common.SetColor
                    Common.Lock();
                    //设置出错运算符的颜色
                    Common.SetWrongOpeColor(exSplit[exSplit.Length - 2].Split(':')[1],
                        int.Parse(exSplit[exSplit.Length - 1].Split(':')[1]),
                        funExpr);
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
                MessageBox.Show(exMessage.ToString(), "提示");


            }
            catch { }
        }
        #endregion

        /// <summary>
        /// 参数变量列表是否合法
        /// </summary>
        private bool isVarsLegit(out string desc)
        {
            desc = "合法";
            txtFunVars.Text = txtFunVars.Text.Replace(" ", "");
            string[] str = txtFunVars.Text.Trim().Split(',');
            for (int i = 0; i < str.Length; i++)
            {
                //参数变量列表不合法
                if (str[i] == "")
                {
                    desc = "参数变量列表不合法";
                    return false;
                }
                //参数变量不能以数字开头
                int n;
                if (int.TryParse(str[i][0].ToString(), out n))
                {
                    desc = "参数变量不能以数字开头";
                    return false;
                }
                //参数变量不能是已存在的函数或常量
                CalOpeList opeList = new CalOpeList();
                CalOpe ope = opeList.GetOpe(str[i]);
                if (ope != null)
                {
                    desc = "参数变量不能是已存在的函数、变量或常量";
                    return false;
                }

                #region 参数变量个数是否与函数表达式中参数变量个数匹配
                //记录表达式
                string tempExpr = funExpr.Text;
                bool hasAnnotate;//表达式是否有注释
                //删除注释
                tempExpr = Common.DeleteAnnotate(funExpr.Text, out hasAnnotate);
                //生成运算因子列表，此过程可以检查表达式是否合法
                //若不合法会抛出异常
                CalValList valList = new CalValList(tempExpr);
                if (!hasAnnotate)//没有注释
                {
                    funExpr.Text = valList.nomalStringExpression;
                }

                List<string> strVarList = new List<string>();//记录表达式中的参数
                //扫描运算因子列表
                for (int k = 0; k < valList.valList.Count; k++)
                {
                    //如果当前运算因子的类型是值类型
                    if (valList.valList[k].type == ValType.Number)
                    {
                        double d;
                        //如果值类型无法转换为数字
                        if (!double.TryParse(valList.valList[k].num.stringValue, out d))
                        {
                            //且没有大于或小于号，为了支持分段函数
                            if (valList.valList[k].num.stringValue.IndexOf("<") == -1
                                && valList.valList[k].num.stringValue.IndexOf(">") == -1)
                            {
                                strVarList.Add(valList.valList[k].num.stringValue);
                            }
                        }
                    }
                }
                //删除重复
                for (int m = 0; m < strVarList.Count; m++)
                {
                    //逐个扫描当前参数变量后面的参数变量
                    for (int k = m + 1; k < strVarList.Count; k++)
                    {
                        if (strVarList[k] == strVarList[m])//存在相同的
                        {
                            strVarList.RemoveAt(k--);//删除相同的变量，注意调整k的值
                        }
                    }
                }
                //如果参数变量个数与函数表达式中参数变量个数不相等
                if (strVarList.Count != txtFunVars.Text.Trim().Split(',').Length)
                {
                    desc = "存储失败！\n\n参数变量过多或过少";
                    return false;
                }
                //检测变量是否对应
                //扫描从表达式中取出的每一个函数变量
                string[] strParameterList = txtFunVars.Text.Trim().Split(',');
                for (i = 0; i < strVarList.Count; i++)
                {
                    bool exists = false;
                    for (int m = 0; m < strParameterList.Length; m++)
                    {
                        if (strVarList[i] == Common.DeleteAnnotate(strParameterList[i], out hasAnnotate))
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                    {
                        desc = "存储失败！\n\n表达式中的变量与参数列表中的变量不对应";
                        return false;
                    }
                }
                #endregion

            }
            return true;
        }

        #region 存储函数
        /// <summary>
        /// 存储函数
        /// </summary>
        /// <param name="expr">存储还是替换</param>
        /// <returns>是否存储成功</returns>
        private bool StoreFun(string expr)
        {
            try
            {
                //记录表达式
                string tempExpr = funExpr.Text;
                bool hasAnnotate;//表达式是否有注释
                //删除注释
                tempExpr = Common.DeleteAnnotate(funExpr.Text, out hasAnnotate);
                //生成运算因子列表，此过程可以检查表达式是否合法
                //若不合法会抛出异常
                CalValList valList = new CalValList(tempExpr);

                //存储函数
                string strFun = txtFunName.Text.Trim()
                    + "|" + txtDesc.Text.Trim()
                    + "|" + funExpr.Text.Trim()
                    + "|" + txtFunVars.Text.Trim()
                    + "|" + (cmbFunSort.Text.Trim() == "" ? "其它" : cmbFunSort.Text.Trim());
                if (Properties.Settings.Default.funList == null)//判断是否为null
                {
                    Properties.Settings.Default.funList = new StringCollection();
                }
                Properties.Settings.Default.funList.Add(strFun);
                Properties.Settings.Default.Save();

                if (expr == "重命名")
                {
                    MessageBox.Show("函数'" + listBox_FunList.SelectedItem.ToString().Split('(')[0] + "'已重命名为'" + txtFunName.Text + "'", "提示");
                    return true;
                }
                MessageBox.Show("函数'" + txtFunName.Text.Trim() + "：" + funExpr.Text.Trim() + "'\n\n" + expr + "成功！", "提示");
                return true;
            }
            catch
            {
                MessageBox.Show("存储失败", "提示");
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 更新函数列表框
        /// </summary>
        private void UpdateListBox()
        {
            try
            {
                Form1.refreshRichTextBoxContextMenu = true;
                if (Properties.Settings.Default.funList == null)
                {
                    return;
                }

                StringCollection searchResult =
                    Common.GetFunSearchResult(Properties.Settings.Default.funList,
                    txtSearchKey.Text,
                    cmbFunSearchSort.Text);

                listBox_FunList.Items.Clear();
                for (int i = 0; i < searchResult.Count; i++)
                {
                    string[] str = searchResult[i].Split('|');
                    listBox_FunList.Items.Add(str[0] + "(" + str[3] + "): " + str[2]);
                }
            }
            catch
            {
            }
        }

        //退出
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //删除选中的函数
        private void 删除选中的函数ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.funList != null)
                {
                    if (MessageBox.Show("确认删除？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //删除选中的函数
                        //扫描选择项
                        int pos = listBox_FunList.SelectedIndex;
                        for (int i = 0; i < listBox_FunList.SelectedItems.Count; i++)
                        {
                            Properties.Settings.Default.funList.Remove(Common.GetFunFull(listBox_FunList.SelectedItems[i].ToString().Split('(')[0]));
                        }
                        //保存修改
                        Properties.Settings.Default.Save();
                        //更新函数列表框
                        UpdateListBox();
                        //更新函数类别
                        UpdateFunSort();

                        //删除后选中下一条
                        if (listBox_FunList.Items.Count > 0)
                        {
                            listBox_FunList.SelectedIndex = pos > listBox_FunList.Items.Count - 1 ? listBox_FunList.Items.Count - 1 : pos;
                        }
                    }
                }
            }
            catch
            { }
        }

        //删除所有函数
        private void 删除所有函数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确认删除所有函数？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    for (int i = 0; i < listBox_FunList.Items.Count; i++)
                    {
                        Properties.Settings.Default.funList.Remove(Common.GetFunFull(listBox_FunList.Items[i].ToString().Split('(')[0]));
                    }
                    Properties.Settings.Default.Save();
                    //更新函数列表框
                    UpdateListBox();
                    //更新函数类别
                    UpdateFunSort();
                }
            }
            catch
            {
            }
        }

        //选择ListBox
        private void listBox_FunList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //取出存储的函数信息
                if (listBox_FunList.SelectedIndex != -1)
                {
                    string[] strList = Common.GetFunFull(listBox_FunList.SelectedItem.ToString().Split('(')[0]).Split('|');
                    //显示
                    txtFunName.Text = strList[0];
                    txtDesc.Text = strList[1];
                    funExpr.Text = strList[2];
                    txtFunVars.Text = strList[3];
                    cmbFunSort.SelectedItem = strList[4];
                }
            }
            catch { }
        }

        //输入搜索关键字
        private void txtSearchKey_TextChanged(object sender, EventArgs e)
        {
            //更新函数列表框
            UpdateListBox();
        }

        //双击ListBox
        private void listBox_FunList_DoubleClick(object sender, EventArgs e)
        {
        }

        //清空
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFunName.Text = "";
            txtDesc.Text = "";
            txtFunVars.Text = "";
            funExpr.Text = "";
            cmbFunSort.SelectedIndex = -1;
            cmbFunSort.Text = "";
        }

        //例子
        private void btnExample_Click(object sender, EventArgs e)
        {
            switch (ExampleIndex)
            {
                case 0:
                    txtFunName.Text = "LogN{X[变量],N[底数]}";
                    txtDesc.Text = "对变量X求以N为底数的对数，例：LogN{变量X,底数N}(9,3)";
                    funExpr.Text = "log(X)/log(N)";
                    txtFunVars.Text = "X（变量）,N（底数）";
                    cmbFunSort.Text = "例子";
                    break;
                case 1:
                    txtFunName.Text = "asinh";
                    txtDesc.Text = "反双曲正弦，使用示例：asinh(9)";
                    funExpr.Text = "ln(X+sqrt(X*X+1))";
                    txtFunVars.Text = "X";
                    cmbFunSort.Text = "例子";
                    break;
                case 2:
                    txtFunName.Text = "2->16";
                    txtDesc.Text = "二进制数转换为十六进制数，例：2->16(110110)";
                    funExpr.Text = "toH(XBIN)";
                    txtFunVars.Text = "X";
                    cmbFunSort.Text = "例子";
                    break;
                case 3:
                    txtFunName.Text = "球体体积{直径}";
                    txtDesc.Text = "求球体的体积，使用示例：球体体积{直径}(9)";
                    funExpr.Text = "4/3*pi*(D/2)^3";
                    txtFunVars.Text = "D（直径）";
                    cmbFunSort.Text = "例子";
                    break;
                case 4:
                    txtFunName.Text = "码->米";
                    txtDesc.Text = "'码'是长度单位";
                    funExpr.Text = "X*1/1.0936";
                    txtFunVars.Text = "X";
                    cmbFunSort.Text = "例子";
                    break;
                case 5:
                    txtFunName.Text = "长方体的体积{长,宽,高}";
                    txtDesc.Text = "求长方体的体积";
                    funExpr.Text = "长*宽*高";
                    txtFunVars.Text = "长,宽,高";
                    cmbFunSort.Text = "例子";
                    break;
                case 6:
                    txtFunName.Text = "fun1{x{参数1},y{参数2},z{参数3}}";
                    txtDesc.Text = "该函数的注释很齐全";
                    funExpr.Text = "x（参数1） * sqrt((-y（参数2）)^2) / z（参数3）";
                    txtFunVars.Text = "x（这是x的注释）,y（这是y的注释）,z（这是z的注释）";
                    cmbFunSort.Text = "例子";
                    break;
                case 7:
                    txtFunName.Text = "fun2";
                    txtDesc.Text = "该函数没有注释";
                    funExpr.Text = "2*x+3*y-z";
                    txtFunVars.Text = "x,y,z";
                    cmbFunSort.Text = "例子";
                    break;
                case 8:
                    txtFunName.Text = "cbrt";
                    txtDesc.Text = "求一个数的立方根，这是一个分段函数，为了区分，表达式中subfuns的参数的分隔符必须是中文全角逗号";
                    funExpr.Text = "subfuns(-1E308，<0，-(-x)^(1/3)，0，0，0，>0，1E308，x^(1/3))";
                    txtFunVars.Text = "x";
                    cmbFunSort.Text = "例子";
                    break;
                default:
                    ExampleIndex = 0;
                    btnExample_Click(sender, e);
                    break;
            }
        }

        //重命名
        private void btnRename_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("确认重命名？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (StoreFun("重命名"))
                    {
                        Properties.Settings.Default.funList.Remove(Common.GetFunFull(listBox_FunList.SelectedItem.ToString().Split('(')[0]));
                        Properties.Settings.Default.Save();
                        UpdateListBox();
                        listBox_FunList.SelectedItem = txtFunName.Text.Trim()
                                + "(" + txtFunVars.Text.Trim() + "): "
                                + funExpr.Text.Trim();
                        //更新函数类别
                        UpdateFunSort();
                    }
                }
            }
            catch
            {
            }
        }

        //复制函数
        private void 复制函数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox_FunList.SelectedIndex != -1)
            {
                Clipboard.SetText(listBox_FunList.SelectedItem.ToString().Split('(')[0] + "(");
            }
        }

        //验证函数名称
        private void txtFunName_Validated(object sender, EventArgs e)
        {
            try
            {
                string str = "()（）〔〕\"";
                bool bl = true;
                for (int i = 0; i < str.Length; i++)
                {
                    if (txtFunName.Text.IndexOf(str[i]) != -1)
                    {
                        bl = false;
                        if (i % 2 == 0)
                        {
                            txtFunName.Text = txtFunName.Text.Replace(str[i].ToString(), "{");
                        }
                        else
                        {
                            txtFunName.Text = txtFunName.Text.Replace(str[i].ToString(), "}");
                        }

                    }
                }
                if (bl == false)
                {
                    MessageBox.Show("您输入的函数名称不合法\n\n程序已自动将其转换为合法的函数名称", "提示");
                }
            }
            catch { }
        }

        //导出
        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.funList.Count == 0)
                {
                    return;
                }
                saveFileDialog1.Filter = "*.dat|*.dat";
                saveFileDialog1.FileName = "自定义函数" + System.DateTime.Now.ToString("yyyyMMdd");
                if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    //存为文件
                    StreamWriter streamWriter = new StreamWriter(saveFileDialog1.FileName);
                    StringCollection funList = Properties.Settings.Default.funList;
                    if (funList != null)
                    {
                        for (int i = 0; i < funList.Count; i++)
                        {
                            streamWriter.WriteLine(funList[i]);
                        }
                    }
                    streamWriter.Flush();
                    //释放资源
                    streamWriter.Close();
                    streamWriter.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败，失败原因：" + ex.Message);
            }
        }

        //导入
        private void 导入ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "*.dat|*.dat";
                openFileDialog1.FileName = "";
                if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
                {
                    StreamReader streamReader = new StreamReader(openFileDialog1.FileName);
                    if (Properties.Settings.Default.funList == null)
                    {
                        Properties.Settings.Default.funList = new StringCollection();
                    }

                    while (!streamReader.EndOfStream)
                    {
                        string str = streamReader.ReadLine();
                        //是否有重复
                        bool exists = false;
                        for (int i = 0; i < Properties.Settings.Default.funList.Count; i++)
                        {
                            if (Properties.Settings.Default.funList[i] == str)
                            {
                                exists = true;
                                break;
                            }
                        }
                        if (!exists)//没有重复
                        {
                            if (str.Split('|').Length != 5)
                            {
                                throw new Exception("文件可能被修改");
                            }
                            Properties.Settings.Default.funList.Add(str);
                        }
                    }
                    Properties.Settings.Default.Save();
                    //释放资源
                    streamReader.Close();
                    streamReader.Dispose();
                    //更新函数列表框
                    UpdateListBox();
                    //更新函数类别
                    UpdateFunSort();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败，失败原因：" + ex.Message);
            }
        }

        //funExpr_KeyDown
        private void funExpr_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Back:
                    Common.BackSpace(funExpr);
                    e.Handled = true;//屏蔽键盘的回退键
                    break;
            }
        }

        //选择函数类别
        private void cmbFunSearchSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int tempIndex = listBox_FunList.SelectedIndex;
                UpdateListBox();
                if (tempIndex + 1 <= listBox_FunList.Items.Count)
                {
                    listBox_FunList.SelectedIndex = tempIndex;
                }
            }
            catch
            {
            }
        }

    }
}
