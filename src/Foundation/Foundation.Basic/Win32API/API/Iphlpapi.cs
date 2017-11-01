using System;
using System.Runtime.InteropServices;

namespace DreamCube.Foundation.Basic.Win32API.API
{
    /// <summary>
    /// Iphlpapi.dll 托管引用
    /// </summary>
    public class Iphlpapi
    {
        [DllImport("Iphlpapi.dll")]
        public static extern Int32 SendARP(Int32 DestIP, Int32 SrcIP, ref Int64 MacAddr, ref Int32 PhyAddrLen); 
    }
}
