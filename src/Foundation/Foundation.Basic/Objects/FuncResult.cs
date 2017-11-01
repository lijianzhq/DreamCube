using System;

namespace DreamCube.Foundation.Basic.Objects
{
    /// <summary>
    /// 执行方法返回的结果
    /// </summary>
    public class FuncResult
    {
        /// <summary>
        /// 执行结果
        /// </summary>
        public Boolean Status = false;

        /// <summary>
        /// 成功的方法
        /// </summary>
        public String SuccessMsg = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        public String ErrorMsg = "";
    }

    /// <summary>
    /// 执行方法返回的结果
    /// </summary>
    public class FuncResult<T> : FuncResult
    {
        /// <summary>
        /// 方法执行的结果
        /// </summary>
        public T Result;
    }
}
