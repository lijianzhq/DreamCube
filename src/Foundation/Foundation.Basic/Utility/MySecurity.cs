using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace DreamCube.Foundation.Basic.Utility
{
    /// <summary>
    /// 安全加密/解密的相关方法
    /// </summary>
    public static class MySecurity
    {
        /// <summary>
        /// MD5加密（最终结果是32位长度小写的字符串）
        /// </summary>
        /// <param name="unEncryptString"></param>
        /// <returns></returns>
        public static String EncryptToMD5_32_Lower(String unEncryptString)
        {
            //if (String.IsNullOrEmpty(text)) return "";
            //String outputStr = String.Empty;
            //Byte[] dataToHash = (new System.Text.ASCIIEncoding()).GetBytes(text);
            //Byte[] hashvalue = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(dataToHash);
            //return Encoding.UTF8.GetString(hashvalue);
            //Byte[] data = System.Text.Encoding.Unicode.GetBytes(text.ToCharArray());
            ////创建一个Md5对象
            //System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            ////加密Byte[]数组
            //Byte[] result = md5.ComputeHash(data);
            ////将加密后的数组转化为字段
            return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(unEncryptString, "MD5").ToLower();
        }

        /// <summary>
        /// 使用dll中默认的key值对数据进行加密
        /// </summary>
        /// <param name="unEncryptString"></param>
        /// <returns>密文(16进制字符表示)</returns>
        public static String DESEncrypt(String unEncryptString)
        {
            return DESEncryptToHexString(unEncryptString, Properties.Resources.CryptKey);
        }

        /// <summary>
        /// 使用dll中默认的key值对数据进行解密（加密串是16进制字符表示）
        /// </summary>
        /// <param name="hexEncryptString"></param>
        /// <returns></returns>
        public static String DESDecrypt(String hexEncryptString)
        {
            return DESDecryptHexString(hexEncryptString, Properties.Resources.CryptKey);
        }

        /// <summary>
        /// 根据提供的Key值，对数据进行加密（可逆的加密方法）
        /// </summary>
        /// <param name="unEncryptString">明文</param>
        /// <param name="key">密匙</param>
        /// <returns>密文(base64字符串表示)</returns>
        public static String DESEncryptToBase64String(String unEncryptString, String key)
        {

            Byte[] byteArray = DESEncryptByKey(unEncryptString, key);
            return Convert.ToBase64String(byteArray);
        }

        /// <summary>
        /// 根据提供的key，把密文解密
        /// </summary>
        /// <param name="base64EncryptString">密文（base64编码字符表示的密文串）</param>
        /// <param name="key">key</param>
        /// <returns>明文</returns>
        public static String DESDecryptBase64String(String base64EncryptString, String key)
        {
            Byte[] byteArray = Convert.FromBase64String(base64EncryptString);
            return DESDecryptByKey(byteArray, key);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="unEncryptString">明文</param>
        /// <param name="key">密钥(长度必须8位以上)</param>
        /// <returns>密文（16进制表示的串）</returns>
        public static String DESEncryptToHexString(String unEncryptString, String key)
        {
            var byteArray = DESEncryptByKey(unEncryptString, key);
            return MyString.ByteArrayToHexString(byteArray);
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="hexEncryptString">密文（16进制字符表示的密文串）</param>
        /// <param name="key">//密钥(长度必须8位以上)</param>
        /// <returns>明文</returns>
        public static String DESDecryptHexString(String hexEncryptString, String key)
        {
            var inputByteArray = MyString.HexStringToByteArray(hexEncryptString);
            return DESDecryptByKey(inputByteArray, key);
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="unEncryptString">明文</param>
        /// <param name="key">密钥(长度必须8位以上)</param>
        /// <returns>密文数组</returns>
        public static Byte[] DESEncryptByKey(String unEncryptString, String key)
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
        /// 解密
        /// </summary>
        /// <param name="inputByteArray">密文Byte数组</param>
        /// <param name="key">//密钥(长度必须8位以上)</param>
        /// <returns>明文</returns>
        public static String DESDecryptByKey(Byte[] inputByteArray, String key)
        {
            if (String.IsNullOrEmpty(key) || key.Length < 8)
            {
                throw new ArgumentException("min length is 8.", "key");
            }
            var cryptoServiceProvider = new DESCryptoServiceProvider();
            Byte[] keyByte = ASCIIEncoding.ASCII.GetBytes(key.Substring(0, 8));
            cryptoServiceProvider.Key = keyByte;
            cryptoServiceProvider.IV = keyByte;
            var memoryStream = new MemoryStream();
            var cryptoStream = new CryptoStream(memoryStream, cryptoServiceProvider.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(inputByteArray, 0, inputByteArray.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }

        /// <summary>
        /// 调用RSA算法加密（通过PEMfile文件的公钥，返回加密二进制数据的16进制表示方式）
        /// </summary>
        /// <param name="unEncryptString">明文（需要加密的数据）</param>
        /// <param name="pemFilePublicKey">pemfile的publickey（公钥）</param>
        /// <returns></returns>
        public static String RSAEncrypt_PEM_ToHexString(String unEncryptString, String pemFilePublicKey)
        {
            RSAParameters param = GetKeyPara(pemFilePublicKey, 1);
            RSACryptoServiceProvider provider = new RSACryptoServiceProvider();
            provider.ImportParameters(param);
            return MyByte.ToHex(provider.Encrypt(Encoding.UTF8.GetBytes(unEncryptString), false));
        }

        /// <summary>
        /// 根据PEM文件的key获取.NET的RSA parameters
        /// </summary>
        /// <param name="PEMFileKey">PEMFilekey</param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static RSAParameters GetKeyPara(String PEMFileKey, Int32 type)
        {
            RSAParameters rsaP = new RSAParameters();
            byte[] tmpKeyNoB64 = Convert.FromBase64String(PEMFileKey);
            int pemModulus = 128;
            int pemPublicExponent = 3;
            int pemPrivateExponent = 128;
            int pemPrime1 = 64;
            int pemPrime2 = 64;
            int pemExponent1 = 64;
            int pemExponent2 = 64;
            int pemCoefficient = 64;


            byte[] arrPemModulus = new byte[128];
            byte[] arrPemPublicExponent = new byte[3];
            byte[] arrPemPrivateExponent = new byte[128];
            byte[] arrPemPrime1 = new byte[64];
            byte[] arrPemPrime2 = new byte[64];
            byte[] arrPemExponent1 = new byte[64];
            byte[] arrPemExponent2 = new byte[64];
            byte[] arrPemCoefficient = new byte[64];

            if (type == 0)//私钥
            {
                //Modulus
                for (int i = 0; i < pemModulus; i++)
                {
                    arrPemModulus[i] = tmpKeyNoB64[11 + i];
                }
                rsaP.Modulus = arrPemModulus;

                //PublicExponent
                for (int i = 0; i < pemPublicExponent; i++)
                {
                    arrPemPublicExponent[i] = tmpKeyNoB64[141 + i];
                }
                rsaP.Exponent = arrPemPublicExponent;

                //PrivateExponent
                for (int i = 0; i < pemPrivateExponent; i++)
                {
                    arrPemPrivateExponent[i] = tmpKeyNoB64[147 + i];
                }
                rsaP.D = arrPemPrivateExponent;

                //Prime1
                for (int i = 0; i < pemPrime1; i++)
                {
                    arrPemPrime1[i] = tmpKeyNoB64[278 + i];
                }
                rsaP.P = arrPemPrime1;

                //Prime2
                for (int i = 0; i < pemPrime2; i++)
                {
                    arrPemPrime2[i] = tmpKeyNoB64[345 + i];
                }
                rsaP.Q = arrPemPrime2;


                //Exponent1
                for (int i = 0; i < pemExponent1; i++)
                {
                    arrPemExponent1[i] = tmpKeyNoB64[412 + i];
                }
                rsaP.DP = arrPemExponent1;

                //Exponent2
                for (int i = 0; i < pemExponent2; i++)
                {
                    arrPemExponent2[i] = tmpKeyNoB64[478 + i];
                }
                rsaP.DQ = arrPemExponent2;

                //Coefficient
                for (int i = 0; i < pemCoefficient; i++)
                {
                    arrPemCoefficient[i] = tmpKeyNoB64[545 + i];
                }
                rsaP.InverseQ = arrPemCoefficient;
            }
            else//公钥
            {
                for (int i = 0; i < pemModulus; i++)
                {
                    arrPemModulus[i] = tmpKeyNoB64[29 + i];
                }
                rsaP.Modulus = arrPemModulus;

                for (int i = 0; i < pemPublicExponent; i++)
                {
                    arrPemPublicExponent[i] = tmpKeyNoB64[159 + i];
                }
                rsaP.Exponent = arrPemPublicExponent;

            }
            return rsaP;
        }
    }
}
