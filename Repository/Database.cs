using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

public class Database
{
    private static Database instance = new Database();
    public static Database Instance { get => instance; }
    private Database()
	{
	}

	public void Save<T>(IEnumerable<T> data)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
        using (TextWriter textWriter = new StreamWriter(GetFileName<T>()))
        {
            serializer.Serialize(textWriter, new List<T>(data));
        }
    }

	public List<T>? Restore<T>()
    {
        string fileName = GetFileName<T>();
        if (!File.Exists(fileName))
        {
            return null;
        }
        XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
        FileStream databaseFileStream = new FileStream(GetFileName<T>(), FileMode.Open);
        return (List<T>?) serializer.Deserialize(databaseFileStream);
    }

    private string GetFileName<T>()
    {
        return string.Format("{0}.xml", typeof(T).Name);
    }
}