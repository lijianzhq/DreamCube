using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DreamCube.Foundation.TraceService;

namespace Test
{
    class TestTrace
    {
        public static void Start()
        {
            Tracer.Instance.TraceInformation("test");
        }
    }
}
