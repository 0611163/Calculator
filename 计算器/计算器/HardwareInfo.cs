using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;//还需添加引用System.Management
using System.Security.Cryptography;
using System.Text;
using 计算器;

namespace Tool
{
    /// <summary>
    /// 获取硬件信息的类
    /// suxiang
    /// 2012年8月12日
    /// </summary>
    class HardwareInfoClass
    {

        #region 生成机器码
        public static string GetMachineCode()
        {
            //先从文件中读机器码
            if (FileOperator.Exists("MachineCode"))
            {
                return FileOperator.GetValue("MachineCode");
            }

            //如果文件中没有再生成机器码并保存到文件中
            string str = GetCPUID() + GetFirstHardDiskID() + GetLocalMAC();

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(Encoding.Default.GetBytes(str));

            StringBuilder result = new StringBuilder();
            foreach (byte b in md5.Hash)
            {
                result.AppendFormat("{0:X2}", b);
            }

            //保存机器码到文件
            FileOperator.SetValue("MachineCode", result.ToString());

            return result.ToString();
        }
        #endregion

        #region 获取机器名
        /// <summary>
        /// 获取机器名
        /// </summary>
        public static string GetHostName()
        {
            return System.Net.Dns.GetHostName();
        }
        #endregion

        #region 获取CPU编号
        /// <summary>
        /// 获取CPU编号
        /// </summary>
        public static string GetCPUID()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");

                ManagementObjectCollection moc = mc.GetInstances();

                string strCUPID = null;
                foreach (ManagementObject mo in moc)
                {
                    try
                    {
                        strCUPID = mo.Properties["ProcessorId"].Value.ToString();
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }

                return strCUPID;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 获取第一块硬盘编号
        /// <summary>
        /// 获取第一块硬盘编号
        /// </summary>
        public static string GetFirstHardDiskID()
        {
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");

                string strFirstHardDiskID = null;
                foreach (ManagementObject mo in mos.Get())
                {
                    try
                    {
                        strFirstHardDiskID = mo["SerialNumber"].ToString().Trim();
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }

                return strFirstHardDiskID;
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region 获取本机的MAC
        /// <summary>
        /// 获取本机的MAC
        /// </summary>
        public static string GetLocalMAC()
        {
            try
            {
                ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM Win32_NetWorkAdapterConfiguration");

                string strFirstHardDiskID = null;
                foreach (ManagementObject mo in mos.Get())
                {
                    try
                    {
                        if (mo["IPEnabled"].ToString().ToLower() == "true")
                        {
                            strFirstHardDiskID = mo["MacAddress"].ToString().Trim();
                            break;
                        }
                        else
                        {
                            strFirstHardDiskID = "";
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                return strFirstHardDiskID;
            }
            catch
            {
                return "";
            }
        }
        #endregion

    }
}
