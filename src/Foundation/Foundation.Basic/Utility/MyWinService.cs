using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyWinService
    {
        /// <summary>
        /// 服务的状态
        /// </summary>
        public enum ServiceStatus
        {
            /// <summary>
            /// 服务未运行。这对应于 Win32 SERVICE_STOPPED 常数，该常数定义为 0x00000001。
            /// </summary>
            [Description("服务未运行")]
            Stopped = 1,
            
            /// <summary>
            /// 服务正在启动。这对应于 Win32 SERVICE_START_PENDING 常数，该常数定义为 0x00000002。
            /// </summary>
            [Description("服务正在启动")]
            StartPending = 2,
            
            /// <summary>
            /// 服务正在停止。这对应于 Win32 SERVICE_STOP_PENDING 常数，该常数定义为 0x00000003。
            /// </summary>
            [Description("服务正在停止")]
            StopPending = 3,
            
            /// <summary>
            /// 服务正在运行。这对应于 Win32 SERVICE_RUNNING 常数，该常数定义为 0x00000004。
            /// </summary>
            [Description("服务正在运行")]
            Running = 4,
           
            /// <summary>
            /// 服务即将继续。这对应于 Win32 SERVICE_CONTINUE_PENDING 常数，该常数定义为 0x00000005。
            /// </summary>
            [Description("服务即将继续")]
            ContinuePending = 5,
            
            /// <summary>
            /// 服务即将暂停。这对应于 Win32 SERVICE_PAUSE_PENDING 常数，该常数定义为 0x00000006。
            /// </summary>
            [Description("服务即将暂停")]
            PausePending = 6,

            /// <summary>
            /// 服务已暂停。这对应于 Win32 SERVICE_PAUSED 常数，该常数定义为 0x00000007。
            /// </summary>
            [Description("服务已暂停")]
            Paused = 7,

             /// <summary>
            /// 服务未安装
            /// </summary>
            [Description("服务未安装")]
            NotExist = 8
        }

        /// <summary>
        /// 判断服务是否存在
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public static Boolean ServiceExisted(String serviceName, String machineName = "")
        {
            ServiceController[] services = String.IsNullOrEmpty(machineName) ?
                                            ServiceController.GetServices() :
                                            ServiceController.GetServices(machineName);
            for (Int32 i = 0; i < services.Length; i++)
            {
                if (String.Equals(services[i].ServiceName, serviceName.ToLower(), StringComparison.CurrentCultureIgnoreCase))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 安装服务
        /// </summary>
        /// <param name="filePath">服务的exe文件路径</param>
        /// <param name="savedState">
        /// 更新 savedState 参数指定的 IDictionary 对象，以反映包含的安装程序运行后安装的状态
        /// 当传递给 Install 方法时， savedState 参数指定的 IDictionary 应为空（不能为null）。
        /// </param>
        /// <returns></returns>
        public static Boolean InstallService(String filePath, ref IDictionary savedState)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = filePath;
                installer.Install(savedState);
                installer.Commit(savedState);
            }
            return true;
        }

        /// <summary>
        /// 获取windows服务的状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ServiceControllerStatus GetServiceStatus(String name)
        {
            using (ServiceController sc = new ServiceController(name))
            {
                return sc.Status;
            }
        }

        /// <summary>
        /// 获取服务的状态
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ServiceStatus GetServiceStatusEx(String name)
        {
            if (ServiceExisted(name))
            {
                using (ServiceController sc = new ServiceController(name))
                {
                    switch (sc.Status)
                    {
                        case ServiceControllerStatus.ContinuePending:
                            return ServiceStatus.ContinuePending;
                        case ServiceControllerStatus.Paused:
                            return ServiceStatus.Paused;
                        case ServiceControllerStatus.PausePending:
                            return ServiceStatus.PausePending;
                        case ServiceControllerStatus.Running:
                            return ServiceStatus.Running;
                        case ServiceControllerStatus.StartPending:
                            return ServiceStatus.StartPending;
                        case ServiceControllerStatus.Stopped:
                            return ServiceStatus.Stopped;
                        default:
                        case ServiceControllerStatus.StopPending:
                            return ServiceStatus.StopPending;
                    }
                }
            }
            else
            {
                return ServiceStatus.NotExist;
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="name">服务名称</param>
        /// <param name="timeout">超时时间，默认为1分钟</param>
        /// <returns></returns>
        public static Boolean StartService(String name, TimeSpan? timeout = null)
        {
            timeout = timeout == null ? new TimeSpan(0, 1, 0) : timeout;
            using (ServiceController sc = new ServiceController(name))
            {
                if (sc != null &&
                    (sc.Status == ServiceControllerStatus.StopPending || sc.Status == ServiceControllerStatus.Stopped))
                {
                    sc.Start();
                    sc.WaitForStatus(ServiceControllerStatus.Running, timeout.Value);
                }
            }
            return true;
        }

        /// <summary>
        /// 停止windows服务
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeout">超时时间，默认为1分钟</param>
        /// <returns></returns>
        public static Boolean StopService(String name, TimeSpan? timeout = null)
        {
            timeout = timeout == null ? new TimeSpan(0, 1, 0) : timeout;
            using (ServiceController sc = new ServiceController(name))
            {
                if (sc != null &&
                    (sc.Status == ServiceControllerStatus.Running || sc.Status == ServiceControllerStatus.StartPending))
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 1, 0));
                }
            }
            return true;
        }

        /// <summary>
        /// 卸载windows服务
        /// </summary>
        /// <param name="filePath">服务的exe文件路径</param>
        /// <param name="savedState">
        /// 更新 savedState 参数指定的 IDictionary 对象，以反映包含的安装程序运行后安装的状态
        /// 当传递给 Install 方法时， savedState 参数指定的 IDictionary 应为空（不能为null）。
        /// </param>
        /// <returns></returns>
        public static Boolean UnInstallService(String filePath, ref IDictionary savedState)
        {
            using (AssemblyInstaller unInstaller = new AssemblyInstaller())
            {
                unInstaller.UseNewContext = true;
                unInstaller.Path = filePath;
                unInstaller.Uninstall(savedState);
            }
            return true;
        }
    }
}
