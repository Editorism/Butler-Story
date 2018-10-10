using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class Card {

    [XmlAttribute("id")]
    public string eventId;

    [XmlAttribute("cardIndex")]
    public int cardIndex;

    [XmlElement("text")]
    public string eventText;

    [XmlElement("causer")]
    public string eventCauser;

    [XmlElement("spriteName")]
    public string spriteName;

    [XmlElement("timeOfDay")]
    public string timeOfDay;


}
