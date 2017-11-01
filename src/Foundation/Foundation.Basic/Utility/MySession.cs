using System;
using System.Web;
using System.Web.Services;

namespace DreamCube.Foundation.Basic.Utility
{
    public class MySession
    {
        /// <summary>
        /// 从Session中获取键对应的值，不存在指定的键则返回NULL
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T Get<T>(String key)
        {
            if (HttpContext.Current != null && HttpContext.Current.Session[key] != null)
                return (T)HttpContext.Current.Session[key];
            return default(T);
        }

        /// <summary>
        /// 把值放到当前http请求上下文上；操作成功返回true；操作失败返回false
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Boolean TrySet(String key, Object value)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session[key] = value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 保存值到Session变量中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Set(String key, Object value)
        {
            TrySet(key, value);
        }
    }
}
