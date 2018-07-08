using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Mini.Rename.Json100r1.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Test2();
            Console.WriteLine("done");
            Console.Read();
        }

        static void Test2()
        {
            
        }

        static void Test()
        {
            //var obj = JsonConvert.DeserializeObject(@"{""name"":""lijian""}");
            Console.WriteLine(Evaluator.Eval("1+2+3+4+5+6+7+8+9"));//45
            Console.WriteLine(Evaluator.Eval("({\"name\":\"lijian\"})"));//45
        }
    }
}
