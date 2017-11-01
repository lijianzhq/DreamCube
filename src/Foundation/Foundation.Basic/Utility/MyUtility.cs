using System;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyUtility
    {
        /// <summary>
        /// 把Html格式的文件转换成mht格式的文件
        /// </summary>
        /// <param name="srcFilePath">源文件路径</param>
        /// <param name="ddstFilePathst">目标文件路径</param>
        //public static void HtmlFileToMhtFile(String srcFilePath, String dstFilePath)
        //{
        //    CDO.Message msg = new CDO.MessageClass();
        //    CDO.Configuration c = new CDO.ConfigurationClass();
        //    msg.Configuration = c;
        //    msg.CreateMHTMLBody(srcFilePath, CDO.CdoMHTMLFlags.cdoSuppressNone, "", "");
        //    ADODB.Stream stream = msg.GetStream();
        //    stream.SaveToFile(dstFilePath, ADODB.SaveOptionsEnum.adSaveCreateOverWrite);
        //}

        /// <summary>
        /// 调用方法的时候捕获异常
        /// </summary>
        /// <param name="callback"></param>
        /// <param name="handleExceptionInTry"></param>
        public static void CatchEx(Action callback, Enums.HandleExceptionInTry handleExceptionInTry = Enums.HandleExceptionInTry.ReturnAndMakeLog)
        {
            try
            {
                callback();
            }
            catch (Exception ex)
            {
                switch (handleExceptionInTry)
                {
                    case Enums.HandleExceptionInTry.ReturnAndIgnoreLog:
                        return;
                    case Enums.HandleExceptionInTry.ReturnAndMakeLog:
                        MyLog.MakeLog(ex);
                        return;
                    case Enums.HandleExceptionInTry.ThrowException:
                        throw;
                }
            }
        }
    }
}
