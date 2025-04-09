using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RoadMaker : InfrastructureBehaviour
{
    public Material defaultMaterial;

    // Different Lanes
    public Material roadMaterial;
    public Material roadSingleLaneMaterial;

    // Surface Material
    public Material asphaltMaterial;
    public Material pavingStoneMaterial;
    public Material pavedMaterial;
    public Material settMaterial;

    // Crossing
    public Material zebraMaterial;
    public Material trafficSignalMaterial;
    public Material defaultCrossingMaterial;

    // Type Highway
    public Material motorwayMaterial;
    public Material trunkMaterial;
    public Material primaryMaterial;
    public Material secondaryMaterial;
    public Material tertiaryMaterial;
    public Material unclassifiedMaterial;
    public Material residentialMaterial;
    public Material motorway_linkMaterial;
    public Material trunk_linkMaterial;
    public Material primary_linkMaterial;
    public Material secondary_linkMaterial;
    public Material tertiary_linkMaterial;
    public Material living_streetMaterial;
    public Material serviceMaterial;
    public Material pedestrianMaterial;
    public Material trackMaterial;
    public Material bus_guidewayMaterial;
    public Material escapeMaterial;
    public Material racewayMaterial;
    public Material roadsMaterial;
    public Material footwayMaterial;
    public Material bridlewayMaterial;
    public Material stepsMaterial;

    public Material roadMaterialWithSidewalk;
    public Material railwayMaterial;
    public Material sidewalkMaterial;



    bool hasSidewalk = false;
    float defaultLaneWidth = 3.7f; // Default width of a single lane (in meters)

    IEnumerator Start()
    {
        // Wait for the map to become ready
        while (!map.IsReady)
        {
            yield return null;
        }



        // Iterate through the railways and create railway geometry
        foreach (var way in map.ways.FindAll((w) => w.IsRailway && !w.IsTunnel))
        {
            CreateObject(way, railwayMaterial, $"Road_{way.ID}_{way.Name}");
            yield return null;
        }

        // Iterate through the roads and build each one
        for (int i = 0; i < map.ways.Count; i++)
        {
            var way = map.ways[i];
            if (IsRoad(way) && !way.IsTunnel)
            {
                string roadType = way.Tags.ContainsKey("highway") ? way.Tags["highway"] : "";
                Material roadMaterial = DetermineRoadMaterial(way, roadType);

                float roadWidth = DetermineRoadWidth(way);
                int numberOfLanes = DetermineNumberOfLanes(way);

                // Check and adjust road width if it connects to another road
                if (i > 0)
                {
                    var prevWay = map.ways[i - 1];
                    if (IsRoad(prevWay) && !prevWay.IsTunnel)
                    {
                        AdjustRoadWidth(ref roadWidth, prevWay, way);
                    }
                }

                CreateObject(way, roadMaterial, $"Road_{way.ID}_{way.Name}");
                yield return null;
            }
        }
    }

   protected override void OnObjectCreated(OsmWay way, Vector3 origin, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices)
{
    float roadWidth = DetermineRoadWidth(way);
    
    // Define the base height
    float baseHeight = 0.3f; // Adjust this value as needed

    for (int i = 1; i < way.NodeIDs.Count; i++)
    {
        OsmNode p1 = map.nodes[way.NodeIDs[i - 1]];
        OsmNode p2 = map.nodes[way.NodeIDs[i]];

        Vector3 s1 = p1 - origin;
        Vector3 s2 = p2 - origin;

        Vector3 diff = (s2 - s1).normalized;
        var cross = Vector3.Cross(diff, Vector3.up) * roadWidth / 2;

        Vector3 v1 = s1 + cross;
        Vector3 v2 = s1 - cross;
        Vector3 v3 = s2 + cross;
        Vector3 v4 = s2 - cross;

        // Apply base height
        v1.y += baseHeight;
        v2.y += baseHeight;
        v3.y += baseHeight;
        v4.y += baseHeight;

        // Add vertices, UVs, normals, and indices to lists
        vectors.Add(v1);
        vectors.Add(v2);
        vectors.Add(v3);
        vectors.Add(v4);

        // Add UVs
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));
        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));

        // Add normals
        normals.Add(-Vector3.forward);
        normals.Add(-Vector3.forward);
        normals.Add(-Vector3.forward);
        normals.Add(-Vector3.forward);

        // Add indices
        int idx1 = vectors.Count - 4;
        int idx2 = vectors.Count - 3;
        int idx3 = vectors.Count - 2;
        int idx4 = vectors.Count - 1;

        indices.Add(idx1);
        indices.Add(idx3);
        indices.Add(idx2);

        indices.Add(idx3);
        indices.Add(idx4);
        indices.Add(idx2);
    }
}

void AdjustRoadWidth(ref float roadWidth, OsmWay prevWay, OsmWay currWay)
    {
        float prevRoadWidth = DetermineRoadWidth(prevWay);
        if (Mathf.Abs(prevRoadWidth - roadWidth) > 0.1f)
        {
            roadWidth = (prevRoadWidth + roadWidth) / 2.0f;
        }
    }





    Material DetermineRoadMaterial(OsmWay way, string roadType)
    {
        if (way.IsCrossing)
        {
            switch (way.CrossingType)
            {
                case "zebra":
                    return zebraMaterial;
                case "traffic_signal":
                    return trafficSignalMaterial;
                default:
                    return defaultCrossingMaterial;
            }
        }

        if (way.Tags.ContainsKey("surface"))
        {
            string surfaceType = way.Tags["surface"];
            switch (surfaceType)
            {
                case "asphalt":
                    return asphaltMaterial;
                case "paving_stone":
                    return pavingStoneMaterial;
                case "paved":
                    return pavedMaterial;
                case "sett":
                    return settMaterial;
                default:
                    return defaultMaterial;
            }
        }

        switch (roadType)
        {
            case "motorway":
                return motorwayMaterial;
            case "trunk":
                return trunkMaterial;
            case "primary":
                return primaryMaterial;
            case "secondary":
                return secondaryMaterial;
            case "tertiary":
                return tertiaryMaterial;
            case "unclassified":
                return unclassifiedMaterial;
            case "residential":
                return residentialMaterial;
            case "motorway_link":
                return motorway_linkMaterial;
            case "trunk_link":
                return trunk_linkMaterial;
            case "primary_link":
                return primary_linkMaterial;
            case "secondary_link":
                return secondary_linkMaterial;
            case "tertiary_link":
                return tertiary_linkMaterial;
            case "living_street":
                return living_streetMaterial;
            case "service":
                return serviceMaterial;
            case "pedestrian":
                return pedestrianMaterial;
            case "track":
                return trackMaterial;
            case "bus_guideway":
                return bus_guidewayMaterial;
            case "escape":
                return escapeMaterial;
            case "raceway":
                return racewayMaterial;
            case "road":
                return roadsMaterial;
            case "footway":
            case "path":
                return footwayMaterial;
            case "bridleway":
                return bridlewayMaterial;
            case "steps":
                return stepsMaterial;
            default:
                return defaultMaterial;
        }
    }

    float DetermineRoadWidth(OsmWay way)
    {
        if (way.Tags.ContainsKey("width") && float.TryParse(way.Tags["width"], out float width))
        {
            return width;
        }
        return defaultLaneWidth * DetermineNumberOfLanes(way);
    }

    int DetermineNumberOfLanes(OsmWay way)
    {
        if (way.Tags.ContainsKey("lanes") && int.TryParse(way.Tags["lanes"], out int lanes))
        {
            return lanes;
        }
        return 1; // Default to one lane if not specified
    }

    bool IsRoad(OsmWay way)
    {
        if (!way.Tags.ContainsKey("highway")) return false;
        if (way.Tags.ContainsKey("area") && way.Tags["area"] == "yes") return false;
        return true;
    }

    public void ChangeMaterial(GameObject building, Material newMaterial)
    {
        Renderer renderer = building.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = newMaterial;
        }
    }
}
