namespace PhonebookUpdater
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Model;
    using NLog;
    using Utils;

    /// <summary>Программа Updater.</summary>
    public class Program
    {
        private const string UpdPropPath = @"Updater\UpdaterProp.xml";
        private const string Excl1 = "Updater";
        private const string Excl2 = "Temp";
        private const string TargetName = "P3";
        private const string SettingsPath = @"Updater\UpdaterProp.xml";
        private static string tempFolderPath = $@"{Environment.CurrentDirectory}\Temp\";
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static SettingsXml<SettingsShell.RootElement> settingsXml;
        private static SettingsShell.RootElement settings;
        private static RemoteSettingsShell.RootElementRemoteSettings remoteSettings;

        /// <summary>Основной метод.</summary>
        /// <param name="args">Аргументы.</param>
        public static void Main(string[] args)
        {
            // удаление процесса            
            var localProcs = System.Diagnostics.Process.GetProcesses();
            try
            {
                // Вычитывание параметров из XML
                // Инициализация модели настроек
                settingsXml = new SettingsXml<SettingsShell.RootElement>(SettingsPath);
                settings.SoftUpdate = new SettingsShell.SoftUpdate();

                if (!File.Exists(SettingsPath))
                {
                    settings = SetDefaultValue(settings); // Значения по умолчанию
                    settingsXml.WriteXml(settings);
                }
                else
                {
                    settings = settingsXml.ReadXml(settings);
                }

                // Вычитывание параметров из удаленного xml
                // Инициализация модели настроек
                var remoteSettingsXml = new SettingsXml<RemoteSettingsShell.RootElementRemoteSettings>(settings.SoftUpdate.RemoteSettingsPath);
                remoteSettings.Phonebook = new RemoteSettingsShell.Phonebook();
                remoteSettings.PhonebookUpd = new RemoteSettingsShell.PhonebookUpd();

                if (!string.IsNullOrEmpty(settings.SoftUpdate.RemoteSettingsPath))
                {
                    remoteSettings = remoteSettingsXml.ReadXml(remoteSettings);
                }

                var targetProc = localProcs.First(p => p.ProcessName == TargetName);
                targetProc.Kill();

                logger.Info("Программа P3.exe выключена");
                
                if (!string.IsNullOrEmpty(settings.SoftUpdate.RemoteSettingsPath))
                {
                    UpdateSoft();
                }
                else
                {
                    Console.WriteLine($"Неверный путь к файлу конфигурации {settings.SoftUpdate.RemoteSettingsPath}");                    
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }            
        }

        private static void UpdateSoft()
        {
            try
            {
                if (Directory.Exists(tempFolderPath))
                {
                    DelDir(tempFolderPath);
                }

                Directory.CreateDirectory(tempFolderPath);

                CopyDir(remoteSettings.Phonebook.Path, tempFolderPath);

                var fullfilesPath = Directory.GetFiles(Environment.CurrentDirectory);
                var fullDirPath = Directory.GetDirectories(Environment.CurrentDirectory);

                foreach (var s in fullfilesPath)
                {
                    File.Delete(s);
                }

                foreach (var s in fullDirPath)
                {
                    var temp = Path.GetFileNameWithoutExtension(s);
                    if (temp != Excl1 && temp != Excl2)
                    {
                        var dir = new DirectoryInfo(s);
                        dir.Delete(true);
                    }
                }

                CopyDir(tempFolderPath, Environment.CurrentDirectory);

                DelDir(tempFolderPath);
                Process.Start($"{TargetName}.exe");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private static void CopyDir(string fromDir, string toDir)
        {
            try
            {
                Directory.CreateDirectory(toDir);
                foreach (var s1 in Directory.GetFiles(fromDir))
                {
                    var s2 = Path.Combine(toDir, Path.GetFileName(s1));
                    File.Copy(s1, s2);
                }

                logger.Info("Копирование файлов Updaytera завершено");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private static void DelDir(string dirPath)
        {
            try
            {
                var dir = new DirectoryInfo(dirPath);

                foreach (var file in dir.GetFiles())
                {
                    file.Delete();
                }

                foreach (var subDirectory in dir.GetDirectories())
                {
                    subDirectory.Delete(true);
                }

                logger.Info($"Удаление папки {dirPath} завершено");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private static SettingsShell.RootElement SetDefaultValue(SettingsShell.RootElement set)
        {
            set.SoftUpdate.RemoteSettingsPath = @"d:\Temp\RemoteProp.xml";
            set.SoftUpdate.VersionUpd = Assembly.GetCallingAssembly().GetName().Version.ToString();
            return set;
        }
    }
}