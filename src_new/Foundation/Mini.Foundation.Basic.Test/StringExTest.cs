using NUnit.Framework;
using Mini.Foundation.Basic.Utility;
using Autofac;

namespace Mini.Foundation.Basic.Test
{
    class StringExNew : StringEx
    {
        public override bool IsInvisibleString(string value)
        {
            return false;
        }
    }

    [TestFixture]
    public class StringExTest
    {
        Autofac.IContainer container = null;
        [SetUp]
        public void RunBeforeAllTests()
        {
            var builder = new Autofac.ContainerBuilder();
            //builder.RegisterType<StringExTest>().As<StringEx>();
            builder.RegisterAssemblyTypes(typeof(StringEx).Assembly).AsSelf();
            container = builder.Build();
        }

        [Test]
        public void InvisibleStringTest()
        {
            Assert.IsTrue(container.Resolve<StringEx>().IsInvisibleString(""));
            Assert.IsTrue(container.Resolve<StringEx>().IsInvisibleString("   "));
        }
    }
}
