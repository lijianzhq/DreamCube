using System;

namespace DreamCube.Foundation.Basic.Utility
{
    /// <summary>
    /// 获取表示全局唯一标识符 (GUID)
    /// </summary>
    public static class MyGuid
    {
        #region "静态方法"

        /// <summary>
        /// 获取表示全局唯一标识符 (GUID)
        /// 其中 GUID 的值表示为一系列小写的十六进制位，这些十六进制位分别以 8 个、4 个、4 个、4 个和 12 个位为一组并由连字符分隔开
        /// 格式：xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx 
        /// </summary>
        /// <returns></returns>
        public static String GetNewGuid(GuidStringFormatType type = GuidStringFormatType.Normal)
        {
            switch (type)
            {
                case GuidStringFormatType.Normal:
                    return Guid.NewGuid().ToString();
                case GuidStringFormatType.ToLower:
                    return Guid.NewGuid().ToString().ToLower();
                case GuidStringFormatType.ToUpper:
                    return Guid.NewGuid().ToString().ToUpper();
                default:
                    return Guid.NewGuid().ToString();
            }
        }

        /// <summary>
        /// 获取表示全局唯一标识符 (GUID)
        /// 格式：xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx 
        /// </summary>
        /// <returns></returns>
        public static String GetNewGuid_N(GuidStringFormatType type = GuidStringFormatType.Normal)
        {
            switch (type)
            {
                case GuidStringFormatType.Normal:
                    return Guid.NewGuid().ToString("N");
                case GuidStringFormatType.ToLower:
                    return Guid.NewGuid().ToString("N").ToLower();
                case GuidStringFormatType.ToUpper:
                    return Guid.NewGuid().ToString("N").ToUpper();
                default:
                    return Guid.NewGuid().ToString("N");
            }
        }

        /// <summary>
        /// 获取表示全局唯一标识符 (GUID)
        /// 格式：xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx 
        /// </summary>
        /// <returns></returns>
        public static String GetNewGuid_D(GuidStringFormatType type = GuidStringFormatType.Normal)
        {
            switch (type)
            {
                case GuidStringFormatType.Normal:
                    return Guid.NewGuid().ToString("D");
                case GuidStringFormatType.ToLower:
                    return Guid.NewGuid().ToString("D").ToLower();
                case GuidStringFormatType.ToUpper:
                    return Guid.NewGuid().ToString("D").ToUpper();
                default:
                    return Guid.NewGuid().ToString("D");
            }
        }

        /// <summary>
        /// 获取表示全局唯一标识符 (GUID)
        /// 格式：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx) 
        /// </summary>
        /// <returns></returns>
        public static String GetNewGuid_P(GuidStringFormatType type = GuidStringFormatType.Normal)
        {
            switch (type)
            {
                case GuidStringFormatType.Normal:
                    return Guid.NewGuid().ToString("P");
                case GuidStringFormatType.ToLower:
                    return Guid.NewGuid().ToString("P").ToLower();
                case GuidStringFormatType.ToUpper:
                    return Guid.NewGuid().ToString("P").ToUpper();
                default:
                    return Guid.NewGuid().ToString("P");
            }
        }

        /// <summary>
        /// 获取表示全局唯一标识符 (GUID)
        /// 格式：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx} 
        /// </summary>
        /// <returns></returns>
        public static String GetNewGuid_B(GuidStringFormatType type = GuidStringFormatType.Normal)
        {
            switch (type)
            {
                case GuidStringFormatType.Normal:
                    return Guid.NewGuid().ToString("B");
                case GuidStringFormatType.ToLower:
                    return Guid.NewGuid().ToString("B").ToLower();
                case GuidStringFormatType.ToUpper:
                    return Guid.NewGuid().ToString("B").ToUpper();
                default:
                    return Guid.NewGuid().ToString("B");
            }
        }

        /// <summary>
        /// 括在大括号的 4 个十六进制值，其中第 4 个值是 8 个十六进制值的子集（也括在大括号中）：
        /// {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static String GetNewGuid_X(GuidStringFormatType type = GuidStringFormatType.Normal)
        {
            switch (type)
            {
                case GuidStringFormatType.Normal:
                    return Guid.NewGuid().ToString("X");
                case GuidStringFormatType.ToLower:
                    return Guid.NewGuid().ToString("X").ToLower();
                case GuidStringFormatType.ToUpper:
                    return Guid.NewGuid().ToString("X").ToUpper();
                default:
                    return Guid.NewGuid().ToString("X");
            }
        }

        /// <summary>
        /// 获取表示全局唯一标识符 (GUID)
        /// 格式：xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx 
        /// </summary>
        /// <returns></returns>
#if NET20
        public static String To_N(Guid oTargetGuid)
#else 
        public static String To_N(this Guid oTargetGuid)
#endif
        {
            return oTargetGuid.ToString("N");
        }

        /// <summary>
        /// 获取表示全局唯一标识符 (GUID)
        /// 格式：xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx 
        /// </summary>
        /// <returns></returns>
#if NET20
        public static String To_D(Guid oTargetGuid)
#else 
        public static String To_D(this Guid oTargetGuid)
#endif
        {
            return oTargetGuid.ToString("D");
        }

        /// <summary>
        /// 获取表示全局唯一标识符 (GUID)
        /// 格式：(xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx) 
        /// </summary>
        /// <returns></returns>
 #if NET20
        public static String To_P(Guid oTargetGuid)
#else 
        public static String To_P(this Guid oTargetGuid)
#endif
        {
            return oTargetGuid.ToString("P");
        }

        /// <summary>
        /// 获取表示全局唯一标识符 (GUID)
        /// 格式：{xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx} 
        /// </summary>
        /// <returns></returns>
#if NET20
        public static String To_B(Guid oTargetGuid)
#else
        public static String To_B(this Guid oTargetGuid)
#endif
        {
            return oTargetGuid.ToString("B");
        }

        /// <summary>
        /// 括在大括号的 4 个十六进制值，其中第 4 个值是 8 个十六进制值的子集（也括在大括号中）：
        /// {0x00000000,0x0000,0x0000,{0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00}}
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String To_X(Guid target)
#else
        public static String To_X(this Guid target)
#endif
        {
            return target.ToString("X");
        }

        #endregion

        #region "枚举类型"

        /// <summary>
        /// 生成的GUID字符串进行格式化处理的类型
        /// </summary>
        public enum GuidStringFormatType
        {
            /// <summary>
            /// 什么都不处理，直接返回生成的字符串
            /// </summary>
            Normal,

            /// <summary>
            /// 转换为大写
            /// </summary>
            ToUpper,

            /// <summary>
            /// 转换为小写
            /// </summary>
            ToLower
        }

        #endregion
    }
}
