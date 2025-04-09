using System.Xml;
using UnityEngine;
using System.Collections.Generic; // For List<> and Dictionary<,>

class OsmNode : BaseOsm
{
    /// <summary>
    /// Node ID.
    /// </summary>
    public ulong ID { get; private set; }

    /// <summary>
    /// Latitude position of the node.
    /// </summary>
    public float Latitude { get; private set; }

    /// <summary>
    /// Longitude position of the node.
    /// </summary>
    public float Longitude { get; private set; }

    /// <summary>
    /// Unity unit X-co-ordinate.
    /// </summary>
    public float X { get; private set; }

    /// <summary>
    /// Unity unit Y-co-ordinate.
    /// </summary>
    public float Y { get; private set; }

    public string City { get; private set; }

    public bool IsTree { get; private set; }
    public bool IsBuilding { get; private set; }

    public Dictionary<string, string> Tags { get; private set; }

    /// <summary>
    /// Property to get the position as a Vector3.
    /// </summary>
    public Vector3 Position => new Vector3(X, 0, Y);

    /// <summary>
    /// Implicit conversion between OsmNode and Vector3.
    /// </summary>
    /// <param name="node">OsmNode instance</param>
    public static implicit operator Vector3 (OsmNode node)
    {
        return new Vector3(node.X, 0, node.Y);
    }
    
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="node">Xml node</param>
    public OsmNode(XmlNode node)
    {
        // Initialize relevant properties
        Tags = new Dictionary<string, string>();

        // Get the attribute values
        ID = GetAttribute<ulong>("id", node.Attributes);
        Latitude = GetAttribute<float>("lat", node.Attributes);
        Longitude = GetAttribute<float>("lon", node.Attributes);

        City = GetTagValue("addr:city", node);

        // Parse tags
        XmlNodeList tags = node.SelectNodes("tag");
        foreach (XmlNode tag in tags)
        {
            string key = GetAttribute<string>("k", tag.Attributes);
            string value = GetAttribute<string>("v", tag.Attributes);
            Tags[key] = value;

            if (key == "natural" && value == "tree")
            {
                IsTree = true;
            } else if (key == "tourism")
            {
                IsBuilding = true;
            }
        }

        // Calculate the position in Unity units
        X = (float)MercatorProjection.lonToX(Longitude);
        Y = (float)MercatorProjection.latToY(Latitude);

    }

    private string GetTagValue(string tagName, XmlNode node)
    {
        foreach (XmlNode tag in node.ChildNodes)
        {
            if (tag.Name == "tag" && tag.Attributes["k"].Value == tagName)
            {
                return tag.Attributes["v"].Value;
            }
        }
        return null;
    }
}
