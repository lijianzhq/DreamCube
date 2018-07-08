#if !(NETSTANDARD1_0 || NETSTANDARD1_3 || NETSTANDARD2_0 || NETCOREAPP2_0)
using System;
using System.CodeDom.Compiler;
using System.Reflection;

namespace Mini.Foundation.Basic.Utility
{
    /// <summary>
    /// 调用微软的jscript引擎执行js的代码
    /// </summary>
    public class JScriptHelper
    {
        /// <summary>
        /// 计算结果,如果表达式出错则抛出异常
        /// </summary>
        /// <param name="statement">表达式,如"1+2+3+4"；转换json对象，如({\"name\":\"lijian\"})</param>
        /// <returns>结果</returns>
        public static Object Eval(string statement)
        {
            return _evaluatorType.InvokeMember(
                        "Eval",
                        BindingFlags.InvokeMethod,
                        null,
                        _evaluator,
                        new object[] { statement }
                     );
        }

        static JScriptHelper()
        {
            //构造JScript的编译驱动代码
            CodeDomProvider provider = CodeDomProvider.CreateProvider("JScript");

            CompilerParameters parameters;
            parameters = new CompilerParameters();
            parameters.GenerateInMemory = true;

            CompilerResults results;
            results = provider.CompileAssemblyFromSource(parameters, _jscriptSource);

            Assembly assembly = results.CompiledAssembly;
            _evaluatorType = assembly.GetType("Evaluator");

            _evaluator = Activator.CreateInstance(_evaluatorType);
        }

        private static object _evaluator = null;
        private static Type _evaluatorType = null;

        /// <summary>
        /// JScript代码
        /// </summary>
        private static readonly string _jscriptSource =
            @"class Evaluator
              {
                  public function Eval(expr : String) : Object 
                  { 
                     var me = {};
                     return eval(expr); 
                  }
              }";
    }
}
#endif
