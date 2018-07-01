using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

using Mini.Framework.Database;
using Mini.Framework.Database.DefaultProviders;

namespace Mini.Framework.Sdmap.Test2
{
    public enum QueryParamType
    {
        SqlParam = 0,
        ValueParam = 1
    }
    class QueryParam
    {
        public String Name { get; set; }

        public Object Value { get; set; }

        /// <summary>
        /// 0=Parameter查询参数；1=Value匹配参数
        /// </summary>
        public QueryParamType Type { get; set; } = QueryParamType.SqlParam;
    }

    class Program
    {
        private static String connectionStr = "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=localhost)(PORT=1521))(CONNECT_DATA=(SERVICE_NAME=ORCL)));User Id=guiyang;Password=guanliyuan";
        private static String connectionStr2 = "User Id = guiyang; Password=guanliyuan;Data Source = orcl; Unicode=True";

        static void Main(string[] args)
        {
            //Test1();
            //QueryByUser("白朋朋");
            //Test2();

            //Test3();
            //Test4();
            //Test5();
            Test6();

            Console.Read();
        }

        static void Test6()
        {
            var sqlTemplate = @"select assetuser,userdept from asset_log2 t
where 1=1
#isNotNullOrEmpty<assetuser,sql{and assetuser like :assetuser}>
#isNotNullOrEmpty<shortdept,sql{and shortdept='#prop<shortdept>'}>";
            var pList = new List<QueryParam>();
            pList.Add(new QueryParam()
            {
                Name = "assetuser",
                Value = "白%",
                Type = QueryParamType.SqlParam
            });
            pList.Add(new QueryParam()
            {
                Name = "shortdept",
                Value = "金阳分院",
                Type = QueryParamType.ValueParam
            });
            QueryByParams(sqlTemplate, pList);
        }

        static void Test5()
        {
            var sqlTemplate = @"select assetuser,userdept from asset_log2 t
where 1=1
#isNotNullOrEmpty<assetuser,sql{and assetuser like :assetuser}>
#isNotNullOrEmpty<shortdept,sql{and shortdept=:shortdept}>";
            var dic = new Dictionary<String, Object>();
            dic.Add("assetuser", "蒋%");
            QueryByParams(sqlTemplate, dic);
        }

        static void Test4()
        {
            var sqlTemplate = @"sql v1{select assetuser,userdept from asset_log2 t
                where 1=1
                #isNotNullOrEmpty<assetuser,sql{and assetuser='#prop<assetuser>'}>
                #isNotNullOrEmpty<shortdept,sql{and shortdept='#prop<shortdept>'}>}";
            var dic = new Dictionary<String, Object>();
            dic.Add("assetuser", "白朋朋");
            dic.Add("shortdept", "金阳分院");
            QueryByOthers(sqlTemplate, dic);
        }

        static void Test3()
        {
            var sqlTemplate = @"sql v1{select assetuser,userdept from asset_log2 t
                where 1=1
                #isNotNullOrEmpty<assetuser,sql{and assetuser='#prop<assetuser>'}>}";
            var dic = new Dictionary<String, Object>();
            dic.Add("assetuser", "白朋朋");
            QueryByOthers(sqlTemplate, dic);
        }

        static void Test2()
        {
            var sqlTemplate = @"sql v1{select assetuser,userdept from asset_log2 t
                where 1=1
                #isNotNullOrEmpty<assetuser,sql{and assetuser='#prop<assetuser>'}>}";
            QueryByOthers(sqlTemplate, null);
        }

        static void Test1()
        {
            var compiler = new sdmap.Compiler.SdmapCompiler();
            var sqlTemplate = "sql v1{select assetuser,userdept from asset_log2 t}";
            compiler.AddSourceCode(sqlTemplate);

            var paramDic = new Dictionary<String, Object>();
            var sql = compiler.Emit("v1", paramDic);

            Console.WriteLine(sql);
            var db = new DB(new OracleProvider("User Id=guiyang;Password=guanliyuan;Data Source=orcl;Unicode=True"));
            using (var ctx = db.BeginExecuteContext())
            {
                var table = ctx.GetDataTable(sql);
                ShowTable(table);
            }
        }

        static void QueryByParams(String sqlTemplate, IList<QueryParam> inputParamList)
        {
            var compiler = new sdmap.Compiler.SdmapCompiler();
            sqlTemplate = $"sql v1{{{sqlTemplate}}}";
            compiler.AddSourceCode(sqlTemplate);

            var db = new DB(new OracleProvider(connectionStr2));
            var parameters = new Dictionary<String, Object>();
            var dbParams = new List<DbParameter>();
            foreach (var p in inputParamList)
            {
                if (p.Type == QueryParamType.SqlParam)
                    dbParams.Add(db.CreateParameter(p.Name, p.Value));
                parameters.Add(p.Name, p.Value);
            }

            var sql = compiler.Emit("v1", parameters);
            Console.WriteLine(sql);

            using (var ctx = db.BeginExecuteContext())
            {
                var table = ctx.GetDataTable(sql, dbParams.ToArray());
                ShowTable(table);
            }
        }

        static void QueryByParams(String sqlTemplate, Dictionary<String, Object> parameters)
        {
            var compiler = new sdmap.Compiler.SdmapCompiler();
            sqlTemplate = $"sql v1{{{sqlTemplate}}}";
            compiler.AddSourceCode(sqlTemplate);

            var sql = compiler.Emit("v1", parameters);
            Console.WriteLine(sql);

            var db = new DB(new OracleProvider(connectionStr2));
            var dbParams = new List<DbParameter>();
            foreach (var p in parameters)
            {
                dbParams.Add(db.CreateParameter(p.Key, p.Value));
            }

            using (var ctx = db.BeginExecuteContext())
            {
                var table = ctx.GetDataTable(sql, dbParams.ToArray());
                ShowTable(table);
            }
        }

        static void QueryByUser(String userName)
        {
            var compiler = new sdmap.Compiler.SdmapCompiler();
            var sqlTemplate = @"sql v1{select assetuser,userdept from asset_log2 t
                where 1=1
                #isNotNullOrEmpty<assetuser,sql{and assetuser='#prop<assetuser>'}>}";
            compiler.AddSourceCode(sqlTemplate);

            var paramDic = new Dictionary<String, Object>();
            paramDic.Add("assetuser", userName);
            var sql = compiler.Emit("v1", paramDic);

            Console.WriteLine(sql);
            var db = new DB(new OracleProvider(connectionStr2));
            using (var ctx = db.BeginExecuteContext())
            {
                var table = ctx.GetDataTable(sql);
                ShowTable(table);
            }
        }

        static void QueryByOthers(String sqlTemplate, Dictionary<String, Object> parameters)
        {
            var compiler = new sdmap.Compiler.SdmapCompiler();
            //var sqlTemplate = @"sql v1{select assetuser,userdept from asset_log2 t
            //    where 1=1
            //    #isNotNullOrEmpty<assetuser,sql{and assetuser='#prop<assetuser>'}>}";
            compiler.AddSourceCode(sqlTemplate);

            var sql = compiler.Emit("v1", parameters);

            Console.WriteLine(sql);
            var db = new DB(new OracleProvider(connectionStr2));
            using (var ctx = db.BeginExecuteContext())
            {
                var table = ctx.GetDataTable(sql);
                ShowTable(table);
            }
        }

        static void ShowTable(DataTable table)
        {
            if (table == null) Console.WriteLine("table null");
            Console.WriteLine($"rowcount:{table.Rows.Count}");
            for (var i = 0; i < table.Rows.Count; i++)
            {
                for (var j = 0; j < Math.Min(table.Columns.Count, 2); j++)
                {
                    Console.Write($"{table.Rows[i][j]}\t");
                }
                Console.WriteLine();
            }
        }
    }
}
