using System;
using System.Runtime.InteropServices;

namespace DreamCube.Foundation.Basic.Win32API.API
{
    public static class ADVAPI32
    {
        [DllImport("ADVAPI32.DLL", SetLastError = true)]
        public static extern int OpenThreadToken(IntPtr thread, int access, bool openAsSelf, ref IntPtr hToken);

        [DllImport("ADVAPI32.DLL", SetLastError = true)]
        public static extern int RevertToSelf();

        [DllImport("ADVAPI32.DLL", SetLastError = true)]
        public static extern bool ImpersonateSelf(int level);
    }
}
