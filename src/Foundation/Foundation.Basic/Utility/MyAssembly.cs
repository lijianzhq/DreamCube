using System;
using System.Reflection;
using System.IO;
using System.Runtime.InteropServices;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyAssembly
    {
        /// <summary>
        /// 判断程序集是否属于COM程序集
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
#if NET20
        public static Boolean IsCOMAssembly(Assembly assembly)
#else
        public static Boolean IsCOMAssembly(this Assembly assembly)
#endif
        {
            Object[] AsmAttributes = assembly.GetCustomAttributes(typeof(ImportedFromTypeLibAttribute), true);
            return AsmAttributes.Length > 0;
        }

        /// <summary>
        /// 获取程序集的标题信息
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String GetAssemblyTitle(Assembly target)
#else
        public static String GetAssemblyTitle(this Assembly target)
#endif
        {
            AssemblyTitleAttribute titleAttribute = MyAssembly.GetAssemblyAttribute<AssemblyTitleAttribute>(target);
            return titleAttribute == null ? Path.GetFileNameWithoutExtension(target.CodeBase) :
              titleAttribute.Title;
        }

        /// <summary>
        /// 获取程序集的公司名
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String GetAssemblyCompany(Assembly target)
#else
        public static String GetAssemblyCompany(this Assembly target)
#endif

        {
            AssemblyCompanyAttribute companyAttribute = MyAssembly.GetAssemblyAttribute<AssemblyCompanyAttribute>(target);
            return companyAttribute == null ? String.Empty : companyAttribute.Company;
        }

        /// <summary>
        /// 获取程序集的版权声明
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String GetAssemblyCopyright(Assembly target)
#else
        public static String GetAssemblyCopyright(this Assembly target)
#endif
        {
            AssemblyCopyrightAttribute copyrightAttribute = MyAssembly.GetAssemblyAttribute<AssemblyCopyrightAttribute>(target);
            return copyrightAttribute == null ? String.Empty : copyrightAttribute.Copyright;
        }

        /// <summary>
        /// 获取程序集的Guid值
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String GetAssemblyGuid(Assembly target)
#else
        public static String GetAssemblyGuid(this Assembly target)
#endif
        {
            GuidAttribute guidAttribute = MyAssembly.GetAssemblyAttribute<GuidAttribute>(target);
            return guidAttribute == null ? String.Empty : guidAttribute.Value;
        }

        /// <summary>
        /// 获取程序集的描述信息
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String GetAssemblyDescription(Assembly target)
#else
        public static String GetAssemblyDescription(this Assembly target)
#endif
        {
            AssemblyDescriptionAttribute description = MyAssembly.GetAssemblyAttribute<AssemblyDescriptionAttribute>(target);
            return description == null ? String.Empty : description.Description;
        }

        /// <summary>
        /// 程序集版本号
        /// 此版本号存储在AssemblyDef清单元数据表中，CLR绑定到强命名程序集时会使用此版本号。
        /// 此版本号唯一标识了一个程序集。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static Version GetAssemblyVersion(Assembly target)
#else
        public static Version GetAssemblyVersion(this Assembly target)
#endif
        {
            return target.GetName().Version;
        }

        /// <summary>
        /// 程序集文件版本号
        /// 此版本号存储在Win32版本资源中，目前仅供参考，CLR不关心此属性
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String GetAssemblyFileVersion(Assembly target)
#else
        public static String GetAssemblyFileVersion(this Assembly target)
#endif
        {
            AssemblyFileVersionAttribute fileVersion = MyAssembly.GetAssemblyAttribute<AssemblyFileVersionAttribute>(target);
            return fileVersion == null ? String.Empty : fileVersion.Version;
        }

        /// <summary>
        /// 程序集信息版本号
        /// 此版本号存储在Win32版本资源中，目前仅供参考，CLR不关心此属性
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static String GetAssemblyInformationalVersion(Assembly target)
#else
        public static String GetAssemblyInformationalVersion(this Assembly target)
#endif
        {
            AssemblyInformationalVersionAttribute informationalVersion = MyAssembly.GetAssemblyAttribute<AssemblyInformationalVersionAttribute>(target);
            return informationalVersion == null ? String.Empty : informationalVersion.InformationalVersion;
        }

        /// <summary>
        /// 获取程序集的配置信息
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
#if NET20
        public static T GetAssemblyAttribute<T>(Assembly target) where T : class
#else
        public static T GetAssemblyAttribute<T>(this Assembly target) where T : class
#endif
        {
            Object[] attributes = target.GetCustomAttributes(typeof(T), false);
            if (attributes != null && attributes.Length > 0)
            {
                T titleAttribute = attributes[0] as T;
                if (titleAttribute != null)
                    return titleAttribute;
            }
            return null;
        }
    }
}
