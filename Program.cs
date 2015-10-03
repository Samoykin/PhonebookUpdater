using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhonebookUpdater
{
    class Program
    {
        private static String pathToXml = @"Updater\UpdProp.xml";
        private static String pathToTemp = Environment.CurrentDirectory + @"\Temp\";
        private static String pathToUpd = "";
        private static String excl1 = "Updater";
        private static String excl2 = "Temp";

        private static List<String> ch = new List<string>();

        private static XMLcode xml = new XMLcode();

        static void Main(string[] args)
        {
            //удаление процесса
            string target_name = "Phonebook";
            System.Diagnostics.Process[] local_procs = System.Diagnostics.Process.GetProcesses();
            try
            {
                System.Diagnostics.Process target_proc = local_procs.First(p => p.ProcessName == target_name);
                target_proc.Kill();
            }
            catch { }

            if (!File.Exists(pathToXml))
            {
                xml.CreateXml();
                xml.CreateNodesXml();
                xml.WriteXml();
            }
            pathToUpd = xml.ReadXml();


                if (pathToUpd != "")
                {
                    ch = xml.ReadXmlForUpdate(pathToUpd);
                    UpdSoft();
                }
                else
                {
                    Console.WriteLine("Неверный путь к файлу конфигурации " + pathToUpd);
                    
                }


            
        }

        static void UpdSoft()
        {
            pathToUpd = ch[1];

            if (Directory.Exists(pathToTemp))
            {
                delFromDir(pathToTemp);
            }

            Directory.CreateDirectory(pathToTemp);

            CopyDir(ch[1], pathToTemp);

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

            CopyDir(pathToTemp, Environment.CurrentDirectory);

            delFromDir(pathToTemp);
            Process.Start(@"Phonebook.exe");
        }

        static void CopyDir(string FromDir, string ToDir)
        {
            Directory.CreateDirectory(ToDir);
            foreach (string s1 in Directory.GetFiles(FromDir))
            {
                string s2 = ToDir + "\\" + Path.GetFileName(s1);
                File.Copy(s1, s2);
            }
            foreach (string s in Directory.GetDirectories(FromDir))
            {
                CopyDir(s, ToDir + "\\" + Path.GetFileName(s));
            }
        }

        static void delFromDir(String dirPath)
        {
            DirectoryInfo dir = new DirectoryInfo(dirPath);

            foreach (FileInfo file in dir.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in dir.GetDirectories()) subDirectory.Delete(true);

        }
         
    }
}
