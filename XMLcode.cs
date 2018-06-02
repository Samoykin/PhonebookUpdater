namespace PhonebookUpdater
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Text;
    using System.Xml;

    /// <summary>Параметры XML.</summary>
    public class XMLcode
    {
        private string updPropPath;

        // private string pathToXml = @"Updater\UpdProp.xml";
        // private string pathForUpd = @"d:\Temp\RemoteProp.xml";
        private string exQ = "0";

        /// <summary>Initializes a new instance of the <see cref="XMLcode" /> class.</summary>
        /// <param name="updPropPath">Путь к файлу.</param>
        public XMLcode(string updPropPath)
        {
            this.updPropPath = updPropPath;
        }

        /// <summary>Создать XML.</summary>
        public void CreateXml()
        {
            var textWritter = new XmlTextWriter(this.updPropPath, Encoding.UTF8);
            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("head");
            textWritter.WriteEndElement();
            textWritter.Close();
        }

        /// <summary>Создать узлы XML.</summary>
        public void CreateNodesXml()
        {
            var document = new XmlDocument();
            document.Load(this.updPropPath);

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

            document.Save(this.updPropPath);
        }

        /// <summary>Записать XML.</summary>
        public void WriteXml()
        {
            var document = new XmlDocument();
            document.Load(this.updPropPath);

            var xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "versionUpd")
                    {
                        childnode.InnerText = Assembly.GetCallingAssembly().GetName().Version.ToString();
                    }
                }
            }

            document.Save(this.updPropPath);
        }

        /// <summary>Считать XML.</summary>
        /// <returns>Параметры.</returns>
        public string ReadLocalPropXml()
        {
            var remotePropPath = string.Empty;

            var document = new XmlDocument();
            document.Load(this.updPropPath);

            var xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    if (childnode.Name == "path")
                    {
                        remotePropPath = childnode.InnerText;
                    }
                }
            }

            return remotePropPath;
        }

        /// <summary>Считать удаленный XML.</summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Параметры.</returns>
        public List<string> ReadRemotePropXml(string path)
        {
            var ch = new List<string>();

            var document = new XmlDocument();
            document.Load(path);

            var xRoot = document.DocumentElement;
            foreach (XmlElement xnode in xRoot)
            {
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    // параметры PhonebookUpdater
                    if (childnode.Name == "version")
                    {
                        ch.Add(childnode.InnerText);
                    }

                    if (childnode.Name == "path")
                    {
                        ch.Add(childnode.InnerText);
                    }

                    if (childnode.Name == "versionUpd")
                    {
                        ch.Add(childnode.InnerText);
                    }

                    if (childnode.Name == "pathUpd")
                    {
                        ch.Add(childnode.InnerText);
                    }
                }
            }

            return ch;
        }
    }
}