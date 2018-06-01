using System;

using Top.Api;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.Utilities.TBAPI
{
    public static class TBUtility
    {
        /// <summary>
        /// 获取一个淘宝调用的TopClient对象
        /// </summary>
        /// <returns></returns>
        public static DefaultTopClient GetTopClient(String appKey, String appSecret)
        {
            return new DefaultTopClient(Properties.Resources.CallUrl, appKey, appSecret);
        }

        /// <summary>
        /// 记录错误日记
        /// </summary>
        /// <param name="response"></param>
        public static void MakeLog(TopResponse response)
        {
            MyLog.MakeLog(String.Format("错误编码：{0}，错误信息：{1}；子错误编码：{2}，子错误信息：{3}", response.ErrCode, response.ErrMsg, response.SubErrCode, response.SubErrMsg));
        }
    }
}
