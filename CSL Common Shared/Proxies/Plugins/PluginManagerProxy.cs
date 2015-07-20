using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.Plugins;

namespace CommonShared.Proxies.Plugins
{
    public class PluginManagerProxy : LightSingleton<PluginManagerProxy>, IPluginManagerInteractor
    {
        public int EnabledModCount
        {
            get { return PluginManager.instance.enabledModCount; }
        }

        public int ModCount
        {
            get { return PluginManager.instance.modCount; }
        }


        public event PluginManager.PluginsChangedHandler OnPluginsChanged
        {
            add
            {
                PluginManager.instance.eventPluginsChanged += value;
            }
            remove
            {
                PluginManager.instance.eventPluginsChanged -= value;
            }
        }

        public event PluginManager.PluginsChangedHandler OnPluginsStateChanged
        {
            add
            {
                PluginManager.instance.eventPluginsStateChanged += value;
            }
            remove
            {
                PluginManager.instance.eventPluginsStateChanged -= value;
            }
        }


        public List<T> GetImplementations<T>() where T : class
        {
            return PluginManager.instance.GetImplementations<T>();
        }

        public IEnumerable<IPluginInfoInteractor> GetPluginsInfo()
        {
            return PluginManager.instance.GetPluginsInfo().Select(i => (IPluginInfoInteractor)new PluginInfoProxy(i));
        }

        public void LoadPlugins()
        {
            PluginManager.instance.LoadPlugins();
        }
    }
}
