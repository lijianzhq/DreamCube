using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyXml
    {
        /// <summary>
        /// 值类型
        /// </summary>
        public enum XmlValueType
        {
            /// <summary>
            /// 表示获取xml节点获取标签的InnertXml属性的值
            /// </summary>
            InnerXml,

            /// <summary>
            /// 表示获取xml节点获取标签的InnerText属性的值
            /// </summary>
            InnerText
        }

        /// <summary>
        /// 根据XML文件的路径，创建xml文档对象
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <returns></returns>
        public static XmlDocument CreateDocByFilePath(String xmlFilePath) 
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);
            return doc;
        }

        /// <summary>
        /// 根据XML字符串创建xml文档
        /// </summary>
        /// <param name="xmlData"></param>
        /// <returns></returns>
        public static XmlDocument CreateDocByXML(String xmlData)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);
            return doc;
        }

        /// <summary>
        /// 本方法会判断Attribute属性是否存在，避免异常
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetNodeAttributeValue(XmlNode node, String attributeName, String attributeValue)
        {
            if (node != null)
            {
                XmlAttribute attr = node.Attributes[attributeName];
                attr.Value = attributeValue;
            }
        }

        /// <summary>
        /// 本方法会判断Attribute属性是否存在，避免异常
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xmlNodeXPath"></param>
        /// <param name="attributeName"></param>
        /// <param name="attributeValue"></param>
        public static void SetNodeAttributeValue(XmlDocument doc, String xmlNodeXPath, String attributeName, String attributeValue)
        {
            if (!String.IsNullOrEmpty(xmlNodeXPath))
            {
                XmlNode node = doc.SelectSingleNode(xmlNodeXPath);
                SetNodeAttributeValue(node, attributeName, attributeValue);
            }
        }

        /// <summary>
        /// 本方法会判断Attribute属性是否存在，避免异常
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static String GetNodeAttributeValue(XmlNode node, String attributeName)
        {
            if (node != null)
            {
                XmlAttribute attr = node.Attributes[attributeName];
                return attr != null ? attr.Value : String.Empty;
            }
            return String.Empty;
        }

        /// <summary>
        /// 本方法会判断Attribute属性是否存在，避免异常
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="xmlNodeXPath"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static String GetNodeAttributeValue(String xmlFilePath, String xmlNodeXPath, String attributeName)
        {
            if (!String.IsNullOrEmpty(xmlNodeXPath))
            {
                XmlDocument doc = MyXml.CreateDocByFilePath(xmlFilePath);
                return MyXml.GetNodeAttributeValue(doc, xmlNodeXPath, attributeName);
            }
            return String.Empty;
        }

        /// <summary>
        /// 本方法会判断Attribute属性是否存在，避免异常
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="xmlNodeXPath"></param>
        /// <param name="attributeName"></param>
        /// <returns></returns>
        public static String GetNodeAttributeValue(XmlDocument doc, String xmlNodeXPath, String attributeName)
        {
            if (!String.IsNullOrEmpty(xmlNodeXPath))
            {
                XmlNode node = doc.SelectSingleNode(xmlNodeXPath);
                if (node != null) return GetNodeAttributeValue(node, attributeName);
            }
            return String.Empty;
        }

        /// <summary>
        /// 获取xml节点的Attribute的值
        /// </summary>
        /// <param name="node"></param>
        /// <param name="attributeName">标签名称</param>
        /// <param name="attributeValue">返回的标签值</param>
        /// <param name="ignoreCase">是否忽略标签属性的大小写</param>
        /// <returns></returns>
#if NET20
        public static Boolean TryGetNodeAttributeValue(XmlNode node,
                                                       String attributeName,
                                                       out String attributeValue,
                                                       Boolean ignoreCase,
                                                       XmlValueType valueType)
#else 
        public static Boolean TryGetNodeAttributeValue(this XmlNode node, 
                                                       String attributeName, 
                                                       out String attributeValue, 
                                                       Boolean ignoreCase = false,
                                                       XmlValueType valueType = XmlValueType.InnerText)
#endif
        {
            attributeValue = String.Empty;
            if (ignoreCase)
            {
                foreach (XmlAttribute attr in node.Attributes)
                {
                    if (String.Compare(attr.Name, attributeName, true) == 0)
                    {
                        if (valueType == XmlValueType.InnerText)
                            attributeValue = attr.InnerText;
                        else if (valueType == XmlValueType.InnerXml)
                            attributeValue = attr.InnerXml;
                        else return false;
                        return true;
                    }
                }
            }
            else
            {
                XmlAttribute attr = node.Attributes[attributeName];
                if (attr != null)
                {
                    if (valueType == XmlValueType.InnerText)
                        attributeValue = attr.InnerText;
                    else if (valueType == XmlValueType.InnerXml)
                        attributeValue = attr.InnerXml;
                    else return false;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 根据Xpath获取xml节点
        /// </summary>
        /// <param name="xmlFilePath"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static XmlNodeList GetXmlNodes(String xmlFilePath, String XPath)
        {
            if (String.IsNullOrEmpty(xmlFilePath)) return null;
            if (String.IsNullOrEmpty(XPath)) return null;
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);

            return doc.SelectNodes(XPath);
        }

        /// <summary>
        /// 根据Xpath获取xml节点
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static XmlNodeList GetXmlNodes(XmlDocument doc, String XPath)
        {
            if (doc == null) return null;
            if (String.IsNullOrEmpty(XPath)) return null;

            return doc.SelectNodes(XPath);
        }

        /// <summary>
        /// 获取xml节点的innerText值，如果不纯在指定的节点，则返回空串
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static String GetNodeInnerText(XmlDocument doc, String XPath)
        {
            if (doc == null) return String.Empty;
            if (String.IsNullOrEmpty(XPath)) return String.Empty;

            XmlNode node = doc.SelectSingleNode(XPath);
            if (node != null) return node.InnerText;
            return String.Empty;
        }

        /// <summary>
        /// 获取xml节点的innerXML值，如果不纯在指定的节点，则返回空串
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static String GetNodeInnerXml(XmlDocument doc, String XPath)
        {
            if (doc == null) return String.Empty;
            if (String.IsNullOrEmpty(XPath)) return String.Empty;

            XmlNode node = doc.SelectSingleNode(XPath);
            if (node != null) return node.InnerXml;
            return String.Empty;
        }

        /// <summary>
        /// 获取xml节点的innerXML值，如果不纯在指定的节点，则返回空串
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="XPath"></param>
        /// <param name="innerText"></param>
        public static void SetNodeInnerText(XmlDocument doc, String XPath, String innerText)
        {
            if (doc != null && !String.IsNullOrEmpty(XPath))
            {
                XmlNode node = doc.SelectSingleNode(XPath);
                node.InnerText = innerText;
            }
        }

        /// <summary>
        /// 获取xml节点的innerXML值，如果不纯在指定的节点，则返回空串
        /// </summary>
        /// <param name="el"></param>
        /// <param name="XPath"></param>
        /// <param name="innerText"></param>
        public static void SetNodeInnerText(XmlElement el, String XPath,String innerText)
        {
            if (el != null && !String.IsNullOrEmpty(XPath))
            {
                XmlNode node = el.SelectSingleNode(XPath);
                node.InnerText = innerText;
            }
        }

        /// <summary>
        /// 获取xml节点的innerXML值，如果不纯在指定的节点，则返回空串
        /// </summary>
        /// <param name="el"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static String GetNodeInnerXml(XmlElement el, String XPath)
        {
            if (el == null) return String.Empty;
            if (String.IsNullOrEmpty(XPath)) return String.Empty;

            XmlNode node = el.SelectSingleNode(XPath);
            if (node != null) return node.InnerXml;
            return String.Empty;
        }

        /// <summary>
        /// 获取xml节点的innerXML值，如果不存在指定的节点，则返回空串
        /// </summary>
        /// <param name="el"></param>
        /// <param name="XPath"></param>
        /// <returns></returns>
        public static String GetNodeInnerText(XmlElement el, String XPath)
        {
            if (el == null) return String.Empty;
            if (String.IsNullOrEmpty(XPath)) return String.Empty;

            XmlNode node = el.SelectSingleNode(XPath);
            if (node != null) return node.InnerText;
            return String.Empty;
        }

        /// <summary>
        /// 序列化成xml字符串
        /// </summary>
        /// <param name="targetObj"></param>
        /// <returns></returns>
#if NET20
        public static String SerializeToXml<T>(T targetObj)
#else 
        public static String SerializeToXml<T>(this T targetObj)
#endif
        {
            StringBuilder buffer = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextWriter writer = new StringWriter(buffer))
            {
                serializer.Serialize(writer, targetObj);
            }
            return buffer.ToString();
        }

        /// <summary>
        /// 从xml反序列话成对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
#if NET20
        public static T DeSerializeFromXml<T>(String value)
#else
        public static T DeSerializeFromXml<T>(this String value)
#endif
        {
            T cloneObject = default(T);
            StringBuilder buffer = new StringBuilder();
            buffer.Append(value);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (TextReader reader = new StringReader(buffer.ToString()))
            {
                Object obj = serializer.Deserialize(reader);
                cloneObject = (T)obj;
            }
            return cloneObject;
        }
    }
}
