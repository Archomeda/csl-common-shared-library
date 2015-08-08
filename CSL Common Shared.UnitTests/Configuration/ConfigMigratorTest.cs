using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CommonShared.Configuration;
using NUnit.Framework;

namespace CommonShared.UnitTests.Configuration
{
    [TestFixture]
    public class ConfigMigratorTest
    {
        public class TestVersionedConfigV0 : VersionedConfig
        {
            public string SomeString { get; set; }

            public TestVersionedConfigV0()
            {
                this.Version = 0;
            }
        }

        public class TestVersionedConfigV1 : VersionedConfig
        {
            public string SomeString2 { get; set; }

            public TestVersionedConfigV1()
            {
                this.Version = 1;
            }
        }

        public class TestConfigMigrator : ConfigMigratorBase<TestVersionedConfigV1>
        {
            public TestConfigMigrator()
            {
                this.MigrationMethods = new Dictionary<uint, Func<object, object>>()
                {
                    { 0, this.MigrateFromVersion0 }
                };

                this.VersionTypes = new Dictionary<uint, Type>()
                {
                    { 0, typeof(TestVersionedConfigV0) }
                };
            }

            protected object MigrateFromVersion0(object oldConfig)
            {
                TestVersionedConfigV0 config = (TestVersionedConfigV0)oldConfig;
                TestVersionedConfigV1 newConfig = new TestVersionedConfigV1();
                newConfig.SomeString2 = config.SomeString;
                return newConfig;
            }
        }

        [TearDown]
        public virtual void Dispose()
        {
            if (File.Exists("testconfig.xml"))
                File.Delete("testconfig.xml");
            if (File.Exists("testconfig.yml"))
                File.Delete("testconfig.yml");
        }

        [TestCase("testconfig.xml")]
        [TestCase("testconfig.yml")]
        public void MigrateFromOlderVersion(string filename)
        {
            TestVersionedConfigV0 oldConfig = new TestVersionedConfigV0();
            oldConfig.SomeString = "SomeTestString";
            oldConfig.SaveConfig(filename);

            using (var stream = File.OpenRead(filename))
            {
                TestConfigMigrator migrator = new TestConfigMigrator();
                TestVersionedConfigV1 newConfig = null;
                switch (Path.GetExtension(filename))
                {
                    case ".xml":
                        newConfig = migrator.MigrateFromXml(oldConfig.Version, stream);
                        break;
                    case ".yml":
                        newConfig = migrator.MigrateFromYaml(oldConfig.Version, stream);
                        break;
                    default:
                        throw new Exception("Not an XML or YAML file");
                }
                Assert.AreEqual(oldConfig.SomeString, newConfig.SomeString2);
            }
        }

        [TestCase("testconfig.xml")]
        [TestCase("testconfig.yml")]
        public void MigrateFromSameVersion(string filename)
        {
            TestVersionedConfigV1 oldConfig = new TestVersionedConfigV1();
            oldConfig.SomeString2 = "SomeTestString";
            oldConfig.SaveConfig(filename);

            using (var stream = File.OpenRead(filename))
            {
                TestConfigMigrator migrator = new TestConfigMigrator();
                TestVersionedConfigV1 newConfig = null;
                switch (Path.GetExtension(filename))
                {
                    case ".xml":
                        newConfig = migrator.MigrateFromXml(oldConfig.Version, stream);
                        break;
                    case ".yml":
                        newConfig = migrator.MigrateFromYaml(oldConfig.Version, stream);
                        break;
                    default:
                        throw new Exception("Not an XML or YAML file");
                }
                Assert.AreEqual(oldConfig.SomeString2, newConfig.SomeString2);
            }
        }

        [TestCase("testconfig.xml")]
        [TestCase("testconfig.yml")]
        public void MigrateFromNewerVersion(string filename)
        {
            // Migrating from a newer version is not supported
            // The migrator should just deserialize the configuration as the latest version available instead

            TestVersionedConfigV1 oldConfig = new TestVersionedConfigV1();
            oldConfig.SomeString2 = "SomeTestString";
            oldConfig.Version = 2; // Force a 'newer' version
            oldConfig.SaveConfig(filename);

            using (var stream = File.OpenRead(filename))
            {
                TestConfigMigrator migrator = new TestConfigMigrator();
                TestVersionedConfigV1 newConfig = null;
                switch (Path.GetExtension(filename))
                {
                    case ".xml":
                        newConfig = migrator.MigrateFromXml(oldConfig.Version, stream);
                        break;
                    case ".yml":
                        newConfig = migrator.MigrateFromYaml(oldConfig.Version, stream);
                        break;
                    default:
                        throw new Exception("Not an XML or YAML file");
                }
                Assert.AreEqual(oldConfig.SomeString2, newConfig.SomeString2);
            }
        }


    }
}
