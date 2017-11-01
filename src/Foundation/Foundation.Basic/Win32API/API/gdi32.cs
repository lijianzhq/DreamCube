using System;
using System.Runtime.InteropServices;

namespace DreamCube.Foundation.Basic.Win32API.API
{
    public class gdi32
    {
        /// <summary>
        /// 这个CreateRoundRectRgn 函数创建一个带圆角的矩形区域。
        /// </summary>
        /// <param name="left">指定了x坐标的左上角区域逻辑单位。</param>
        /// <param name="top"> 指定了y坐标的左上角区域逻辑单位。</param>
        /// <param name="right">指定了x坐标的右下角区域逻辑单位。 </param>
        /// <param name="bottom">指定了y坐标的右下角区域逻辑单位。</param>
        /// <param name="width">指定创建圆角的宽度逻辑单位。 </param>
        /// <param name="height">指定创建圆角的高度逻辑单位。</param>
        /// <returns>
        /// 如果函数成功，返回该区域的句柄。
        /// 如果函数失败，返回NULL。
        /// 在Windows NT/2000/XP中: 取得错误信息, 调用GetLastError。
        /// </returns>
        [DllImport("gdi32.dll",ExactSpelling=true,SetLastError=true)]
        public static extern Int32 CreateRoundRectRgn(Int32 left, Int32 top, Int32 right, Int32 bottom, Int32 width, Int32 height);

        [DllImport("gdi32.dll")]
        static public extern uint GetPixel(IntPtr hDC, Int32 XPos, Int32 YPos);
        [DllImport("gdi32.dll")]
        static public extern IntPtr CreateDC(String driverName, String deviceName, String output, IntPtr lpinitData);
        [DllImport("gdi32.dll")]
        static public extern bool DeleteDC(IntPtr DC);   
    }
}
