using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalFramework.Steamworks;

namespace CommonShared.Proxies.Plugins
{
    public interface IPluginInfoInteractor
    {
        IList<Assembly> Assemblies { get; }
        string AssembliesString { get; }
        int AssemblyCount { get; }
        bool IsBuiltIn { get; }
        bool IsEnabled { get; }
        string ModPath { get; }
        string Name { get; }
        PublishedFileId PublishedFileID { get; }
        object UserModInstance { get; }

        void AddAssembly(Assembly asm);
        T[] GetInstances<T>() where T : class;
        string ToString();
        void Unload();
    }
}
