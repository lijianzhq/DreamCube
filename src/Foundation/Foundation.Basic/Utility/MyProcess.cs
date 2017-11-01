using System;
using System.Diagnostics;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyProcess
    {
        /// <summary>
        /// 使用ie打开指定的网页
        /// </summary>
        /// <param name="url"></param>
        public static void OpenUrlWithIE(String url)
        {
            //调用IE浏览器  
            System.Diagnostics.Process.Start("iexplore.exe", url);   
        }

        /// <summary>
        /// 查看系统是否存在指定名称的进程名(直接就是进程名（不需要后缀名），不区分大小写）
        /// </summary>
        /// <param name="processName">(直接就是进程名（不需要后缀名），不区分大小写）</param>
        /// <returns></returns>
        public static Boolean IsExistProcess(String processName)
        {
            Process[] p = System.Diagnostics.Process.GetProcessesByName(processName);
            //遍历所有的进程，判断状态
            if (p != null && p.Length > 0)
            {
                for (Int32 i = 0; i < p.Length; i++)
                {
                    if (!p[i].HasExited)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据进程名称关闭所有进程；退出成功，返回true；否则返回false(直接就是进程名（不需要后缀名），不区分大小写）
        /// </summary>
        /// <param name="processName">(直接就是进程名（不需要后缀名），不区分大小写）</param>
        /// <param name="waitForExitmilliseconds">等待进程退出的时长（毫秒），null表示不等待；</param>
        /// <param name="handleExceptionInTry"></param>
        /// <returns></returns>
        public static Boolean TryKillProcessByName(String processName, Int32? waitForExitmilliseconds = null, Enums.HandleExceptionInTry handleExceptionInTry = Enums.HandleExceptionInTry.ReturnAndMakeLog)
        {
            try
            {
                KillProcessByName(processName, waitForExitmilliseconds);
                return true;
            }
            catch (Exception ex)
            {
                switch (handleExceptionInTry)
                {
                    case Enums.HandleExceptionInTry.ReturnAndIgnoreLog:
                        return false;
                    case Enums.HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex);
                        return false;
                    default:
                    case Enums.HandleExceptionInTry.ThrowException:
                        throw;
                }
            }
        }

        /// <summary>
        /// 根据进程名称关闭所有进程
        /// </summary>
        /// <param name="processName">(直接就是进程名（不需要后缀名），不区分大小写）</param>
        /// <param name="waitForExitmilliseconds">等待进程退出的时长（毫秒），null表示不等待；</param>
        /// <returns></returns>
        public static void KillProcessByName(String processName, Int32? waitForExitmilliseconds = null)
        {
            Process[] p = System.Diagnostics.Process.GetProcessesByName(processName);
            if (p != null && p.Length > 0)
            {
                for (Int32 i = 0; i < p.Length; i++)
                {
                    if (waitForExitmilliseconds.HasValue)
                    {
                        p[i].WaitForExit(waitForExitmilliseconds.Value);
                    }
                    p[i].Kill();
                }
            }
        }

        /// <summary>
        /// 根据进程的标识符获取进程；获取成功返回true，获取失败返回false
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Boolean TryGetProcessById(Int32 pid, out Process p)
        {
            p = null;
            try
            {
                p = System.Diagnostics.Process.GetProcessById(pid);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
