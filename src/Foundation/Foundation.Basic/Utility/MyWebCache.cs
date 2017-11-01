using System;
using System.Web;

namespace DreamCube.Foundation.Basic.Utility
{
    /// <summary>
    /// 网站缓存器
    /// </summary>
    public static class MyWebCache
    {
        /// <summary>
        /// 添加项目(如果存在,则覆盖)
        /// </summary>
        /// <param name="sItemName"></param>
        /// <param name="vItemValue"></param>
        /// <remarks></remarks>
        public static void AddSessionItem(string sItemName, object vItemValue)
        {
            if (HttpContext.Current.Items.Contains(sItemName))
            {
                HttpContext.Current.Items.Remove(sItemName);
            }
            HttpContext.Current.Items.Add(sItemName, vItemValue);
        }

        /// <summary>
        /// 获取项目
        /// </summary>
        /// <param name="sItemName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static T GetSessionItem<T>(string sItemName)
        {
            return (T)HttpContext.Current.Items[sItemName];
        }

        /// <summary>
        /// 获取项目
        /// </summary>
        /// <param name="sItemName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static object GetSessionItem(string sItemName)
        {
            return HttpContext.Current.Items[sItemName];
        }

        /// <summary>
        /// 删除项目
        /// </summary>
        /// <param name="sItemName"></param>
        /// <remarks></remarks>
        public static void RemoveSessionItem(string sItemName)
        {
            HttpContext.Current.Items.Remove(sItemName);
        }
    }
}
