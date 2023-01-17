using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Tool;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace 计算器
{
    static class Program
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);
        private const int SW_SHOWNORMAL = 1;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //软件自动注册
            //注释掉此行，则软件由免费版转换为共享版
            Common.AutoRegister();

            #region 如果程序没有注册
            bool isFirstRun = true;
            //如果程序没有注册
            if (!Common.IsRegister())
            {

                #region 判断用户有没有修改系统日期
                //如果程序不是第一次运行
                if (FileOperator.Exists("FirstRunDate")
                    || RegisterOperator.IsRegDataExists(DESEncrypt.Encrypt("FirstRunDate", Common.DESKey))
                    || Common.FirstRunFlagExists())
                {
                    try
                    {
                        //不是第一次运行
                        isFirstRun = false;
                        //获取最后一次运行的日期
                        DateTime lastRunDate = Convert.ToDateTime(FileOperator.GetValue("LastRunDate"));
                        DateTime lastRunDate2 = Convert.ToDateTime(DESEncrypt.Decrypt(RegisterOperator.GetRegData(DESEncrypt.Encrypt("LastRunDate", Common.DESKey)), Common.DESKey));
                        if (!lastRunDate.Equals(lastRunDate2))
                        {
                            Application.Run(new RegisterForm());
                            return;
                        }

                        //如果当前日期早于程序最后一次运行的日期，说明用户修改了系统日期
                        if (DateTime.Now.Date.CompareTo(lastRunDate) < 0)
                        {
                            MessageBox.Show("日期更改无效\r\n请改回正确日期，或者注册", "计算器", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            Application.Run(new RegisterForm());
                            return;
                        }
                        else
                        {
                            //记录程序最后一次运行的日期
                            FileOperator.SetValue("LastRunDate", DateTime.Now.Date.ToString());
                            RegisterOperator.SetRegData(DESEncrypt.Encrypt("LastRunDate", Common.DESKey), DESEncrypt.Encrypt(DateTime.Now.Date.ToString(), Common.DESKey));
                        }
                    }
                    catch
                    {
                        Application.Run(new RegisterForm());
                        return;
                    }
                }
                else//程序第一次运行
                {
                    //记录程序第一次运行的标志
                    Common.MemorizeFirstRunFlag();

                    //记录程序首次运行的日期
                    FileOperator.SetValue("FirstRunDate", DateTime.Now.Date.ToString());
                    RegisterOperator.SetRegData(DESEncrypt.Encrypt("FirstRunDate", Common.DESKey), DESEncrypt.Encrypt(DateTime.Now.Date.ToString(), Common.DESKey));

                    //记录程序最后一次运行的日期
                    FileOperator.SetValue("LastRunDate", DateTime.Now.Date.ToString());
                    RegisterOperator.SetRegData(DESEncrypt.Encrypt("LastRunDate", Common.DESKey), DESEncrypt.Encrypt(DateTime.Now.Date.ToString(), Common.DESKey));
                }
                #endregion

                #region 判断有没有超过试用期
                try
                {
                    //读取程序第一次运行的日期
                    DateTime dtFirstRunDate = Convert.ToDateTime(FileOperator.GetValue("FirstRunDate"));
                    DateTime dtFirstRunDate2 = Convert.ToDateTime(DESEncrypt.Decrypt(RegisterOperator.GetRegData(DESEncrypt.Encrypt("FirstRunDate", Common.DESKey)), Common.DESKey));
                    if (!dtFirstRunDate.Equals(dtFirstRunDate2))
                    {
                        Application.Run(new RegisterForm());
                        return;
                    }
                    //如果超过试用日期
                    if (DateTime.Now.Date.Subtract(dtFirstRunDate).Days > Common.probationalTime)
                    {
                        MessageBox.Show("软件试用期已满" + Common.probationalTime.ToString() + "天，请注册", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Application.Run(new RegisterForm());
                        return;
                    }
                    else//没有超过试用日期
                    {
                        int remainderDays = Common.probationalTime - DateTime.Now.Date.Subtract(dtFirstRunDate).Days;
                        if (isFirstRun)
                        {
                            MessageBox.Show("您可以试用" + remainderDays.ToString() + "天，欢迎注册使用", "提示", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("您还可以试用" + remainderDays.ToString() + "天，欢迎注册使用", "提示", MessageBoxButtons.OK);
                        }
                    }
                }
                catch
                {
                    Application.Run(new RegisterForm());
                    return;
                }
                #endregion

            }
            #endregion

            bool createNew;
            using (System.Threading.Mutex m = new System.Threading.Mutex(true, Application.ProductName, out createNew))
            {
                if (createNew)
                {
                    Properties.Settings.Default.processId = Process.GetCurrentProcess().Id; //保存进程ID
                    Properties.Settings.Default.Save();
                    Application.Run(new Form1());
                }
                else
                {
                    try
                    {
                        int processId = Properties.Settings.Default.processId; //读取进程ID
                        Process process = Process.GetProcessById(processId);
                        HandleRunningInstance(process);
                    }
                    catch
                    {
                        Properties.Settings.Default.processId = Process.GetCurrentProcess().Id; //保存进程ID
                        Properties.Settings.Default.Save();
                        Application.Run(new Form1());
                    }
                }
            } //end of Mutex
        } //end of Main

        #region 显示已运行的程序
        /// <summary>
        /// 显示已运行的程序
        /// </summary>
        public static void HandleRunningInstance(Process instance)
        {
            try
            {
                IntPtr formHwnd = FindWindow(null, "计算器 v5.2.0");
                ShowWindow(formHwnd, SW_SHOWNORMAL); //显示
                SetForegroundWindow(formHwnd); //放到前端
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

    } //end of Program
}
