using System;
using System.Collections.Generic;
using System.Text;

using Mini.Foundation.TraceService;

namespace Test_Framework
{
    class TestTracer
    {
        public static void Start()
        {
            new Tracer().TraceInformation("test trace!!!!");
        }
    }
}
