using System;
using System.Runtime.InteropServices;

namespace DreamCube.Framework.Utilities.ActiveX
{
    [ComImport,
     Guid("00000118-0000-0000-C000-000000000046"),
     InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IOleClientSite
    {
        void SaveObject();
        void GetMoniker(uint dwAssign, uint dwWhichMoniker, object ppmk);
        void GetContainer(out IOleContainer ppContainer);
        void ShowObject();
        void OnShowWindow(bool fShow);
        void RequestNewObjectLayout();
    }
}
