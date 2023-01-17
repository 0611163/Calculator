using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Tool
{
    /// <summary>
    /// DES加密/解密类。
    /// suxiang
    /// 2012年8月12日
    /// </summary>
    public class DESEncrypt
    {

        #region ========加密========

        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text">原文</param> 
        /// <param name="sKey">密钥</param> 
        /// <returns>密文</returns> 
        public static string Encrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider desKey = new DESCryptoServiceProvider();

            byte[] inputByteArray = Encoding.Default.GetBytes(Text);
            byte[] keyByteArray = Encoding.Default.GetBytes(sKey);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(keyByteArray);

            desKey.Key = HalveByteArray(md5.Hash);
            desKey.IV = HalveByteArray(md5.Hash);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, desKey.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder result = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                result.AppendFormat("{0:X2}", b);
            }

            return result.ToString();
        }

        #endregion

        #region ========解密========

        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text">密文</param> 
        /// <param name="sKey">密钥</param> 
        /// <returns>原文</returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider desKey = new DESCryptoServiceProvider();

            byte[] inputByteArray = HalveByteArray(Text);
            byte[] keyByteArray = Encoding.Default.GetBytes(sKey);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(keyByteArray);

            desKey.Key = HalveByteArray(md5.Hash);
            desKey.IV = HalveByteArray(md5.Hash);

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, desKey.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion

        #region 把字节数组缩短一半
        /// <summary>
        /// 把字节数组缩短一半
        /// </summary>
        private static byte[] HalveByteArray(byte[] data)
        {
            byte[] result = new byte[data.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                int x = (data[i * 2] + data[i * 2 + 1]) / 2;
                result[i] = (byte)x;
            }

            return result;
        }
        #endregion

        #region 把十六进制字符串转换成字节数组，再把字节数组缩短一半
        /// <summary>
        /// 把十六进制字符串转换成字节数组，再把字节数组缩短一半
        /// </summary>
        private static byte[] HalveByteArray(string text)
        {
            byte[] result = new byte[text.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                int x = Convert.ToInt32(text.Substring(i * 2, 2), 16);
                result[i] = (byte)x;
            }

            return result;
        }
        #endregion

    }
}
