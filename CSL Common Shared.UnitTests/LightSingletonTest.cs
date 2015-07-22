using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework;
using NUnit.Framework;

namespace CommonShared.UnitTests
{
    [TestFixture]
    public class LightSingletonTest : SingletonLite<LightSingletonTest>
    {
        [Test]
        public void SameInstance()
        {
            LightSingletonTest instance1 = instance;
            LightSingletonTest instance2 = instance;
            Assert.AreSame(instance1, instance2);
        }
    }
}
