using System;
using System.Xml;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.Networking;

public class Program : MonoBehaviour
{
    static void Main(string[] args)
    {
        string xmlData = @"[XML_DATA_GOES_HERE]"; 

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlData);

        XmlNodeList nodeList = xmlDoc.SelectNodes("//node");

        foreach (XmlNode node in nodeList)
        {
            string nodeId = node.Attributes["id"].Value;
            string latitude = node.Attributes["lat"].Value;
            string longitude = node.Attributes["lon"].Value;

            Console.WriteLine($"Node ID: {nodeId}, Latitude: {latitude}, Longitude: {longitude}");


        }
    }
}
