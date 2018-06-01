using System;
using System.Text;
using System.Collections.Generic;

using DreamCube.Foundation.Basic.Utility;
using DreamCube.Foundation.Serialization;

namespace DreamCube.Framework.Utilities.BasicObject
{
    /// <summary>
    /// ObjectX的辅助类
    /// </summary>
    public static class ObjectXHelper
    {
        /// <summary>
        /// 把对象的json格式数据保存到注册表中
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="subKey">注册表项，例如：SOFTWARE\WinRAR</param>
        /// <param name="propertyName">对应注册表项中的属性名</param>
        public static void SaveJSONToRegistry_LocalMachine(ObjectX obj, String subKey, String propertyName)
        {
            String json = MyJson.Serialize(obj);
            MyRegistry.Basic.SetLocalMachineKeyPropertyValue(subKey, propertyName, json, true);
        }

        /// <summary>
        /// 保存对象的json格式到一个文件中
        /// </summary>
        /// <param name="obj">指定的对象</param>
        /// <param name="filePath">指定的文件路径</param>
        /// <param name="append">是否是追加写入</param>
        public static void SaveJSONToFile(ObjectX obj, String filePath, Boolean append = false)
        {
            String json = MyJson.Serialize(obj);
            MyIO.Write(filePath, json, append, Encoding.UTF8);
        }

        /// <summary>
        /// 根据json字符串创建一个对象
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static ObjectX CreateObjFromJSON(String json)
        {
            try
            {
                return MyJson.Deserialize<ObjectX>(json);
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            return null;
        }

        /// <summary>
        /// 根据json字符串创建一个对象
        /// </summary>
        /// <param name="subKey">注册表项，例如：SOFTWARE\WinRAR</param>
        /// <param name="propertyName">对应注册表项中的属性名</param>
        /// <returns></returns>
        public static ObjectX CreateObjFromJSONRegistry_LocalMachine(String subKey, String propertyName)
        {
            try
            {
                String json = MyObject.ToStringEx(MyRegistry.Basic.GetLocalMachineSubKeyPropertyValue(subKey, propertyName));
                return MyJson.Deserialize<ObjectX>(json);
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            return null;
        }

        /// <summary>
        /// 从指定的注册表中的指定键值(json字符串)创建指定的对象实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subKey">注册表中的键</param>
        /// <param name="propertyName">属性名</param>
        /// <returns></returns>
        public static T CreateObjFromJSONRegistry_LocalMachine<T>(String subKey, String propertyName)
            where T : ObjectX
        {
            try
            {
                String json = MyObject.ToStringEx(MyRegistry.Basic.GetLocalMachineSubKeyPropertyValue(subKey, propertyName));
                return MyJson.Deserialize<T>(json);
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            return null;
        }

        /// <summary>
        /// 根据对象json数据反序列化为源对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T CreateObjFromJSON<T>(String json)
            where T : ObjectX
        {
            return MyJson.Deserialize<T>(json);
        }

        /// <summary>
        /// 从文本文件中获取json数据创建一个对象
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static ObjectX CreateObjFromJSONFile(String filePath)
        {
            try
            {
                String json = MyIO.ReadText(filePath, Encoding.UTF8);
                if (!String.IsNullOrEmpty(json))
                    return MyJson.Deserialize<ObjectX>(json);
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            return null;
        }

        /// <summary>
        /// 创建一个空的ObjectXduixiang 
        /// </summary>
        /// <returns></returns>
        public static ObjectX CreateEmptyObj()
        {
            return new ObjectX();
        }
    }
}
