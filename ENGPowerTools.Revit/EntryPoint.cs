using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.UI;
using ENGPowerTools.Helpers.Interfaces;
using ENGPowerTools.ProjectSetupApp;



namespace ENGPowerTools.Revit
{
    //*********DO NOT WRITE CODE IN THIS NAMESPACE*************************************

    /// <summary>
    /// Serves as the entry point that all revit versions.
        /// </summary>
    public class EntryPoint : IExternalApplication
    {
        public static UIControlledApplication UIApp { get; private set; }
        public static List<IExternalApplicationHelper> AppHelpers { get; set; }

        /// <summary>
        /// Initializes client side applications
        /// </summary>
        public EntryPoint()
        {
            //impliments all applications 
            AppHelpers = new List<IExternalApplicationHelper>();
            IExternalApplicationHelper EngineeringLightingToolsApp =
                new AppMain();
            AppHelpers.Add(EngineeringLightingToolsApp);
        }

        public Result OnShutdown(UIControlledApplication application)
        {
            //if on shut down any application failes the result will be failure
            bool result = AppHelpers.Select(n => n.OnShutDown(application))
                .Contains(Result.Failed);
            if (result) { return Result.Failed; } else { return Result.Succeeded; }
        }

        public Result OnStartup(UIControlledApplication application)
        {
            //if on startup any application failes the result will be failure
            bool result = AppHelpers.Select(n => n.OnStartUp(application))
                .Contains(Result.Failed);
            if (result) { return Result.Failed; } else { return Result.Succeeded; }
        }
    }
}
