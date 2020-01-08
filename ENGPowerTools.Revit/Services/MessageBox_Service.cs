using System;
using System.Collections.Generic;
using System.Text;
using ENGPowerTools.Helpers.Contracts;
using Autodesk.Revit.UI;

namespace ENGPowerTools.Revit.Services
{
    internal class MessageBox_Service : IMessageBoxService
    {
        public void Show(string title, string content)
        {
            TaskDialog.Show(title, content, TaskDialogCommonButtons.Ok);
        }
    }
}
