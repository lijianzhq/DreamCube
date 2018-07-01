using sdmap.Compiler;
using sdmap.Functional;
using sdmap.Macros.Implements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace sdmap.test
{
    public class SqlTextTest2
    {
        [Fact]
        public void Test1()
        {
            var code = @"sql v1{select * from tablename where aa='123'}";
            var result = Run(code, "v1");
            Assert.Equal("select * from tablename where aa='123'", result);
        }

        [Fact]
        public void Test2()
        {
            var code = @"sql v1{select * from tablename where aa='\#123\#'}";
            var result = Run(code, "v1");
            Assert.Equal("select * from tablename where aa='#123#'", result);
        }

        [Fact]
        public void Test3()
        {
            var code = @"sql v1{select * from tablename #isNull<Param1,sql{ where aa=@Param1}>}";
            var result = Run(code, "v1", new { Param1 = "lijian" });
            Assert.Equal("select * from tablename ", result);

            dynamic dynimicObj = new ExpandoObject();
            dynimicObj.Param1 = "lijian";
            result = Run(code, "v1", dynimicObj);
            Assert.Equal("select * from tablename ", result);

            var dic = new Dictionary<String, String>();
            dic.Add("Param1", "lijian");
            result = Run(code, "v1", dic);
            Assert.Equal("select * from tablename ", result);


            code = @"sql v1{select * from tablename
where 1=1
#isNotNullOrEmpty<Param1,sql{ and aa=@Param1}>";
            dic = new Dictionary<String, String>();
            result = Run(code, "v1", dic);
            Assert.Equal(@"select * from tablename
where 1=1
", result);


            code = @"sql v1{select * from tablename
where 1=1 
#isNotNullOrEmpty<Param1,sql{and aa=@Param1}>";
            var dic2 = new Dictionary<String, Object>();
            dic2.Add("Param1", 3);
            result = Run(code, "v1", dic2);
            Assert.Equal(@"select * from tablename
where 1=1
and aa=@Param1", result);
        }

        [Fact]
        public void Hello()
        {
            var emited = Run("sql v1{#fuck<>}", "v1");
            Assert.Equal("Hello World", emited);
        }

        [Fact]
        public void IsNotNullOrEmptyTest()
        {
            var emited = Run("sql v1{#isNotNullOrEmpty<A, 'test'>}", "v1", new { });
            Assert.Equal("", emited);
        }

        public static string Run(string code, string sqlId, object obj = null)
        {
            var c = new SdmapCompiler();
            c.AddMacro("fuck", (ctx, ns, self, args) =>
            {
                return Result.Ok("Hello World");
            });
            //c.AddMacro("IsNotNullOrEmpty", (ctx, ns, self, args) =>
            //{
            //    if (self == null) return Result.Fail<string>($"Query requires not null in macro 'shit'.");

            //    var prop = GetProp(self, args[0]);
            //    if (prop == null) return Result.Ok("");

            //    if (!RuntimeMacros.IsEmpty(RuntimeMacros.GetPropValue(self, (string)args[0])))
            //        return EvalToString(args[1], ctx, self);
            //    return Result.Ok("");
            //});
            c.AddSourceCode(code);
            return c.Emit(sqlId, obj);
        }
        public static QueryPropertyInfo GetProp(object self, object syntax)
        {
            var props = (syntax as string).Split('.');
            var fronts = props.Take(props.Length - 1);

            if (self is IDictionary dicSelf)
            {
                if (!dicSelf.Contains(syntax))
                    return null;

                var val = dicSelf[syntax];
                if (val == null)
                    return new QueryPropertyInfo(props[0], typeof(object));

                return new QueryPropertyInfo(props[0], val.GetType());
            }
            else
            {
                var frontValue = fronts.Aggregate(self, (s, p) =>
                    s?.GetType().GetTypeInfo().GetProperty(p)?.GetValue(s));

                var pi = frontValue
                    ?.GetType()
                    .GetTypeInfo()
                    .GetProperty(props.Last());
                if (pi == null) return null;

                return new QueryPropertyInfo(pi.Name, pi.PropertyType);
            }
        }

        public static Result<string> EvalToString(object value, OneCallContext context, object self)
        {
            if (value is string)
                return Result.Ok((string)value);
            if (value is EmitFunction)
                return ((EmitFunction)value)(context.DigNewFragments(self));
            throw new ArgumentOutOfRangeException(nameof(value));
        }
    }
}
