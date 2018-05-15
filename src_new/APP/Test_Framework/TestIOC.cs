using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Mini.Foundation.IOC;

namespace Test_Framework
{
    class TestIOC
    {
        public static void Start()
        {
            ContainerFactory.RegisterConfigFile(new String[] { @"config/ioc.xml" });
            IA a = ContainerFactory.GetContainer().Resolve<IA>();
            a.PrintA();
        }
    }

    public interface IA
    {
        void PrintA();
    }
    public interface IB
    {
        void PrintB();
    }

    public class ObjA : IA, IB
    {
        public void PrintA()
        {
            Console.WriteLine("{0},{1}", nameof(ObjA), nameof(IA));
        }

        public void PrintB()
        {
            Console.WriteLine("{0},{1}", nameof(ObjA), nameof(IB));
        }
    }

    public class ObjB : IA, IB
    {
        public void PrintA()
        {
            Console.WriteLine("{0},{1}", nameof(ObjB), nameof(IA));
        }

        public void PrintB()
        {
            Console.WriteLine("{0},{1}", nameof(ObjB), nameof(IB));
        }
    }
}
