using System;

namespace DreamCube.Foundation.Basic.Objects
{
    /// <summary>
    /// 文件系统的容量大小
    /// </summary>
    public class FileSystemSize
    {
        #region "内建枚举值"

        /// <summary>
        /// 容量的单位枚举值
        /// </summary>
        public enum HardDiskSizeType
        {
            /// <summary>
            /// byte
            /// </summary>
            B,
            /// <summary>
            /// kbyte
            /// </summary>
            KB,
            /// <summary>
            /// 兆字节（MB）
            /// </summary>
            MB,
            /// <summary>
            /// 或千兆字节（GB），
            /// </summary>
            GB
        }

        #endregion

        #region "属性"

        public UInt64 B
        {
            get;
            private set;
        }

        public UInt64 KB
        {
            get;
            private set;
        }

        public UInt64 MB
        {
            get;
            private set;
        }

        public UInt64 GB
        {
            get;
            private set;
        }

        #endregion

        public FileSystemSize(UInt64 size, HardDiskSizeType sizeType = HardDiskSizeType.B)
        {
            switch (sizeType)
            {
                case HardDiskSizeType.B:
                    B = size;
                    KB = size / 1024;
                    MB = KB / 1024;
                    GB = MB / 1024;
                    break;
                case HardDiskSizeType.KB:
                    B = size * 1024;
                    KB = size;
                    MB = KB / 1024;
                    GB = MB / 1024;
                    break;
                case HardDiskSizeType.MB:
                    KB = size * 1024;
                    B = KB * 1024;
                    MB = size;
                    GB = MB / 1024;
                    break;
                case HardDiskSizeType.GB:
                    GB = size;
                    MB = GB * 1024;
                    KB = MB * 1024;
                    B = KB * 1024;
                    break;
                default:
                    throw new ArgumentException("sizeType",
                                    String.Format(Properties.Resources.ExceptionArgumentEnumError, "sizeType", sizeType.ToString()));
            }
        }
    }
}
