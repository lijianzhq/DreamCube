using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using DreamCube.Foundation.Log;

namespace Foundation.Log.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DreamCube.Foundation.Log.Log.Root.LogDebug("LogDebug");
            DreamCube.Foundation.Log.Log.Root.LogError("LogError");
            DreamCube.Foundation.Log.Log.Root.LogFatal("LogFatal");
            DreamCube.Foundation.Log.Log.Root.LogInfo("LogInfo");
            DreamCube.Foundation.Log.Log.Root.LogWarn("LogWarn");
        }
    }
}
