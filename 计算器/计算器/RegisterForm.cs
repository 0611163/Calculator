using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool;
using System.Security.Cryptography;

namespace 计算器
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();

            #region 读取机器码及注册信息
            try
            {
                //获取本机的机器码
                txtMachineCode.Text = string.Empty; //获取机器码的方法已删除

                //判断程序有没有注册
                if (Common.IsRegister())
                {
                    lblRegisterInfo.Text = "注册信息： 已注册";
                    txtRegisterCode.Text = Common.GetRegisterCodeFromRegTable();
                    txtRegisterCode.ReadOnly = true;
                    txtRegisterCode.BackColor = SystemColors.Window;
                    btnRegister.Enabled = false;
                    btnCopyMachineCode.Enabled = false;
                }
                else
                {
                    //读取程序第一次运行的日期
                    DateTime dtFirstRunDate = Convert.ToDateTime(FileOperator.GetValue("FirstRunDate"));
                    //获取剩余试用天数
                    int remainderDays = Common.probationalTime - DateTime.Now.Date.Subtract(dtFirstRunDate).Days;
                    if (remainderDays < 0)
                    {
                        remainderDays = 0;
                    }
                    if (remainderDays > Common.probationalTime)
                    {
                        remainderDays = 0;
                    }
                    DateTime dtFirstRunDate2 = Convert.ToDateTime(DESEncrypt.Decrypt(RegisterOperator.GetRegData(DESEncrypt.Encrypt("FirstRunDate", Common.DESKey)), Common.DESKey));
                    if (!dtFirstRunDate.Equals(dtFirstRunDate2))
                    {
                        remainderDays = 0;
                    }
                    //获取最后一次运行的日期
                    DateTime lastRunDate = Convert.ToDateTime(FileOperator.GetValue("LastRunDate"));
                    DateTime lastRunDate2 = Convert.ToDateTime(DESEncrypt.Decrypt(RegisterOperator.GetRegData(DESEncrypt.Encrypt("LastRunDate", Common.DESKey)), Common.DESKey));
                    if (!lastRunDate.Equals(lastRunDate2))
                    {
                        remainderDays = 0;
                    }
                    //如果当前日期早于程序最后一次运行的日期，说明用户修改了系统日期
                    if (DateTime.Now.Date.CompareTo(lastRunDate) < 0)
                    {
                        remainderDays = 0;
                    }

                    lblRegisterInfo.Text = "注册信息： 未注册，您还可以试用" + remainderDays.ToString() + "天";
                }
            }
            catch
            {
                Common.ExitProgram();
            }
            #endregion

        }

        #region 软件注册
        //软件注册
        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRegisterCode.Text.Trim() != "")
                {
                    string strRegisterCodeFromMachineCode = Common.CreateRegisterCodeFromMachineCode();
                    //验证注册码
                    if (txtRegisterCode.Text.Trim() == strRegisterCodeFromMachineCode)
                    {
                        //注册信息写入注册表
                        RegisterOperator.SetRegData(DESEncrypt.Encrypt("RegisterInfo", Common.DESKey), DESEncrypt.Encrypt(strRegisterCodeFromMachineCode, Common.DESKey));

                        MessageBox.Show("注册成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblRegisterInfo.Text = "注册信息： 已注册";
                        this.Close();
                    }
                    else
                    {
                        //注册失败处理
                        FailProcess();
                    }
                }
                else
                {
                    MessageBox.Show("请输入注册码！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                //注册失败处理
                FailProcess();
            }
        }
        #endregion

        #region 机器码已复制到剪切板
        private void btnCopyMachineCode_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtMachineCode.Text.Trim());
            MessageBox.Show("机器码已复制到剪切板", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region 没有注册码？
        private void lkNoRegisterCode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:651029594@qq.com");
        }
        #endregion

        #region RegisterForm_Shown
        private void RegisterForm_Shown(object sender, EventArgs e)
        {
            txtMachineCode.SelectionStart = txtMachineCode.Text.Length;
        }
        #endregion

        #region 注册失败处理
        /// <summary>
        /// 注册失败处理
        /// </summary>
        private void FailProcess()
        {
            MessageBox.Show("注册失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //退出程序，并关闭所有窗体
            Common.ExitProgram();
        }
        #endregion

    }
}
