namespace PhonebookUpdater
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;

    /// <summary>Программа Updater.</summary>
    public class Program
    {
        private static string updPropPath = @"Updater\UpdaterProp.xml";
        private static string tempFolderPath = Environment.CurrentDirectory + @"\Temp\";
        private static string remotePropPath = string.Empty;
        private static string excl1 = "Updater";
        private static string excl2 = "Temp";
        private static string targetName = "P3";

        private static LogFile logFile = new LogFile();
        private static List<string> ch = new List<string>();
        private static XMLcode xml = new XMLcode(updPropPath);

        /// <summary>Основной метод.</summary>
        /// <param name="args">Аргументы.</param>
        public static void Main(string[] args)
        {
            // удаление процесса            
            System.Diagnostics.Process[] local_procs = System.Diagnostics.Process.GetProcesses();
            try
            {
                System.Diagnostics.Process target_proc = local_procs.First(p => p.ProcessName == targetName);
                target_proc.Kill();

                var logText = DateTime.Now.ToString() + "|event|PhonebookUpdater - Program - Main|Программа P3.exe выключена";
                logFile.WriteLog(logText);            

                if (!File.Exists(updPropPath))
                {
                    xml.CreateXml();
                    xml.CreateNodesXml();
                    xml.WriteXml();
                }

                remotePropPath = xml.ReadLocalPropXml();

                if (remotePropPath != string.Empty)
                {
                    ch = xml.ReadRemotePropXml(remotePropPath);
                    UpdateSoft();
                }
                else
                {
                    Console.WriteLine("Неверный путь к файлу конфигурации " + remotePropPath);                    
                }
            }
            catch (Exception ex)
            {
                var logText = DateTime.Now.ToString() + "|fail|PhonebookUpdater - Program - Main|" + ex.Message;
                logFile.WriteLog(logText);
            }            
        }

        private static void UpdateSoft()
        {
            try
            {
                remotePropPath = ch[1];

                if (Directory.Exists(tempFolderPath))
                {
                    DelDir(tempFolderPath);
                }

                Directory.CreateDirectory(tempFolderPath);

                CopyDir(ch[1], tempFolderPath);

                string[] fullfilesPath = Directory.GetFiles(Environment.CurrentDirectory);
                string[] fullDirPath = Directory.GetDirectories(Environment.CurrentDirectory);

                string temp;

                foreach (var s in fullfilesPath)
                {
                    File.Delete(s);
                }

                foreach (var s in fullDirPath)
                {
                    temp = Path.GetFileNameWithoutExtension(s);
                    if (temp != excl1 && temp != excl2)
                    {
                        DirectoryInfo dir = new DirectoryInfo(s);
                        dir.Delete(true);
                    }
                }

                CopyDir(tempFolderPath, Environment.CurrentDirectory);

                DelDir(tempFolderPath);
                Process.Start(targetName + @".exe");
            }
            catch (Exception ex)
            {
                var logText = DateTime.Now.ToString() + "|fail|PhonebookUpdater - Program - UpdateSoft|" + ex.Message;
                logFile.WriteLog(logText);
            }
        }

        private static void CopyDir(string fromDir, string toDir)
        {
            try
            {
                Directory.CreateDirectory(toDir);
                foreach (string s1 in Directory.GetFiles(fromDir))
                {
                    var s2 = toDir + "\\" + Path.GetFileName(s1);
                    File.Copy(s1, s2);
                }

                var logText = DateTime.Now.ToString() + "|event|PhonebookUpdater - Program - CopyDir| Копирование файлов Updaytera завершено";
                logFile.WriteLog(logText);
            }
            catch (Exception ex)
            {
                string logText = DateTime.Now.ToString() + "|fail|PhonebookUpdater - Program - CopyDir|" + ex.Message;
                logFile.WriteLog(logText);
            }
        }

        private static void DelDir(string dirPath)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(dirPath);

                foreach (FileInfo file in dir.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo subDirectory in dir.GetDirectories())
                {
                    subDirectory.Delete(true);
                }

                var logText = DateTime.Now.ToString() + "|event|PhonebookUpdater - Program - DelDir| Удаление папки " + dirPath + " завершено";
                logFile.WriteLog(logText);
            }
            catch (Exception ex)
            {
                var logText = DateTime.Now.ToString() + "|fail|PhonebookUpdater - Program - DelDir|" + ex.Message;
                logFile.WriteLog(logText);
            }
        }         
    }
}