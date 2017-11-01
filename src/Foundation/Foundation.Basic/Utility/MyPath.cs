using System;
using System.Reflection;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyPath
    {
        #region "属性"

        /// <summary>
        /// 获取程序的基目录
        /// </summary>
        public static String BaseDirectory
        {
            get
            {
                return System.AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        /// <summary>
        /// 获取模块的完整路径
        /// 可获得当前执行的exe的文件名。    
        /// </summary>
        public static String MainModuleFileName
        {
            get
            {
                return System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            }
        }

        /// <summary>
        /// 获取和设置当前目录(该进程从中启动的目录)的完全限定目录。
        /// 按照定义，如果该进程在本地或网络驱动器的根目录中启动，则此属性的值为驱动器名称后跟一个尾部反斜杠（如“C:\”）。
        /// 如果该进程在子目录中启动，则此属性的值为不带尾部反斜杠的驱动器和子目录路径（如“C:\mySubDirectory”）。 
        /// </summary>
        public static String CurrentDirectory
        {
            get
            {
                return System.Environment.CurrentDirectory;
            }
        }

        /// <summary>
        /// 获取应用程序的当前工作目录
        /// </summary>
        public static String CurrentWorkDirectory
        {
            get
            {
                return System.IO.Directory.GetCurrentDirectory();
            }
        }

        /// <summary>
        /// 获取和设置包括该应用程序的目录的名称
        /// </summary>
        public static String ApplicationBase
        {
            get
            {
                return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            }
        }

        /// <summary>
        /// 只能用于WinForm窗体中。
        /// 获取启动了应用程序的可执行文件的路径，不包括可执行文件的名称。
        /// </summary>
        public static String StartupPath
        {
            get
            {
                return System.Windows.Forms.Application.StartupPath;
            }
        }

        /// <summary>
        /// 获取启动了应用程序的可执行文件的路径，包括可执行文件的名称。
        /// </summary>
        public static String ExecutablePath
        {
            get
            {
                return System.Windows.Forms.Application.ExecutablePath;
            }
        }

        /// <summary>
        /// 获取启动了应用程序的可执行文件的路径，包括可执行文件的名称。
        /// </summary>
        public static String ExecutablePathEx
        {
            get
            {
                return Assembly.GetEntryAssembly().Location;
            }
        }

        #endregion

        #region "方法"

        /// <summary>
        /// 移除路径中不合法的字符
        /// </summary>
        /// <param name="fileFullName"></param>
        /// <returns></returns>
        public static String RemovePathAndFileNameInvalidChars(String fileFullName)
        {
            return RemoveFileNameInvalidChars(RemovePathInvalidChars(fileFullName));
        }

        /// <summary>
        /// 移除文件名中不合法的字符
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static String RemovePathInvalidChars(String path)
        {
            StringBuilder sb = new StringBuilder(path);
            Char [] invalidChars = Path.GetInvalidPathChars();
            //处理文件名的
            foreach (char rInvalidChar in invalidChars)
                sb.Replace(rInvalidChar.ToString(), string.Empty);
            //额外的操作，上面的数组并没有返回冒号
            sb.Replace(":", string.Empty);
            return sb.ToString();
        }

        /// <summary>
        /// 移除文件名中不合法的字符
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static String RemoveFileNameInvalidChars(String fileName)
        {
            StringBuilder sb = new StringBuilder(fileName);
            Char[] invalidChars = Path.GetInvalidFileNameChars();
            //处理文件名的
            foreach (char rInvalidChar in invalidChars)
                sb.Replace(rInvalidChar.ToString(), string.Empty);
            return sb.ToString();
        }

        /// <summary>
        /// 合并两个路径字符串（自动处理斜杠符号 \ ）
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        /// <returns></returns>
        public static String Combine(String path1, String path2)
        {
            path1 = path1.Replace("/", "\\");
            path2 = path2.Replace("/", "\\");
            if (!path1.EndsWith("\\")) path1 += "\\";
            if (path2.StartsWith("\\")) path2 = path2.Substring(1);
            return path1 + path2;
        }

        #endregion
    }
}
