﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.Steamworks;

namespace CommonShared.Utils
{
    /// <summary>
    /// Contains various utilities regarding DLCs.
    /// </summary>
    public static class DlcUtils
    {
        /// <summary>
        /// An enumerator that contains all available DLCs.
        /// </summary>
        [Flags]
        public enum Dlc
        {
            /// <summary>
            /// No DLC.
            /// </summary>
            None = 0,

            /// <summary>
            /// After Dark DLC.
            /// </summary>
            AfterDark = 1
        }

        /// <summary>
        /// Gets the installed DLCs.
        /// </summary>
        /// <value>
        /// The installed DLCs.
        /// </value>
        public static Dlc InstalledDlcs
        {
            get
            {
                Dlc dlcs = Dlc.None;
                if (IsAfterDarkInstalled) dlcs |= Dlc.AfterDark;
                return dlcs;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the After Dark DLC is installed.
        /// </summary>
        /// <value>
        /// <c>true</c> if After Dark is installed; otherwise, <c>false</c>.
        /// </value>
        public static bool IsAfterDarkInstalled { get { return Steam.IsDlcInstalled(369150); } }
    }
}
