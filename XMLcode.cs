using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PhonebookUpdater
{
    class XMLcode
    {
        private string pathToXml = @"Updater\UpdProp.xml";
        private string pathForUpd = @"d:\Temp\prop.xml";
        private string exQ = "0";

        public void CreateXml()
        {
            XmlTextWriter textWritter = new XmlTextWriter(pathToXml, Encoding.UTF8);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("head");
            textWritter.WriteEndElement();
            textWritter.Close();
        }

        public void CreateNodesXml()
        {
            XmlDocument document = new XmlDocument();
            document.Load(pathToXml);

            XmlNode upd = document.CreateElement("Update");
            document.DocumentElement.AppendChild(upd);

            XmlNode t1 = document.CreateElement("path");
            t1.InnerText = "1";
            upd.AppendChild(t1);

            XmlNode ver = document.CreateElement("versionUpd");
            ver.InnerText = "1";
            upd.AppendChild(ver);

            XmlNode t2 = document.CreateElement("exQ");
            t2.InnerText = "1";
            upd.AppendChild(t2);

            XmlNode t3 = document.CreateElement("ex1");
            t3.InnerText = "1";
            upd.AppendChild(t3);

            XmlNode t4 = document.CreateElement("ex2");
            t4.InnerText = "1";
            upd.AppendChild(t4);

            XmlNode t5 = document.CreateElement("ex3");
            t5.InnerText = "1";
            upd.AppendChild(t5);

            XmlNode t6 = document.CreateElement("ex4");
            t6.InnerText = "1";
            upd.AppendChild(t6);

            XmlNode t7 = document.CreateElement("ex5");
            t7.InnerText = "1";
            upd.AppendChild(t7);



            document.Save(pathToXml);

        }

        public void WriteXml()
        {

            XmlDocument document = new XmlDocument();
            document.Load(pathToXml);

            XmlElement xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "path")
                        childnode.InnerText = pathForUpd;
                    if (childnode.Name == "versionUpd")
                        childnode.InnerText = Assembly.GetCallingAssembly().GetName().Version.ToString();
                }
            }
            document.Save(pathToXml);
        }

        public String ReadXml()
        {
            String ch = "";

            XmlDocument document = new XmlDocument();
            document.Load(pathToXml);

            XmlElement xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "path")
                        ch = childnode.InnerText;
                }
            }

            return ch;
        }

        public List<String> ReadXmlForUpdate(string path)
        {
            //String[] ch = new String[27];
            List<String> ch = new List<string>();

            XmlDocument document = new XmlDocument();
            document.Load(path);

            XmlElement xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    //параметры PhonebookUpdater
                    if (childnode.Name == "version")
                        ch.Add(childnode.InnerText);
                    if (childnode.Name == "path")
                        ch.Add(childnode.InnerText);
                    if (childnode.Name == "versionUpd")
                        ch.Add(childnode.InnerText);
                    if (childnode.Name == "pathUpd")
                        ch.Add(childnode.InnerText);
                }
            }
            return ch;
        }


    }
}
