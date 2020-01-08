using Autodesk.Revit.UI;

namespace ENGPowerTools.Helpers.Interfaces
{
    public interface IExternalApplicationHelper
    {
        Result OnShutDown(UIControlledApplication application);
        Result OnStartUp(UIControlledApplication application);
    }
}
