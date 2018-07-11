using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Mini.Framework.ResourceCommon;

namespace Mini.Framework.ResourceCommon.Test2
{
    class Program
    {
        static void Main(string[] args)
        {
            Test();
            Console.WriteLine("done");
            Console.Read();
        }

        static void Test()
        {
            while (true)
            {
                Thread.Sleep(5000);
                Console.WriteLine(StrResourceManager.Current.GetString("judgeUserRole", @"\1.xml"));
            }
        }
    }
}
