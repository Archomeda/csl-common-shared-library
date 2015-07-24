using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using NUnit.Framework;

namespace CommonShared.UnitTests
{
    [TestFixture]
    public class LoggerTest
    {
        [Test]
        public void ConstructorPrefix()
        {
            Assembly assembly = this.GetType().Assembly;
            string expected = string.Format("[{0}]", assembly.GetName().Name);

            Logger logger = new Logger(assembly);
            Assert.AreEqual(expected, logger.Prefix);
        }
    }
}
