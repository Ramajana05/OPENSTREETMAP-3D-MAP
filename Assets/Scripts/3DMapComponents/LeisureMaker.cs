using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
    "Leisure" in OSM is used to generally denotes places where people go in their spare time 
    For sport, recreation, relaxation or entertainment. 
    Documentation: https://wiki.openstreetmap.org/wiki/Key:leisure

*/

class LeisureMaker : InfrastructureBehaviour
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

 foreach (var way in map.ways)
    {
      if (way.IsLeisure && way.NodeIDs.Count > 1)
    {
    string landuseType = "";
    if (way.Tags.ContainsKey("leisure"))
    {
        landuseType = way.Tags["leisure"];
    }

    Material landuseMaterial = defaultMaterial;

    switch (landuseType)
    {
        case "adult_gaming_centre":
            landuseMaterial = grassMaterial;
            break;
        case "amusement_arcade":
            landuseMaterial = residentialMaterial;
            break;
        case "bandstand":
            landuseMaterial = forestMaterial;
            break;
        case "bathing_place":
            landuseMaterial = parkMaterial;
            break;
        case "beach_resort":
            landuseMaterial = commercialMaterial;
            break;
        case "bird_hide":
            landuseMaterial = industrialMaterial;
            break;
        case "bleachers":
            landuseMaterial = militaryMaterial;
            break;
        case "bowling_alley":
            landuseMaterial = farmMaterial;
            break;
        case "common":
            landuseMaterial = greenfieldMaterial;
            break;
        case "village_green":
            landuseMaterial = village_greenMaterial;
            break;
        case "dance":
            landuseMaterial = flowerbedMaterial;
            break;
        case "disc_golf_course":
            landuseMaterial = parkMaterial;
            break;
        case "dog_park":
            landuseMaterial = constructionMaterial;
            break;
        case "firepit":
            landuseMaterial = railwayMaterial;
            break;
        case "fishing":
            landuseMaterial = railwayMaterial;
            break;
        case "escape_game":
            landuseMaterial = railwayMaterial;
            break;
        case "fitness_centre":
            landuseMaterial = orchardMaterial;
            break;
        case "fitness_station":
            landuseMaterial = healthcareMaterial;
            break;
        case "garden":
            landuseMaterial = parkMaterial;
            break;
        case "golf_course":
            landuseMaterial = greeneryMaterial;
            break;
        case "hackerspace":
            landuseMaterial = cemeteryMaterial;
            break;
        case "horse_riding":
            landuseMaterial = retailMaterial;
            break;
        case "ice_rink":
            landuseMaterial = institutionalMaterial;
            break;
        case "marina":
            landuseMaterial = institutionalMaterial;
            break;
        case "miniature_golf":
            landuseMaterial = institutionalMaterial;
            break;
        case "nature_reserve":
            landuseMaterial = institutionalMaterial;
            break;
        case "outdoor_seating":
            landuseMaterial = institutionalMaterial;
            break;
        case "park":
            landuseMaterial = parkMaterial;
            break;
        case "pitch":
            landuseMaterial = institutionalMaterial;
            break;
        case "playground":
            landuseMaterial = institutionalMaterial;
            break;
        case "resort":
            landuseMaterial = institutionalMaterial;
            break;
        case "sauna":
            landuseMaterial = institutionalMaterial;
            break;
        case "slipway":
            landuseMaterial = institutionalMaterial;
            break;
        case "sports_centre":
            landuseMaterial = institutionalMaterial;
            break;
        case "sports_hall":
            landuseMaterial = institutionalMaterial;
            break;
        case "stadium":
            landuseMaterial = institutionalMaterial;
            break;
        case "summer_camp":
            landuseMaterial = institutionalMaterial;
            break;
        case "swimming_area":
            landuseMaterial = institutionalMaterial;
            break;
        case "swimming_pool":
            landuseMaterial = institutionalMaterial;
            break;
        case "tanning_salon":
            landuseMaterial = institutionalMaterial;
            break;
        case "track":
            landuseMaterial = institutionalMaterial;
            break;
        case "trampoline_park":
            landuseMaterial = institutionalMaterial;
            break;
        case "water_park":
            landuseMaterial = institutionalMaterial;
            break;
        case "wildlife_hide":
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

    // Create the object with the appropriate material
   if (way.NodeIDs.Count <= 45)
                {
                    CreateObject(way, landuseMaterial, $"{way.ID}, {way.Name}, Total References: {totalReferences})");
                }
    yield return null;
}
}
}


    /// <summary>
    /// Build the object using the data from the OsmWay instance.
    /// </summary>
    /// <param name="way">OsmWay instance</param>
    /// <param name="origin">The origin of the structure</param>
    /// <param name="vectors">The vectors (vertices) list</param>
    /// <param name="normals">The normals list</param>
    /// <param name="uvs">The UVs list</param>
    /// <param name="indices">The indices list</param>
    
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

