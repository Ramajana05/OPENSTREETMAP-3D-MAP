using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class WaterWayMaker : InfrastructureBehaviour
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
      if (way.IsWaterway && way.NodeIDs.Count > 1)
    {
    string landuseType = "";
    if (way.Tags.ContainsKey("waterway"))
    {
        landuseType = way.Tags["waterway"];
    }

    Material landuseMaterial = amendityMaterial;

    switch (landuseType)
    {
        case "bar":
            landuseMaterial = amendityMaterial;
            break;
        case "biergarten":
            landuseMaterial = amendityMaterial;
            break;
        case "cafe":
            landuseMaterial = amendityMaterial;
            break;
        case "fast_food":
            landuseMaterial = amendityMaterial;
            break;
        case "food_court":
            landuseMaterial = amendityMaterial;
            break;
        case "ice_cream":
            landuseMaterial = amendityMaterial;
            break;
        case "pub":
            landuseMaterial = amendityMaterial;
            break;
        case "restaurant":
            landuseMaterial = amendityMaterial;
            break;
        case "college":
            landuseMaterial = amendityMaterial;
            break;
        case "dancing_school":
            landuseMaterial = amendityMaterial;
            break;
        case "driving_school":
            landuseMaterial = amendityMaterial;
            break;
        case "first_aid_school":
            landuseMaterial = amendityMaterial;
            break;
        case "language_school":
            landuseMaterial = amendityMaterial;
            break;
        case "library":
            landuseMaterial = amendityMaterial;
            break;
        case "surf_school":
            landuseMaterial = amendityMaterial;
            break;
        case "toy_library":
            landuseMaterial = amendityMaterial;
            break;
        case "research_institute":
            landuseMaterial = amendityMaterial;
            break;
        case "training":
            landuseMaterial = amendityMaterial;
            break;
        case "music_school":
            landuseMaterial = amendityMaterial;
            break;
        case "school":
            landuseMaterial = amendityMaterial;
            break;
        case "traffic_park":
            landuseMaterial = amendityMaterial;
            break;
        case "university":
            landuseMaterial = amendityMaterial;
            break;
        case "bicycle_parking":
            landuseMaterial = amendityMaterial;
            break;
        case "bicycle_repair_station":
            landuseMaterial = amendityMaterial;
            break;
        case "bicycle_rental":
            landuseMaterial = amendityMaterial;
            break;
        case "bicycle_wash":
            landuseMaterial = amendityMaterial;
            break;
        case "boat_rental":
            landuseMaterial = amendityMaterial;
            break;
        case "boat_sharing":
            landuseMaterial = amendityMaterial;
            break;
        case "bus_station":
            landuseMaterial = amendityMaterial;
            break;
        case "car_rental":
            landuseMaterial = amendityMaterial;
            break;
        case "car_sharing":
            landuseMaterial = amendityMaterial;
            break;
        case "car_wash":
            landuseMaterial = amendityMaterial;
            break;
        case "compressed_air":
            landuseMaterial = amendityMaterial;
            break;
        case "vehicle_inspection":
            landuseMaterial = amendityMaterial;
            break;
        case "charging_station":
            landuseMaterial = amendityMaterial;
            break;
        case "driver_training":
            landuseMaterial = amendityMaterial;
            break;
        case "ferry_terminal":
            landuseMaterial = amendityMaterial;
            break;
        case "fuel":
            landuseMaterial = amendityMaterial;
            break;
        case "grit_bin":
            landuseMaterial = amendityMaterial;
            break;
        case "motorcycle_parking":
            landuseMaterial = amendityMaterial;
            break;
        case "parking":
            landuseMaterial = amendityMaterial;
            break;
        case "parking_entrance":
            landuseMaterial = amendityMaterial;
            break;
        case "parking_space":
            landuseMaterial = amendityMaterial;
            break;
        case "taxi":
            landuseMaterial = amendityMaterial;
            break;
        case "weighbridge":
            landuseMaterial = amendityMaterial;
            break;
        case "atm":
            landuseMaterial = amendityMaterial;
            break;
        case "payment_terminal":
            landuseMaterial = amendityMaterial;
            break;
        case "bank":
            landuseMaterial = amendityMaterial;
            break;
        case "bureau_de_change":
            landuseMaterial = amendityMaterial;
            break;
        case "money_transfer":
            landuseMaterial = amendityMaterial;
            break;
        case "payment_centre":
            landuseMaterial = amendityMaterial;
            break;
        case "baby_hatch":
            landuseMaterial = amendityMaterial;
            break;
        case "clinic":
            landuseMaterial = amendityMaterial;
            break;
        case "dentist":
            landuseMaterial = amendityMaterial;
            break;
        case "doctors":
            landuseMaterial = amendityMaterial;
            break;
        case "hospital":
            landuseMaterial = amendityMaterial;
            break;
        case "nursing_home":
            landuseMaterial = amendityMaterial;
            break;
        case "pharmacy":
            landuseMaterial = amendityMaterial;
            break;
        case "veterinary":
            landuseMaterial = amendityMaterial;
            break;
        case "arts_centre":
            landuseMaterial = amendityMaterial;
            break;
        case "brothel":
            landuseMaterial = amendityMaterial;
            break;
        case "casino":
            landuseMaterial = amendityMaterial;
            break;
        case "cinema":
            landuseMaterial = amendityMaterial;
            break;
        case "community_centre":
            landuseMaterial = amendityMaterial;
            break;
        case "conference_centre":
            landuseMaterial = amendityMaterial;
            break;
        case "events_venue":
            landuseMaterial = amendityMaterial;
            break;
        case "exhibition_centre":
            landuseMaterial = amendityMaterial;
            break;
        case "fountain":
            landuseMaterial = amendityMaterial;
            break;
        case "gambling":
            landuseMaterial = amendityMaterial;
            break;
        case "love_hotel":
            landuseMaterial = amendityMaterial;
            break;
        case "music_venue":
            landuseMaterial = amendityMaterial;
            break;
        case "nightclub":
            landuseMaterial = amendityMaterial;
            break;
        case "planetarium":
            landuseMaterial = amendityMaterial;
            break;
        case "public_bookcase":
            landuseMaterial = amendityMaterial;
            break;
        case "social_centre":
            landuseMaterial = amendityMaterial;
            break;
        case "stage":
            landuseMaterial = amendityMaterial;
            break;
        case "stripclub":
            landuseMaterial = amendityMaterial;
            break;
        case "swingerclub":
            landuseMaterial = amendityMaterial;
            break;
        case "theatre":
            landuseMaterial = amendityMaterial;
            break;
             case "courthouse":
            landuseMaterial = amendityMaterial;
            break;
        case "fire_station":
            landuseMaterial = amendityMaterial;
            break;
        case "police":
            landuseMaterial = amendityMaterial;
            break;
        case "post_box":
            landuseMaterial = amendityMaterial;
            break;
        case "post_depot":
            landuseMaterial = amendityMaterial;
            break;
        case "post_office":
            landuseMaterial = amendityMaterial;
            break;
        case "prison":
            landuseMaterial = amendityMaterial;
            break;
        case "ranger_station":
            landuseMaterial = amendityMaterial;
            break;
             case "townhall":
            landuseMaterial = amendityMaterial;
            break;
        case "bbq":
            landuseMaterial = amendityMaterial;
            break;
        case "bench":
            landuseMaterial = amendityMaterial;
            break;
        case "dog_toilet":
            landuseMaterial = amendityMaterial;
            break;
        case "dressing_room":
            landuseMaterial = amendityMaterial;
            break;
        case "drinking_water":
            landuseMaterial = amendityMaterial;
            break;
        case "give_box":
            landuseMaterial = amendityMaterial;
            break;
        case "mailroom":
            landuseMaterial = amendityMaterial;
            break;
        case "parcel_locker":
            landuseMaterial = amendityMaterial;
            break;
        case "shelter":
            landuseMaterial = amendityMaterial;
            break;
        case "shower":
            landuseMaterial = amendityMaterial;
            break;
        case "telephone":
            landuseMaterial = amendityMaterial;
            break;
        case "toilets":
            landuseMaterial = amendityMaterial;
            break;
        case "water_point":
            landuseMaterial = amendityMaterial;
            break;
        case "watering_place":
            landuseMaterial = amendityMaterial;
            break;
        case "sanitary_dump_station":
            landuseMaterial = amendityMaterial;
            break;
        case "recycling":
            landuseMaterial = amendityMaterial;
            break;
        case "waste_basket":
            landuseMaterial = amendityMaterial;
            break;
        case "waste_transfer_station":
            landuseMaterial = amendityMaterial;
            break;
        case "waste_disposal":
            landuseMaterial = amendityMaterial;
            break;
        case "animal_boarding":
            landuseMaterial = amendityMaterial;
            break;
        case "animal_breeding":
            landuseMaterial = amendityMaterial;
            break;
        case "animal_shelter":
            landuseMaterial = amendityMaterial;
            break;
        case "animal_training":
            landuseMaterial = amendityMaterial;
            break;
        case "baking_oven":
            landuseMaterial = amendityMaterial;
            break;
        case "clock":
            landuseMaterial = amendityMaterial;
            break;
        case "crematorium":
            landuseMaterial = amendityMaterial;
            break;
        case "dive_centre":
            landuseMaterial = amendityMaterial;
            break;
        case "funeral_hall":
            landuseMaterial = amendityMaterial;
            break;
        case "grave_yard":
            landuseMaterial = amendityMaterial;
            break;
        case "hunting_stand":
            landuseMaterial = amendityMaterial;
            break;
        case "internet_cafe":
            landuseMaterial = amendityMaterial;
            break;
        case "kitchen":
            landuseMaterial = amendityMaterial;
            break;
        case "kneipp_water_cure":
            landuseMaterial = amendityMaterial;
            break;
        case "lounger":
            landuseMaterial = amendityMaterial;
            break;
        case "marketplace":
            landuseMaterial = amendityMaterial;
            break;
        case "monastery":
            landuseMaterial = amendityMaterial;
            break;
        case "mortuary":
            landuseMaterial = amendityMaterial;
            break;
        case "photo_booth":
            landuseMaterial = amendityMaterial;
            break;
        case "place_of_mourning":
            landuseMaterial = amendityMaterial;
            break;
        case "place_of_worship":
            landuseMaterial = amendityMaterial;
            break;
        case "public_bath":
            landuseMaterial = amendityMaterial;
            break;
        case "refugee_site":
            landuseMaterial = amendityMaterial;
            break;
        case "vending_machine":
            landuseMaterial = amendityMaterial;
            break;
        case "user defined":
            landuseMaterial = amendityMaterial;
            break;

        default:
            landuseMaterial = defaultMaterial;
            break;
    }
    
    // Create the object with the appropriate material
    CreateObject(way, landuseMaterial, $"{way.Name}, {way.ID})");
    yield return null;
}}
}
    
    protected override void OnObjectCreated(OsmWay way, Vector3 origin, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices)
    {
        int height = 0;
        // Get the centre of the roof
        Vector3 oTop = new Vector3(0, height, 0);

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
            Vector3 v3 = v1 + new Vector3(0, height, 0);
            Vector3 v4 = v2 + new Vector3(0, height, 0);

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


