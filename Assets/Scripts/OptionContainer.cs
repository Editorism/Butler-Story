using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("OptionCollection")]
public class OptionContainer
{

    [XmlArray("Options")]
    [XmlArrayItem("Option")]
    public List<Option> options = new List<Option>();

    public static OptionContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(OptionContainer));

        StringReader reader = new StringReader(_xml.text);

        OptionContainer options = serializer.Deserialize(reader) as OptionContainer;

        reader.Close();

        return options;

    }

    public void Save(string path)
    {

        XmlSerializer serializer = new XmlSerializer(typeof(OptionContainer));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
            stream.Close();
        }
    }

}
