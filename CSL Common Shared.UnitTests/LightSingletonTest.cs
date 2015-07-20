using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace CommonShared.UnitTests
{
    [TestFixture]
    public class LightSingletonTest : LightSingleton<LightSingletonTest>
    {
        [Test]
        public void SameInstance()
        {
            LightSingletonTest instance1 = Instance;
            LightSingletonTest instance2 = Instance;
            Assert.AreSame(instance1, instance2);
        }
    }
}
