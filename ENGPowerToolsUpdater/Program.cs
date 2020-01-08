using System;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace PowerToolsUpdater
{
    class Program
    {
        public static bool IsClosed { get; set; }

        static void Main(string[] args)
        {
            string path = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.MyDocuments), "PowerTools\\ENGPowerToolsAppData");
            string Path1 = File.ReadAllText(path);
            ProcessInfo Proc = new ProcessInfo();
            Proc = JsonConvert.DeserializeObject<ProcessInfo>(Path1);
            int ShiftReg = 0;

            while (!IsClosed)
            {
                ShiftReg++;
                IsClosed = IsProcessClosed(Proc);
                if (!IsClosed)
                { Thread.Sleep(10); }
                else
                {
                    try
                    {
                        UpdateFiles();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed to update Power Tools Add-in.\n\n"
                            + ex.Message);
                    }
                }
                if (ShiftReg > 1000) { break; }
            }
            Console.Read();
        }

        public static void UpdateFiles()
        {
            bool exists = false;
            int ShiftReg = 0;
            string FullName = "";
            string path = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.CommonApplicationData),
                "Autodesk\\ApplicationPlugins\\ENGPowerTools.bundle\\");
            DirectoryInfo Dir = new DirectoryInfo(path);
            FileInfo[] files = GetAllFiles();
            FileInfo[] UserFiles = Dir.GetFiles("*", SearchOption.AllDirectories);

            foreach (FileInfo file in files)
            {
                ShiftReg = 0;
                exists = false;
                foreach (FileInfo file2 in UserFiles)
                {
                    if (file.Name == file2.Name &&
                        file.Name != "ENGPowerToolsUpdater.exe")
                    {
                        if (file.Extension != ".dll")
                        {
                            if (file.CreationTime > file2.CreationTime)
                            {
                                FullName = file2.FullName;
                                file2.Delete();
                                file.CopyTo(FullName);
                            }
                        }
                        else
                        {
                            FileVersionInfo FileInfo = FileVersionInfo.GetVersionInfo(file.FullName);
                            FileVersionInfo FileInfo2 = FileVersionInfo.GetVersionInfo(file2.FullName);
                            Console.WriteLine(FileInfo.FileMajorPart + "," + FileInfo.FileMinorPart);
                            if (FileInfo.FileMajorPart > FileInfo2.FileMajorPart ||
                                (FileInfo.FileMajorPart == FileInfo2.FileMajorPart &&
                                FileInfo.FileMinorPart > FileInfo2.FileMinorPart))
                            {
                                FullName = file2.FullName;
                                file2.Delete();
                                file.CopyTo(FullName);
                            }
                        }
                        exists = true;
                        break;
                    }
                    ShiftReg++;
                }
                if (!exists && file.Name != "ENGPowerToolsUpdater.exe")
                {
                    file.CopyTo(Path.Combine(path, "contents", file.Name));
                }
            }
        }

        /// <summary>
        /// Retieves all the application files from the source repository
        /// </summary>
        /// <returns></returns>        
        public static FileInfo[] GetAllFiles()
        {
            string path = "S:\\Engineering\\Engineering Tools &"
                + " Spreadsheets\\ENG -Add-ins\\ENGPowerToolsPluggin\\Application Files";
            DirectoryInfo Dir = new DirectoryInfo(path);
            return Dir.GetFiles();
        }

        public static bool IsProcessClosed(ProcessInfo Proc)
        {
            bool Returnbool = true;
            Console.WriteLine(Proc.ProcessName);
            Process[] Procs = Process.GetProcessesByName(Proc.ProcessName);
            if (Procs.Count() > 0)
            {
                Returnbool = false;
            }
            return Returnbool;
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
    }
}
