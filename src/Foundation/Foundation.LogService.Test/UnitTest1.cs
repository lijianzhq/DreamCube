using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DreamCube.Foundation.Log.Test
{
    [TestClass]
    public class UnitTest1
    {
        [DeploymentItem("Config", "Config")]
        [TestMethod]
        public void TestMethod1()
        {
            DreamCube.Foundation.LogService.Log.Root.LogDebug("LogDebug");
            DreamCube.Foundation.LogService.Log.Root.LogInfo("LogInfo");
            DreamCube.Foundation.LogService.Log.Root.LogWarn("LogWarn");
            DreamCube.Foundation.LogService.Log.Root.LogError("LogError");
            DreamCube.Foundation.LogService.Log.Root.LogFatal("LogFatal");
        }
    }
}
