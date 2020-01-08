using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ENGPowerTools.Helpers.Interfaces;
using ENGPowerTools.Helpers.Services;
using Autodesk.Revit.UI;

namespace ENGPowerTools.ProjectSetupApp
{
    public class AppMain : IExternalApplicationHelper
    {
        internal AppMain ThisApp;

        public UIControlledApplication ThisUIApp;
        public Ribbon ThisAppRibbon;

        //********* WRITE YOUR APPLICATION CODE HERE*************************************
        public Result OnShutDown(UIControlledApplication application)
        {
            TaskDialog.Show("MyShinyApp", "Bye World");

            return Result.Succeeded;
        }

        public Result OnStartUp(UIControlledApplication application)
        {
            TaskDialog.Show("MyShinyApp", "Hello World");
            ThisApp = this;
            ThisUIApp = application;
            BiuldExampleRibbon();
            return Result.Succeeded;
        }

        static string AssemblyPath = typeof(AppMain).Assembly.Location;

        //******************************************************************************
        //**************Ribbon Creation Example*****************************************
        private void BiuldExampleRibbon()
        {
            //Creates Tab and Panel
            ThisAppRibbon = new Ribbon(ThisUIApp, "Example Panel");

            //Loads contextual help from resources 
            ContextualHelp Help = new ContextualHelp(ContextualHelpType.ChmFile,
                Path.GetDirectoryName(AssemblyPath) + @"\Engineering Lighting Tools.chm");

            //Adds Push Button 1
            ThisAppRibbon.AddPushButton("Command 1", "Run Command 1"
                , AssemblyPath, "EntryPoint.RevitCommands.Command1",
                "Lighting Fixtures Costs Manager", Help,
                "EntryPoint.RevitCommands.Command1");
        }
    }
}
