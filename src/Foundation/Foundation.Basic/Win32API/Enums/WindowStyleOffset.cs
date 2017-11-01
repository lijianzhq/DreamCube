using System;

namespace DreamCube.Foundation.Basic.Win32API.Enums
{
    /// <summary>
    /// 用于GetWindowLong()
    /// </summary>
    public enum WindowStyleOffset
    {
        GWL_EXSTYLE = -20,
        GWL_STYLE = -16,
        GWL_WNDPROC = -4,
        GWL_HINSTANCE = -6,
        GWL_HWNDPARENT = -8,
        GWL_ID = -12,
        GWL_USERDATA = -21,

        /// <summary>
        /// 当窗体为对话框时，可以使用
        /// </summary>
        DWL_DLGPROC = 4,
        /// <summary>
        /// 当窗体为对话框时，可以使用
        /// </summary>
        DWL_MSGRESULT = 0,
        /// <summary>
        /// 当窗体为对话框时，可以使用
        /// </summary>
        DWL_USER = 8
    }
}
