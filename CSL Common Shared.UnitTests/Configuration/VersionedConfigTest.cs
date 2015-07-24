using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonShared.Configuration;
using NSubstitute;
using NUnit.Framework;

namespace CommonShared.UnitTests.Configuration
{
    [TestFixture]
    public class VersionedConfigTest : ConfigTest
    {
        public class TestVersionedConfig : VersionedConfig
        {
            public TestVersionedConfig()
            {
                this.Version = 0;
            }
        }

        [Test]
        public void LoadVersionedConfig()
        {
            IConfigMigrator<TestVersionedConfig> migrator = Substitute.For<IConfigMigrator<TestVersionedConfig>>();
            TestVersionedConfig testConfig = this.CreateConfig<TestVersionedConfig>("testconfig.xml");
            migrator.Migrate(testConfig.Version, Arg.Any<Stream>()).Returns(testConfig);
            TestVersionedConfig testConfigActual = TestVersionedConfig.LoadConfig<TestVersionedConfig>("testconfig.xml", migrator);
            migrator.Received().Migrate(testConfig.Version, Arg.Any<Stream>());
            Assert.AreEqual(testConfig.Version, testConfigActual.Version, "Returned config is not the same as a new config");
        }

        [Test]
        public void LoadNonExistingVersionedConfig()
        {
            IConfigMigrator<TestVersionedConfig> migrator = Substitute.For<IConfigMigrator<TestVersionedConfig>>();
            TestVersionedConfig testConfig = new TestVersionedConfig();
            TestVersionedConfig testConfigActual = null;
            Assert.DoesNotThrow(() => testConfigActual = TestVersionedConfig.LoadConfig<TestVersionedConfig>("testnonexistingconfig.xml", migrator), "Loading non existing config should not throw exceptions, but return a new config instead");
            Assert.AreEqual(testConfig.Version, testConfigActual.Version, "Returned config is not the same as a new config");
        }

        [Test]
        public void LoadVersionedConfigSubDirectory()
        {
            string path = Path.Combine("testconfig", "testconfig.xml");
            IConfigMigrator<TestVersionedConfig> migrator = Substitute.For<IConfigMigrator<TestVersionedConfig>>();
            TestVersionedConfig testConfig = this.CreateConfig<TestVersionedConfig>(path);
            migrator.Migrate(testConfig.Version, Arg.Any<Stream>()).Returns(testConfig);
            TestVersionedConfig testConfigActual = TestVersionedConfig.LoadConfig<TestVersionedConfig>(path, migrator);
            migrator.Received().Migrate(testConfig.Version, Arg.Any<Stream>());
            Assert.AreEqual(testConfig.Version, testConfigActual.Version, "Returned config is not the same as a new config");
        }
    }
}
