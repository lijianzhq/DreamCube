using System;
using System.Reflection;
using Microsoft.Win32;

namespace DreamCube.Foundation.Basic.Utility
{
    public static class MyRegistry
    {
        /// <summary>
        /// 操作注册表的基础方法
        /// </summary>
        public static class Basic
        {
            /// <summary>
            /// 判断注册表指定的key是否存在
            /// </summary>
            /// <param name="parentKey">
            /// 父节点;通常取以下值：
            /// Registry.LocalMachine;Registry.ClassesRoot;Registry.CurrentConfig;Registry.CurrentUser;Registry.Users
            /// </param>
            /// <param name="key">注册表项，例如：SOFTWARE\WinRAR</param>
            /// <returns></returns>
            public static Boolean IsKeyExists(RegistryKey parentKey, String key)
            {
                RegistryKey tempKey = parentKey.OpenSubKey(key);
                return tempKey != null;
            }

            /// <summary>
            /// 判断HKEY_LOCAL_MACHINE是否存在子项
            /// </summary>
            /// <param name="key">注册表项，例如：SOFTWARE\WinRAR</param>
            /// <returns></returns>
            public static Boolean IsLocalMachineSubKeyExists(String key)
            {
                return IsKeyExists(Registry.LocalMachine, key);
            }

            /// <summary>
            /// 以写方式打开注册表项HKEY_LOCAL_MACHINE的子项
            /// </summary>
            /// <param name="subKey">注册表项，例如：SOFTWARE\WinRAR</param>
            /// <param name="createWhenSubKeyNotFound">当没有找到该注册表项时，是否创建该项；默认为创建</param>
            /// <returns></returns>
#if NET20
            public static RegistryKey OpenLocalMachineSubKeyForWrite(String subKey, Boolean createWhenSubKeyNotFound = true)
#else
            public static RegistryKey OpenLocalMachineSubKeyForWrite(String subKey, Boolean createWhenSubKeyNotFound = true)
#endif
            {
                return OpenKeyForWrite(Registry.LocalMachine, subKey, createWhenSubKeyNotFound);
            }

            /// <summary>
            /// 以读方式打开注册表项HKEY_LOCAL_MACHINE的子项
            /// </summary>
            /// <param name="subKey"></param>
            /// <returns></returns>
            public static RegistryKey OpenLocalMachineSubKeyForRead(String subKey)
            {
                return OpenKeyForWrite(Registry.LocalMachine, subKey);
            }

            /// <summary>
            /// 读取HKEY_LOCAL_MACHINE的子项的注册表项的某个属性的值
            /// </summary>
            /// <param name="registryKeyName">注册表项，例如：HKEY_LOCAL_MACHINE\SOFTWARE\WinRAR【前面不需要再带HKEY_LOCAL_MACHINE】</param>
            /// <param name="propertyName">注册表项下面的属性，指定的属性名称，如果为空，则是查询“(默认)”键的值</param>
            /// <returns></returns>
            public static Object GetLocalMachineSubKeyPropertyValue(String registryKeyName, String propertyName = "")
            {
                return GetKeyPropertyValue(Registry.LocalMachine, registryKeyName, propertyName);
            }

            /// <summary>
            /// 读取某个注册表项的某个属性的值
            /// </summary>
            /// <param name="parentKey">
            /// 父节点;通常取以下值：
            /// Registry.LocalMachine;Registry.ClassesRoot;Registry.CurrentConfig;Registry.CurrentUser;Registry.Users
            /// </param>
            /// <param name="registryKeyName">注册表项，例如：SOFTWARE\WinRAR</param>
            /// <param name="propertyName">注册表项下面的属性，指定的属性名称，如果为空，则是查询“(默认)”键的值</param>
            public static Object GetKeyPropertyValue(RegistryKey parentKey, String registryKeyName, String propertyName)
            {
                RegistryKey targetKey = OpenKeyForRead(parentKey, registryKeyName);
                if (targetKey != null)
                {
                    using (targetKey)
                    {
                        return targetKey.GetValue(propertyName);
                    }
                }
                return String.Empty;
            }

            /// <summary>
            /// 设置HKEY_LOCAL_MACHINE的子项的注册表项的某个属性的值
            /// </summary>
            /// <param name="subKey">注册表项，例如：SOFTWARE\South\SINFO</param>
            /// <param name="propertyName">属性名，例如：OAConfiger，如果是设置“（默认）”键的值，此参数传入空串即可</param>
            /// <param name="propertyValue">属性值，例如：C:\Program Files\广东南方数码科技有限公司\OAConfiger</param>
            /// <param name="createWhenSubKeyNotFound">当没有找到该注册表项时，是否创建该项；默认为创建</param>
            public static void SetLocalMachineKeyPropertyValue(String subKey, String propertyName, Object propertyValue, Boolean createWhenSubKeyNotFound = true)
            {
                RegistryKey targetKey = OpenLocalMachineSubKeyForWrite(subKey, createWhenSubKeyNotFound);
                using (targetKey)
                {
                    if (targetKey != null)
                    {
                        targetKey.SetValue(propertyName, propertyValue);
                    }
                }
            }

            /// <summary>
            /// 更改某个注册表项的某个属性的值
            /// </summary>
            /// <param name="parentKey">
            /// 父节点;通常取以下值：
            /// Registry.LocalMachine;Registry.ClassesRoot;Registry.CurrentConfig;Registry.CurrentUser;Registry.Users
            /// </param>
            /// <param name="registryKeyName">注册表项，例如：SOFTWARE\WinRAR</param>
            /// <param name="propertyName">属性名，例如：OAConfiger，如果是设置“（默认）”键的值，此参数传入空串即可</param>
            /// <param name="propertyValue">属性值</param>
#if NET20
            public static void SetKeyPropertyValue(RegistryKey parentKey, String registryKeyName, String propertyName, Object propertyValue)
#else
            public static void SetKeyPropertyValue(RegistryKey parentKey, String registryKeyName,
                                                    String propertyName, Object propertyValue)
#endif
            {
                RegistryKey targetKey = OpenKeyForWrite(parentKey, registryKeyName, true);
                using (targetKey)
                {
                    if (targetKey != null)
                    {
                        targetKey.SetValue(propertyName, propertyValue);
                    }
                }
            }

            /// <summary>
            /// 以写方式打开注册表项
            /// </summary>
            /// <param name="parentKey">
            /// 父节点;通常取以下值：
            /// Registry.LocalMachine;Registry.ClassesRoot;Registry.CurrentConfig;Registry.CurrentUser;Registry.Users
            /// </param>
            /// <param name="subKey">注册表项，例如：SOFTWARE\WinRAR</param>
            /// <param name="createWhenSubKeyNotFound">当没有找到该注册表项时，是否创建该项；默认为创建</param>
            /// <returns></returns>
#if NET20
            public static RegistryKey OpenKeyForWrite(RegistryKey parentKey, String subKey, Boolean createWhenSubKeyNotFound = true)
#else
            public static RegistryKey OpenKeyForWrite(RegistryKey parentKey, String subKey, Boolean createWhenSubKeyNotFound = true)
#endif
            {
                RegistryKey regKey = parentKey.OpenSubKey(subKey, true);
                if (regKey == null && createWhenSubKeyNotFound)
                    regKey = parentKey.CreateSubKey(subKey);

                return regKey;
            }

            /// <summary>
            /// 以只读方式打开注册表项
            /// </summary>
            /// <param name="parentKey">
            /// 父节点;通常取以下值：
            /// Registry.LocalMachine;Registry.ClassesRoot;Registry.CurrentConfig;Registry.CurrentUser;Registry.Users
            /// </param>
            /// <param name="subKey">注册表项，例如：SOFTWARE\WinRAR</param>
            /// <returns></returns>
            public static RegistryKey OpenKeyForRead(RegistryKey parentKey, String subKey)
            {
                RegistryKey regKey = parentKey.OpenSubKey(subKey, false);
                return regKey;
            }

            /// <summary>
            /// 获取HKEY_LOCAL_MACHINE的给定子项的所有子项，会把所有子项返回
            /// </summary>
            /// <param name="itemKey">注册表项，例如：SOFTWARE\South\SINFO</param>
            public static String[] GetLocalMachineItemSubItems(String itemKey)
            {
                RegistryKey subItem = Registry.LocalMachine.OpenSubKey(itemKey);
                if (subItem != null) return subItem.GetValueNames();
                return null;
            }
        }

        /// <summary>
        /// 实际开发过程中常用的案例
        /// </summary>
        public static class Example
        {
            /// <summary>
            /// 获取本机的IE版本
            /// </summary>
            /// <returns></returns>
            public static String GetIEVersion()
            {
                using (RegistryKey key = Basic.OpenLocalMachineSubKeyForRead(@"SOFTWARE\Microsoft\Internet Explorer"))
                {
                    if (key == null) return String.Empty;
                    String svcVersion = Convert.ToString(key.GetValue("svcVersion"));
                    String version = Convert.ToString(key.GetValue("Version"));
                    if (!String.IsNullOrEmpty(svcVersion)) return svcVersion;
                    return version;
                    //String[] names = key.GetValueNames();
                    //if (MyString.ContainsTargetStringInArray(names, "svcVersion")) //64位操作系统的
                    //{
                    //    return Convert.ToString(key.GetValue("svcVersion"));
                    //}
                    //else
                    //{
                    //    String version = Convert.ToString(key.GetValue("Version"));
                    //    return version;
                    //}
                }
            }

            /// <summary>
            /// 配置系统注册表的自动启动项
            /// </summary>
            /// <param name="appName">应用程序名</param>
            /// <param name="autoStart">是否自启动</param>
            /// <returns></returns>
            public static Boolean ConfigAutoStartApp(String appName, Boolean autoStart)
            {
                using (RegistryKey key = Basic.OpenLocalMachineSubKeyForWrite(@"SoftWare\Microsoft\Windows\CurrentVersion\Run", false))
                {
                    if (key == null) return false;
                    String currentAppPath = Assembly.GetEntryAssembly().Location;
                    if (autoStart)
                        key.SetValue(appName, currentAppPath);
                    else
                        key.DeleteValue(appName, false);
                    key.Close();
                }
                return true;
            }

            /// <summary>
            /// 设置本地执行脚本是否允许
            /// </summary>
            /// <param name="enabled"></param>
            public static void SetLocalScriptsEnabled(Boolean enabled)
            {
                Basic.SetKeyPropertyValue(Registry.LocalMachine, @"SOFTWARE\Microsoft\Windows\Windows Script Host\Settings", "Enabled", enabled ? "1" : "0");
            }
        }
    }
}
