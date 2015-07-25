using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalFramework.Plugins;
using ColossalFramework.Steamworks;
using CommonShared.Proxies.Plugins;
using CommonShared.Utils;
using ICities;
using NSubstitute;
using NUnit.Framework;

namespace CommonShared.UnitTests.Utils
{
    [TestFixture]
    public class PluginUtilsTest
    {
        private IPluginManagerInteractor pluginManagerInteractor;

        [SetUp]
        public void Init()
        {
            this.pluginManagerInteractor = Substitute.For<IPluginManagerInteractor>();
            PluginUtils.PluginManagerInteractor = pluginManagerInteractor;
        }

        [TearDown]
        public virtual void Dispose()
        {
            PluginUtils.CleanUp();
        }

        [Test]
        public void GetPluginInfo()
        {
            IUserMod mod = Substitute.For<IUserMod>();
            IPluginInfoInteractor pluginInfoInteractor = Substitute.For<IPluginInfoInteractor>();
            this.pluginManagerInteractor.GetPluginsInfo().Returns(new[] { pluginInfoInteractor });

            Assembly assembly = this.GetType().Assembly;
            pluginInfoInteractor.Assemblies.Returns(new[] { assembly });
            pluginInfoInteractor.UserModInstance.Returns(mod);

            IPluginInfoInteractor pluginInfo = PluginUtils.GetPluginInfo(mod);
            Assert.AreEqual(pluginInfoInteractor, pluginInfo);
        }

        private HashSet<ulong>[] GetPluginInfosOfData = new HashSet<ulong>[]
        {
            new HashSet<ulong>() { 1 },
            new HashSet<ulong>() { 1, 5 },
            new HashSet<ulong>(),
            null
        };

        [Test, TestCaseSource("GetPluginInfosOfData")]
        public void GetPluginInfosOf(HashSet<ulong> ids)
        {
            var expected = new Dictionary<ulong, IPluginInfoInteractor>();
            if (ids != null)
            {
                List<IPluginInfoInteractor> pluginInfoInteractors = new List<IPluginInfoInteractor>();
                foreach (var id in ids)
                {
                    IPluginInfoInteractor pluginInfoInteractor = Substitute.For<IPluginInfoInteractor>();
                    pluginInfoInteractor.PublishedFileID.Returns(new PublishedFileId(id));
                    pluginInfoInteractors.Add(pluginInfoInteractor);
                    expected.Add(id, pluginInfoInteractor);
                }
                this.pluginManagerInteractor.GetPluginsInfo().Returns(pluginInfoInteractors);
            }

            var actual = PluginUtils.GetPluginInfosOf(ids);
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [Test]
        public void SubscribePluginStateChangeIPluginInfoInteractor()
        {
            IPluginInfoInteractor pluginInfoInteractor = Substitute.For<IPluginInfoInteractor>();
            this.pluginManagerInteractor.GetPluginsInfo().Returns(new[] { pluginInfoInteractor });

            bool? enabled = null;
            pluginInfoInteractor.IsEnabled.Returns(false);
            PluginUtils.SubscribePluginStateChange(pluginInfoInteractor, isEnabled => enabled = isEnabled);
            pluginInfoInteractor.IsEnabled.Returns(true);
            this.pluginManagerInteractor.OnPluginsStateChanged += Raise.Event<PluginManager.PluginsChangedHandler>();
            Assert.NotNull(enabled, "No callback");
            Assert.IsTrue(enabled.Value, "Plugin isEnabled did not evaluate to true");
        }

        [Test]
        public void SubscribePluginStateChangeIUserMod()
        {
            IUserMod mod = Substitute.For<IUserMod>();
            IPluginInfoInteractor pluginInfoInteractor = Substitute.For<IPluginInfoInteractor>();
            pluginInfoInteractor.UserModInstance.Returns(mod);
            this.pluginManagerInteractor.GetPluginsInfo().Returns(new[] { pluginInfoInteractor });

            bool? enabled = null;
            pluginInfoInteractor.IsEnabled.Returns(false);
            PluginUtils.SubscribePluginStateChange(mod, isEnabled => enabled = isEnabled);
            pluginInfoInteractor.IsEnabled.Returns(true);
            this.pluginManagerInteractor.OnPluginsStateChanged += Raise.Event<PluginManager.PluginsChangedHandler>();
            Assert.NotNull(enabled, "No callback");
            Assert.IsTrue(enabled.Value, "Plugin isEnabled did not evaluate to true");
        }

        [Test]
        public void UnsubscribePluginStateChangeIPluginInfoInteractor()
        {
            IPluginInfoInteractor pluginInfoInteractor = Substitute.For<IPluginInfoInteractor>();
            this.pluginManagerInteractor.GetPluginsInfo().Returns(new[] { pluginInfoInteractor });

            bool? enabled = null;
            Action<bool> callback = isEnabled => enabled = isEnabled;
            pluginInfoInteractor.IsEnabled.Returns(false);
            PluginUtils.SubscribePluginStateChange(pluginInfoInteractor, callback);
            pluginInfoInteractor.IsEnabled.Returns(true);
            PluginUtils.UnsubscribePluginStateChange(pluginInfoInteractor, callback);
            this.pluginManagerInteractor.OnPluginsStateChanged += Raise.Event<PluginManager.PluginsChangedHandler>();
            Assert.IsNull(enabled, "Callback or plugin isEnabled did evaluate to true");
        }

        [Test]
        public void UnsubscribePluginStateChangeIUserMod()
        {
            IUserMod mod = Substitute.For<IUserMod>();
            IPluginInfoInteractor pluginInfoInteractor = Substitute.For<IPluginInfoInteractor>();
            pluginInfoInteractor.UserModInstance.Returns(mod);
            this.pluginManagerInteractor.GetPluginsInfo().Returns(new[] { pluginInfoInteractor });

            bool? enabled = null;
            Action<bool> callback = isEnabled => enabled = isEnabled;
            pluginInfoInteractor.IsEnabled.Returns(false);
            PluginUtils.SubscribePluginStateChange(pluginInfoInteractor, callback);
            pluginInfoInteractor.IsEnabled.Returns(true);
            PluginUtils.UnsubscribePluginStateChange(pluginInfoInteractor, callback);
            this.pluginManagerInteractor.OnPluginsStateChanged += Raise.Event<PluginManager.PluginsChangedHandler>();
            Assert.IsNull(enabled, "Callback or plugin isEnabled did evaluate to true");
        }
    }
}
