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

        [TestCase("testconfig.xml")]
        [TestCase("testconfig.yml")]
        public void LoadVersionedConfig(string filename)
        {
            IConfigMigrator<TestVersionedConfig> migrator = Substitute.For<IConfigMigrator<TestVersionedConfig>>();
            TestVersionedConfig testConfig = this.CreateConfig<TestVersionedConfig>(filename);
            TestVersionedConfig testConfigActual = null;
            switch (Path.GetExtension(filename))
            {
                case ".xml":
                    migrator.MigrateFromXml(testConfig.Version, Arg.Any<Stream>()).Returns(testConfig);
                    testConfigActual = TestVersionedConfig.LoadConfig<TestVersionedConfig>(filename, migrator);
                    migrator.Received().MigrateFromXml(testConfig.Version, Arg.Any<Stream>());
                    break;
                case ".yml":
                    migrator.MigrateFromYaml(testConfig.Version, Arg.Any<Stream>()).Returns(testConfig);
                    testConfigActual = TestVersionedConfig.LoadConfig<TestVersionedConfig>(filename, migrator);
                    migrator.Received().MigrateFromYaml(testConfig.Version, Arg.Any<Stream>());
                    break;
                default:
                    throw new Exception("Not an XML or YAML file");
            }
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

        [TestCase("testconfig.xml")]
        [TestCase("testconfig.yml")]
        public void LoadVersionedConfigSubDirectory(string filename)
        {
            string path = Path.Combine("testconfig", filename);
            IConfigMigrator<TestVersionedConfig> migrator = Substitute.For<IConfigMigrator<TestVersionedConfig>>();
            TestVersionedConfig testConfig = this.CreateConfig<TestVersionedConfig>(path);
            TestVersionedConfig testConfigActual = null;
            switch (Path.GetExtension(filename))
            {
                case ".xml":
                    migrator.MigrateFromXml(testConfig.Version, Arg.Any<Stream>()).Returns(testConfig);
                    testConfigActual = TestVersionedConfig.LoadConfig<TestVersionedConfig>(path, migrator);
                    migrator.Received().MigrateFromXml(testConfig.Version, Arg.Any<Stream>());
                    break;
                case ".yml":
                    migrator.MigrateFromYaml(testConfig.Version, Arg.Any<Stream>()).Returns(testConfig);
                    testConfigActual = TestVersionedConfig.LoadConfig<TestVersionedConfig>(path, migrator);
                    migrator.Received().MigrateFromYaml(testConfig.Version, Arg.Any<Stream>());
                    break;
                default:
                    throw new Exception("Not an XML or YAML file");
            }
            Assert.AreEqual(testConfig.Version, testConfigActual.Version, "Returned config is not the same as a new config");
        }
    }
}
