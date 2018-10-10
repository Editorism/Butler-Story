using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

[XmlRoot("CardCollection")]
public class CardContainer {

    [XmlArray("Cards")]
    [XmlArrayItem("Card")]
    public List<Card> cards = new List<Card>();

    [XmlArray("DiscardPile")]
    [XmlArrayItem("Card")]
    public List<Card> discardPile = new List<Card>();

    public static CardContainer Load(string path)
    {
        TextAsset _xml = Resources.Load<TextAsset>(path);

        XmlSerializer serializer = new XmlSerializer(typeof(CardContainer));

        StringReader reader = new StringReader(_xml.text);

        CardContainer cards = serializer.Deserialize(reader) as CardContainer;

        reader.Close();

        return cards;

    }

    public void Save(string path)
    {

        XmlSerializer serializer = new XmlSerializer(typeof(CardContainer));
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            serializer.Serialize(stream, this);
            stream.Close();
        }
    }

}
