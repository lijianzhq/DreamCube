using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Management;
using System.Xml;

//自定义命名空间
using DreamCube.Foundation.Basic.Utility;
using DreamCube.Foundation.Basic.Win32API.API;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyNet
    {
        #region "公共方法"

        /// <summary>
        /// 获取某个主机的所有IPAddress
        /// </summary>
        /// <param name="hostNameOrAddress">指定需要获取IP的主机名或者地址，默认获取本机</param>
        /// <param name="addressFamily">指定获取的IP地址类型，默认获取IPV4地址族的地址</param>
        /// <param name="include127001">指定是否包含127.0.0.1的地址，默认是不包括的</param>
        /// <returns></returns>
        public static IPAddress[] GetIPAddress(AddressFamily addressFamily,
                                               String hostNameOrAddress = "",
                                               Boolean include127001 = false)
        {
            if (String.IsNullOrEmpty(hostNameOrAddress))
                hostNameOrAddress = Dns.GetHostName();
            IPHostEntry entry = Dns.GetHostEntry(hostNameOrAddress);
            //根据获取IP地址的方案帅选IP地址
            List<IPAddress> result = new List<IPAddress>();
            foreach (IPAddress item in entry.AddressList)
                if (item.AddressFamily == addressFamily)
                    result.Add(item);
            if (include127001) result.Add(IPAddress.Parse("127.0.0.1"));
            return result.ToArray();
        }

        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <param name="addressFamily"></param>
        /// <param name="hostNameOrAddress"></param>
        /// <param name="include127001"></param>
        /// <returns></returns>
        public static String[] GetIPAddressEx(AddressFamily addressFamily,
                                              String hostNameOrAddress = "",
                                              Boolean include127001 = false)
        {
            IPAddress[] address = GetIPAddress(addressFamily, hostNameOrAddress, include127001);
            List<String> addressStrList = new List<String>();
            for (Int32 i = 0; i < address.Length; i++)
                addressStrList.Add(address[i].ToString());
            return addressStrList.ToArray();
        }

        /// <summary>
        /// 获取某个主机的所有IPAddress
        /// </summary>
        /// <param name="hostNameOrAddress">指定需要获取IP的主机名或者地址，默认获取本机</param>
        /// <returns></returns>
        public static IPAddress[] GetIPAddress(String hostNameOrAddress = "")
        {
            if (String.IsNullOrEmpty(hostNameOrAddress))
                hostNameOrAddress = Dns.GetHostName();
            IPHostEntry entry = Dns.GetHostEntry(hostNameOrAddress);
            return entry.AddressList;
        }

        /// <summary>
        /// 验证是否合法的IP地址
        /// </summary>
        /// <param name="value">如果传入字符串为空或者空串，则返回false</param>
        /// <returns></returns>
        public static Boolean IsValidateIPAddress(String value)
        {
            if (String.IsNullOrEmpty(value)) return false;
            IPAddress ip = null;
            return IPAddress.TryParse(value, out ip);
        }

        /// <summary>
        /// 判断是否合法的端口号，合法端口号是从0-65535
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static Boolean IsValidatePort(Int32 port)
        {
            return port > 0 && port < 65535;
        }

        /// <summary>
        /// 获取目标主机所有的网卡的Mac地址，如果不传入目标主机的IP地址，则默认是获取本机的网卡MAC地址（所有网卡的）；
        /// 如果传入了目标主机的IP地址，则获取的是与目标主机当前通信的网卡的MAC地址（只有一个）
        /// </summary>
        /// <param name="ip">指定需要获取Mac地址的主机IP地址地址，为空时获取本机的IP地址，默认为空</param>
        /// <returns></returns>
        public static String[] GetMacAddress(String ip = "")
        {
            if (String.IsNullOrEmpty(ip))
                return GetLocalMacAddress();
            else
                return new String[] { GetRemoteServerMacAddress(ip) };
        }

        /// <summary>
        /// 对指定目标主机执行ping命令
        /// </summary>
        /// <param name="ip">指定主机的IP地址或者主机名</param>
        /// <param name="timeOut">指定超时时间</param>
        /// <returns>ping成功返回true，ping失败返回false</returns>
        public static Boolean ExecPing_Result(String ip, Int32 timeOut = 500)
        {
            Ping pingSender = new Ping();
            PingOptions pingOptions = new PingOptions();
            pingOptions.DontFragment = true;
            PingReply reply = pingSender.Send(ip, timeOut);
            if (reply.Status == IPStatus.Success)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 对指定目标主机执行ping命令
        /// </summary>
        /// <param name="ip">指定主机的IP地址或者主机名</param>
        /// <param name="timeOut">指定超时时间</param>
        /// <returns>ping操作的返回状态IPStatus对象实例</returns>
        public static IPStatus ExecPing_Status(String ip, Int32 timeOut = 500)
        {
            PingReply reply = ExecPing(ip, timeOut);
            return reply.Status;
        }

        /// <summary>
        /// 对指定目标主机执行ping命令
        /// </summary>
        /// <param name="ip">指定主机的IP地址或者主机名</param>
        /// <param name="timeOut">指定超时时间</param>
        /// <returns>ping的操作结果PingReply对象实例</returns>
        public static PingReply ExecPing(String ip, Int32 timeOut = 500)
        {
            Ping pingSender = new Ping();
            PingOptions pingOptions = new PingOptions();
            pingOptions.DontFragment = true;
            return pingSender.Send(ip, timeOut);
        }

        /// <summary>
        /// 检测端口是否可用
        /// </summary>
        /// <param name="port"></param>
        /// <param name="millisecondTimeOut">检测端口超时时间</param>
        /// <returns></returns>
        public static Boolean IsPortEnable(Int32 port, Int32 millisecondTimeOut = 3000)
        {
            String result = MyCMD.RunEXE("netstat.exe", "-an", millisecondTimeOut);
            return result.IndexOf(String.Format("127.0.0.1:{0}", port)) < 0 &&
                   result.IndexOf(String.Format("0.0.0.1:{0}", port)) < 0 &&
                   result.IndexOf(String.Format("{0}:{1}", Environment.MachineName, port)) < 0;
        }

        #endregion

        #region "私有方法"

        /// <summary>
        /// 获取本机的所有网卡的Mac地址
        /// </summary>
        /// <returns></returns>
        private static String[] GetLocalMacAddress()
        {
            List<String> macAddresses = new List<String>();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject item in moc)
            {
                if (Convert.ToBoolean(item["IPEnabled"]))
                    macAddresses.Add(item["MacAddress"].ToString().Replace(":", "-"));
            }
            return macAddresses.ToArray();
        }

        /// <summary>
        /// 获取远程主机的MAC地址
        /// </summary>
        /// <returns></returns>
        private static String GetRemoteServerMacAddress(String remoteIP)
        {
            StringBuilder strReturn = new StringBuilder();
            Int32 remote = Ws2_32.inet_addr(remoteIP);
            Int64 macinfo = new Int64();
            Int32 length = 6;
            Iphlpapi.SendARP(remote, 0, ref macinfo, ref length);
            String temp = System.Convert.ToString(macinfo, 16).PadLeft(12, '0').ToUpper();
            Int32 x = 12;
            for (Int32 i = 0; i < 6; i++)
            {
                if (i == 5) { strReturn.Append(temp.Substring(x - 2, 2)); }
                else { strReturn.Append(temp.Substring(x - 2, 2) + ":"); }
                x -= 2;
            }
            return strReturn.ToString();
        }

        #endregion
    }
}
