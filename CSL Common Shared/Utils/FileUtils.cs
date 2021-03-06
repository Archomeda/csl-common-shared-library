﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using ColossalFramework.IO;
using CommonShared.Proxies.IO;
using ICities;

namespace CommonShared.Utils
{
    /// <summary>
    /// Contains various utilities regarding files.
    /// </summary>
    public static class FileUtils
    {
        private static IDataLocationInteractor dataLocationInteractor = DataLocationProxy.instance;
        /// <summary>
        /// Gets or sets the data location interactor that will be used.
        /// By default this is <see cref="DataLocationProxy"/>.
        /// </summary>
        public static IDataLocationInteractor DataLocationInteractor
        {
            get { return dataLocationInteractor; }
            set { dataLocationInteractor = value; }
        }

        /// <summary>
        /// Gets the assembly folder of the calling mod. This is where the DLL and every other static file are located.
        /// </summary>
        /// <param name="modInstance">The mod instance.</param>
        /// <returns>The assembly folder.</returns>
        public static string GetAssemblyFolder(IUserMod modInstance)
        {
            var pluginInfo = PluginUtils.GetPluginInfo(modInstance);
            return pluginInfo != null ? pluginInfo.ModPath : null;
        }

        /// <summary>
        /// Gets the storage folder of the calling mod. This is where dynamic files are located.
        /// </summary>
        /// <param name="modInstance">The mod instance.</param>
        /// <returns>The storage folder.</returns>
        public static string GetStorageFolder(IUserMod modInstance)
        {
            Assembly assembly = modInstance.GetType().Assembly;
            return Path.Combine(DataLocationInteractor.ModsPath, assembly.GetName().Name);
        }
    }
}
