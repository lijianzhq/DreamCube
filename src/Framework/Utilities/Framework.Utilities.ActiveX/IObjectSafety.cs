using System;
using System.Runtime.InteropServices;

namespace DreamCube.Framework.Utilities.ActiveX
{
    [ComImport, GuidAttribute("CB5BDC81-93C1-11CF-8F20-00805F2CD064")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IObjectSafety
    {
        [PreserveSig]
        Int32 GetInterfaceSafetyOptions(ref Guid riid,
                      [MarshalAs(UnmanagedType.U4)] ref Int32 pdwSupportedOptions,
                      [MarshalAs(UnmanagedType.U4)] ref int pdwEnabledOptions);

        [PreserveSig()]
        Int32 SetInterfaceSafetyOptions(ref Guid riid,
                      [MarshalAs(UnmanagedType.U4)] Int32 dwOptionSetMask,
                      [MarshalAs(UnmanagedType.U4)] Int32 dwEnabledOptions);
    }
}
