using System;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;

namespace DreamCube.Foundation.LogService
{
    class SecurityHelper
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="unEncryptString">明文</param>
        /// <param name="key">密钥(长度必须8位以上)</param>
        /// <returns>密文数组</returns>
        public static Byte[] EncryptByKey(String unEncryptString, String key)
        {
            if (string.IsNullOrEmpty(key) || key.Length < 8)
            {
                throw new ArgumentException("min length is 8.", "key");
            }
            var cryptoServiceProvider = new DESCryptoServiceProvider();
            var inputByteArray = Encoding.UTF8.GetBytes(unEncryptString);
            Byte[] keyByte = ASCIIEncoding.ASCII.GetBytes(key.Substring(0, 8));
            cryptoServiceProvider.Key = keyByte;
            cryptoServiceProvider.IV = keyByte;
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, cryptoServiceProvider.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }

        /// <summary>
        /// byte数组转换为16进制字符串
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public static String ByteArrayToHexString(Byte[] byteArray)
        {
            var stringBuilder = new StringBuilder();
            foreach (var b in byteArray)
            {
                stringBuilder.AppendFormat("{0:X2}", b);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="unEncryptString">明文</param>
        /// <param name="key">密钥(长度必须8位以上)</param>
        /// <returns>密文（16进制表示的串）</returns>
        public static String EncryptToHexString(String unEncryptString, String key)
        {
            var byteArray = EncryptByKey(unEncryptString, key);
            return ByteArrayToHexString(byteArray);
        }
    }
}
