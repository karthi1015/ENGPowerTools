using System;
using System.Collections.Generic;
using Autodesk.Revit.UI;
using Autodesk.Windows;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using ENGPowerTools.Helpers.Constants;

namespace ENGPowerTools.Helpers.Services
{
    /// <summary>
    /// Helper class to create and maintain external application ribbon panel
    /// </summary>
    public class Ribbon
    {
        UIControlledApplication UIApp { get; set; }
        RibbonControl MyRibbon { get; set; }
        RibbonTab MyTab { get; set; }
        static Autodesk.Revit.UI.RibbonPanel MyPanel { get; set; }
        List<Icontrol> Controls { get; set; }

        public Ribbon(UIControlledApplication UIApplication, string PanelName)
        {
            UIApp = UIApplication;
            MyRibbon = Autodesk.Windows.ComponentManager.Ribbon;
            MyTab = GetThisTab();
            CreatePanel(PanelName);
        }

        private RibbonTab GetThisTab()
        {
            bool Found = false;
            foreach (RibbonTab Tab in MyRibbon.Tabs)
            {
                if (Tab.AutomationName == RootConstants.RootDirectoryName)
                {
                    Found = true;
                    return Tab;
                }
            }
            if (!Found) { return CreateTab(); }
            return null;
        }

        private RibbonTab CreateTab()
        {
            UIApp.CreateRibbonTab(RootConstants.RootDirectoryName);
            return GetThisTab();
        }

        private void CreatePanel(string PanelName)
        {
            MyPanel = UIApp.CreateRibbonPanel(MyTab.Name, PanelName);
        }

        private interface Icontrol
        {
            void LargeImage(System.Drawing.Bitmap LargeImage);
            void Image(System.Drawing.Bitmap Image);
        }

        public void AddPushButton(
            string ButtonName,
            string PopupText,
            string AssemblyFullPath,
            string FullClassName)
        {
            PushButtonControl PBC = new PushButtonControl(ButtonName,
                PopupText, AssemblyFullPath, FullClassName);
            PBC.CreateButton();
            Icontrol Control = PBC;
        }

        public void AddPushButton(string ButtonName,
            string PopupText,
            string AssemblyFullPath,
            string FullClassName,
            string LongDescription,
            ContextualHelp ContextualHelp
            )
        {
            PushButtonControl PBC = new PushButtonControl(ButtonName,
                PopupText, AssemblyFullPath, FullClassName);
            PBC.LongDiscription(LongDescription);
            PBC.SetContextualHelp(ContextualHelp);
            PBC.CreateButton();
            Icontrol Control = PBC;
        }


        public void AddPushButton(string ButtonName,
            string PopupText,
            string AssemblyFullPath,
            string FullClassName,
            string LongDescription,
            ContextualHelp ContextualHelp,
            string AvailabilityFullClassName
            )
        {
            PushButtonControl PBC = new PushButtonControl(ButtonName,
                PopupText, AssemblyFullPath, FullClassName);
            PBC.LongDiscription(LongDescription);
            PBC.SetContextualHelp(ContextualHelp);
            PBC.AvailablilityClass(AvailabilityFullClassName);
            PBC.CreateButton();
            Icontrol Control = PBC;

        }

        public void AddPushButton(
            string ButtonName,
            string PopupText,
            string AssemblyFullPath,
            string FullClassName,
            System.Drawing.Bitmap LargeImage,
            System.Drawing.Bitmap Image,
            string LongDescription,
            ContextualHelp ContextualHelp)
        {
            PushButtonControl PBC = new PushButtonControl(ButtonName,
                PopupText, AssemblyFullPath, FullClassName);
            PBC.LargeImage(LargeImage);
            PBC.Image(Image);
            PBC.LongDiscription(LongDescription);
            PBC.SetContextualHelp(ContextualHelp);
            PBC.CreateButton();
            Icontrol Control = PBC;
        }

        public void AddPushButton(
            string ButtonName,
            string PopupText,
            string AssemblyFullPath,
            string FullClassName,
            System.Drawing.Bitmap LargeImage,
            System.Drawing.Bitmap Image,
            string LongDescription,
            ContextualHelp ContextualHelp,
            string AvailabilityFullClassName)
        {
            PushButtonControl PBC = new PushButtonControl(ButtonName,
                PopupText, AssemblyFullPath, FullClassName);
            PBC.LargeImage(LargeImage);
            PBC.Image(Image);
            PBC.LongDiscription(LongDescription);
            PBC.SetContextualHelp(ContextualHelp);
            PBC.AvailablilityClass(AvailabilityFullClassName);
            PBC.CreateButton();

            Icontrol Control = PBC;
        }

        private class PushButtonControl : Icontrol
        {
            PushButtonData ButtonData = null;
            PushButton Button = null;
            public PushButtonControl(
                string ButtonName,
                string PopupText,
                string AssemblyFullPath,
                string FullClassName)
            {
                ButtonData = new PushButtonData(ButtonName,
                    PopupText, AssemblyFullPath, FullClassName);
            }

            public void LargeImage(System.Drawing.Bitmap LargeImage)
            {
                ButtonData.LargeImage = convertFromBitmap(LargeImage);
            }

            public void Image(System.Drawing.Bitmap Image)
            {
                ButtonData.Image = convertFromBitmap(Image);
            }

            public void LongDiscription(string LongDescription)
            {
                ButtonData.LongDescription = LongDescription;
            }

            public void SetContextualHelp(ContextualHelp ContextHelp)
            {
                ButtonData.SetContextualHelp(ContextHelp);
            }

            public void AvailablilityClass(string AvailibilityFullClassName)
            {
                ButtonData.AvailabilityClassName = AvailibilityFullClassName;
            }

            public void CreateButton()
            {
                Button = MyPanel.AddItem(ButtonData) as PushButton;
            }
        }

        private static BitmapSource convertFromBitmap(System.Drawing.Bitmap bitmap)
        {
            return Imaging.CreateBitmapSourceFromHBitmap(
                bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions());
        }
    }
}
