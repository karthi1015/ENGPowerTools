using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGPowerTools.Helpers.Constants
{
    public static class RootConstants
    {
        //The assembly name of the Updater exicutable
        public const string Updater_exe_Name = "ENGPowerToolsUpdater.exe";
        // The local Root Directory Name that stores application files
        public const string RootDirectoryName = "ENGPowerTools";
        //file name of given to the persistent data file created by the Manager
        public const string UpdaterPersitantDataFileName = "ENGPowerToolsAppData";
        // The network drive path to were the updated application files are store
        // This location marks were the released versions of the files are pushed 
        public const string UpdateRepositoryPath = "S:\\Engineering\\Engineering Tools &"
                + " Spreadsheets\\ENG -Add-ins\\Application Files";
    }
}
