using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonShared.Proxies.IO
{
    public interface IDataLocationInteractor
    {
        string CompanyName { get; }
        uint ProductVersion { get; }
        string ProductVersionString { get; }
        string ProductName { get; }
        string ApplicationBase { get; }
        string GameContentPath { get; }
        string AddonsPath { get; }
        string ModsPath { get; }
        string AssetsPath { get; }
        string CurrentDirectory { get; }
        string AssemblyDirectory { get; }
        string ExecutableDirectory { get; }
        string TempFolder { get; }
        string LocalApplicationData { get; }
        string SaveLocation { get; }
        string MapLocation { get; }
    }
}
