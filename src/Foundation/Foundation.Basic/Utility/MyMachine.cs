using System;
using System.Management;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyMachine
    {
        /// <summary>
        /// 获取服务器CPU ID
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string CPUID
        {
            get
            {
                ManagementClass oMC = new ManagementClass("Win32_Processor");
                ManagementObjectCollection oMoc = oMC.GetInstances();
                string sCPUID = "";
                foreach (ManagementObject mo in oMoc)
                {
                    sCPUID = Convert.ToString(mo.Properties["ProcessorId"].Value);
                    if (string.IsNullOrEmpty(sCPUID) == false)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                return sCPUID;
            }
        }

        /// <summary>
        /// 获取服务器硬盘ID
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string DiskID
        {
            get
            {
                ManagementClass oMC = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection oMoc = oMC.GetInstances();
                string sDiskID = "";
                foreach (ManagementObject mo in oMoc)
                {
                    sDiskID = Convert.ToString(mo.Properties["Model"].Value);
                    if (string.IsNullOrEmpty(sDiskID) == false)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                return sDiskID;
            }
        }


        /// <summary>
        /// 获取服务器网卡Mac地址(如果有多个网卡，只获取到第一个)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string NetCardID
        {
            get
            {
                ManagementClass oMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection oMoc = oMC.GetInstances();
                string sNetCardID = "";
                foreach (ManagementObject mo in oMoc)
                {
                    sNetCardID = Convert.ToString(mo.Properties["MacAddress"].Value);
                    if (string.IsNullOrEmpty(sNetCardID) == false)
                    {
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
                return sNetCardID;
            }
        }

        /// <summary>
        /// 获取所有网卡的Mac地址(多值之间以;隔开)
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string AllNetCardIDs
        {
            get
            {
                string sReturn = "";
                ManagementClass oMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection oMoc = oMC.GetInstances();
                string sNetCardID = "";
                foreach (ManagementObject mo in oMoc)
                {
                    sNetCardID = Convert.ToString(mo.Properties["MacAddress"].Value);
                    if (string.IsNullOrEmpty(sNetCardID) == false)
                    {
                        sReturn = MyString.Connect(sReturn, sNetCardID, ";");
                    }
                }
                return sReturn;
            }
        }

        /// <summary>
        /// 功能：获取本机的IP地址，可能不止一个, 多个IP之间以分号隔开
        /// </summary>
        public static string CurrentMachineIPs
        {
            get
            {
                string sIPs = "";
                System.Net.IPHostEntry oEntry = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
                if (oEntry != null)
                {
                    for (int i = 0; i <= oEntry.AddressList.Length - 1; i++)
                    {
                        sIPs = MyString.Connect(sIPs, oEntry.AddressList.GetValue(i).ToString(), ";");
                    }
                }
                return sIPs;
            }
        }
    }
}