using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Win32;
using 计算器;

namespace Tool
{
    /// <summary>
    /// 操作注册表的类
    /// suxiang
    /// 2012年8月13日
    /// </summary>
    class RegisterOperator
    {

        #region 待操作的注册表项
        /// <summary>
        /// 待操作的注册表项
        /// </summary>
        private static RegistryKey RegKey
        {
            get
            {
                try
                {
                    //获取HKEY_CURRENT_USER\Software
                    RegistryKey regKey_Software = Registry.CurrentUser.OpenSubKey(DESEncrypt.Decrypt("5F122CC9BDF7F67BC2D9750C48AC6820", Common.DESKey), true);

                    //生成注册表项的名称
                    string strSubKeyName = DESEncrypt.Decrypt("A775D99F8A2ECEFE", Common.DESKey);

                    //获取待操作的注册表项
                    RegistryKey regSubKey = regKey_Software.OpenSubKey(strSubKeyName, true);
                    //如果该注册表项不存在则创建它
                    if (regSubKey == null)
                    {
                        regKey_Software.CreateSubKey(strSubKeyName);
                        regSubKey = regKey_Software.OpenSubKey(strSubKeyName, true);
                    }

                    return regSubKey;
                }
                catch
                {
                    return null;
                }
            }
        }
        #endregion

        #region 设置指定的名称/值对
        /// <summary>
        /// 设置指定的名称/值对
        /// </summary>
        /// <param name="strName">数值名称</param>
        /// <param name="strValue">数值数据</param>
        public static void SetRegData(string strName, string strValue)
        {
            try
            {
                //将数据写入注册表项
                RegKey.SetValue(strName, strValue);
            }
            catch
            {
            }
        }
        #endregion

        #region 读取与指定名称关联的值，不存在则返回null
        /// <summary>
        /// 读取与指定名称关联的值，不存在则返回null
        /// </summary>
        /// <param name="strName">数值名称</param>
        public static string GetRegData(string strName)
        {
            try
            {
                return RegKey.GetValue(strName).ToString();
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 根据名称判断指定的名称/值对是否存在
        /// <summary>
        /// 根据名称判断指定的名称/值对是否存在
        /// </summary>
        /// <param name="strName">数值名称</param>
        public static bool IsRegDataExists(string strName)
        {
            if (RegKey.GetValue(strName) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

    }
}
