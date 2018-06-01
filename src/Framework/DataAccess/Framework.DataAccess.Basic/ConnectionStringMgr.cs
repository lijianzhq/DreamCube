using System;
using System.Xml;

using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Framework.DataAccess.Basic
{
    /// <summary>
    /// 连接字符串管理对象
    /// </summary>
    public static class ConnectionStringMgr
    { 
        /// <summary>
        /// 根据连接配置的xml文件创建一个管理对象
        /// </summary>
        /// <param name="configXMLFilePath"></param>
        /// <returns></returns>
        public static ConnectionStrings CreateConnectionStrings(String configXMLFilePath)
        {
            ConnectionStrings connectionStrs = new ConnectionStrings();
            if (MyIO.IsFileExists(configXMLFilePath))
            {
                XmlDocument dbDoc = MyXml.CreateDocByFilePath(configXMLFilePath);
                XmlNodeList connectionStringNodes = dbDoc.SelectNodes("/ConnectionStrings/ConnectionString");
                for (Int32 i = 0; i < connectionStringNodes.Count; i++)
                {
                    String name = "";
                    String value = "";
                    String providerTypeStr = "";
                    MyXml.TryGetNodeAttributeValue(connectionStringNodes[i], "name", out name, true, MyXml.XmlValueType.InnerText);
                    MyXml.TryGetNodeAttributeValue(connectionStringNodes[i], "value", out value, true, MyXml.XmlValueType.InnerText);
                    MyXml.TryGetNodeAttributeValue(connectionStringNodes[i], "providerType", out providerTypeStr, true, MyXml.XmlValueType.InnerText);
                    DBProviderType providerType;
                    if (!String.IsNullOrEmpty(name) && MyEnum.TryToEnum<DBProviderType>(providerTypeStr, out providerType, true))
                        connectionStrs.Add(name, new ConnectionString(value, providerType));
                }
            }
            return connectionStrs;
        }
    }
}
