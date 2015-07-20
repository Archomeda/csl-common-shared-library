using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ColossalFramework.IO;

namespace CommonShared.Proxies.IO
{
    public class DataLocationProxy : LightSingleton<DataLocationProxy>, IDataLocationInteractor
    {
        public string CompanyName
        {
            get { return DataLocation.companyName; }
        }

        public uint ProductVersion
        {
            get { return DataLocation.productVersion; }
        }

        public string ProductVersionString
        {
            get { return DataLocation.productVersionString; }
        }

        public string ProductName
        {
            get { return DataLocation.productName; }
        }

        public string ApplicationBase
        {
            get { return DataLocation.applicationBase; }
        }

        public string GameContentPath
        {
            get { return DataLocation.gameContentPath; }
        }

        public string AddonsPath
        {
            get { return DataLocation.addonsPath; }
        }

        public string ModsPath
        {
            get { return DataLocation.modsPath; }
        }

        public string AssetsPath
        {
            get { return DataLocation.assetsPath; }
        }

        public string CurrentDirectory
        {
            get { return DataLocation.currentDirectory; }
        }

        public string AssemblyDirectory
        {
            get { return DataLocation.assemblyDirectory; }
        }

        public string ExecutableDirectory
        {
            get { return DataLocation.executableDirectory; }
        }

        public string TempFolder
        {
            get { return DataLocation.tempFolder; }
        }

        public string LocalApplicationData
        {
            get { return DataLocation.localApplicationData; }
        }

        public string SaveLocation
        {
            get { return DataLocation.saveLocation; }
        }

        public string MapLocation
        {
            get { return DataLocation.mapLocation; }
        }
    }
}
