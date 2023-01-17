using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Tool;

namespace 计算器
{
    /// <summary>
    /// 文件操作
    /// suxiang
    /// 2012年8月13日
    /// </summary>
    class FileOperator
    {
        //文件路径及名称
        private static string filePathName = DESEncrypt.Decrypt("0398850297D8564B5732C8FF167C4C8A", Common.DESKey);

        #region 名称/值对加密并写入文件
        /// <summary>
        /// 名称/值对加密并写入文件
        /// </summary>
        /// <returns>是否保存成功</returns>
        public static bool SetValue(string strName, string strValue)
        {
            try
            {
                if (!File.Exists(filePathName))//如果文件不存在
                {
                    //创建文件
                    FileStream fileStream = new FileStream(filePathName, FileMode.Create);
                    //释放资源
                    fileStream.Close();
                    fileStream.Dispose();
                }
                //设置文件属性，取消只读等属性，以便可以写入
                File.SetAttributes(filePathName, FileAttributes.Archive);
                //打开文件
                StreamReader streamReader = new StreamReader(filePathName);
                //读取文件
                string str = streamReader.ReadToEnd();
                //释放资源
                streamReader.Close();
                streamReader.Dispose();
                //解密
                if (str != "")
                {
                    str = DESEncrypt.Decrypt(str, Common.DESKey);
                }
                //替换内容
                if (str.IndexOf(strName) == -1)//名称不存在
                {
                    if (str == "")
                    {
                        str += strName + "，，" + strValue;
                    }
                    else
                    {
                        str += "|" + strName + "，，" + strValue;
                    }
                }
                else//名称存在
                {
                    string[] items = str.Split('|');
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].IndexOf(strName) != -1)//找到
                        {
                            //替换
                            items[i] = items[i].Substring(0, items[i].IndexOf("，，")) + "，，" + strValue;
                        }
                    }
                    StringBuilder newStr = new StringBuilder();
                    newStr.Append(items[0]);
                    for (int i = 1; i < items.Length; i++)
                    {
                        newStr.Append("|" + items[i]);
                    }
                    str = newStr.ToString();
                }
                //重新写入文件，注意写入前加密
                StreamWriter streamWriter = new StreamWriter(filePathName);
                streamWriter.Write(DESEncrypt.Encrypt(str, Common.DESKey));
                streamWriter.Flush();
                //释放资源
                streamWriter.Close();
                streamWriter.Dispose();
                //伪装和隐藏文件
                Random rnd = new Random();
                DateTime dt = new DateTime(DateTime.Now.Year - 3, 6, 13, 9, 37, 27);
                File.SetCreationTime(filePathName, dt);
                File.SetLastWriteTime(filePathName, dt);
                File.SetLastAccessTime(filePathName, dt);
                File.SetAttributes(filePathName, FileAttributes.Hidden | FileAttributes.ReadOnly | FileAttributes.System | FileAttributes.Archive);
                //操作成功返回true
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 根据名称读取文件中的名称/值对对应的值
        /// <summary>
        /// 根据名称读取文件中的名称/值对对应的值
        /// </summary>
        public static string GetValue(string strName)
        {
            try
            {
                if (File.Exists(filePathName))//文件存在
                {
                    string result = "";
                    //打开文件
                    StreamReader streamReader = new StreamReader(filePathName);
                    //读取文件
                    string str = streamReader.ReadToEnd();
                    //释放资源
                    streamReader.Close();
                    streamReader.Dispose();
                    //解密
                    if (str != "")
                    {
                        str = DESEncrypt.Decrypt(str, Common.DESKey);
                    }
                    //读取内容
                    string[] items = str.Split('|');
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (items[i].IndexOf(strName) != -1)//找到
                        {
                            //读取
                            result = items[i].Substring(items[i].IndexOf("，，") + 2);
                        }
                    }

                    return result;
                }
                else//文件不存在
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

        #region 根据名称判断名称/值对是否存在
        /// <summary>
        /// 根据名称判断名称/值对是否存在
        /// </summary>
        public static bool Exists(string strName)
        {
            if (GetValue(strName) == "")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

    }
}
