using System;
using System.Configuration;
using System.Web.Configuration;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyConfig
    {
        #region "字段"

        private static Boolean? isDebug = null;

        #endregion

        #region "属性"

        /// <summary>
        /// 判断当前程序是否打开Debug状态
        /// </summary>
        /// <returns></returns>
        public static Boolean IsDebug
        {
            get
            {
                if (isDebug == null)
                {
                    CompilationSection ds = (CompilationSection)ConfigurationManager.GetSection("system.web/compilation");
                    isDebug = ds.Debug;
                }
                return isDebug.Value;
            }
        }

        #endregion

        #region "方法"

        /// <summary>
        /// 获取应用程序名（获取*.config文件的appSettings节点下面 key="AppName" 的值
        /// </summary>
        /// <returns></returns>
        public static String GetAppName()
        {
            return ConfigurationManager.AppSettings["AppName"];
        }

        /// <summary>
        /// 获取应用程序配置文件中的AppSetting节点的配置值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static String GetAppSettingValue(String key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        /// <summary>
        /// 设置应用程序配置文件中的AppSetting节点的配置值（经测试无法保存）
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetAppSettingValue(String key, String value)
        {
            if (ConfigurationManager.AppSettings[key] != null)
            {
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configuration.AppSettings.Settings[key].Value = value;
                configuration.Save(); 
            }
        }

        #endregion
    }
}
