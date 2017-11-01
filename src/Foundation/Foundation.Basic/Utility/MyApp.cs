using System;
using System.Threading;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyApp
    {
        #region "字段"

        private static Mutex mutex = null;

        #endregion

        #region "公共方法"

        /// <summary>
        /// 采用互斥体变量来限制应用程序或者服务只能单开
        /// </summary>
        /// <param name="appKey"></param>
        /// <returns></returns>
        public static Boolean IsExistApp(String appKey)
        {
            appKey = appKey.Replace("\\", "").ToLower() ;
            try
            {
                mutex = Mutex.OpenExisting(appKey);
                return true;
            }
            catch (Exception ex)
            {
                //出现异常表示无法找到对应key值的互斥体，表示应用程序还没有打开
            }
            mutex = new Mutex(true, appKey);
            return false;
        }

        #endregion
    }
}
