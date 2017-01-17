using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Timekeeping
{
    [Serializable()]
    [System.Xml.Serialization.XmlRoot("RunCollection")]
    public class DataClass
    {
        static string filePath = AppDomain.CurrentDomain.BaseDirectory + "/data.xml";

        [XmlArray("Runs")]
        [XmlArrayItem("Run", typeof(Run))]
        public List<Run> Runs { get; set; } = new List<Run>();

        public class Run
        {
            [System.Xml.Serialization.XmlElementAttribute("CreatedDate")]
            public DateTime CreatedDate { get; set; }

            [System.Xml.Serialization.XmlElementAttribute("Name")]
            public string Name { get; set; }

            [System.Xml.Serialization.XmlElementAttribute("TimeA")]
            public string TimeA { get; set; }

            [System.Xml.Serialization.XmlElementAttribute("TimeB")]
            public string TimeB { get; set; }
        }


        public static DataClass Load()
        {
            var result = new DataClass();
            if (File.Exists(filePath))
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(DataClass));
                TextReader reader = new StreamReader(filePath);
                result = (DataClass)deserializer.Deserialize(reader);
                reader.Close();
            }

            return result;
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DataClass));
            using (TextWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, this);
            }
        }

    }
}
