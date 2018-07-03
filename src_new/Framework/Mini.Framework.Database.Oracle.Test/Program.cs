using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mini.Framework.Database.DefaultProviders;
using Mini.Framework.Database.Oracle;

namespace Mini.Framework.Database.Oracle.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Test1();
            Console.Read();
        }

        static void Test1()
        {
            var db = new DB(new OracleProvider("User Id=MQCSBUS;Password=MQCSBUS;Data Source=172.26.136.162/KFMQCS;Unicode=True"));
            using (var ctx = db.BeginExecuteContext())
            {
                var table = ctx.GetDataTable("select * from V1_ALL_QUES", 10, 2);
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
