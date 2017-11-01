using System;
using System.Text;
using System.Runtime.InteropServices;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyByte
    {
        /// <summary>
        /// 格式化二进制数组的长度，如果数组不够指定长度，则在后面补足0，如果超长，则截取长度
        /// </summary>
        /// <param name="target"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static Byte[] FormatLength(Byte[] target, Int32 length)
        {
            if (target == null) return null;
            if (target.Length == length) return target;
            Int32 copyLength = length;
            Byte[] tempByte = new Byte[length];
            //直接复制数组
            Array.Copy(target, tempByte, length);
            return tempByte;
        }

        /// <summary>
        /// 比较两个byte数组是否相等
        /// </summary>
        /// <param name="target"></param>
        /// <param name="target2"></param>
        /// <returns></returns>
#if NET20
        public static Boolean EqualEx(Byte[] target, Byte[] target2)
#else
        public static Boolean EqualEx(this Byte[] target, Byte[] target2)
#endif
        {
            if (target.Length != target2.Length) return false;
            if (target == null || target2 == null) return false;
            for (Int32 i = 0; i < target2.Length; i++)
                if (target2[i] != target[i])
                    return false;
            return true;
        }

        /// <summary>
        /// 结构体转成Byte数组
        /// </summary>
        /// <param name="structObj"></param>
        /// <returns></returns>
#if NET20
        public static Byte[] StructToBytes(Object structObj)
#else
        public static Byte[] StructToBytes(this Object structObj)
#endif
        {
            ///得到结构体的大小
            Int32 size = Marshal.SizeOf(structObj);
            Byte[] bytes = new Byte[size];
            ///在非托管中分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            ///将结构体拷贝到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            ///从内存空间中拷贝到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            ///释放内存空间
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }

        /// <summary>
        /// byte数组转结构体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
#if NET20
        public static T BytesToStruct<T>(Byte[] bytes) where T : struct
#else
        public static T BytesToStruct<T>(this Byte[] bytes) where T : struct
#endif
        {
            Type type = typeof(T);
            Int32 size = Marshal.SizeOf(type);
            //byte数组长度小于结构体的大小
            if (size > bytes.Length)
                return (T)new Nullable<T>();
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构体
            Object obj = Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            return (T)obj;
        }

        /// <summary>
        /// 把二进制转换为16进制
        /// </summary>
        /// <param name="bytes">二进制数组</param>
        /// <param name="formatType">格式化类型，可以为：{0:X},{0:X1},{0:X2}...将二进制转换为指定位数的十六进制数，不足位数的，前补零</param>
        /// <param name="startIndex">数组的起始位置</param>
        /// <param name="length">从起始位置开始算起，格式化多少个字节；-1表示格式化到最后一位字节</param>
        /// <returns></returns>
        public static String ToHex(Byte[] bytes, String formatType = "{0:X2}", Int32 startIndex = 0, Int32 length = -1)
        {
            if (null == bytes) return String.Empty;
            Int32 total = bytes.Length;
            Int32 end = startIndex + (length < 0 ? bytes.Length : length);
            if (total < end) return String.Empty;
            StringBuilder sb = new StringBuilder();
            for (Int32 i = startIndex; i < end; ++i)
            {
                sb.AppendFormat(formatType, bytes[i]);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 把二进制数组的数据转换成字符串，默认采用UTF8的编码方式
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
#if NET20
        public static String ToStringEx(Byte[] bytes, Encoding encoding = null)
#else
        public static String ToStringEx(this Byte[] bytes, Encoding encoding = null)
#endif
        {
            if (encoding == null) encoding = Encoding.UTF8;
            return encoding.GetString(bytes);
        }
    }
}
