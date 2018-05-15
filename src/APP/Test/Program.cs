using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DreamCube.Foundation.Basic.Utility;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("start!!!!!");
            //CodeTimer.Time("Test", 1000000, () => { });
            //TestTrace.Start();
            TestExcelHelper.Start(@"C:\Users\ljmacx64\Desktop\test.xlsx");
            Console.WriteLine("end!!!!!!");
            Console.Read();
        }
    }
}
