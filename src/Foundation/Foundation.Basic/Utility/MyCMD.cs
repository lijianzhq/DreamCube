using System;
using System.IO;
using System.Diagnostics;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyCMD
    {
        /// <summary>
        /// 执行bat文件
        /// </summary>
        /// <param name="batPath"></param>
        public static void RunBat(String batPath)
        {
            Process pro = new Process();
            FileInfo file = new FileInfo(batPath);
            pro.StartInfo.WorkingDirectory = file.Directory.FullName;
            pro.StartInfo.FileName = batPath;
            pro.StartInfo.CreateNoWindow = false;
            pro.Start();
            pro.WaitForExit();
        }

        /// <summary>
        /// 打开文件夹
        /// </summary>
        /// <param name="folderPath"></param>
#if NET20
        public static void OpenFolder(String folderPath)
#else
        public static void OpenFolder(this String folderPath)
#endif
        {
            if (String.IsNullOrEmpty(folderPath)) return;
            Boolean folderExists = false;
            folderExists = Directory.Exists(folderPath);
            if (!folderExists)
            {
                throw new ArgumentException(String.Format(Properties.Resources.ExceptionFolderNotFound, folderPath));
            }
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e," + folderPath;
            System.Diagnostics.Process.Start(psi);
        }

        /// <summary>
        /// 打开文件夹并定位指定文件
        /// </summary>
        /// <param name="fileFullName"></param>
#if NET20
        public static void OpenFolderAndSelectFile(String fileFullName)
#else
        public static void OpenFolderAndSelectFile(this String fileFullName)
#endif
        {
            ProcessStartInfo psi = new ProcessStartInfo("Explorer.exe");
            psi.Arguments = "/e,/select," + fileFullName;
            System.Diagnostics.Process.Start(psi);
        }

        /// <summary>
        /// 直接在CMD窗口上面执行命令，不调用其他的exe文件，例如：ping 命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="workingDirectory">启动进程的初始目录</param>
        /// <returns></returns>
#if NET20
        public static String RunCmd(String[] cmd, String workingDirectory = "")
#else
        public static String RunCmd(this String[] cmd)
#endif
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.WorkingDirectory = workingDirectory;
            p.Start();
            p.StandardInput.AutoFlush = true;
            for (Int32 i = 0; i < cmd.Length; i++)
                p.StandardInput.WriteLine(cmd[i].ToString());
            p.StandardInput.WriteLine("exit");
            String strRst = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            p.Close();
            return strRst;
        }

        /// <summary>
        /// 调用*.MSI文件
        /// </summary>
        /// <param name="fileName">MSI文件路径</param>
        /// <param name="millisecondTimeOut">等等cmd进程退出的超时时间；如果为null，则无限等待终止</param>
        /// <returns></returns>
        public static String RunMSI(String fileName, String arguments, Int32? millisecondTimeOut = 3000)
        {
            String exeFileName = "msiexec.exe";
            String newArguments = string.Format(" /i \"{0}\" {1}", fileName, arguments);
            return MyCMD.RunCmd(new String[] { exeFileName + newArguments });
        }

        /// <summary>
        /// 调用一个exe程序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static String RunEXE(Objects.CmdRunParameters param)
        {
            return RunEXE(param.FileName, param.Arguments, param.MillisecondTimeOut, param.WaitForExit);
        }

        /// <summary>
        /// 调用一个exe程序
        /// </summary>
        /// <param name="fileName">命令名称（exe文件名）例如：netstat.exe</param>
        /// <param name="arguments">参数，例如：-an；-t</param>
        /// <param name="millisecondTimeOut">等等cmd进程退出的超时时间；如果为null，则无限等待终止</param>
        /// <param name="waitForExit">控制是否等待cmd进程退出（与第二个参数配置控制等待的时间）。true：等待（默认值）；false：不等待；</param>
#if NET20
        public static String RunEXE(String fileName, String arguments, Int32? millisecondTimeOut = 3000, Boolean waitForExit = true)
#else
        public static String RunEXE(this String fileName, String arguments, Int32? millisecondTimeOut = 3000)
#endif

        {
            Process process = new Process();
            process.StartInfo = new ProcessStartInfo(fileName, arguments);
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            String outputText = String.Empty;
            if (process.Start())
            {
                try
                {
                    if (waitForExit)
                    {
                        if (millisecondTimeOut == null || millisecondTimeOut == System.Threading.Timeout.Infinite)
                        {
                            process.WaitForExit();
                        }
                        else
                        {
                            process.WaitForExit(millisecondTimeOut.Value);
                            process.StandardInput.WriteLine("exit");
                        }
                    }
                    else
                    {
                        process.StandardInput.WriteLine("exit");
                    }
                    //指定一个超时任务对象，因为StreamReader.ReadToEnd方法读取不到任何字符的时候，会一直阻塞的
                    //等待一秒钟就退出
                    Objects.Task<Process> task = new Objects.Task<Process>(new Action<Process>((p) =>
                    {
                        try
                        {
                            StreamReader reader = p.StandardOutput;
                            outputText = reader.ReadToEnd();
                        }
                        catch (Exception ex)
                        {
                            MyLog.MakeLog(ex);
                        }
                    }), new TimeSpan(0, 0, 1));
                    task.Start(process);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    try
                    {
                        //因为很有可能是被强制退出了
                        if (process != null)
                            process.Close();
                    }
                    catch (Exception)
                    { }
                }
            }
            return outputText;
        }
    }
}
