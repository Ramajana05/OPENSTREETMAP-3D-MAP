using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    "Landuse" in OSM is used to describe the primary use of land by humans. 
    For general information on mapping/classifying land areas
    Documentation: https://wiki.openstreetmap.org/wiki/Key:landuse

*/

class LanduseMaker : InfrastructureBehaviour
{
    public Material defaultMaterial;
    public Material grassMaterial;
    public Material residentialMaterial;
    public Material commercialMaterial;
    public Material industrialMaterial;
    public Material forestMaterial;
    public Material parkMaterial;
    public Material militaryMaterial;
    public Material greenfieldMaterial;
    public Material village_greenMaterial;
    public Material farmMaterial;
    public Material farmlandMaterial;
    public Material flowerbedMaterial;
    public Material recreation_groundMaterial;
    public Material constructionMaterial;
    public Material railwayMaterial;
    public Material orchardMaterial;
    public Material healthcareMaterial;
    public Material greeneryMaterial;
    public Material cemeteryMaterial;
    public Material retailMaterial;
    public Material institutionalMaterial;
    public Material amendityMaterial;

    IEnumerator Start()
{
    // Wait until the map is ready
    while (!map.IsReady)
    {
        yield return null;
    }

    // Iterate through all the buildings in the 'ways' list
    foreach (var way in map.ways)
    {
      if (way.IsLanduse && way.NodeIDs.Count > 1)
        {
        string landuseType = "";
        if (way.Tags.ContainsKey("landuse"))
        {
            landuseType = way.Tags["landuse"];
        }

        Material landuseMaterial = defaultMaterial;

        switch (landuseType)
        {
            case "grass":
                landuseMaterial = grassMaterial;
                break;
            //case "residential":
                //landuseMaterial = residentialMaterial;
                //break;
            case "forest":
                landuseMaterial = forestMaterial;
                break;
            case "park":
                landuseMaterial = parkMaterial;
                break;
            //case "commercial":
                //landuseMaterial = commercialMaterial;
                //break;
            case "industrial":
                landuseMaterial = industrialMaterial;
                break;
            case "military":
                landuseMaterial = militaryMaterial;
                break;
            case "farm":
                landuseMaterial = farmMaterial;
                break;
            case "greenfield":
                landuseMaterial = greenfieldMaterial;
                break;
            case "village_green":
                landuseMaterial = village_greenMaterial;
                break;
            case "flowerbed":
                landuseMaterial = flowerbedMaterial;
                break;
            case "recreation_ground":
                landuseMaterial = recreation_groundMaterial;
                break;
            case "construction":
                landuseMaterial = constructionMaterial;
                break;
            case "railway":
                landuseMaterial = railwayMaterial;
                break;
            case "orchard":
                landuseMaterial = orchardMaterial;
                break;
            case "healthcare":
                landuseMaterial = healthcareMaterial;
                break;
            case "farmland":
                landuseMaterial = farmlandMaterial;
                break;
            case "greenery":
                landuseMaterial = greeneryMaterial;
                break;
            case "cemetery":
                landuseMaterial = cemeteryMaterial;
                break;
            //case "retail":
                //landuseMaterial = retailMaterial;
                //break;
            case "institutional":
                landuseMaterial = institutionalMaterial;
                break;
            case "education":
                landuseMaterial = institutionalMaterial;
                break;
            case "fairground":
                landuseMaterial = institutionalMaterial;
                break;
            case "aquaculture":
                landuseMaterial = institutionalMaterial;
                break;
            case "allotments":
                landuseMaterial = institutionalMaterial;
                break;
            case "farmyard":
                landuseMaterial = institutionalMaterial;
                break;
            case "paddy":
                landuseMaterial = institutionalMaterial;
                break;
            case "animal_keeping":
                landuseMaterial = institutionalMaterial;
                break;
            case "greenhouse_horticulture":
                landuseMaterial = institutionalMaterial;
                break;
            case "meadow":
                landuseMaterial = institutionalMaterial;
                break;
            case "plant_nursery":
                landuseMaterial = institutionalMaterial;
                break;
            case "avineyard":
                landuseMaterial = institutionalMaterial;
                break;
            case "basin":
                landuseMaterial = institutionalMaterial;
                break;
            case "salt_pond":
                landuseMaterial = institutionalMaterial;
                break;
            case "brownfield":
                landuseMaterial = institutionalMaterial;
                break;
            case "depot":
                landuseMaterial = institutionalMaterial;
                break;
            case "garages":
                landuseMaterial = institutionalMaterial;
                break;
            case "landfill":
                landuseMaterial = institutionalMaterial;
                break;
            case "port":
                landuseMaterial = institutionalMaterial;
                break;
            case "quarry":
                landuseMaterial = institutionalMaterial;
                break;
            case "religious":
                landuseMaterial = institutionalMaterial;
                break;
            case "winter_sports":
                landuseMaterial = institutionalMaterial;
                break;
            case "user defined":
                landuseMaterial = institutionalMaterial;
                break;

            default:
                landuseMaterial = defaultMaterial;
                break;
        }

         int totalReferences = way.CountReferences();

        
        if (way.NodeIDs.Count <= 45){
CreateObject(way, landuseMaterial, $"{way.Name}, {way.ID}, {totalReferences})");
        }
        
        yield return null;
        }

    }
}
    
    protected override void OnObjectCreated(OsmWay way, Vector3 origin, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices)
    {
       int ndCount = way.NodeIDs.Count;

    // Define the base height
    float baseHeight = 0f;

    // Adjust the base height based on the number of nd elements
    if (ndCount > 200)
    {
        baseHeight = 0f; // Adjust this value as needed
    }
    else if (ndCount > 150)
    {
        baseHeight = 0.05f; // Adjust this value as needed
    }
    else if (ndCount > 100)
    {
        baseHeight = 0.1f; // Adjust this value as needed
    }else if (ndCount > 50)
    {
        baseHeight = 0.15f; // Adjust this value as needed
    }
    else if (ndCount > 25)
    {
        baseHeight = 0.18f; // Adjust this value as needed
    }
    else if (ndCount > 0)
    {
        baseHeight = 0.2f; // Adjust this value as needed
    }


    // Increment the height position based on the base height
    float heightPosition = 0f;
    heightPosition += baseHeight;

    // Get the centre of the roof
    Vector3 oTop = new Vector3(0, heightPosition, 0);

    // First vector is the middle point in the roof
    vectors.Add(oTop);
    normals.Add(Vector3.up);
    uvs.Add(new Vector2(0.5f, 0.5f));

        for (int i = 1; i < way.NodeIDs.Count; i++)
        {
            OsmNode p1 = map.nodes[way.NodeIDs[i - 1]];
            OsmNode p2 = map.nodes[way.NodeIDs[i]];

            Vector3 v1 = p1 - origin;
            Vector3 v2 = p2 - origin;
            Vector3 v3 = v1 + new Vector3(0, baseHeight, 0);
            Vector3 v4 = v2 + new Vector3(0, baseHeight, 0);

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

            // first triangle v1, v3, v2
            indices.Add(idx1);
            indices.Add(idx3);
            indices.Add(idx2);

            // second         v3, v4, v2
            indices.Add(idx3);
            indices.Add(idx4);
            indices.Add(idx2);

            // third          v2, v3, v1
            indices.Add(idx2);
            indices.Add(idx3);
            indices.Add(idx1);

            // fourth         v2, v4, v3
            indices.Add(idx2);
            indices.Add(idx4);
            indices.Add(idx3);

            // And now the roof triangles
            indices.Add(0);
            indices.Add(idx3);
            indices.Add(idx4);
            
            // Don't forget the upside down one!
            indices.Add(idx4);
            indices.Add(idx3);
            indices.Add(0);
        }
    }
    
}

