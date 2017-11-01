using System;
using System.Net;
using System.IO;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Web;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using Microsoft.CSharp;

//自定义命名空间
using DreamCube.Foundation.Basic.Cache;
using DreamCube.Foundation.Basic.Cache.Interface;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyWeb
    {
        #region "字段"

        private static IDictionaryCachePool<String, Assembly> assemblyCache = new DictionaryCachePool<String, Assembly>();

        #endregion

        #region "公共方法"

        /// <summary>
        /// 从给定的Url下载文件
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static void DownloadFileFromUrl(String url, String savePath)
        {
            var myWebClient = new WebClient();
            myWebClient.DownloadFile(url, savePath);
        }

        /// <summary>
        /// 请求给定的Url获取返回的html内容
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="cookieContainer">cookie的包装对象，发送请求</param>
        /// <returns></returns>
        public static String GetHtmlByUrl(String url, Encoding encoding = null, CookieContainer cookieContainer = null)
        {
            string result = string.Empty;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.CookieContainer = cookieContainer;  
            var response = (HttpWebResponse)request.GetResponse();
            using (var receiveStream = response.GetResponseStream())
            {
                if (receiveStream != null)
                {
                    using (var readStream = new StreamReader(receiveStream, encoding == null ? Encoding.UTF8 : encoding))
                    {
                        result = readStream.ReadToEnd();
                        response.Close();
                        readStream.Close();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 获取客户端的IP
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetClientIP()
        {
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
        }

        /// <summary>
        /// 添加一个cookie
        /// </summary>
        /// <param name="key">key值</param>
        public static String GetCookie(String key)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null) return MyString.Right(HttpUtility.UrlDecode(cookie.Value), key + "=", true, "");
            return String.Empty;
        }

        /// <summary>
        /// 添加一个cookie
        /// </summary>
        /// <param name="key">key值</param>
        /// <param name="value">value值</param>
        /// <param name="expires">有效期</param>
        public static void AddCookie(String key, String value, DateTime expires)
        {
            if (!String.IsNullOrEmpty(value))
            {
                HttpCookie cookie = new HttpCookie(key);
                cookie.Values.Add(key, HttpUtility.UrlEncode(value));
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Response.Cookies[key].Expires = expires;
            }
        }

        /// <summary>
        /// 把post过来的文件数据保存成文件
        /// </summary>
        /// <param name="fileName">指定的文件名（完整的文件路径）</param>
        /// <param name="fileIndex">post过来的第几个文件，默认是0</param>
        public static void SavePostFileAs(String fileFullPath, Int32 fileIndex = 0)
        {
            if (HttpContext.Current != null)
            {
                HttpFileCollection postFiles = HttpContext.Current.Request.Files;
                if (fileIndex >= 0 && fileIndex < postFiles.Count)
                {
                    HttpPostedFile file = postFiles[fileIndex];
                    file.SaveAs(fileFullPath);
                }
            }
        }

        /// <summary>
        /// 获取post过来的文件数据
        /// </summary>
        /// <param name="fileIndex">获取post过来的第几个文件数据（从0开始，默认获取第一个文件返回）</param>
        /// <returns></returns>
        public static Byte[] GetPostFile(Int32 fileIndex = 0)
        {
            if (HttpContext.Current != null)
            {
                HttpFileCollection postFiles = HttpContext.Current.Request.Files;
                if (fileIndex >= 0 && fileIndex < postFiles.Count)
                {
                    HttpPostedFile file = postFiles[fileIndex];
                    using (Stream fs = file.InputStream)
                    {
                        Byte[] buffer = new Byte[fs.Length];
                        fs.Read(buffer, 0, buffer.Length);
                        return buffer;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 获取post过来的文件数据
        /// </summary>
        /// <returns></returns>
        public static Int32 GetPostFileCount()
        {
            if (HttpContext.Current != null)
            {
                HttpFileCollection postFiles = HttpContext.Current.Request.Files;
                return postFiles.Count;
            }
            return 0;
        }

        /// <summary>
        /// 获取post过来的字符串数据
        /// </summary>
        /// <returns></returns>
        public static String GetPostString()
        {
            if (HttpContext.Current != null)
            {
                Stream inputStream = HttpContext.Current.Request.InputStream;
                Byte[] buffer = new Byte[1024];
                Int32 read = 0;
                StringBuilder sb = new StringBuilder();
                do
                {
                    read = inputStream.Read(buffer, 0, buffer.Length);
                    sb.Append(HttpContext.Current.Request.ContentEncoding.GetString(buffer, 0, read));
                } while (read > 0);
                return HttpUtility.UrlDecode(sb.ToString());
            }
            return "";
        }

        /// <summary>
        /// 以form的方式post数据到指定的url；目标页面使用Request.Form["keyname"]的方式获取数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="formValues"></param>
        /// <param name="responseEncoding">返回内容的编码格式，采用此编码格式把返回的二进制数据读取成字符串（默认采用UTF8编码）</param>
        /// <returns></returns>
        public static String PostTo(String url, Dictionary<String, String> formValues, Encoding responseEncoding = null)
        {
            return PostTo(url, null, Enums.HttpContentType.text_plain, responseEncoding);
        }
        
        /// <summary>
        /// Post文件到指定的url
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath"></param>
        /// <param name="responseEncoding"></param>
        /// <returns></returns>
        public static String PostTo(String url, String filePath, Encoding responseEncoding)
        {
            return PostTo(url, filePath, "file", null, responseEncoding);
        }

        /// <summary>
        /// post数据到指定的url
        /// 支持：图片等文件，带参数 
        /// </summary>
        /// <param name="url">接收数据的url</param>
        /// <param name="fileData">指定文件的二进制数据</param>
        /// <param name="fileExtension">文件后缀名，入：.exe  .png 等，前面必须加.号</param>
        /// <param name="fileParamName">附件参数名（默认是file）</param>
        /// <param name="nvc">post到服务器的form数据</param>
        /// <param name="responseEncoding">返回内容的编码格式，采用此编码格式把返回的二进制数据读取成字符串（默认采用UTF8编码）</param>
        public static String PostTo(String url, Byte[] fileData, String fileExtension, String fileParamName = "file", Dictionary<String, String> nvc = null, Encoding responseEncoding = null)
        {
            String boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            Byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;
            Stream rs = wr.GetRequestStream();
            String formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            if (nvc != null)
            {
                foreach (String key in nvc.Keys)
                {
                    rs.Write(boundarybytes, 0, boundarybytes.Length);
                    String formitem = String.Format(formdataTemplate, key, nvc[key]);
                    Byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                    rs.Write(formitembytes, 0, formitembytes.Length);
                }
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            if (fileData != null)
            {
                String headerTemplate = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n";
                String fileName = "postFile" + fileExtension;
                String header = String.Format(headerTemplate, fileParamName, fileName, Enums.HttpContentTypeHelper.GetContentTypeByFileExtension(fileExtension));
                Byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);
                rs.Write(fileData, 0, fileData.Length);
            }

            Byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                if (responseEncoding == null) responseEncoding = Encoding.UTF8;
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2, responseEncoding);
                return reader2.ReadToEnd();
            }
            finally
            {
                wr = null;
            }
        }

        /// <summary>
        /// post数据到指定的url
        /// 支持：图片等文件，带参数 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="filePath">post到服务器的文件路径</param>
        /// <param name="fileParamName">附件参数名（默认是file）</param>
        /// <param name="nvc"></param>
        public static String PostTo(String url, String filePath, String fileParamName = "file", Dictionary<String, String> nvc = null, Encoding responseEncoding = null)
        {
            Byte[] data = File.ReadAllBytes(filePath);
            return PostTo(url, data, "." + MyString.RightOfLast(filePath, "."), fileParamName, nvc, responseEncoding);
        }

        /// <summary>
        /// post二进制数据到指定的服务器
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">指定请求的http头contentType</param>
        /// <param name="responseEncoding">返回内容的编码格式，采用此编码格式把返回的二进制数据读取成字符串（默认采用UTF8编码）</param>
        /// <param name="timeOut">超时时间，以毫秒为单位</param>
        /// <returns></returns>
        public static String PostTo(String url, Byte[] postData, Enums.HttpContentType contentType = Enums.HttpContentType.text_plain, Encoding responseEncoding = null, Int32 timeOut = -1)
        {
            if (responseEncoding == null) responseEncoding = Encoding.UTF8;
            Stream outstream = null;
            Stream instream = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            //设置参数 准备请求 
            request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            //指定不允许超时
            request.Timeout = timeOut;
            request.ContentType = Enums.HttpContentTypeHelper.GetContentTypeByEnmu(contentType);
            request.ContentLength = postData.Length;
            instream = request.GetRequestStream();
            instream.Write(postData, 0, postData.Length);
            instream.Close();
            //发送请求并获取相应回应数据  
            response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求  
            outstream = response.GetResponseStream();
            StreamReader sr = new StreamReader(outstream, responseEncoding);
            return sr.ReadToEnd();
        }

        /// <summary>
        /// post数据到指定的网页上
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post到服务器的数据（如果包含了字符串，必须要进行urldecode编码，否则会出现乱码）</param>
        /// <param name="requestEncoding">发送请求的内容编码格式，默认采用UTF8编码</param>
        /// <param name="responseEncoding">服务器返回的内容编码格式，默认采用UTF8编码</param>
        /// <returns></returns>
        public static String PostTo(String url, String postData, Encoding requestEncoding, Encoding responseEncoding)
        {
            if (requestEncoding == null) requestEncoding = Encoding.UTF8;
            postData = MyWebUtility.UrlEncode(postData);
            Byte[] data = requestEncoding.GetBytes(postData);
            return PostTo(url, data, Enums.HttpContentType.text_plain, responseEncoding);
        }

        /// <summary>
        /// post数据到指定的网页上
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post到服务器的数据（如果包含了字符串，必须要进行urldecode编码，否则会出现乱码）</param>
        /// <param name="timeOut">超时时间，以毫秒为单位</param>
        /// <returns></returns>
        public static String PostTo(String url, String postData, Int32 timeOut)
        {
            Byte[] data = Encoding.UTF8.GetBytes(postData);
            return PostTo(url, data, Enums.HttpContentType.text_plain, null, timeOut);
        }

        /// <summary>
        /// post数据到指定的网页上
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData">post到服务器的数据（如果包含了字符串，必须要进行urldecode编码，否则会出现乱码）</param>
        /// <returns></returns>
        public static String PostTo(String url, String postData)
        {
            return PostTo(url, postData, null, null);
        }

        /// <summary>
        /// 从指定的url获取xml文档
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static XmlDocument GetXmlDocFromUrl(String url)
        {
            String data = GetStringFromUrl(url);
            if (!String.IsNullOrEmpty(data))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(data);
                return doc;
            }
            return null;
        }

        /// <summary>
        /// 从指定的url获取二进制数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Byte[] GetDataFromUrl(String url)
        {
            WebRequest webreq = WebRequest.Create(url);
            WebResponse webres = webreq.GetResponse();
            Stream stream = webres.GetResponseStream();
            using (MemoryStream ms = new MemoryStream())
            {
                Byte[] buffer = new Byte[1024];
                Int32 read = stream.Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    ms.Write(buffer, 0, read);
                    read = stream.Read(buffer, 0, buffer.Length);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 从指定的url获取字符串
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding">返回的数据采用的编码方式，默认采用UTF8编码</param>
        /// <returns></returns>
        public static String GetStringFromUrl(String url, Encoding responseEncoding = null)
        {
            if (responseEncoding == null) responseEncoding = Encoding.UTF8;
            WebRequest webreq = WebRequest.Create(url);
            WebResponse webres = webreq.GetResponse();
            Stream stream = webres.GetResponseStream();
            using (MemoryStream ms = new MemoryStream())
            {
                Byte[] buffer = new Byte[1024];
                Int32 read = stream.Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    ms.Write(buffer, 0, read);
                    read = stream.Read(buffer, 0, buffer.Length);
                }
                return responseEncoding.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 根据Url路径，获取图片数据流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
#if NET20
        public static Byte[] GetImageFromUrl(String url)
#else
        public static Byte[] GetImageFromUrl(this String url)
#endif
        {
            WebRequest webreq = WebRequest.Create(url);
            WebResponse webres = webreq.GetResponse();
            Stream stream = webres.GetResponseStream();
            using (MemoryStream ms = new MemoryStream())
            {
                Byte[] buffer = new Byte[1024];
                Int32 read = stream.Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    ms.Write(buffer, 0, read);
                    read = stream.Read(buffer, 0, buffer.Length);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 从Url获取图片数据
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Image GetImageFromUrlEx(String url)
        {
            WebResponse respones = WebRequest.Create(url).GetResponse();
            Image image = Image.FromStream(respones.GetResponseStream());
            respones.Close();
            return image;
        }

        /// <summary>
        /// 根据url获取图片对象
        /// </summary>
        /// <param name="picUrl"></param>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Boolean TryGetImageFormUrl(String picUrl, out Image image)
        {
            image = null;
            try
            {
                WebResponse respones = WebRequest.Create(picUrl).GetResponse();
                image = Image.FromStream(respones.GetResponseStream());
                respones.Close();
                return true;
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            return false;
        }

        /// <summary>
        /// 根据url获取图片对象（自动根据url判断保存的图片类型）
        /// </summary>
        /// <param name="picUrl">图片的Url路径</param>
        /// <param name="imageFilePath">保存到本地的物理路径</param>
        /// <returns></returns>
        public static Boolean TryLoadImageFormUrl(String picUrl, String imageFilePath)
        {
            try
            {
                ImageFormat imageType = ImageFormat.Jpeg;
                String inputImageType = MyString.RightOfLast(picUrl, ".").ToLower();
                imageType = MyImage.GetImageFormatByImageType(inputImageType);
                return TryLoadImageFormUrl(picUrl, imageFilePath, imageType);
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            return false;
        }

        /// <summary>
        /// 根据url获取图片对象
        /// </summary>
        /// <param name="picUrl">图片的Url路径</param>
        /// <param name="imageFilePath">保存到本地的物理路径</param>
        /// <param name="imageType">保存的图片类型</param>
        /// <returns></returns>
        public static Boolean TryLoadImageFormUrl(String picUrl, String imageFilePath, ImageFormat imageType)
        {
            try
            {
                Image image = null;
                if (TryGetImageFormUrl(picUrl, out image))
                {
                    image.Save(imageFilePath, imageType);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MyLog.MakeLog(ex);
            }
            return false;
        }

        /// <summary>
        /// 根据url获取图片并保存到文件(直接写文件，没有通过Image进行格式化)
        /// </summary>
        /// <param name="url"></param>
        /// <param name="imageFilePath">图片文件保存路径（文件的完整名称）</param>
#if NET20
        public static void LoadImageFromUrl(String url, String imageFilePath)
#else
        public static void LoadImageFromUrl(this String url, String imageFilePath)
#endif
        {
            imageFilePath = imageFilePath.Replace("/", "\\");
            String path = MyString.LeftOfLast(imageFilePath , @"\");
            //确保目录存在
            MyIO.EnsurePath(path);
            Byte[] imageData = GetImageFromUrl(url);
            using (FileStream fs = new FileStream(imageFilePath, global::System.IO.FileMode.OpenOrCreate))
            {
                fs.Write(imageData, 0, imageData.Length);
            }
        }

        /// < summary>           
        /// 动态调用web服务           
        /// < /summary>           
        /// < param name="url">WSDL服务地址< /param>           
        /// < param name="methodname">方法名< /param>          
        /// /// < param name="args">参数< /param>           
        /// < returns>< /returns>          
        public static Object InvokeWebService(String url, String methodname, Object[] args)
        {
            return MyWeb.InvokeWebService(url, null, methodname, args);
        }

        /// < summary>          
        /// 动态调用web服务           
        /// < /summary>           
        /// < param name="url">WSDL服务地址</param>          
        /// < param name="classname">类名</param>           
        /// < param name="methodname">方法名</param>           
        /// < param name="args">参数</param>          
        /// < returns>< /returns>           
        public static Object InvokeWebService(String url, String classname, String methodname, Object[] args)
        {
            String @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            if (String.IsNullOrEmpty(classname)) classname = MyWeb.GetWsClassName(url);
            try
            {
                Assembly assembly = CreateAssembly(url, @namespace);
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                Object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodname);
                if (mi == null) throw new Exception(String.Format("无法从指定的webservice：{0} 找到调用入口：{1}", url, methodname));
                return mi.Invoke(obj, CreateMethodParams(assembly, mi, args));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
        }

        #endregion

        #region "私有方法"

        /// <summary>
        /// 获取程序集
        /// </summary>
        /// <param name="url">web服务的url地址</param>
        /// <param name="@namespace">程序集的根命名空间</param>
        /// <returns></returns>
        private static Assembly CreateAssembly(String url, String @namespace)
        {
            Assembly assembly = null;
            String key = url + @namespace;
            if (!assemblyCache.TryGetValue(key, out assembly))  //如果缓存中没有，则马上编译一个
            {
                // 1. 使用 WebClient 下载 WSDL 信息。                
                WebClient wc = new WebClient();
                using (Stream stream = wc.OpenRead(url + "?WSDL"))
                {
                    // 2. 创建和格式化 WSDL 文档。
                    ServiceDescription sd = ServiceDescription.Read(stream);
                    // 3. 创建客户端代理代理类。
                    ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                    sdi.ProtocolName = "Soap"; // 指定访问协议。
                    sdi.Style = ServiceDescriptionImportStyle.Client; // 生成客户端代理。
                    sdi.CodeGenerationOptions = CodeGenerationOptions.GenerateProperties | CodeGenerationOptions.GenerateNewAsync;
                    sdi.AddServiceDescription(sd, null, null); // 添加 WSDL 文档。
                    // 4. 使用 CodeDom 编译客户端代理类。
                    CodeNamespace cn = new CodeNamespace(@namespace);
                    //生成客户端代理类代码                   
                    CodeCompileUnit ccu = new CodeCompileUnit();
                    ccu.Namespaces.Add(cn);
                    sdi.Import(cn, ccu);
                    CSharpCodeProvider icc = new CSharpCodeProvider();
                    //设定编译参数                   
                    CompilerParameters cplist = new CompilerParameters();
                    cplist.GenerateExecutable = false;
                    cplist.GenerateInMemory = true;
                    cplist.ReferencedAssemblies.Add("System.dll");
                    cplist.ReferencedAssemblies.Add("System.XML.dll");
                    cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                    cplist.ReferencedAssemblies.Add("System.Data.dll");
                    //编译代理类                   
                    CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);

                    if (true == cr.Errors.HasErrors)
                    {
                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                        {
                            sb.Append(ce.ToString());
                            sb.Append(System.Environment.NewLine);
                        }
                        throw new Exception(sb.ToString());
                    }
                    //获取动态编译的程序集
                    assembly = cr.CompiledAssembly;
                    assemblyCache.TryAdd(key, assembly, Enums.CollectionsAddOper.ReplaceIfExist);
                }
            }
            return assembly;
        }

        /// <summary>
        /// 创建方法的参数
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Object[] CreateMethodParams(Assembly ab, MethodInfo method, Object[] args)
        {
            if (args == null || args.Length == 0) return null;
            ParameterInfo[] info = method.GetParameters();
            if (info == null || info.Length != args.Length) return null;
            List<Object> paramValues = new List<Object>(args.Length);
            for (Int32 i = 0; i < info.Length; i++)
            {
                if (info[i].ParameterType.IsPrimitive || info[i].ParameterType == typeof(String))
                {
                    paramValues.Add(args[i]);
                    continue;
                }
                else
                {
                    Object newObj = ab.CreateInstance(info[i].ParameterType.ToString());
                    if (newObj != null)
                    {
                        MyObject.CopyValueTo(args[i], newObj);
                        paramValues.Add(newObj);
                    }
                    else paramValues.Add(args[i]);
                }
            }
            return paramValues.ToArray();
        }

        /// <summary>
        /// 获取webservice默认的类名（也就是webservice的文件名）
        /// </summary>
        /// <param name="wsUrl"></param>
        /// <returns></returns>
        private static String GetWsClassName(String wsUrl)
        {
            String[] parts = wsUrl.Split('/');
            String[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
        }

        #endregion
    }
}
