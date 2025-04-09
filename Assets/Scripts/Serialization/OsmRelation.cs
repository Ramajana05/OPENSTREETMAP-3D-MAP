using System.Collections.Generic;
using System.Xml;

public class OsmRelation
{
    public ulong ID { get; private set; }
    public Dictionary<string, string> Tags { get; private set; }
    public List<OsmRelationMember> Members { get; private set; }

    public string Type { get; private set; } // Type of the relation, e.g., "multi-polygon"
    public string OuterRole { get; private set; } // Role indicating outer members
    public string InnerRole { get; private set; } // Role indicating inner members

    public OsmRelation(XmlNode xmlNode)
    {
        ID = ulong.Parse(xmlNode.Attributes["id"].Value);
        Tags = new Dictionary<string, string>();
        Members = new List<OsmRelationMember>();

        foreach (XmlNode childNode in xmlNode.ChildNodes)
        {
            if (childNode.Name == "tag")
            {
                Tags.Add(childNode.Attributes["k"].Value, childNode.Attributes["v"].Value);
            }
            else if (childNode.Name == "member")
            {
                Members.Add(new OsmRelationMember(childNode));
            }
        }
    }
}

public class OsmRelationMember
{
    public string Type { get; private set; }
    public ulong Ref { get; private set; }
    public string Role { get; private set; }

    public OsmRelationMember(XmlNode xmlNode)
    {
        Type = xmlNode.Attributes["type"].Value;
        Ref = ulong.Parse(xmlNode.Attributes["ref"].Value);
        Role = xmlNode.Attributes["role"].Value;
    }
}
