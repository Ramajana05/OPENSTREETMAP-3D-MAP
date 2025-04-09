using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSM.Model;
using System.Runtime.InteropServices;

[RequireComponent(typeof(MapReader))]
abstract class InfrastructureBehaviour : MonoBehaviour
{

    protected MapReader map;

    void Awake()
    {
        map = GetComponent<MapReader>();
    }

    protected Vector3 GetCentre(OsmWay way)
    {
        Vector3 total = Vector3.zero;

        foreach (var id in way.NodeIDs)
        {
            total += map.nodes[id];
        }

        return total / way.NodeIDs.Count;
    }

    protected void CreateObject(OsmWay way, Material mat, string objectName)
    {
        
        objectName = string.IsNullOrEmpty(objectName) ? "OsmWay" : objectName;

        GameObject go = new GameObject(objectName);
        Vector3 localOrigin = GetCentre(way);
        go.transform.position = localOrigin - map.bounds.Centre;

        Way wayComponent = go.AddComponent<Way>();
        wayComponent.ID = way.ID;
        wayComponent.Name = objectName;
        wayComponent.Longitude = way.Longitude;
        wayComponent.Latitude = way.Latitude;
        wayComponent.City = way.City;

        // Create the collections for the object's vertices, indices, UVs etc.
        List<Vector3> vectors = new List<Vector3>();
        List<Vector3> normals = new List<Vector3>();
        List<Vector2> uvs = new List<Vector2>();
        List<int> indices = new List<int>();

        // Call the child class' object creation code
        OnObjectCreated(way, localOrigin, vectors, normals, uvs, indices);

        // Check if the mesh has enough vertices
        if (vectors.Count < 3 || indices.Count < 3)
        {
            Debug.LogError($"Mesh must have at least three distinct vertices to be a valid collision mesh. Object: {objectName}");
            Destroy(go); // Destroy the GameObject
            return;
        }

        // Add the mesh filter and renderer components to the object
        MeshFilter mf = go.AddComponent<MeshFilter>();
        MeshRenderer mr = go.AddComponent<MeshRenderer>();

        // Enable shadows
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        mr.receiveShadows = true;

        // Apply the material
        mr.material = mat;
        go.tag = "Object";

        // Apply the data to the mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vectors.ToArray();
        mesh.normals = normals.ToArray();
        mesh.triangles = indices.ToArray();
        mesh.uv = uvs.ToArray();
        mf.mesh = mesh;

        MeshCollider meshCollider = go.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true; 
        meshCollider.isTrigger = true; 

        Rigidbody rb = go.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        go.transform.SetParent(transform);

        if (objectName.Contains("Building"))
        {   
            go.tag = "Building";
        }
        else if (objectName.Contains("Road"))
        {
            go.tag = "Road";
        } 
        else if (objectName.Contains("Tree"))
        {
            go.tag = "Tree";
        }
        else if (objectName.Contains("Object"))
        {
            go.tag = "Object";
        }
    }

     protected void CreateObject(OsmNode node, Material mat, string objectName)
    {
        objectName = string.IsNullOrEmpty(objectName) ? "OsmNode" : objectName;

        GameObject go = new GameObject(objectName);
        Vector3 position = node.Position - map.bounds.Centre;
        go.transform.position = position;

        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.SetParent(go.transform);
        sphere.transform.localPosition = Vector3.zero;
        sphere.transform.localScale = Vector3.one * 10f; 
        MeshRenderer renderer = sphere.GetComponent<MeshRenderer>();
        renderer.material = mat;

        go.tag = "Object";

        go.transform.SetParent(transform);
    }


    protected abstract void OnObjectCreated(OsmWay way, Vector3 origin, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices);

    protected virtual void OnObjectCreated(OsmNode node, Vector3 position, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices)
    {
        
    }

  
    public void ChangeMaterial(GameObject building, Material newMaterial)
    {
        Renderer renderer = building.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = newMaterial;
        }
    }

    /*
    public void SendLocationData(Way way, OsmNode node)
    {
        Location location = new Location
        {
            userID = "1", // Replace with appropriate userID (converted to string)
            xmllocationID = way.ID.ToString(), // Convert to string
            xmllocationname = way.Name,
            longitude = node.Longitude.ToString(), // Convert to string
            latitude = node.Latitude.ToString(), // Convert to string
            xmllocationcity = way.City,
            xmllocationcountry = "DE" // Replace with appropriate country if necessary
        };

        // Now you can send 'location' data to your server or perform any other necessary actions
        Debug.Log("Sending location data: " + JsonUtility.ToJson(location));
        // You can add the logic to send 'location' data to your server using an HTTP request here
    }
    */
}
