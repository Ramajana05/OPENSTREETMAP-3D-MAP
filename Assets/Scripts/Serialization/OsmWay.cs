using System.Xml;
using UnityEngine;
using System.Collections.Generic; // For List<> and Dictionary<,>
using System.Xml; // For XmlNodeList and XmlNode


class OsmWay : BaseOsm
{
    public ulong ID { get; private set; }
    public bool Visible { get; private set; }
    public List<ulong> NodeIDs { get; private set; }
    public bool IsBoundary { get; private set; }
    public bool IsBuilding { get; private set; }
    public bool IsRoad { get; private set; }
    public bool IsRailway { get; private set; }
    public bool IsLanduse { get; private set; }
    public bool IsLeisure { get; private set; }
    public bool IsAmenity { get; private set; }
    public bool IsWaterway { get; private set; }
    public bool IsNatural { get; private set; }
    public bool IsTree { get; private set; }
    public bool IsTunnel { get; private set; }
    public float Height { get; private set; }
    public string Name { get; private set; }
    public int Lanes { get; private set; }
    public Dictionary<string, string> Tags { get; private set; }
    public string Surface { get; private set; }
    public bool HasSidewalk { get; private set; }
    public bool IsCrossing { get; private set; }
    public string CrossingType { get; private set; }
    public string City { get; private set; } // Add City property

     // Add Longitude and Latitude properties
    public float Longitude { get; private set; }
    public float Latitude { get; private set; }

    public OsmWay(XmlNode node)
    {
        // Initialize relevant properties
        NodeIDs = new List<ulong>();
        Height = 3.0f;
        Lanes = 1;
        Name = "";
        Tags = new Dictionary<string, string>();
        HasSidewalk = false;

        ID = GetAttribute<ulong>("id", node.Attributes);
        XmlNodeList nds = node.SelectNodes("nd");
        foreach (XmlNode n in nds)
        {
            ulong refNo = GetAttribute<ulong>("ref", n.Attributes);
            NodeIDs.Add(refNo);
        }

        if (NodeIDs.Count > 1)
        {
            IsBoundary = NodeIDs[0] == NodeIDs[NodeIDs.Count - 1];
        }

        XmlNodeList tags = node.SelectNodes("tag");
        foreach (XmlNode t in tags)
        {
            string key = GetAttribute<string>("k", t.Attributes);
            string value = GetAttribute<string>("v", t.Attributes);
            Tags[key] = value;

            // Handle specific tag types
            if (key == "building:levels")
            {
                Height = 3.0f * GetAttribute<float>("v", t.Attributes);
            }
            else if (key == "height")
            {
                Height = 0.3048f * GetAttribute<float>("v", t.Attributes);
            }
            else if (key == "building")
            {
                IsBuilding = true;
            }
            else if (key == "highway")
            {
                IsRoad = true;
            }
            else if (key == "railway")
            {
                IsRailway = true;
            }
            else if (key == "landuse")
            {
                IsLanduse = true;
            }
            else if (key == "leisure")
            {
                IsLeisure = true;
            }
            else if (key == "lanes")
            {
                Lanes = GetAttribute<int>("v", t.Attributes);
            }
            else if (key == "name")
            {
                Name = value;
            }
            else if (key == "surface")
            {
                Surface = value;
            }
            else if (key == "sidewalk" && (value == "both" || value == "left" || value == "right"))
            {
                HasSidewalk = true;
            }
            else if (key == "highway" && value == "crossing")
            {
                IsCrossing = true;
            }
            else if (key == "crossing_ref")
            {
                CrossingType = value;
            }
            else if (key == "natural")
            {
                IsNatural = true;
            }
            else if (key == "natural")
        {
            IsNatural = true;
            if (value == "tree")
            {
                IsTree = true;
                Debug.Log($"Tree detected in tags for way ID: {ID}");
            }
        }
            else if (key == "amenity")
            {
                IsAmenity = true;
            }
            else if (key == "waterway")
            {
                IsWaterway = true;
            }
            else if (key == "loacation" && value != "underground")
            {
                //IsWaterway = true;
            }
            else if (key == "tunnel" && value == "yes")
            {
                IsTunnel = true;
            }
            else if (key == "addr:city") // Set City property
            {
                City = value;
            } 
            else if (key == "longitude") // Set Longitude property
            {
                Longitude = float.Parse(value);
            }
            else if (key == "latitude") // Set Latitude property
            {
                Latitude = float.Parse(value);
            }
        
        }
    }

   public int CountReferences()
    {
        return NodeIDs.Count;
    }
}
