using System;
using System.IO;
using System.Xml.Serialization;

namespace RPGSample
{
    public class XmlManager<T>
    {
        public Type Type { get; set; }
        T instance;
        public XmlManager()
        {
            Type = typeof(T);
        }
        public XmlManager(string path)
        {
            Type = typeof(T);
            Load(path);
        }
        public T Get()
        {
            return instance;
        }
        public T Load(string path)
        {
            using (TextReader reader = new StreamReader("Content/GameData/"+ path + ".xml"))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                instance = (T)xml.Deserialize(reader);
            }
            return instance;
        }
        public void Save(string path, object obj)
        {
            using (TextWriter writer = new StreamWriter(path))
            {
                XmlSerializer xml = new XmlSerializer(Type);
                xml.Serialize(writer, obj);
            }
        }
    }
}
