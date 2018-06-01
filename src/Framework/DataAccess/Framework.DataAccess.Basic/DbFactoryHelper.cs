using System;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.DataAccess.Basic
{
    internal static class DBFactoryHelper
    {
        #region "字段"

        private static List<Config.DBFactoryElement> factoryList = null;

        /// <summary>
        /// 用于控制锁的
        /// </summary>
        private static Object locker = new Object();

        #endregion

        #region "属性"

        public static List<Config.DBFactoryElement> FactoryList
        {
            get
            {
                if (factoryList != null) return factoryList;
                //先获取此程序集的工厂配置信息
                lock (locker)
                {
                    if (factoryList != null) return factoryList;
                    List<Config.DBFactoryElement> tempList = new List<Config.DBFactoryElement>();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(Properties.Resources.DreamCube_Framework_DataAccess_Basic_DbFactory);
                    XmlNodeList nodeList = doc.SelectNodes("/DbFactories/DBFactory");
                    for (Int32 i = 0; i < nodeList.Count; i++)
                    {
                        Config.DBFactoryElement element = new Config.DBFactoryElement();
                        MyObject.LoadPropertyValueFromXmlNode(nodeList[i], element);
                        tempList.Add(element);
                    }
                    //最后再赋值，否则会有bug
                    factoryList = tempList;
                }
                return factoryList;
            }
        }

        #endregion

        /// <summary>
        /// 根据工厂类型名获得数据库工厂类
        /// 获取不到指定名字的工厂类型对象，则抛出ArgumentException异常
        /// </summary>
        /// <param name="providerType"></param>
        /// <returns></returns>
        public static Config.DBFactoryElement GetFactoryElementByProviderType(String providerType)
        {
            foreach (Config.DBFactoryElement factoryElement in FactoryList)
                if (factoryElement.ProviderType == providerType)
                    return factoryElement;
            throw new ArgumentException(String.Format(Properties.Resources.ExceptionNotFoundFactoryRecord, providerType));
        }

        /// <summary>
        /// 根据工厂名获得数据库工厂类
        /// 获取不到指定名字的工厂对象，则抛出ArgumentException异常
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        public static Config.DBFactoryElement GetFactoryElementByProviderName(String providerName)
        {
            foreach (Config.DBFactoryElement factoryElement in FactoryList)
                if (factoryElement.ProviderName == providerName)
                    return factoryElement;
            throw new ArgumentException(String.Format(Properties.Resources.ExceptionNotFoundFactoryRecord, providerName));
        }
    }
}
