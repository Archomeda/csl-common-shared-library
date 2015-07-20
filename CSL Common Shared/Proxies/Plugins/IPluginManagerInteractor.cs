using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.Plugins;

namespace CommonShared.Proxies.Plugins
{
    public interface IPluginManagerInteractor
    {
        int EnabledModCount { get; }
        int ModCount { get; }

        event PluginManager.PluginsChangedHandler OnPluginsChanged;
        event PluginManager.PluginsChangedHandler OnPluginsStateChanged;

        List<T> GetImplementations<T>() where T : class;
        IEnumerable<IPluginInfoInteractor> GetPluginsInfo();
        void LoadPlugins();
    }
}
