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
    public class StrResourceManager : IStrResourceManager
    {
        private static Dictionary<String, Dictionary<String, String>> strItems = new Dictionary<string, Dictionary<string, string>>();
        private static StrResourceManager _current;
        private static String currentLanguage = String.Empty;
        private static String resourcePath = String.Empty;
        private const String currentLanguageStr = "cn";

        public static String ResourcePath
        {
            get
            {
                if (String.IsNullOrEmpty(StrResourceManager.resourcePath))
                {
                    String resourceVirtualPath = AsmConfigerHelper.AsmConfiger.ConfigFileReader.AppSettings("ResourceVirtualPath");
                    StrResourceManager.resourcePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, resourceVirtualPath);
                }
                return StrResourceManager.resourcePath;
            }
            set
            {
                StrResourceManager.resourcePath = value;
            }
        }

        public static String CurrentLanguage
        {
            get
            {
                if (String.IsNullOrEmpty(StrResourceManager.currentLanguage))
                {
                    StrResourceManager.currentLanguage = currentLanguageStr;
                }
                return StrResourceManager.currentLanguage;
            }
            set
            {
                StrResourceManager.currentLanguage = value;
            }
        }

        public static StrResourceManager Current
        {
            get
            {
                if (_current == null)
                    _current = new StrResourceManager();
                return _current;
            }
        }

        private static FileSystemWatcher _watcher = null;
        static StrResourceManager()
        {
            try
            {

                _watcher = new FileSystemWatcher();
                _watcher.Path = StrResourceManager.ResourcePath;
                _watcher.Changed += new FileSystemEventHandler(watcher_Changed);
                _watcher.IncludeSubdirectories = true;
                _watcher.EnableRaisingEvents = true;
                _watcher.Filter = "*.xml";
            }
            catch (Exception ex)
            {
                Log.Root.LogError("StrResourceManager static create exception:", ex);
            }
        }

        static void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            //重新执行初始化
            Current.Init();
        }

        private StrResourceManager()
        {
            this.Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void Init()
        {
            try
            {
                String path = StrResourceManager.ResourcePath;
                //初始化语言
                String[] dirs = Directory.GetDirectories(path, "*", SearchOption.TopDirectoryOnly);
                foreach (String item in dirs)
                {
                    strItems[MyString.LastRightOf(item.Replace("\\", "/"), "/")] = new Dictionary<string, string>();
                }
                //初始化所有的字符串
                foreach (String dir in dirs)
                {
                    String[] files = Directory.GetFiles(dir, "*.xml", SearchOption.AllDirectories);
                    String language = MyString.LastRightOf(dir.Replace("\\", "/"), "/");
                    Dictionary<String, String> languageDic = strItems[language];
                    foreach (String file in files)
                    {
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
                                    languageDic[n.Attributes["key"].Value] = n.InnerText;
                                }
                                else
                                {
                                    languageDic[n.Attributes["key"].Value] = attr.Value;
                                }
                            }
                        }
                        doc = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Root.LogError("StrResourceManager static create exception:", ex);
            }
        }

        public String GetString(String key, String language)
        {
            String val = String.Empty;
            try
            {
                Dictionary<String, String> lItems = null;
                if (strItems.TryGetValue(language, out lItems))
                {
                    lItems.TryGetValue(key, out val);
                }
            }
            catch (Exception ex)
            {
                Log.Root.LogError("StrResourceManager GetString exception:", ex);
            }
            return val;
        }

        public string GetString(String key)
        {
            return this.GetString(key, currentLanguageStr);
        }
    }
}
