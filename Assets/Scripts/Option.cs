using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.Xml;

[System.Serializable]
public class Option
{

    [XmlAttribute("optionIndex")]
    public int optionIndex;

    [XmlAttribute("cardIndex")]
    public int cardIndex;

    [XmlElement("text")]
    public string optionText;

    [XmlElement("mastersEffect")]
    public float mastersEffect;

    [XmlElement("staffEffect")]
    public float staffEffect;

    [XmlElement("timeEffect")]
    public int timeEffect;

    [XmlElement("moneyEffect")]
    public float moneyEffect;

}
