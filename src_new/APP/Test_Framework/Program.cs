using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_Framework
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test start!!!");

            //TestTracer.Start();
            //Console.WriteLine(nameof(Test_Framework.Program));
            TestIOC.Start();

            Console.WriteLine("Test end!!!");
            Console.Read();
        }
    }
}
