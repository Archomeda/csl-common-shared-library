using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CommonShared.Proxies.IO;
using CommonShared.Proxies.Plugins;
using CommonShared.Utils;
using ICities;
using NSubstitute;
using NUnit.Framework;

namespace CommonShared.UnitTests.Utils
{
    [TestFixture]
    public class FileUtilsTest
    {
        private IPluginManagerInteractor pluginManagerInteractor;
        private IDataLocationInteractor dataLocationInteractor;

        [SetUp]
        public virtual void Init()
        {
            this.pluginManagerInteractor = Substitute.For<IPluginManagerInteractor>();
            PluginUtils.PluginManagerInteractor = this.pluginManagerInteractor;

            this.dataLocationInteractor = Substitute.For<IDataLocationInteractor>();
            FileUtils.DataLocationInteractor = this.dataLocationInteractor;
        }

        [TearDown]
        public virtual void Dispose()
        {
            PluginUtils.Cleanup();
        }

        [Test]
        public void GetAssemblyFolder()
        {
            IUserMod mod = Substitute.For<IUserMod>();
            IPluginInfoInteractor pluginInfoInteractor = Substitute.For<IPluginInfoInteractor>();
            this.pluginManagerInteractor.GetPluginsInfo().Returns(new[] { pluginInfoInteractor });

            pluginInfoInteractor.UserModInstance.Returns(mod);
            pluginInfoInteractor.ModPath.Returns("some/random.path");
            Assert.AreEqual("some/random.path", FileUtils.GetAssemblyFolder(mod));
        }

        [Test]
        public void GetStorageFolderRelative()
        {
            IUserMod mod = Substitute.For<IUserMod>();
            Assembly assembly = mod.GetType().Assembly;

            this.dataLocationInteractor.ModsPath.Returns("some/random/path");
            Assert.AreEqual(Path.Combine("some/random/path", assembly.GetName().Name), FileUtils.GetStorageFolder(mod));
        }

    }
}
