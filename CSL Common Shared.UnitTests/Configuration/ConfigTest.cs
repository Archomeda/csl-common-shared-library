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
    public class ConfigTest
    {
        public class TestConfig : Config
        {
            public TestConfig()
            {
                this.SomeString = "SomeString";
            }

            public string SomeString { get; set; }
        }

        [TearDown]
        public virtual void Dispose()
        {
            if (File.Exists("testconfig.xml"))
                File.Delete("testconfig.xml");
            if (Directory.Exists("testconfig"))
                Directory.Delete("testconfig", true);
        }

        protected virtual T CreateConfig<T>(string filename) where T : Config, new()
        {
            T config = new T();
            config.SaveConfig(filename);
            return (T)config;
        }

        [Test]
        public void LoadConfig()
        {
            TestConfig testConfig = this.CreateConfig<TestConfig>("testconfig.xml");
            TestConfig testConfigActual = TestConfig.LoadConfig<TestConfig>("testconfig.xml");
            Assert.AreEqual(testConfig.SomeString, testConfigActual.SomeString);
        }

        [Test]
        public void LoadNonExistingConfig()
        {
            TestConfig testConfig = new TestConfig();
            TestConfig testConfigActual = null;
            Assert.DoesNotThrow(() => testConfigActual = TestConfig.LoadConfig<TestConfig>("testnonexistingconfig.xml"), "Loading non existing config should not throw exceptions, but return a new config instead");
            Assert.AreEqual(testConfig.SomeString, testConfigActual.SomeString, "Returned config is not the same as a new config");
        }

        [Test]
        public void SaveConfig()
        {
            Assert.DoesNotThrow(() => this.CreateConfig<TestConfig>("testconfig.xml"), "Should succeed properly when saving config");
            Assert.IsTrue(File.Exists("testconfig.xml"), "Config file should exist");
        }

        [Test]
        public void LoadConfigSubDirectory()
        {
            string path = Path.Combine("testconfig", "testconfig.xml");
            TestConfig testConfig = this.CreateConfig<TestConfig>(path);
            TestConfig testConfigActual = TestConfig.LoadConfig<TestConfig>(path);
            Assert.AreEqual(testConfig.SomeString, testConfigActual.SomeString);
        }

        [Test]
        public void SaveConfigSubDirectory()
        {
            string path = Path.Combine("testconfig", "testconfig.xml");
            Assert.DoesNotThrow(() => this.CreateConfig<TestConfig>(path), "Should succeed properly when saving config");
            Assert.IsTrue(File.Exists(path), "Config file should exist");
        }
    }
}
