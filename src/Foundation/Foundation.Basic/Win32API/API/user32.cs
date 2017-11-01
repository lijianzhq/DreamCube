using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

//自定义命名空间
using DreamCube.Foundation.Basic.Win32API.Enums;

namespace DreamCube.Foundation.Basic.Win32API.API
{
    public partial class user32
    {
        /// <summary>
        ///  找出某个窗口的创建者（线程或进程），返回创建者的【线程号】
        /// </summary>
        /// <param name="hWnd">（向函数提供的）被查找窗口的句柄. </param>
        /// <returns></returns>
        public static IntPtr GetWindowThreadProcessId(IntPtr hWnd)
        {
            IntPtr ptr = new IntPtr();
            user32.GetWindowThreadProcessId(hWnd, out ptr);
            return ptr;
        }

        /// <summary>
        /// 找出某个窗口的创建者（线程或进程），返回创建者的【线程号】
        /// </summary>
        /// <param name="hWnd">（向函数提供的）被查找窗口的句柄. </param>
        /// <returns></returns>
        public static IntPtr GetWindowThreadProcessId(Int32 hWnd)
        {
            IntPtr ptr = new IntPtr();
            user32.GetWindowThreadProcessId(new IntPtr(hWnd), out ptr);
            return ptr;
        }
    }

    /// <summary>
    /// user32.dll 托管引用
    /// </summary>
    public partial class user32
    {
        /// <summary>
        /// SetWindowRgn 函数是设置了一个窗口的区域.只有被包含在这个区域内的地方才会被重绘,而不包含在区域内的其他区域系统将不会显示.
        /// 它所设置的区域是从最左上角开始的,而不是从 用户区域 开始的.(既是说从标题栏那里就已经生效了);
        /// 可以使用 GetWindowRgn 来获取一个窗口的作用区域.
        /// int SetWindowRgn(HWND hWnd, HRGN hRgn, BOOL bRedraw);
        /// </summary>
        /// <param name="hwnd">窗口的句柄</param>
        /// <param name="handle">
        /// 指向的区域.函数起作用后将把窗体变成这个区域的形状.
        /// 如果这个参数是空值,窗体区域也会被设置成空值,也就是什么也看不到.
        /// </param>
        /// <param name="b">
        /// 这个参数是用于设置 当函数起作用后,窗体是不是该重绘一次. true 则重绘,false 则相反.
        /// 如果你的窗体是可见的,通常建议设置为 true.
        /// </param>
        /// <returns>如果函数执行成功,就返回非零的数字.如果失败,就返回零.</returns>
        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern Int32 SetWindowRgn(IntPtr hwnd, Int32 handle, Boolean b);

        /// <summary>
        /// GetWindowLong是一个函数。该函数获得有关指定窗口的信息，函数也获得在额外窗口内存中指定偏移位地址的32位度整型值。
        /// </summary>
        /// <param name="hWnd">hWnd:窗口句柄及间接给出的窗口所属的窗口类。</param>
        /// <param name="nlndex">
        /// nlndex：指定要获得值的大于等于0的值的偏移量。有效值的范围从0到额外窗口内存空间的字节数一4;
        /// 例如，若指定了12位或多于12位的额外类存储空间，则应设为第三个32位整数的索引位8。要获得任意其他值，指定下列值之一：
        /// GWL_EXSTYLE(-20)；获得扩展窗口风格。
        /// GWL_STYLE(-16)：获得窗口风格。
        /// GWL_WNDPROC(-4)：获得窗口过程的地址，或代表窗口过程的地址的句柄。必须使用CallWindowProc函数调用窗口过程。
        /// GWL_HINSTANCE(-6)：获得应用事例的句柄。
        /// GWL_HWNDPARENT(-8)：如果父窗口存在，获得父窗口句柄。
        /// GWL_ID(-12):获得窗口标识。
        /// GWL_USERDATA(-21)：获得与窗口有关的32位值。每一个窗口均有一个由创建该窗口的应用程序使用的32位值。
        /// 在hWnd参数标识了一个对话框时也可用下列值：
        /// DWL_DLGPROC(4)：获得对话框过程的地址，或一个代表对话框过程的地址的句柄。必须使用函数CallWindowProc来调用对话框过程。
        /// DWL_MSGRESULT(0)：获得在对话框过程中一个消息处理的返回值。
        /// DWL_USER(8)：获得应用程序私有的额外信息，例如一个句柄或指针。[1]
        ///</param>
        /// <returns>
        /// 返回值：如果函数成功，返回值是所需的32位值；如果函数失败，返回值是0。若想获得更多错误信息请调用 GetLastError函数。
        /// 备注：通过使用函数RegisterClassEx将结构WNDCLASSEX中的cbWndExtra单元指定为一个非0值来保留额外类的存储空间。
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Int32 GetWindowLong(IntPtr hWnd, Int32 nIndex);

        /// <summary>
        /// SetWindowLong该函数改变指定窗口的属性．函数也将指定的一个32位值设置在窗口的额外存储空间的指定偏移位置。
        /// </summary>
        /// <param name="hWnd">hWnd：窗口句柄及间接给出的窗口所属的类。</param>
        /// <param name="nlndex">
        /// nlndex：指定将设定的大于等于0的偏移值。有效值的范围从0到额外类的存储空间的字节数减4：
        /// 例如若指定了12或多于12个字节的额外类存储空间，则应设索引位8来访问第三个4字节，同样设置0访问第一个4字节，
        /// 4访问第二个4字节。要设置其他任何值，可以指定下面值之一：
        /// GWL_EXSTYLE：设定一个新的扩展风格。
        /// GWL_STYLE：设定一个新的窗口风格。
        /// GWL_WNDPROC：为窗口过程设定一个新的地址。
        /// GWL_ID：设置一个新的窗口标识符。
        /// GWL_HINSTANCE：设置一个新的应用程序实例句柄。
        /// GWL_USERDATA：设置与窗口有关的32位值。每个窗口均有一个由创建该窗口的应用程序使用的32位值。
        /// 当hWnd参数标识了一个对话框时，也可使用下列值：
        /// DWL_DLGPROC：设置对话框过程的新地址。
        /// DWL_MSGRESULT：设置在对话框过程中处理的消息的返回值。
        /// DWL_USER：设置的应用程序私有的新的额外信息，例如一个句柄或指针。
        ///</param>
        /// <param name="dwNewLong">指定的替换值。</param>
        /// <returns>如果函数成功，返回值是指定的32位整数的原来的值。如果函数失败，返回值为0。若想获得更多错误信息，请调用GetLastError函数。
        /// 如果指定32位整数的原来的值为0，并且函数成功，则返回值为0，但是函数并不清除最后的错误信息，这就很难判断函数是否成功。
        /// 这时，就应在调用SetWindowLong之前调用callingSetLastError（0）函数来清除最后的错误信息。
        /// 这样，如果函数失败就会返回0，并且GetLastError。也返回一个非零值。
        /// </returns>
        [DllImport("user32.dll")]
        public static extern Int32 SetWindowLong(IntPtr hWnd, Int32 nIndex, Int32 dwNewLong);

        /// FindWindow这个函数检索处理顶级窗口的类名和窗口名称匹配指定的字符串。这个函数不搜索子窗口.
        /// lpClassName参数指向类名，lpWindowName指向窗口名，
        /// 如果有指定的类名和窗口的名字则表示成功返回一个窗口的句柄。否则返回零。
        /// </summary>
        /// <param name="lpClassName"></param>
        /// <param name="lpWindowName"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

        /// <summary>
        /// 函数功能：该函数改变某个子窗口的父窗口。
        /// </summary>
        /// <param name="hWndChild">子窗口句柄。</param>
        /// <param name="hWndNewParent">新的父窗口句柄。
        /// 如果该参数是NULL，则桌面窗口就成为新的父窗口。在WindowsNT5.0中，如果参数为HWND_MESSAGE,则子窗口成为消息窗口。</param>
        /// <returns>返回值：如果函数成功，返回值为子窗口的原父窗口句柄；如果函数失败，返回值为NULL。若想获得多错误信息，请调用GetLastError函数。</returns>
        [DllImport("user32.dll")]
        public static extern Int32 SetParent(Int32 hWndChild, Int32 hWndNewParent);

        /// <summary>
        /// 该函数设置由不同线程产生的窗口的显示状态。
        /// </summary>
        /// <param name="hWnd">窗口句柄。</param>
        /// <param name="nCmdShow">指定窗口如何显示</param>
        /// <returns>如果窗口原来可见，返回值为非零；如果窗口原来被隐藏，返回值为零。</returns>
        [DllImport("user32.dll")]
        public static extern Boolean ShowWindowAsync(IntPtr hWnd, WindowShowMode nCmdShow);

        /// <summary>
        /// 函数将创建指定的窗口，并激活到前台窗口的线程 。键盘输入窗口，并为用户更改不同的视觉线索。该系统分配一个优先略高前景的窗口，比它其他线程创建的线程。
        /// </summary>
        /// <param name="hWnd">应该被激活，并带到前景的窗口句柄。</param>
        /// <returns>
        /// 如果窗口被带到前台，返回值为非零。
        /// 如果窗口不带到前景，返回值是零。
        /// </returns>
        [DllImport("user32.dll")]
        public static extern Boolean SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern Boolean IsIconic(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern Boolean IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        /// <summary>
        ///  找出某个窗口的创建者（线程或进程），返回创建者的标志符。
        ///  注意：输出参数 ProcessId 是存放【进程号】的变量。【函数返回值】是【线程号】
        /// </summary>
        /// <param name="hWnd">（向函数提供的）被查找窗口的句柄. </param>
        /// <param name="ProcessId">是进程号存放处；如果参数不为NULL，即提供了存放处--变量，那么本函数把进程标志拷贝到存放处，否则不动作。</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out IntPtr ProcessId);

        [DllImport("user32.dll")]
        public static extern IntPtr AttachThreadInput(IntPtr idAttach, IntPtr idAttachTo, Int32 fAttach);

        [DllImport("user32.dll")]
        public static extern Int32 GetWindowText(Int32 hWnd, StringBuilder title, Int32 size);

        [DllImport("user32.dll")]
        public static extern Int32 GetWindowModuleFileName(Int32 hWnd, StringBuilder title, Int32 size);

        [DllImport("user32.dll")]
        public static extern Int32 EnumWindows(EnumWindowsProc ewp, Int32 lParam);

        [DllImport("user32.dll")]
        public static extern Boolean IsWindowVisible(Int32 hWnd);

        /// <summary>
        /// 注册快捷键
        /// 如果函数执行成功，返回值不为0。       
        //  如果函数执行失败，返回值为0。要得到扩展错误信息，调用GetLastError。  
        /// </summary>
        /// <param name="hWnd">要定义热键的窗口的句柄</param>
        /// <param name="id">定义热键ID（不能与其它ID重复）</param>
        /// <param name="fsModifiers">标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效</param>
        /// <param name="vk">定义热键的内容 </param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean RegisterHotKey(IntPtr hWnd, Int32 id, KeyModifiers fsModifiers, Keys vk);

        /// <summary>
        /// 取消注册快捷键
        /// </summary>
        /// <param name="hWnd">要取消热键的窗口的句柄</param>
        /// <param name="id">要取消热键的ID</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern Boolean UnregisterHotKey(IntPtr hWnd, Int32 id);
    }
}
