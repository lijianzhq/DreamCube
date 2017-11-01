using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Collections.Generic;

//自定义命名空间
using DreamCube.Foundation.Basic.Enums;
using DreamCube.Foundation.Basic.Utility;

namespace DreamCube.Foundation.Basic.Utility
{
    /// <summary>
    /// String 扩展方法（这里把String看成是一个方法名、属性名，执行反射获得相对于的对象实例）
    /// </summary>
    public static class MyReflection
    {
        /// <summary>
        /// 通过反射调用指定的类静态方法
        /// </summary>
        /// <param name="functionFullName">
        /// 方法的完整名称（完整的方法名，必须带上命名空间）；并且还可以带上指定的参数类型（只支持.NET自带的类型）：
        /// 传入样式1：MyReflection.ExecFunction
        /// 传入样式2：MyReflection.ExecFunction(String,String,Object[])【效率较高】
        /// </param>
        /// <param name="assemblyName">方法所在的程序集的名称，如果程序集名称与名模空间名称对应的话，可以不传入</param>
        /// <param name="methodParams">方法的参数数组</param>
        /// <returns></returns>
        public static Object ExecFunction(String functionFullName, String assemblyName, params Object[] methodParams)
        {
            MethodInfo methodInfo = null;
            String className = String.Empty;
            String inputFunctionName = functionFullName; //把传入的完整方法名记录下来，方便记录日志
            String functionName = String.Empty;
            List<Type> typeList = new List<Type>();
            if (functionFullName.Contains("("))
            {
                //生成类型
                String[] types = MyString.SplitEx(MyString.SubStringBetweenStr(functionFullName, "(", ")", true, false)[0], ",", StringSplitOptions.None, MyString.TrimType.trimBoth);
                if (types != null && types.Length > 0)
                {
                    for (Int32 i = 0, j = types.Length; i < j; i++)
                        typeList.Add(MyType.GetType(types[i]));
                }
                functionFullName = MyString.LeftOfLast(functionFullName, "(");
            }
            if (functionFullName.Contains("."))
            {
                className = MyString.LeftOfLast(functionFullName, ".");
                functionName = MyString.RightOfLast(functionFullName, ".");
            }
            else
            {
                //如果传入的方法名不包含.符号，则默认为调用本类的方法
                className = "DreamCube.Foundation.Basic.Utility.MyReflection";
                functionName = functionFullName;
            }

            Assembly assembly = null;
            if (String.IsNullOrEmpty(assemblyName))
            {
                assemblyName = MyString.LeftOfLast(className, ".");
                while (!String.IsNullOrEmpty(assemblyName))
                {
                    if (TryLoadAssembly(assemblyName, out assembly, false))
                        break;
                    assemblyName = MyString.LeftOfLast(assemblyName, ".");
                }
                if (assembly == null) throw new Exception(Properties.Resources.ExceptionAssemblyLoadFail);
            }
            else if (!TryLoadAssembly(assemblyName, out assembly))
                return false;

            Type type = assembly.GetType(className);
            //默认的绑定方式
            BindingFlags defaultFlag = BindingFlags.IgnoreCase |
                                        BindingFlags.NonPublic |
                                        BindingFlags.Public |
                                        BindingFlags.Static;
            //如果没有传入类型数组参数，则直接根据方法名获取方法对象
            //仅仅搜索静态方法
            if (type != null)
            {
                if (methodParams == null || methodParams.Length == 0)
                {
                    try
                    { methodInfo = type.GetMethod(functionName, defaultFlag); }
                    catch (AmbiguousMatchException)
                    { throw new AmbiguousMatchException(String.Format(Properties.Resources.ExceptionMutilMethod, functionName)); }
                }
                else
                {
                    if (typeList.Count > 0)
                    {
                        try
                        {
                            Type[] types = typeList.ToArray();
                            methodInfo = type.GetMethod(functionName, types);
                            //需要把传入的方法参数进行数据格式化
                            Object[] outPutParameters = null;
                            try
                            {
                                if (MyConverter.TryConvertInOrder(methodParams, types, out outPutParameters, null, null, HandleExceptionInTry.ThrowException))
                                    methodParams = outPutParameters;
                            }
                            catch (ArgumentException)
                            {
                                String error = String.Format("远程调用方法【{0}】失败，指定的方法参数个数为【{1}】与传入的方法参数个数【{2}】不匹配！", inputFunctionName, types.Length, methodParams.Length);
                                MyLog.MakeLog(error);
                                return null;
                            }
                            catch (Exception ex)
                            {
                                MyLog.MakeLog(ex);
                                return null;
                            }
                        }
                        catch (AmbiguousMatchException)
                        {
                            throw new AmbiguousMatchException(String.Format(Properties.Resources.ExceptionMutilMethod, functionName));
                        }
                    }
                    else
                    {
                        //搜索同名方法，然后判断参数
                        MethodInfo[] methods = type.GetMethods(defaultFlag);
                        for (Int32 i = 0, j = methods.Length; i < j; i++)
                        {
                            if (String.Compare(methods[i].Name, functionName, true) == 0)
                            {
                                ParameterInfo[] parameters = methods[i].GetParameters();
                                //如果参数个数匹配，则再判断参数类型
                                if (parameters.Length == methodParams.Length)
                                {
                                    Object[] outPutParameters = null;
                                    Type[] types = MyType.GetParameterInfoType(parameters).ToArray();
                                    if (MyConverter.TryConvertInOrder(methodParams, types, out outPutParameters, null, null, HandleExceptionInTry.ReturnAndIgnoreLog))
                                    {
                                        methodParams = outPutParameters;
                                        methodInfo = methods[i];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //记录日志部分
            if (methodInfo != null)
            {
                return methodInfo.Invoke(null, methodParams);
            }
            else
            {
                String tempFunctionFullName = String.Empty;
                if (methodParams == null || methodParams.Length == 0)
                    tempFunctionFullName = String.Format("{0}()", functionFullName);
                else
                {
                    //如果在传入的方法名上面直接传入类型参数表，则完整的方法名就是传入的方法名
                    if (typeList.Count > 0) tempFunctionFullName = inputFunctionName;
                    else
                    {
                        String paramString = String.Empty;
                        for (Int32 i = 0, j = methodParams.Length; i < j; i++)
                        {
                            String methodTypeString = "Object.NullValue";
                            if (methodParams[i] != null) methodTypeString = methodParams[i].GetType().ToString();
                            if (i == 0) paramString += MyString.RightOfLast(methodTypeString, ".");
                            else paramString += "," + MyString.RightOfLast(methodTypeString, ".");
                        }
                        tempFunctionFullName = String.Format("{0}({1})", functionFullName, paramString);
                    }
                }
                MyLog.MakeLog(String.Format("在指定的程序集【{0}】找不到方法【{1}】！", assembly, tempFunctionFullName));
            }
            return null;
        }

        /// <summary>
        /// 从指定的目录加载程序集到AppDomain中
        /// </summary>
        /// <param name="dirPath">目录名</param>
        /// <param name="loadedAssembly">返回加载到AppDomain中的程序集对象</param>
        /// <param name="searchPattern">搜索程序集的命名规则，模式搜索所有的程序集</param>
        /// <param name="searchOption">搜索目录的方式，默认为只搜索顶级目录，不进行递归搜索</param>
#if NET20
        public static void LoadDllToAppDomain(String dirPath,
                                              out List<Assembly> loadedAssembly,
                                              String searchPattern = "*.dll",
                                              SearchOption searchOption = SearchOption.TopDirectoryOnly,
                                              HandleExceptionInLoop handleExceptionInLoop = HandleExceptionInLoop.ContinueAndThrowUntilLoopEnd)
#else
        public static void LoadDllToAppDomain(this String dirPath, 
                                              out List<Assembly> loadedAssembly,
                                              String searchPattern = "*.dll",
                                              SearchOption searchOption = SearchOption.TopDirectoryOnly,
                                              HandleExceptionInLoop handleExceptionInLoop = HandleExceptionInLoop.ContinueAndThrowUntilLoopEnd)
#endif
        {
            String[] dllFiles = Directory.GetFiles(dirPath, searchPattern, searchOption);
            loadedAssembly = new List<Assembly>();
            List<Exception> innerExceptionList = new List<Exception>();
            Assembly asm = null;
            for (Int32 i = 0; i < dllFiles.Length; i++)
            {
                try
                {
                    asm = Assembly.LoadFrom(dllFiles[i]);
                    loadedAssembly.Add(asm);
                }
                catch (Exception ex)
                {
                    switch (handleExceptionInLoop)
                    {
                        case HandleExceptionInLoop.ContinueAndThrowUntilLoopEnd:
                            innerExceptionList.Add(ex);
                            break;
                        case HandleExceptionInLoop.ContinueReturnAndIgnoreAllException:
                            break;
                        case HandleExceptionInLoop.ContinueReturnAndMylog:
                            MyLog.MakeLog(ex);
                            break;
                        case HandleExceptionInLoop.ThrowExceptionRightNow :
                            throw;
                    }
                }
            }
            if (innerExceptionList.Count > 0)
            {
                StringBuilder builder = new StringBuilder();
                for (Int32 i = 0; i < innerExceptionList.Count; i++)
                    builder.Append(innerExceptionList[i].Message);
                Exception ex = null;
#if NET20
                ex = new Exception(String.Format(Properties.Resources.ExceptionMismatchParams, dirPath, builder.ToString()));
#elif NET40
                ex = new AggregateException(innerExceptionList);
#endif
                throw ex;
            }
        }

        /// <summary>
        /// 根据完整的方法名通过反射获得方法对象实例;
        /// </summary>
        /// <param name="fullFunctionName"></param>
        /// <param name="parameterTypes">
        /// 表示此方法要获取的参数的个数、顺序和类型的 Type 对象数组。
        /// - 或 -
        /// 空的 Type 对象数组（由 EmptyTypes 字段提供），用来获取不采用参数的方法。 
        /// </param>
        /// <param name="assemblyName">
        /// 程序集名称，可选参数，如果不传入程序集名称，则代码会根据完整的方法名进行回退创建程序集对象，
        /// 直到方法名被回退到起始位置或者创建程序集对象成功，此方法能成功的前提是程序集名称和完整的方法名中的命名空间是一样的
        /// </param>
        /// <returns></returns>
#if NET20
        public static Boolean TryCreateMethodInfo(String fullFunctionName,
                                                  out MethodInfo methodInfo,
                                                  String assemblyName = "",
                                                  Type[] parameterTypes = null)
#else
        public static Boolean TryCreateMethodInfo(this String fullFunctionName,
                                                  out MethodInfo methodInfo, 
                                                  String assemblyName = "", 
                                                  Type[] parameterTypes = null)
#endif
        {
            methodInfo = null;
            String className = String.Empty;
            String functionName = String.Empty;
            if (fullFunctionName.Contains("."))
            {
                className = MyString.LeftOfLast(fullFunctionName,".");
                functionName = MyString.RightOfLast(fullFunctionName, ".");
            }
            else
            {
                //如果传入的方法名不包含.符号，则默认为调用本类的方法
                className = "DreamCube.Foundation.Basic.Utility.MyReflection";
                functionName = fullFunctionName;
            }

            Assembly assembly = null;
            if (String.IsNullOrEmpty(assemblyName))
            {
                assemblyName = MyString.LeftOfLast(className,".");
                while (!String.IsNullOrEmpty(assemblyName))
                {
                    if (TryLoadAssembly(assemblyName, out assembly, false))
                        break;
                    assemblyName = MyString.LeftOfLast(assemblyName, ".");
                }
                if (assembly == null) throw new Exception(Properties.Resources.ExceptionAssemblyLoadFail);
            }
            else if (!TryLoadAssembly(assemblyName, out assembly))
                return false;

            Type type = assembly.GetType(className);
            ///如果没有传入类型数组参数，则直接根据方法名获取方法对象，但是如果方法对象是重载的方法，则会抛出异常
            if (type != null)
            {
                if (parameterTypes == null || parameterTypes.Length == 0)
                {
                    try
                    { methodInfo = type.GetMethod(functionName); }
                    catch (AmbiguousMatchException)
                    { throw new AmbiguousMatchException(String.Format(Properties.Resources.ExceptionMutilMethod, functionName)); }
                }
                else methodInfo = type.GetMethod(functionName, parameterTypes);
            }
            else
                return false;

            return true;
        }

        /// <summary>
        /// 根据指定的程序集字符串，加载程序集；
        /// 创建成功返回true；创建失败返回false；
        /// 方法
        /// </summary>
        /// <param name="target"></param>
        /// <param name="assembly"></param>
        /// <param name="throwAllException">此参数用于框架内部使用；默认为true，抛出所有的异常，框架内部需要忽略某些异常</param>
        /// <param name="loadType">
        /// 程序集的加载类型，决定如何处理目标字符串
        /// AssemblyLoadType.LoadFromAssemblyName:表示把目标字符串看成程序集名
        /// AssemblyLoadType.LoadFromFileName:表示把目标字符串看成是程序集的文件名
        /// </param>
        /// <returns></returns>
#if NET20
        public static Boolean TryLoadAssembly(String target,
                                              out Assembly assembly,
                                              Boolean throwAllException = true,
                                              AssemblyLoadType loadType = AssemblyLoadType.LoadFromAssemblyName)
#else
        public static Boolean TryLoadAssembly(this String target,
                                              out Assembly assembly,
                                              Boolean throwAllException = true,
                                              AssemblyLoadType loadType = AssemblyLoadType.LoadFromAssemblyName)
#endif
        {
            assembly = null;
            try
            {
                if (loadType == AssemblyLoadType.LoadFromAssemblyName)
                    assembly = Assembly.Load(target);
                else if (loadType == AssemblyLoadType.LoadFromFileName)
                    assembly = Assembly.LoadFrom(target);
                return true;
            }
            catch (ArgumentNullException)
            {
                //assemblyString 为 null。 
                throw;
            }
            catch (ArgumentException)
            {
                //assemblyString 是零长度字符串。 
                throw;
            }
            catch (FileNotFoundException)
            {
                // assemblyString 未找到。 
                if (throwAllException)
                    throw;
                return false;
            }
            catch (FileLoadException)
            {
                // 发现一个未能加载的文件。
                if (throwAllException)
                    throw;
                return false;
            }
            catch (BadImageFormatException)
            {
                //assemblyString 不是有效程序集。 
                //- 或 -
                //当前正在加载公共语言运行时的 2.0 版或更高版本， assemblyString 是使用更高版本进行编译的。
                throw;
            }
            catch (Exception)
            { throw; }
        }

        #region "创建对象实例"

        /// <summary>
        /// 根据指定的类型名创建一个类型实例，类型名可以兼容两种形式：
        /// 1：如果该类型位于当前正在执行的程序集中或者 Mscorlib.dll 中，则提供由命名空间限定的类型名称就足够了。
        /// 2：如果该类型位于其他的程序集中，则必须提供完整的类型名称
        /// （此方法的values参数都没有被格式化好的，可以都是字符串格式）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="instance"></param>
        /// <param name="assembly">程序集，用于创建类型的程序集对象，不传入则调用创建本程序集的类型</param>
        /// <param name="types">对应构造函数的参数类型数组（顺序必须匹配某个构造函数）</param>
        /// <param name="values">对应构造函数的参数数组（顺序必须匹配某个构造函数），并且制定的值必须与类型一致</param>
        /// <returns></returns>
#if NET20
        public static Boolean TryCreateNewInstanceByTypeName<T>(String target,
                                                                out T instance, 
                                                                Assembly assembly = null, 
                                                                Type[] types = null, 
                                                                Object[] values = null) where T : class
#else
        public static Boolean TryCreateNewInstanceByTypeName<T>(this String target,
                                                                out T instance, 
                                                                Assembly assembly = null, 
                                                                Type[] types = null, 
                                                                Object[] values = null) where T : class
#endif
        {
            instance = null;
            if (String.IsNullOrEmpty(target)) return false;
            Type type = null;
            if (assembly != null)
                type = assembly.GetType(target);
            else
                type = Type.GetType(target);
            if (type != null)
            {
                ConstructorInfo info = types == null ? type.GetConstructor(new Type[0]) : type.GetConstructor(types);
                if (info == null) return false;
                instance = info.Invoke(MyConverter.ConvertInOrder(values, types)) as T;
                return instance != null;
            }
            return false;
        }

        /// <summary>
        /// 根据指定的类型名创建一个类型实例，类型名可以兼容两种形式：
        /// 1：如果该类型位于当前正在执行的程序集中或者 Mscorlib.dll 中，则提供由命名空间限定的类型名称就足够了。
        /// 2：如果该类型位于其他的程序集中，则必须提供完整的类型名称
        /// （此方法的values参数都必须是已经格式化好了）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="instance"></param>
        /// <param name="assembly">程序集，用于创建类型的程序集对象，不传入则调用创建本程序集的类型</param>
        /// <param name="values">此参数数组在数量、顺序和类型方面必须与要调用的构造函数的参数匹配。</param>
        /// <returns></returns>
        
#if NET20
        public static Boolean TryCreateNewInstanceByTypeName<T>(String target,
                                                                out T instance,
                                                                Assembly assembly = null,
                                                                Object[] values = null) where T : class
#else
        public static Boolean TryCreateNewInstanceByTypeName<T>(this String target,
                                                                out T instance, 
                                                                Assembly assembly = null, 
                                                                Object[] values = null) where T : class
#endif
        {
            instance = null;
            if (String.IsNullOrEmpty(target)) return false;
            Type type = null;
            if (assembly == null)
            {
                type = Type.GetType(target);
                assembly = type.Assembly;
            }
            instance = assembly.CreateInstance(target, true, BindingFlags.Default, null, values, null, null) as T;
            return false;
        }

        /// <summary>
        /// 根据完整的类型名，创建字符串指定的类型实例；
        /// 字符串必须符合格式：【类型名,程序集名】
        /// 例如：DreamCube.Framework.DataAccess.Sqlserver.SqlserverDb,DreamCube.Framework.DataAccess.Sqlserver,Version=1.0.0.0,Culture=neutral,PublicKeyToken=null
        /// 创建成功返回true；创建失败返回false；
        /// </summary>
        /// <param name="target"></param>
        /// <param name="assembly"></param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
#if NET20
        public static Boolean TryCreateNewInstanceByTypeFullName<T>(String target,
                                                                    out T instance,
                                                                    Boolean ignoreCase = false) where T : class
#else
        public static Boolean TryCreateNewInstanceByTypeFullName<T>(this String target,
                                                                    out T instance,
                                                                    Boolean ignoreCase = false) where T : class
#endif
        {
            Assembly assembly = null;
            instance = null;
            String assemblyName = MyString.Right(target, ",");
            if (!TryLoadAssembly(assemblyName, out assembly, false))
                throw new Exception(Properties.Resources.ExceptionAssemblyLoadFail);
            Object tempNewInstance = null;
            String typeName = MyString.Left(target, ",");
            if (assembly != null)
                tempNewInstance = assembly.CreateInstance(typeName, false);
            if (tempNewInstance != null)
            {
                instance = tempNewInstance as T;
                if (instance != null) return true;
            }
            return false;
        }

        /// <summary>
        /// 根据指定的类型字符串，创建字符串指定的类型实例；字符串必须是类型的完整命名空间；
        /// 并且有一个大前提：程序集名与命名空间必须是对应的关系，例如：DreamCube.Foundation.Basic.Logger类型，程序集名必须符合类型名的某个前缀
        /// 创建成功返回true；创建失败返回false；
        /// </summary>
        /// <param name="target"></param>
        /// <param name="instance"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static Boolean TryCreateNewInstanceByTypeName(String target, out Object instance, Boolean ignoreCase = false)
        {
            Assembly assembly = null;
            instance = null;
            String assemblyName = target;
            while (!String.IsNullOrEmpty(assemblyName))
            {
                if (TryLoadAssembly(assemblyName, out assembly, false))
                    break;
                assemblyName = MyString.LeftOfLast(assemblyName, ".");
            }
            if (assembly != null)
                instance = assembly.CreateInstance(target, false);
            return instance != null;
        }

        /// <summary>
        /// 根据指定的类型字符串，创建字符串指定的类型实例；字符串必须是类型的完整命名空间；
        /// 并且有一个大前提：程序集名与命名空间必须是对应的关系，例如：DreamCube.Foundation.Basic.Logger类型，程序集名必须符合类型名的某个前缀
        /// 创建成功返回true；创建失败返回false；
        /// </summary>
        /// <param name="target"></param>
        /// <param name="instance"></param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
#if NET20
        public static Boolean TryCreateNewInstanceByTypeName<T>(String target, out T instance, Boolean ignoreCase = false) where T : class
#else
            public static Boolean TryCreateNewInstanceByTypeName<T>(this String target,
                                out T instance, Boolean ignoreCase = false) where T : class
#endif
        {
            instance = default(T);
            Object tempNewInstance = null;
            if (TryCreateNewInstanceByTypeName(target, out tempNewInstance, ignoreCase))
            {
                instance = tempNewInstance as T;
                if (instance != null) return true;
            }
            return false;
        }

        #endregion

        #region "设置属性、字段的值"

        /// <summary>
        /// 获取属性值，目标字符串为属性名，传入指定的实例，获取该实例的指定属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="instance"></param>
        /// <param name="index">索引化属性的可选属性值</param>
        /// <returns></returns>
#if NET20
        public static Object GetPropertyValue(String propertyName, Object instance, Object[] index = null)
#else
        public static Object GetPropertyValue(this String propertyName, Object instance, Object[] index = null)
#endif
        {
            if (instance == null) return null;
            PropertyInfo pro = MyType.GetPropertyEx(instance, propertyName);
            if (pro != null)
                return pro.GetValue(instance, index);
            return null;
        }

        /// <summary>
        /// 获取某个对象指定属性的属性值；
        /// </summary>
        /// <param name="target"></param>
        /// <param name="propertyName"></param>
        /// <param name="defualtValue">如果指定的属性不存在，则返回此参数的值</param>
        /// <param name="unbindingFlags">排除以外类型的属性</param>
        /// <param name="bindingFlags">包括在内的类型属性</param>
        /// <returns></returns>
        public static TValue GetPropertyValue<TValue>(String propertyName, 
                                                      Object target,
                                                      TValue defualtValue = default(TValue),
                                                      BindingFlags[] unbindingFlags = null,
                                                      BindingFlags[] bindingFlags = null)
        {
            Type type = target.GetType();
            PropertyInfo property = MyType.GetPropertyEx(target, propertyName, unbindingFlags, bindingFlags);
            if (property != null) return (TValue)property.GetValue(target, null);
            return defualtValue;
        }

        /// <summary>
        /// 尝试获取某对象的属性值，获取成功返回true，获取失败返回false；
        /// 注意：此方法不向外排除异常；但是有异常会写入日志
        /// 属性名区分大小写
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyName">属性名</param>
        /// <param name="target"></param>
        /// <param name="value">返回对应的属性值</param>
        /// <param name="defualtValue">如果获取失败时，返回的默认值</param>
        /// <param name="unbindingFlags">排除以外类型的属性</param>
        /// <param name="bindingFlags">包括在内的类型属性</param>
        /// <returns></returns>
        public static Boolean TryGetPropertyValue<TValue>(String propertyName, 
                                                          Object target,
                                                          out TValue value,
                                                          TValue defualtValue = default(TValue),
                                                          BindingFlags[] unbindingFlags = null,
                                                          BindingFlags[] bindingFlags = null)
        {
            value = default(TValue);
            try
            {
                value = GetPropertyValue<TValue>(propertyName, target, defualtValue, unbindingFlags, bindingFlags);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 设置属性值
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="index">索引属性的索引值</param>
        /// <param name="bindingAttr"></param>
        public static void SetPropertyValue(String propertyName, 
                                            Object target,
                                            Object value,
                                            Object[] index = null,
                                            BindingFlags[] unbindingFlags = null,
                                            BindingFlags[] bindingFlags = null)
        {
            Type t = target.GetType();
            PropertyInfo info = MyType.GetPropertyEx(t, propertyName, unbindingFlags, bindingFlags);
            //增加一个类型转换
            info.SetValue(target, MyConverter.Convert(value, info.PropertyType), index);
        }

        /// <summary>
        /// 尝试设置属性值
        /// 返回true；设置成功，返回false；设置失败
        /// 注意：此方法不向外抛出异常；但是有异常会写入日志
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="target"></param>
        /// <param name="value"></param>
        /// <param name="index">索引属性的索引值</param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        public static Boolean TrySetPropertyValue(String propertyName, 
                                                  Object target,
                                                  Object value,
                                                  Object[] index = null,
                                                  BindingFlags[] unbindingFlags = null,
                                                  BindingFlags[] bindingFlags = null)
        {
            try
            {
                SetPropertyValue(propertyName, target, value, index, unbindingFlags, bindingFlags);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
