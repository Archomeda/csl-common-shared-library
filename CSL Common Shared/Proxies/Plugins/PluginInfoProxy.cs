using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalFramework.Plugins;
using ColossalFramework.Steamworks;
using CommonShared.Utils;

namespace CommonShared.Proxies.Plugins
{
    public class PluginInfoProxy : IPluginInfoInteractor
    {
        public PluginManager.PluginInfo OriginalPluginInfo { get; private set; }

        public PluginInfoProxy(PluginManager.PluginInfo originalPluginInfo)
        {
            this.OriginalPluginInfo = originalPluginInfo;
        }


        private IList<Assembly> assemblies;
        public IList<Assembly> Assemblies
        {
            get
            {
                if (this.assemblies == null)
                    this.assemblies = ReflectionUtils.GetPrivateField<List<Assembly>>(this.OriginalPluginInfo, "m_Assemblies");
                return this.assemblies;
            }
        }

        public string AssembliesString
        {
            get { return this.OriginalPluginInfo.assembliesString; }
        }

        public int AssemblyCount
        {
            get { return this.OriginalPluginInfo.assemblyCount; }
        }

        public bool IsBuiltIn
        {
            get { return this.OriginalPluginInfo.isBuiltin; }
        }

        public bool IsEnabled
        {
            get { return this.OriginalPluginInfo.isEnabled; }
        }

        public string ModPath
        {
            get { return this.OriginalPluginInfo.modPath; }
        }

        public string Name
        {
            get { return this.OriginalPluginInfo.name; }
        }

        public PublishedFileId PublishedFileID
        {
            get { return this.OriginalPluginInfo.publishedFileID; }
        }

        public object UserModInstance
        {
            get { return this.OriginalPluginInfo.userModInstance; }
        }


        public void AddAssembly(Assembly asm)
        {
            this.OriginalPluginInfo.AddAssembly(asm);
        }

        public T[] GetInstances<T>() where T : class
        {
            return this.OriginalPluginInfo.GetInstances<T>();
        }

        public override string ToString()
        {
            return this.OriginalPluginInfo.ToString();
        }

        public void Unload()
        {
            this.OriginalPluginInfo.Unload();
        }
    }
}
