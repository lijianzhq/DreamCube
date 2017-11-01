using System;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Text;
using System.Drawing;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.DirectoryServices;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyWebsite
    {
        #region "字段"

        /// <summary>
        /// 当前网站的url
        /// </summary>
        private static String serverAddr = null;

        /// <summary>
        /// 标志当前的网站是否是访问本地的网站
        /// </summary>
        private static Boolean? isLocal = null;

        #endregion

        #region "属性"

        /// <summary>
        /// Web目录的绝对路径
        /// 注意: SystemPhysicalPath与WebFolderPhysicalPath是不一定相同的, 在IIS环境中两者相同
        ///       在ASP.NET调试环境中, 两者不相等
        /// </summary>
        /// <value></value>
        /// <returns>如: E:\Web\ </returns>
        /// <remarks></remarks>
        public static string WebFolderPhysicalPath
        {
            get
            {
                return HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"];
            }
        }

        /// <summary>
        /// 返回Web应用程序的虚拟目录根路径
        /// </summary>
        public static String WebFolderPath
        {
            get
            {
                return HttpContext.Current.Request.ApplicationPath;
            }
        }

        /// <summary>
        /// 网站是否在Debug状态下（web.config里面配置的）
        /// </summary>
        public static Boolean IsDebug
        {
            get
            {
                CompilationSection ds = (CompilationSection)ConfigurationManager.GetSection("system.web/compilation");
                return ds.Debug;
            }
        }

        #endregion

        #region "公共方法"

        /// <summary>
        /// 获取当前网站的序号值
        /// </summary>
        /// <returns></returns>
        public static String GetCurrentWebsiteIndex()
        {
            return HttpContext.Current.Request.ServerVariables["INSTANCE_ID"];
        }

        /// <summary>
        /// 获取当前网站的虚拟目录映射的物理路径
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public static String GetCurWebsiteVirtualDirectory(String virtualPath)
        {
            if (HttpContext.Current != null)
                return GetVirtualDirectory(HttpContext.Current.Request.Url.Port.ToString(), virtualPath);
            return String.Empty;
        }

        /// <summary>
        /// 获取网站虚拟目录的物理路径（根据当前网站的序号来获取的）
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <param name="websiteIndex"></param>
        /// <param name="serverIP"></param>
        /// <returns></returns>
        public static String GetCurWebsiteVirtualDirectoryEx(String virtualPath, String websiteIndex = "", String serverIP = "localhost")
        {
            if (String.IsNullOrEmpty(websiteIndex))
                websiteIndex = GetCurrentWebsiteIndex();
            DirectoryEntry de = new DirectoryEntry("IIS://LOCALHOST/W3SVC/" + websiteIndex + "/ROOT/" + virtualPath);
            String path = (String)de.Properties["Path"].Value;
            return path;
        }

        /// <summary>
        /// 获取网站虚拟目录的物理路径
        /// </summary>
        /// <param name="portNumber">网站端口号</param>
        /// <param name="name">虚拟目录名称</param>
        /// <returns></returns>
        public static String GetVirtualDirectory(String portNumber, String virtualPath, String serverIP = "localhost")
        {
            // 获取网站的标识符，默认为1
            String identifier = null;
            DirectoryEntry root = new DirectoryEntry("IIS://" + serverIP + "/W3SVC");
            foreach (DirectoryEntry e in root.Children)
            {
                if (e.SchemaClassName == "IIsWebServer")
                {
                    foreach (Object property in e.Properties["ServerBindings"])
                    {
                        if (property.Equals(":" + portNumber + ":"))
                        {
                            identifier = e.Name;
                            break;
                        }
                    }
                    if (identifier != null) break;
                }
            }
            if (identifier == null) identifier = "1";
            //注意: "IIS://" & sServerIP & "/W3SVC/" & iWebSiteIndex & "/ROOT" 中的iWebSiteIndex并不是序号值, 第三个网站的Index并不是等于3
            DirectoryEntry de = new DirectoryEntry("IIS://LOCALHOST/W3SVC/" + identifier + "/ROOT/" + virtualPath);
            String path = (String)de.Properties["Path"].Value;
            return path;
        }

        /// <summary>
        /// 获取本机网站的本地访问路径；
        /// 包括：http(s)://localhost/，http(s)://127.0.0.1/，http(s)://本地的ip
        /// </summary>
        /// <returns></returns>
        public static List<String> GetLocalhostWebServerBasicUrl()
        {
            List<String> localWebUrl = new List<String>();
            localWebUrl.Add("http://localhost");
            localWebUrl.Add("https://localhost");
            localWebUrl.Add("http://127.0.0.1");
            localWebUrl.Add("https://127.0.0.1");
            //获取本机所有的IP地址
            String[] ips = MyNet.GetIPAddressEx(System.Net.Sockets.AddressFamily.InterNetwork, "", true);
            for (Int32 i = 0; i < ips.Length; i++)
            {
                localWebUrl.Add(String.Format("https://{0}", ips[i]));
                localWebUrl.Add(String.Format("http://{0}", ips[i]));
            }
            return localWebUrl;
        }

        /// <summary>
        /// 判断是否是本地的网站
        /// </summary>
        /// <returns></returns>
        public static Boolean IsLocalhostWeb()
        {
            if (isLocal != null) return isLocal.Value;
            String webBasic = GetServerBasicAddr();
            isLocal = MyString.StartsWith(webBasic, GetLocalhostWebServerBasicUrl());
            return isLocal.Value;
        }

        /// <summary>
        /// 加密queryString字符串
        /// </summary>
        /// <param name="qs"></param>
        /// <param name="encryptKey">加密的key串，可以使用系统默认的</param>
        /// <returns></returns>
        public static String DecryptQueryString(String qs, String encryptKey = "")
        {
            if (String.IsNullOrEmpty(qs)) return "";
            return HttpUtility.UrlEncode(MySecurity.DESEncryptByKey(qs, encryptKey));
        }

        /// <summary>
        /// 获取QueryString值, 考虑了QueryString被加密的情况
        /// 如果QueryString中同一个变量重复了多次,则GetQueryString(keyName)方式返回来的是以逗号隔开的多个值
        /// </summary>
        /// <param name="keyName">键</param>
        /// <param name="needDecrypt">
        /// 是否需要解密;
        /// true：需要进行解密，false：不需要解密
        /// 注意加密串必须是使用MyWebsite.DecryptQueryString方法进行加密的
        /// </param>
        /// <param name="decryptKey">解密的key值</param>
        /// <returns></returns>
        public static String GetQueryString(String keyName, Boolean needDecrypt = false, String decryptKey = "")
        {
            String qs = HttpUtility.UrlDecode(MyString.RightOfLast(HttpContext.Current.Request.RawUrl, "?"));
            if (!String.IsNullOrEmpty(qs))
            {
                if (needDecrypt) qs = MySecurity.DESDecrypt(decryptKey);
                NameValueCollection qsCollection = HttpUtility.ParseQueryString(qs);
                //说明: 如果QueryString中同一个变量重复了多次,则oCollection(sItemName)方式返回来的是以逗号隔开的多个值
                return qsCollection[keyName];
            }
            return String.Empty;
        }

        /// <summary>
        /// 如果使用了安全的加密连接,则返回https,否则返回http
        /// </summary>
        /// <returns></returns>
        public static String GetCurrentRequestHttpType()
        {
            return HttpContext.Current.Request.IsSecureConnection ? "https" : "http";
        }

        /// <summary>
        /// 获取网站的物理路径
        /// </summary>
        /// <returns></returns>
        public static String GetServerPath()
        {
            return HttpContext.Current.Server.MapPath("~");
        }

        /// <summary>
        /// 获取服务器基础地址，以斜杠“/”结束（不包括网站的虚拟路径，要包括虚拟路径的话，需要调用GetServerAddr())
        /// </summary>
        /// <returns></returns>
        public static String GetServerBasicAddr()
        {
            return String.Format("{0}://{1}/", GetCurrentRequestHttpType(), HttpContext.Current.Request.ServerVariables["HTTP_HOST"]);
        }

        /// <summary>
        /// 获取服务器地址，以斜杠“/”结束（包括虚拟目录的）
        /// </summary>
        /// <returns></returns>
        public static String GetServerAddr()
        {
            //如果是本地访问路径的话，会有错的，是不允许缓存的。因为本地可能通过localhost访问，或者通过本地的IP访问
            if (serverAddr == null)
            {
                String webPathName = HttpContext.Current.Request.ApplicationPath;
                String serverBasicAddr = GetServerBasicAddr();
                if (webPathName == "/")
                    serverAddr = serverBasicAddr;
                else
                    serverAddr = serverBasicAddr + MyString.Right(webPathName, "/") + "/";
            }
            return serverAddr;
        }

        /// <summary>
        /// 根据url获取相对web目录的相对路径（去掉协议，主机，端口，虚拟目录部分）
        /// 例如：传入：http(s)://localhost:8225/index.aspx； 得到：index.aspx
        /// (返回的相对路径前面不包含斜杠符号）
        /// </summary>
        /// <param name="url">需要计算相对web目录的相对路径的url</param>
        /// <param name="basicVirtualPath">指定需要去掉的虚拟目录名；如果不指定，则默认会获取HttpContext.Current.Request.ApplicationPath</param>
        /// <returns></returns>
        public static String GetRelativePath(String url, String basicVirtualPath = null)
        {
            //计算到下一个斜杠符号“/”的位置
            Int32 index = -1;
            if (url.StartsWith("http:")) index = url.IndexOf('/', "http://".Length);
            else if (url.StartsWith("https:")) index = url.IndexOf('/', "https://".Length);
            if (index < 0)
            {
                return url;
            }
            else
            {
                if(String.IsNullOrEmpty(basicVirtualPath))
                    basicVirtualPath = HttpContext.Current.Request.ApplicationPath;
                if (basicVirtualPath != "/") index = index + basicVirtualPath.Length;
                return url.Substring(index + 1);
            }
        }

        /// <summary>
        /// 根据相对路径或者根据Url地址获取对应的绝对路径
        /// </summary>
        /// <param name="sRelativePathOrUrl">相对路径（相对web目录的路径或者是相对当前请求页面的路径）或者url</param>
        /// <returns></returns>
        public static String MapPath(String sRelativePathOrUrl)
        {
            if (sRelativePathOrUrl.IndexOf("://") > 0)
            {
                //对于本地的路径，需要特别的处理的
                if (MyString.StartsWith(sRelativePathOrUrl, GetLocalhostWebServerBasicUrl()))
                {
                    sRelativePathOrUrl = GetRelativePath(sRelativePathOrUrl);
                    return HttpContext.Current.Request.MapPath("~/" + sRelativePathOrUrl);
                }
                else
                {
                    String sServerAddr = GetServerAddr();
                    if (MyString.StartsWith(sServerAddr, GetLocalhostWebServerBasicUrl()))
                    {
                        sRelativePathOrUrl = GetRelativePath(sRelativePathOrUrl);
                        return HttpContext.Current.Request.MapPath("~/" + sRelativePathOrUrl);
                    }
                    else
                    {
                        return HttpContext.Current.Request.MapPath("~/" + MyString.Right(sRelativePathOrUrl, sServerAddr, true, ""));
                    }
                }
            }
            else
            {
                return HttpContext.Current.Server.MapPath(sRelativePathOrUrl);
            }
        }

        /// <summary>
        /// 根据相对路径或者根据Url地址获取对应的绝对路径
        /// </summary>
        /// <param name="sRelativePathOrUrl">相对路径（相对web目录的路径或者是相对当前请求页面的路径）或者url</param>
        /// <param name="webBasicUrl">可以传入web的根url，如果不传入，则获取当前的网站目录进行计算</param>
        /// <param name="webBasicPath">可以传入web的根目录，如果不传入，则获取当前的网站目录进行计算</param>
        /// <param name="webBasicVirtualPath">可以传入web的虚拟目录，默认为斜杠符号</param>
        /// <returns></returns>
        public static String MapPath(String sRelativePathOrUrl, String webBasicUrl, String webBasicPath, String webBasicVirtualPath = "/")
        {
            if (String.IsNullOrEmpty(webBasicUrl) || String.IsNullOrEmpty(webBasicPath))
                return MapPath(sRelativePathOrUrl);
            webBasicPath = webBasicPath.Replace("/", "\\");
            sRelativePathOrUrl = sRelativePathOrUrl.Replace("\\", "/");

            //格式化传入的字符串
            if (!webBasicPath.EndsWith("\\")) webBasicPath += "\\";
            if (!webBasicUrl.EndsWith("/")) webBasicUrl += "/";
            if (sRelativePathOrUrl.IndexOf("://") > 0)
            {
                List<String> localWebUrl = GetLocalhostWebServerBasicUrl();
                //如果参考的web根url属于本地的url地址，则可以直接计算本地的目录即可。否则就需要按照字符串来助理
                if (MyString.StartsWith(webBasicUrl, localWebUrl) && MyString.StartsWith(sRelativePathOrUrl, localWebUrl))
                {
                    String relativePath = GetRelativePath(sRelativePathOrUrl, webBasicVirtualPath);
                    return webBasicPath + relativePath.Replace("/", "\\");
                }
                else
                {
                    return (webBasicPath + MyString.Right(sRelativePathOrUrl, webBasicUrl, true, "")).Replace("/", "\\");
                }
            }
            else
            {
                if (sRelativePathOrUrl.StartsWith("/"))
                {
                    //处理当仅仅传入一个斜杠符号的情况
                    if (sRelativePathOrUrl.Length == 1) sRelativePathOrUrl = "";
                    else sRelativePathOrUrl = sRelativePathOrUrl.Substring(1);
                }
                return (webBasicPath + sRelativePathOrUrl).Replace("/", "\\");
            }
        }

        /// <summary>
        /// 功能: 将绝对路径转换成相对路径(注意:这个绝对路径必须是web内的目录)
        /// </summary>
        /// <param name="sAbsolutePath"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static String AbsolutePathToVirtualPath(string sAbsolutePath)
        {
            String sWebPath = HttpContext.Current.Request.ServerVariables["APPL_PHYSICAL_PATH"].ToLower();
            String sPath = sAbsolutePath.ToLower().Replace("/", "\\").Replace(sWebPath, "").Replace("\\", "/");
            return sPath;
        }

        /// <summary>
        /// 功能: 将绝对路径转换成Url(注意:这个绝对路径必须是web内的目录)
        /// </summary>
        /// <param name="sAbsolutePath"></param>
        /// <returns></returns>
        public static String AbsolutePathToUrl(String sAbsolutePath)
        {
            return MyWebsite.GetServerAddr() + AbsolutePathToVirtualPath(sAbsolutePath);
        }

        #endregion
    }
}
