﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.Steamworks;
using CommonShared.Proxies.Plugins;
using CommonShared.Utils;
using ICities;

namespace CommonShared
{
    /// <summary>
    /// Provides basic implementation for a mod.
    /// </summary>
    /// <typeparam name="T">The type of the inherited class of this abstract class.</typeparam>
    public abstract class UserModBase<T> : LoadingExtensionBase, IUserMod where T : IUserMod
    {
        private bool initialized;
        private bool isValid;

        /// <summary>
        /// Gets the workshop identifier. If set to 0, the mod validity check will be disabled.
        /// If you want to republish the mod, you have to change this value to the new workshop ID and recompile it.
        /// But please try to improve it first by submitting pull requests on GitHub instead.
        /// </summary>
        /// <value>
        /// The workshop identifier.
        /// </value>
        protected virtual ulong WorkshopId { get { return 0; } }

        /// <summary>
        /// Gets the list of incompatible mods.
        /// Upon loading these mods will be checked if they are installed and enabled.
        /// If at least one is, a warning will be shown to the user.
        /// </summary>
        /// <value>
        /// The list of incompatible mods.
        /// </value>
        protected virtual IEnumerable<ulong> IncompatibleMods { get { return new HashSet<ulong>(); } }


        /// <summary>
        /// Gets the instance of this mod.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static T Instance { get; private set; }

        /// <summary>
        /// Gets the plugin information.
        /// </summary>
        /// <value>
        /// The plugin information.
        /// </value>
        public IPluginInfoInteractor PluginInfo { get; private set; }

        /// <summary>
        /// Gets the logger for this mod.
        /// </summary>
        /// <value>
        /// The log.
        /// </value>
        public Logger Log { get; private set; }


        /// <summary>
        /// Gets the name of the mod.
        /// </summary>
        /// <value>
        /// The name of the mod.
        /// </value>
        public abstract string ModName { get; }

        /// <summary>
        /// Gets the mod description.
        /// </summary>
        /// <value>
        /// The mod description.
        /// </value>
        public abstract string ModDescription { get; }


        private void Initialize()
        {
            this.Log = new Logger(this.GetType().Assembly);
            this.PluginInfo = PluginUtils.GetPluginInfo(this);
            UserModBase<T>.Instance = (T)(object)this; // Make the compiler shut up about non-existing conversions

            this.isValid = this.CheckValidity();
            if (!this.isValid)
                return;

            this.CheckIncompatibility();

            PluginUtils.SubscribePluginStateChange(this, enabled =>
            {
                if (isValid)
                {
                    if (enabled)
                        this.OnModActivated();
                    else
                        this.OnModDeactivated();
                }
            });

            this.OnModInitializing(this.PluginInfo.IsEnabled);
        }

        private bool CheckIncompatibility()
        {
            var list = PluginUtils.GetPluginInfosOf(this.IncompatibleMods);
            if (list.Count > 0)
            {
                string text = string.Join(", ",
                    list.Where(kvp => kvp.Value.IsEnabled)
                        .Select(kvp => string.Format("{0} ({1})", kvp.Value.GetInstances<IUserMod>()[0].Name, kvp.Value.PublishedFileID.ToString()))
                        .OrderBy(s => s)
                        .ToArray()
                    );

                if (!string.IsNullOrEmpty(text))
                    this.Log.Warning("You've got some known incompatible mods installed and enabled! It's possible that this mod doesn't work as expected.\nThe following incompatible mods are enabled: {0}.", text);
                return true;
            }
            return false;
        }

        private bool CheckValidity()
        {
            var pluginInfo = PluginUtils.GetPluginInfo(this);
            if (this.WorkshopId > 0 && pluginInfo.PublishedFileID != PublishedFileId.invalid && pluginInfo.PublishedFileID.AsUInt64 != this.WorkshopId)
            {
                // The mod is not published officially
                this.Log.Error("YOU ARE CURRENTLY USING AN UNAUTHORIZED PUBLICATION OF THE MOD '{0}' WITH WORKSHOP ID {1}.\r\n" +
                    "Please use the original version that can be found at http://steamcommunity.com/sharedfiles/filedetails/?id={2}.\r\n\r\n" +
                    "This version will not be loaded. Don't forget to report this version on the original workshop item page as it's most likely stolen (it has happened before).",
                    this.ModName.ToUpper(), pluginInfo.PublishedFileID.AsUInt64, this.WorkshopId);
                return false;
            }
            return true;
        }


        #region IUserMod members

        /// <summary>
        /// Gets the name of the mod.
        /// </summary>
        /// <value>
        /// The name of the mod.
        /// </value>
        public string Name
        {
            get
            {
                // Hacky way to load on main menu here, but it will have to do
                if (!this.initialized)
                {
                    this.Initialize();
                    this.initialized = true;
                }
                if (this.isValid && !LoadingManager.instance.m_loadingComplete)
                    this.OnMainMenuLoading();
                return this.ModName;
            }
        }

        /// <summary>
        /// Gets the mod description.
        /// </summary>
        /// <value>
        /// The mod description.
        /// </value>
        public string Description { get { return this.ModDescription; } }

        #endregion


        #region LoadingExtensionBase members

        /// <summary>
        /// Called when the mod instance is created.
        /// </summary>
        /// <param name="loading">The loading instance.</param>
        public sealed override void OnCreated(ILoading loading)
        {
            base.OnCreated(loading);
            if (this.isValid)
                this.OnModCreated(loading);
        }

        /// <summary>
        /// Called when the mod instance is released.
        /// </summary>
        public sealed override void OnReleased()
        {
            base.OnReleased();
            if (this.isValid)
                this.OnModReleased();
        }

        /// <summary>
        /// Called when the game has loaded a level.
        /// </summary>
        /// <param name="mode">The game mode.</param>
        public sealed override void OnLevelLoaded(LoadMode mode)
        {
            base.OnLevelLoaded(mode);
            if (this.isValid)
            {
                this.CheckIncompatibility();
                this.OnGameLoaded(mode);
            }
        }

        /// <summary>
        /// Called when the game has started unloading a level.
        /// </summary>
        public sealed override void OnLevelUnloading()
        {
            base.OnLevelUnloading();
            if (this.isValid)
                this.OnGameUnloading();
        }

        #endregion


        /// <summary>
        /// Called when this mod is being initialized.
        /// </summary>
        /// <param name="enabled">This value is <c>true</c> if the mod is enabled; false otherwise.</param>
        public virtual void OnModInitializing(bool enabled) { }

        /// <summary>
        /// Called when this mod is activated through the mod panel.
        /// </summary>
        public virtual void OnModActivated() { }

        /// <summary>
        /// Called when this mod is deactivated through the mod panel.
        /// </summary>
        public virtual void OnModDeactivated() { }

        /// <summary>
        /// Called when the game has started loading the main menu.
        /// </summary>
        public virtual void OnMainMenuLoading() { }

        /// <summary>
        /// Called when the mod instance is created.
        /// </summary>
        /// <param name="loading">The loading instance.</param>
        public virtual void OnModCreated(ILoading loading) { }

        /// <summary>
        /// Called when the mod instance is released.
        /// </summary>
        public virtual void OnModReleased() { }

        /// <summary>
        /// Called when the game has loaded a level.
        /// </summary>
        /// <param name="mode">The game mode.</param>
        public virtual void OnGameLoaded(LoadMode mode) { }

        /// <summary>
        /// Called when the game has started unloading a level.
        /// </summary>
        public virtual void OnGameUnloading() { }
    }
}
