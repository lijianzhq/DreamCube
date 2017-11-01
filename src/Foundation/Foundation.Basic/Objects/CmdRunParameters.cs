using System;
using System.Collections.Generic;
using System.Text;

namespace DreamCube.Foundation.Basic.Objects
{
    /// <summary>
    /// 执行CMD命令需要的参数
    /// </summary>
    public class CmdRunParameters
    {
        /// <summary>
        /// EXE文件名
        /// </summary>
        public String FileName;
        /// <summary>
        /// 启动参数
        /// </summary>
        public String Arguments;
        /// <summary>
        /// 超时时间
        /// </summary>
        public Int32? MillisecondTimeOut = 3000;
        /// <summary>
        /// 是否等待退出
        /// </summary>
        public Boolean WaitForExit;
    }
}
