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
