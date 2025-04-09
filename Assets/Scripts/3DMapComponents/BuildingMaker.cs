using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class BuildingMaker : InfrastructureBehaviour
{
    public Material defaultMaterial;

     public void GenerateBuildings()
    {
        StartCoroutine(Start());
    }

    IEnumerator Start()
{
    while (!map.IsReady)
    {
        yield return null;
    }

    foreach (var way in map.ways.FindAll((w) => { return w.IsBuilding && w.NodeIDs.Count > 1; }))
    {
        
            OsmNode firstNode = map.nodes[way.NodeIDs[0]];
            float latitude = firstNode.Latitude;
            float longitude = firstNode.Longitude;
           
            string buildingType = "";

            // Create the object with the appropriate material
            CreateObject(way, defaultMaterial, $"Building_{way.ID}_{way.Name}");
            yield return null;
        
    }

       foreach (var node in map.nodes.Values)
{
    if (node.IsBuilding)
    {
        CreateBuildingFromNode(node);
        yield return null;
    }
}
}

    protected override void OnObjectCreated(OsmWay way, Vector3 origin, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices)
    {

    float buildingHeight = way.Tags.ContainsKey("height") ? float.Parse(way.Tags["height"]) : 10.0f;
    int buildingLevels = way.Tags.ContainsKey("building:levels") ? int.Parse(way.Tags["building:levels"]) : 1; 
    int roofLevels = way.Tags.ContainsKey("roof:levels") ? int.Parse(way.Tags["roof:levels"]) : 0; 
    float totalHeight = buildingHeight + (buildingLevels * 3.0f); 

        Vector3 oTop = new Vector3(0, buildingHeight, 0);

        vectors.Add(oTop);
        normals.Add(Vector3.up);
        uvs.Add(new Vector2(0.5f, 0.5f));

        for (int i = 1; i < way.NodeIDs.Count; i++)
        {
            OsmNode p1 = map.nodes[way.NodeIDs[i - 1]];
            OsmNode p2 = map.nodes[way.NodeIDs[i]];

            Vector3 v1 = p1 - origin;
            Vector3 v2 = p2 - origin;
            Vector3 v3 = v1 + new Vector3(0, buildingHeight, 0);
            Vector3 v4 = v2 + new Vector3(0, buildingHeight, 0);

            vectors.Add(v1);
            vectors.Add(v2);
            vectors.Add(v3);
            vectors.Add(v4);

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(1, 0));
            uvs.Add(new Vector2(0, 1));
            uvs.Add(new Vector2(1, 1));

            normals.Add(-Vector3.forward);
            normals.Add(-Vector3.forward);
            normals.Add(-Vector3.forward);
            normals.Add(-Vector3.forward);

            int idx1, idx2, idx3, idx4;
            idx4 = vectors.Count - 1;
            idx3 = vectors.Count - 2;
            idx2 = vectors.Count - 3;
            idx1 = vectors.Count - 4;

            indices.Add(idx1);
            indices.Add(idx3);
            indices.Add(idx2);

            indices.Add(idx3);
            indices.Add(idx4);
            indices.Add(idx2);


            indices.Add(idx2);
            indices.Add(idx3);
            indices.Add(idx1);

            indices.Add(idx2);
            indices.Add(idx4);
            indices.Add(idx3);

            indices.Add(0);
            indices.Add(idx3);
            indices.Add(idx4);

            indices.Add(idx4);
            indices.Add(idx3);
            indices.Add(0);
           
        }
    }

    void CreateBuildingFromNode(OsmNode node)
    {
        float latitude = node.Latitude;
        float longitude = node.Longitude;
        string buildingName = node.Tags.ContainsKey("name") ? node.Tags["name"] : "Unknown";
        Material buildingMaterial = defaultMaterial;
        
    }
    
    
}