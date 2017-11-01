using System;
using System.Runtime.InteropServices;

namespace DreamCube.Foundation.Basic.Win32API.API
{
    /// <summary>
    /// Ws2_32.dll引用
    /// </summary>
    public class Ws2_32
    {
        /// <summary>
        /// 将一个点分十进制的IP转换成一个长整数型数（u_long类型） 
        /// </summary>
        /// <param name="ipaddr">字符串，一个点分十进制的IP地址</param>
        /// <returns></returns>
        [DllImport("Ws2_32.dll")] 
        public static extern Int32 inet_addr(String ipaddr);   
    }
}
