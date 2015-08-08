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
            if (File.Exists("testconfig.yml"))
                File.Delete("testconfig.yml");
            if (Directory.Exists("testconfig"))
                Directory.Delete("testconfig", true);
        }

        protected virtual T CreateConfig<T>(string filename) where T : Config, new()
        {
            T config = new T();
            config.SaveConfig(filename);
            return (T)config;
        }

        [TestCase("testconfig.xml")]
        [TestCase("testconfig.yml")]
        public void LoadConfig(string filename)
        {
            TestConfig testConfig = this.CreateConfig<TestConfig>(filename);
            TestConfig testConfigActual = TestConfig.LoadConfig<TestConfig>(filename);
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

        [TestCase("testconfig.xml")]
        [TestCase("testconfig.yml")]
        public void SaveConfig(string filename)
        {
            Assert.DoesNotThrow(() => this.CreateConfig<TestConfig>(filename), "Should succeed properly when saving config");
            Assert.IsTrue(File.Exists(filename), "Config file should exist");
        }

        [TestCase("testconfig.xml")]
        [TestCase("testconfig.yml")]
        public void LoadConfigSubDirectory(string filename)
        {
            string path = Path.Combine("testconfig", filename);
            TestConfig testConfig = this.CreateConfig<TestConfig>(path);
            TestConfig testConfigActual = TestConfig.LoadConfig<TestConfig>(path);
            Assert.AreEqual(testConfig.SomeString, testConfigActual.SomeString);
        }

        [TestCase("testconfig.xml")]
        [TestCase("testconfig.yml")]
        public void SaveConfigSubDirectory(string filename)
        {
            string path = Path.Combine("testconfig", filename);
            Assert.DoesNotThrow(() => this.CreateConfig<TestConfig>(path), "Should succeed properly when saving config");
            Assert.IsTrue(File.Exists(path), "Config file should exist");
        }
    }
}
