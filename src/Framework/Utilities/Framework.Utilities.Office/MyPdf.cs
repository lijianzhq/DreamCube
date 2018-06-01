using System;
using System.IO;

using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.Office
{
    public class MyPdf
    {
        #region "静态方法"

        /// <summary>
        /// 把文件转换成swf格式
        /// </summary>
        /// <param name="filePath">pdf源文件的路径</param>
        /// <param name="outPath">
        /// swf文件的输出路径
        /// 注意：可以是输出目录路径，也可以是文件路径
        /// 当输出是一页一个文件的时候，此参数必须是目录，否则就是文件路径
        /// </param>
        /// <param name="pdf2swfAppPath">pdf2swf.exe文件的路径</param>
        /// <param name="onePageOneFile">
        /// 标志转换的时候是否一页转换成一个swf文件；默认为false
        /// 注意：如果此参数为true，则outpath参数表示目录路径，而不是文件路径
        /// 生成的文件格式为：“源文件名_页码.swf”
        /// </param>
        /// <returns></returns>
        public static void ConvertToSwf(String filePath,
                                        String outPath,
                                        String pdf2swfAppPath = "",
                                        Boolean onePageOneFile = false)
        {
            // 通过注册表获取FlashPrinter.exe注册的路径。。
            if (String.IsNullOrEmpty(pdf2swfAppPath))
                pdf2swfAppPath = MyObject.ToStringEx(MyRegistry.Basic.GetLocalMachineSubKeyPropertyValue(@"SOFTWARE\quiss.org\SWFTools\InstallPath"));
            if (String.IsNullOrEmpty(pdf2swfAppPath)) return;
            pdf2swfAppPath = Path.Combine(pdf2swfAppPath, "pdf2swf.exe");
            String param = "";
            if (onePageOneFile)
            {
                String fileName = MyString.LeftOfLast(MyString.RightOfLast(filePath, "\\"), ".");
                MyIO.EnsurePath(outPath);
                outPath = Path.Combine(outPath, fileName + "_%.swf");
                param = String.Format(" {0} -o {1}  -s drawonlyshapes -s flashversion=9", filePath, outPath);  // 合并需要的参数信息。
            }
            else
            {
                param = String.Format("{0} -o {1}  -s drawonlyshapes  -s flashversion=9", filePath, outPath);  // 合并需要的参数信息。
            }
            MyCMD.RunEXE(pdf2swfAppPath, param);
        }

        #endregion
    }
}
