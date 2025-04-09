using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class OSMDataFetcher : MonoBehaviour
{
    void Start()
    {
        // Construct the URL string for querying Nominatim API in XML format
        string urlString = "https://nominatim.openstreetmap.org/search?format=xml&q=Stuttgart&polygon_geojson=1";

        // Print the URL string for examination
        Debug.Log("URL string: " + urlString);

        // Uncomment the line below to make the actual API call
        StartCoroutine(MakeNominatimAPIRequest(urlString));
    }

    // Method to make the Nominatim API request
    IEnumerator MakeNominatimAPIRequest(string url)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Nominatim API request error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Nominatim API response: " + webRequest.downloadHandler.text);

                // Parse the response XML to extract boundary information
                List<Boundary> boundaries = ParseBoundaryInfo(webRequest.downloadHandler.text);

                // Convert boundary data to XML format
                XmlDocument xmlDoc = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                xmlDoc.AppendChild(xmlDeclaration);
                XmlElement osmElement = xmlDoc.CreateElement("osm");
                osmElement.SetAttribute("version", "0.6");
                osmElement.SetAttribute("generator", "CGImap 0.9.0 (2221918 spike-06.openstreetmap.org)");
                osmElement.SetAttribute("copyright", "OpenStreetMap and contributors");
                osmElement.SetAttribute("attribution", "http://www.openstreetmap.org/copyright");
                osmElement.SetAttribute("license", "http://opendatacommons.org/licenses/odbl/1-0/");
                xmlDoc.AppendChild(osmElement);

                // Add nodes to XML
                foreach (Boundary boundary in boundaries)
                {
                    XmlElement placeElement = xmlDoc.CreateElement("place");
                    placeElement.SetAttribute("place_id", boundary.PlaceId);
                    placeElement.SetAttribute("osm_type", boundary.OsmType);
                    placeElement.SetAttribute("osm_id", boundary.OsmId);
                    placeElement.SetAttribute("display_name", boundary.DisplayName);
                    placeElement.SetAttribute("type", boundary.Type);
                    placeElement.SetAttribute("importance", boundary.Importance.ToString());
                    placeElement.SetAttribute("boundingbox", boundary.BoundingBox);

                    foreach (Vector2 coordinate in boundary.Coordinates)
                    {
                        XmlElement nodeElement = xmlDoc.CreateElement("node");
                        nodeElement.SetAttribute("lat", coordinate.x.ToString());
                        nodeElement.SetAttribute("lon", coordinate.y.ToString());
                        placeElement.AppendChild(nodeElement);
                    }

                    osmElement.AppendChild(placeElement);
                }

                // Print XML for examination
                Debug.Log("XML Output: " + xmlDoc.InnerXml);
            }
        }
    }

    // Method to parse the XML response and extract boundary information
    List<Boundary> ParseBoundaryInfo(string xmlResponse)
    {
        List<Boundary> boundaries = new List<Boundary>();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlResponse);

        XmlNodeList placeNodes = xmlDoc.SelectNodes("//place");
        foreach (XmlNode placeNode in placeNodes)
        {
            string placeId = placeNode.Attributes["place_id"].Value;
            string osmType = placeNode.Attributes["osm_type"].Value;
            string osmId = placeNode.Attributes["osm_id"].Value;
            string displayName = placeNode.Attributes["display_name"].Value;
            string type = placeNode.Attributes["type"].Value;
            double importance = double.Parse(placeNode.Attributes["importance"].Value);
            string boundingBox = placeNode.Attributes["boundingbox"].Value;

            // Extract polygon coordinates from geojson attribute
            List<Vector2> coordinates = new List<Vector2>();
            string geojson = placeNode.Attributes["geojson"].Value;
            XmlDocument geoJsonDoc = new XmlDocument();
            geoJsonDoc.LoadXml("<root>" + geojson + "</root>");
            XmlNodeList coordNodes = geoJsonDoc.SelectNodes("//coordinates/*");
            foreach (XmlNode coordNode in coordNodes)
            {
                float lon = float.Parse(coordNode.ChildNodes[0].InnerText);
                float lat = float.Parse(coordNode.ChildNodes[1].InnerText);
                coordinates.Add(new Vector2(lat, lon));
            }

            // Create boundary object and add it to the list
            Boundary boundary = new Boundary(placeId, osmType, osmId, displayName, type, importance, boundingBox, coordinates);
            boundaries.Add(boundary);
        }
        Debug.Log("XML Output: " + boundaries);
        return boundaries;
    }

    // Class to represent boundary information
    public class Boundary
    {
        public string PlaceId { get; set; }
        public string OsmType { get; set; }
        public string OsmId { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public double Importance { get; set; }
        public string BoundingBox { get; set; }
        public List<Vector2> Coordinates { get; set; }

        public Boundary(string placeId, string osmType, string osmId, string displayName, string type, double importance, string boundingBox, List<Vector2> coordinates)
        {
            PlaceId = placeId;
            OsmType = osmType;
            OsmId = osmId;
            DisplayName = displayName;
            Type = type;
            Importance = importance;
            BoundingBox = boundingBox;
            Coordinates = coordinates;
        }
    }
}
