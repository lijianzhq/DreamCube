using System;
using System.Collections.Generic;
using System.Text;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyRand
    {
        #region "字段"

        /// <summary>
        /// 伪随机数生成器
        /// </summary>
        private static Random rand = new Random();

        /// <summary>
        /// 16进制的16个字符
        /// 定义一个字符串数组储存汉字编码的组成元素 
        /// </summary>
        private static String hexadecimalChars = "0123456789abcdef";

        #endregion

        #region "属性"

        /// <summary>
        /// 返回内核中的随机种子
        /// </summary>
        public static Random Random
        { get { return rand; } }

        #endregion

        #region "方法"

        /// <summary>
        /// 随机生成汉子字符串
        /// </summary>
        /// <param name="strLength">汉子个数</param>
        /// <returns></returns>
#if NET20
        public static String NextString_ChineseCharacters(Int32 strLength = 4)
#else
        public static String NextString_ChineseCharacters(Int32 strLength = 4)
#endif
        {
            /*  每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中 
                每个汉字有四个区位码组成 
                区位码第1位和区位码第2位作为字节数组第一个元素 
                区位码第3位和区位码第4位作为字节数组第二个元素 
            */
            StringBuilder builder = new StringBuilder();
            Encoding gb2312 = Encoding.GetEncoding("gb2312");  //中文字符编码对象
            for (Int32 i = 0; i < strLength; i++)
            {
                //区位码第1位 
                Int32 r1 = rand.Next(11, 14);
                String str_r1 = hexadecimalChars.Substring(r1, 1);
                //区位码第2位 
                Int32 r2;
                if (r1 == 13) r2 = rand.Next(0, 7);
                else r2 = rand.Next(0, 16);
                String str_r2 = hexadecimalChars.Substring(r2, 1);
                //区位码第3位 
                Int32 r3 = rand.Next(10, 16);
                String str_r3 = hexadecimalChars.Substring(r3, 1);
                //区位码第4位 
                Int32 r4;
                if (r3 == 10) r4 = rand.Next(1, 16);
                else if (r3 == 15) r4 = rand.Next(0, 15);
                else r4 = rand.Next(0, 16);
                String str_r4 = hexadecimalChars.Substring(r4, 1);
                //定义两个字节变量存储产生的随机汉字区位码 
                Byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                Byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中 
                Byte[] str_r = new Byte[] { byte1, byte2 };
                //将产生的一个汉字的字节数组放入object数组中 
                builder.Append(gb2312.GetString(str_r));
            }
            return builder.ToString();
        }

        /// <summary>
        /// 随机获取字符串中的几个字符
        /// </summary>
        /// <param name="target">指定的随机选取字符集</param>
        /// <param name="charsCount">随机获取字符的个数</param>
        /// <returns></returns>
#if NET20
        public static String GetRandomChars(String allChars, Int32 charsCount = 4)
#else
        public static String GetRandomChars(this String allChars, Int32 charsCount = 4)
#endif
        {
            if (String.IsNullOrEmpty(allChars)) return String.Empty;
            StringBuilder builder = new StringBuilder();
            for (Int32 i = 0; i < charsCount; i++)
                builder.Append(allChars[MyRand.Random.Next(0, allChars.Length)]);
            return builder.ToString();
        }

        /// <summary>
        /// 随机生成数字字符串
        /// </summary>
        /// <returns></returns>
#if NET20
        public static String NextString_Number(Int32 strLength = 4, String allChars = "0123465789")
#else
        public static String NextString_Number(Int32 strLength = 4, String allChars = "0123465789")
#endif
        {
            return GetRandomChars(allChars, strLength);
        }

        /// <summary>
        /// 随机生成英文字符(大写)
        /// </summary>
        /// <param name="strLength"></param>
        /// <returns></returns>
#if NET20
        public static String NextString_EnglishAlphabets_UpperCase(Int32 strLength = 4, String allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
#else
        public static String NextString_EnglishAlphabets_UpperCase(Int32 strLength = 4, String allChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ")
#endif
        {
            return GetRandomChars(allChars, strLength);
        }

        /// <summary>
        /// 随机生成英文字符(小写)
        /// </summary>
        /// <param name="strLength">随机字符串长度</param>
        /// <returns></returns>
#if NET20
        public static String NextString_EnglishAlphabets_LowerCase(Int32 strLength = 4, String allChars = "abcdefghijklmnopqrstuvwxyz")
#else
        public static String NextString_EnglishAlphabets_LowerCase(Int32 strLength = 4, String allChars = "abcdefghijklmnopqrstuvwxyz")
#endif
        {
            return GetRandomChars(allChars, strLength);
        }

        #endregion
    }
}
