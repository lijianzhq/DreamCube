using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Linq;

using Mini.Foundation.Basic.Utility;
using Mini.Foundation.LogService;

namespace Mini.Framework.ResourceCommon
{
    /// <summary>
    /// 字符串资源管理类，只支持xml文件
    /// 目录下面的第一层子目录，代表了语言，例如：en，cn，jp（其实是自己定义）
    /// 注意：
    /// 同一个语言目录下面的不同文件，如果出现一样的key值，请使用命名空间的方式进行读取，否则读取到的值可能不是您想要的值；
    /// 命名空间：【也就是从语言目录下来的完整文件相对路径，录入cn目录下有一个error.xml文件，则命名空间为：\error.xml，区分大小写，包含文件扩展名；cn目录有一个子目录error\error.xml，则命名空间为\error\error.xml】
    /// 当然，如果同一个文件出现同样的key值，则。。。。game over
    /// </summary>
    public class StrResourceManager : IStrResourceManager
    {
        /// <summary>
        /// key1：language name，也就是首层子目录名；key2：命名空间（也就是从语言目录下来的完整文件相对路径）；key3：对象xml文件里面的key
        /// </summary>
        private Dictionary<String, Dictionary<String, Dictionary<String, String>>> _strItems =
                    new Dictionary<String, Dictionary<String, Dictionary<String, String>>>();
        private String currentLanguage = String.Empty;
        private String _resourcePath = String.Empty;
        private FileSystemWatcher _watcher = null;

        #region "静态"

        private static StrResourceManager _current;
        private static Object _locker = new Object();

        /// <summary>
        /// 当前实例（单例模式）
        /// </summary>
        public static IStrResourceManager Current
        {
            get
            {
                if (_current == null)
                {
                    lock (_locker)
                    {
                        if (_current == null)
                            _current = new StrResourceManager(AsmConfigerHelper.GetConfiger().ConfigFileReader.AppSettings("ResourceVirtualPath") ?? "Resource");
                    }
                }
                return _current;
            }
        }

        #endregion

        #region "构造函数"

        public StrResourceManager(String path)
        {
            MyArgumentsHelper.ThrowsIfNullOrEmpty(path, nameof(path));
            if (!Path.IsPathRooted(path))
            {
                path = path.Replace("/", "\\");
                if (path.StartsWith("\\")) path = path.Substring(1);
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
            }
            this._resourcePath = path;
            this.Init();
            this.StartWatch();
        }

        #endregion

        #region "protected method"

        protected virtual void StartWatch()
        {
            try
            {
                _watcher = new FileSystemWatcher();
                _watcher.Path = this._resourcePath;
                _watcher.Changed += new FileSystemEventHandler(watcher_Changed);
                _watcher.IncludeSubdirectories = true;
                _watcher.EnableRaisingEvents = true;
                _watcher.Filter = "*.xml";
            }
            catch (Exception ex)
            {
                Log.Root.LogError($"Watch path[{this._resourcePath}] error:", ex);
            }
        }

        /// <summary>
        /// 文件发生变化的时候，重新加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            //重新执行初始化
            this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init()
        {
            try
            {
                //初始化语言
                String[] dirs = Directory.GetDirectories(this._resourcePath, "*", SearchOption.TopDirectoryOnly);
                foreach (String item in dirs)
                {
                    _strItems[MyString.LastRightOf(item.Replace("\\", "/"), "/")] = new Dictionary<String, Dictionary<String, String>>();
                }
                //初始化所有的字符串
                foreach (String dir in dirs)
                {
                    String[] files = Directory.GetFiles(dir, "*.xml", SearchOption.AllDirectories);
                    String language = MyString.LastRightOf(dir.Replace("\\", "/"), "/");
                    foreach (String file in files)
                    {
                        var fileDic = new Dictionary<String, String>();
                        XmlDocument doc = new XmlDocument();
                        doc.Load(file);
                        XmlNodeList nodes = doc.SelectNodes("/items/add");
                        if (nodes != null)
                        {
                            foreach (XmlNode n in nodes)
                            {
                                XmlAttribute attr = n.Attributes["value"];
                                if (attr == null)
                                {
                                    fileDic[n.Attributes["key"].Value] = n.InnerText;
                                }
                                else
                                {
                                    fileDic[n.Attributes["key"].Value] = attr.Value;
                                }
                            }
                        }
                        doc = null;
                        var key = MyString.RightOf(file, dir);
                        _strItems[language][key] = fileDic;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Root.LogError($"init StrResourceManager from path[{_resourcePath}] error:", ex);
            }
        }

        #endregion

        #region "public method"


        /// <summary>
        /// 指定从某个语言目录下面读取字符串
        /// 注意：
        /// 同一个语言目录下面的不同文件，如果出现一样的key值，请使用命名空间的方式进行读取，否则读取到的值可能不是您想要的值；
        /// 命名空间：【也就是从语言目录下来的完整文件相对路径，录入cn目录下有一个error.xml文件，则命名空间为：\error.xml，区分大小写，包含文件扩展名；cn目录有一个子目录error\error.xml，则命名空间为\error\error.xml】
        /// 当然，如果同一个文件出现同样的key值，则。。。。game over
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ns">命名空间【也就是从语言目录下来的完整文件相对路径，录入cn目录下有一个error.xml文件，则命名空间为：\error.xml，区分大小写，包含文件扩展名；cn目录有一个子目录error\error.xml，则命名空间为\error\error.xml】</param>
        /// <param name="language">这个就是语言目录（其实就是资源目录下面的第一层子目录名），所以这里可以固定读取一些公共的配置之类的东西</param>
        /// <returns></returns>
        public String GetString(String key, String ns = "", String language = "")
        {
            if (String.IsNullOrWhiteSpace(language))
                language = AsmConfigerHelper.GetConfiger().ConfigFileReader.AppSettings("DefaultLanguage");
            MyArgumentsHelper.ThrowsIfNullOrEmpty(language, nameof(language));
            String val = String.Empty;
            try
            {
                Dictionary<String, Dictionary<String, String>> lItems = null;
                if (_strItems.TryGetValue(language, out lItems))
                {
                    if (!String.IsNullOrEmpty(ns))
                    {
                        if (lItems.ContainsKey(ns))
                            lItems[ns].TryGetValue(key, out val);
                    }
                    else
                    {
                        foreach (var keyPair in lItems)
                        {
                            if (keyPair.Value.TryGetValue(key, out val))
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Root.LogError($"StrResourceManager GetString[language:{language},key:{key}] exception:", ex);
            }
            return val;
        }

        #endregion
    }
}
