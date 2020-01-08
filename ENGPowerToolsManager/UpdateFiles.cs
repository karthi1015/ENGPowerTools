using Autodesk.Revit.UI;
using System.IO;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Linq;
using Newtonsoft.Json;

namespace PowerToolsManager
{
    public class UpdateFiles : IExternalApplication
    {
        public SetupAddins Setup = null;

        public Result OnShutdown(UIControlledApplication application)
        {
            ManageAppData AppDataManager = new ManageAppData();
            string Path = Setup.SourceDirectory + "\\contents\\ENGPowerToolsUpdater.exe";
            Process.Start(Path);
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            Setup = new SetupAddins();
            PreUpdate.UpdateUpdater();
            return Result.Succeeded;
        }
    }

    public class ManageAppData
    {
        private FileInfo AppData = null;
        private Process ClosingApp = null;
        public ProcessInfo Proc = null;
        readonly string Directory = "PowerTools";
        readonly string FileName = "ENGPowerToolsAppData";

        public ManageAppData()
        {
            GetFile();
            AppData.Directory.Create();
            GetProcess();
            CreateProcessInfo();
            string JsonData = CreateJsonText();
            File.WriteAllText(AppData.FullName, JsonData);
        }

        private void GetFile()
        {
            string path = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments), Directory, FileName);
            AppData = new FileInfo(path);
        }

        private void GetProcess()
        {
            ClosingApp = Process.GetCurrentProcess();
        }

        private string CreateJsonText()
        {
            return JsonConvert.SerializeObject(Proc);
        }

        private void CreateProcessInfo()
        {
            Proc = new ProcessInfo();
            Proc.GetProcessInfo(ClosingApp);
        }
    }

    public class ProcessInfo
    {
        public void GetProcessInfo(Process Proc)
        {
            ProcessId = Proc.Id;
            ProcessFullName = Proc.MainModule.FileName;
            ProcessName = Proc.ProcessName;
        }

        public int ProcessId;
        public string ProcessName;
        public string ProcessFullName;
    }

    /// <summary>
    /// this class add the requisit files and assemblies 
    /// to the proper directeries
    /// </summary>
    public class SetupAddins
    {
        readonly private string PathBundle = "\\contents\\";
        readonly private string PathRevit2015 = "Autodesk\\Revit\\Addins\\2015";
        readonly private string PathRevit2016 = "Autodesk\\Revit\\Addins\\2016";
        readonly private string PathRevit2017 = "Autodesk\\Revit\\Addins\\2017";
        readonly private string PathRevit2018 = "Autodesk\\Revit\\Addins\\2018";
        readonly private string PathRevit2019 = "Autodesk\\Revit\\Addins\\2019";
        public string SourceDirectory { get; set; }

        public SetupAddins()
        {
            SourceDirectory = GetAssemblyDirectory();

            AddFiles(PathRevit2015);
            AddFiles(PathRevit2016);
            AddFiles(PathRevit2017);
            AddFiles(PathRevit2018);
            AddFiles(PathRevit2019);
        }

        public string GetAssemblyDirectory()
        {
            Assembly ThisAssembly = Assembly.GetExecutingAssembly();
            Uri Path = new Uri(ThisAssembly.GetName().CodeBase);
            return new FileInfo(Path.AbsolutePath).Directory.FullName;
        }

        public void AddFiles(string path)
        {
            if (Directory.Exists(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), path)))
            {
                String FileFullName = "PowerTools.Revit." +
                    path.Substring(path.Length - 4) + ".addin";
                FileInfo file = new FileInfo(Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.ApplicationData), path, FileFullName));
                if (!file.Exists)
                {
                    try
                    {
                        File.Copy(SourceDirectory + PathBundle +
                            FileFullName, file.FullName);
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error", ex.Message +
                            "\n\n The Add-in may have to be reinstalled");
                    }
                }
                else
                {
                    try
                    {
                        file.Delete();
                        File.Copy(SourceDirectory + PathBundle +
                            FileFullName, file.FullName);
                    }
                    catch (Exception ex)
                    {
                        TaskDialog.Show("Error", ex.Message +
                            "\n\n The Add-in may have to be reinstalled");
                    }
                }
            }
        }
    }

    public static class PreUpdate
    {
        /// <summary>
        /// Updates the Updater exicutable
        /// </summary>
        public static void UpdateUpdater()
        {
            string path = "S:\\Engineering\\Engineering Tools &"
                + " Spreadsheets\\ENG -Add-ins\\ENGPowerToolsPluggin\\Application Files";
            DirectoryInfo Dir = new DirectoryInfo(path);
            FileInfo[] info = Dir.GetFiles("PowerToolsUpdater.exe", SearchOption.AllDirectories);

            if (info.Count() > 1)
            {
                TaskDialog.Show("Failed to update", "Multiple updaters detected, the app cannot be updated.");
            }
            else if (info.Count() == 1)
            {
                DirectoryInfo DirLocal = new FileInfo(new Uri(Assembly.GetExecutingAssembly().GetName()
                    .CodeBase).AbsolutePath).Directory;
                FileInfo[] localFile = DirLocal.GetFiles("ENGPowerToolsUpdater.exe", SearchOption.AllDirectories);

                if (localFile.Count() != 1)
                {
                    TaskDialog.Show("Failed to update", "Duplicate dependancies detected, the app cannot be updated.");
                }
                else
                {
                    FileVersionInfo LocalFileInfo = FileVersionInfo.GetVersionInfo(localFile[0].FullName);
                    FileVersionInfo RepositoryFileInfo = FileVersionInfo.GetVersionInfo(info[0].FullName);

                    //if the files version is later or it was created later 
                    if ((RepositoryFileInfo.FileMajorPart > LocalFileInfo.FileMajorPart ||
                        (RepositoryFileInfo.FileMajorPart == LocalFileInfo.FileMajorPart &&
                        RepositoryFileInfo.FileMinorPart > LocalFileInfo.FileMinorPart)))
                    {
                        string LocalPath = localFile[0].FullName;
                        localFile[0].Delete();
                        //bool done = true;
                        //while(done)
                        //{
                        //    done = File.Exists(LocalPath);
                        //}
                        info[0].CopyTo(LocalPath);
                    }
                }
            }
            else
            {
                TaskDialog.Show("Failed to update", "Missing dependancies detected. please reinstall the add-in");
            }
        }
    }


    #region "Comment Code"
    //public class Backroundworker : IDisposable
    //{
    //    public int ProcId;

    //    public void Dispose()
    //    {

    //    }

    //    public Backroundworker(int ProcId2)
    //    {

    //    }

    //    public void ThreadFuction()
    //    {
    //        DateTime Current = DateTime.Now;
    //        Assembly assem = Assembly.GetExecutingAssembly();
    //        string str = assem.GetName().CodeBase;
    //        var Path = new Uri(str);
    //        string ActualPath = new System.IO.FileInfo(Path.AbsolutePath).Directory.FullName;
    //        System.IO.StreamWriter SW = System.IO.File.AppendText(ActualPath + "\\Debugging.txt");

    //        string MainMod = "";

    //        while (Current.CompareTo(DateTime.Now.AddSeconds(-60)) > 0)
    //        {
    //            Thread.Sleep(200);

    //            try
    //            {
    //                MainMod = Process.GetProcessById(ProcId).MainModule.FileName;
    //            }
    //            catch
    //            {
    //                MainMod = "Nothing";
    //            }

    //            SW.WriteLineAsync(DateTime.Now.ToString() + "," + MainMod);
    //        }

    //        SW.Close();
    //        //Thread.CurrentThread.Abort();
    //    }
    //}
    #endregion
}
