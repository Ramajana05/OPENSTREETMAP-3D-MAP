using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSM.Model;

class TreeMaker : InfrastructureBehaviour
{
    // Add public variables for the tree prefabs
    public GameObject treePrefab;
    public GameObject broadleavedTreePrefab;

    // Adjust this value based on your specific terrain height
    public float terrainHeight = 0f;

    IEnumerator Start()
    {
        // Wait until the map is ready
        while (!map.IsReady)
        {
            yield return null;
        }
        
        Debug.Log("Map is ready");

        if (map.nodes == null)
        {
            Debug.LogError("map.nodes is null");
            yield break;
        }

        Debug.Log($"Total nodes: {map.nodes.Count}");

        // Iterate through all the nodes in the 'nodes' list
        foreach (var node in map.nodes.Values)
        {

            if (node.IsTree)
            {
                // Create the object with the appropriate tree prefab
                //Debug.Log($"Creating tree for node {node.ID}");
                CreateObject(node, null, $"{node.ID}");
                yield return null;
            }
        }
    }

    protected override void OnObjectCreated(OsmWay way, Vector3 origin, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices)
    {
        // Iterate through the node IDs in the way
        foreach (var nodeId in way.NodeIDs)
        {
            // Get the node
            OsmNode node = map.nodes[nodeId];
            
            // Check if the node represents a tree
            if (node.IsTree)
            {
                CreateObject(node, null, $"{node.ID}");
            }
        }
    }

    protected override void OnObjectCreated(OsmNode node, Vector3 position, List<Vector3> vectors, List<Vector3> normals, List<Vector2> uvs, List<int> indices)
    {
        // Convert node geographic coordinates to world coordinates relative to the map center
        float x = (float)MercatorProjection.lonToX(node.Longitude) - position.x;
        float z = (float)MercatorProjection.latToY(node.Latitude) - position.z;
        Vector3 treePosition = new Vector3(x, GetElevation(node), z);

        // Adjust the elevation based on terrain height
        treePosition.y = GetElevation(node);

        // Adjust the position to center the tree object
        treePosition += Vector3.one;

        // Create an instance of the appropriate tree object with the specified prefab
        Instantiate(GetTreePrefab(node), treePosition, Quaternion.identity);
    }

    // Example function to get elevation data
    float GetElevation(OsmNode node)
    {
        // You can implement your logic here to obtain elevation data
        // For demonstration purposes, returning a constant value
        return terrainHeight; // Set the elevation based on your terrain height
    }

    protected void CreateObject(OsmNode node, Material mat, string objectName)
    {
        // Make sure we have some name to display
        objectName = string.IsNullOrEmpty(objectName) ? "OsmNode" : objectName;

        // Create an instance of the object and place it in the centre of its points
        GameObject go = new GameObject(objectName);
        Vector3 position = node.Position - map.bounds.Centre;
        go.transform.position = position;

        // Add the appropriate tree prefab as a visual representation
        GameObject tree = Instantiate(GetTreePrefab(node));
        tree.transform.SetParent(go.transform);
        tree.transform.localPosition = Vector3.zero;

        go.tag = "Tree";

        // Set the parent transform
        go.transform.SetParent(transform);
    }

    private GameObject GetTreePrefab(OsmNode node)
    {
        // Check if the node has the 'leaf_type' tag set to 'broadleaved'
        if (node.Tags != null && node.Tags.ContainsKey("leaf_type") && node.Tags["leaf_type"] == "broadleaved")
        {
            return broadleavedTreePrefab;
        }
        else
        {
            return treePrefab;
        }
    }
}
