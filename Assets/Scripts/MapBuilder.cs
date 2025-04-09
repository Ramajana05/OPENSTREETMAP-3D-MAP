using UnityEngine;

public class MapBuilder : MonoBehaviour
{
    public Camera camera; // Public variable to specify the camera to use
    private MapReader mapReader;
    private Vector3 lastCameraPosition;
    public float cameraMovementThreshold = 1f;

    void Start()
    {
        mapReader = GetComponent<MapReader>();
        if (camera == null)
        {
            Debug.LogError("Camera reference not set in MapBuilder!");
            return;
        }
        lastCameraPosition = camera.transform.position; // Use the specified camera's position

        // Start by fetching and building data for the initial bounding box (MAP)
        StartCoroutine(mapReader.FetchDataAndInitialize());
    }

    void Update()
    {
        if (camera == null)
        {
            Debug.LogError("Camera reference not set in MapBuilder!");
            return;
        }

        // Check if the camera has moved beyond the threshold
        if (Vector3.Distance(lastCameraPosition, camera.transform.position) > cameraMovementThreshold)
        {
            // Determine the direction of camera movement
            Vector3 cameraMovement = camera.transform.position - lastCameraPosition;

            // Update last camera position
            lastCameraPosition = camera.transform.position;

            // Expand the bounding box and fetch data
            ExpandAndFetch(cameraMovement);
        }
    }

    void ExpandAndFetch(Vector3 cameraMovement)
    {
        if (camera == null)
        {
            Debug.LogError("Camera reference not set in MapBuilder!");
            return;
        }

        // Determine the direction and fetch the corresponding bounding box
        if (Mathf.Abs(cameraMovement.x) > Mathf.Abs(cameraMovement.z))
        {
            // Horizontal movement
            if (cameraMovement.x > 0)
            {
                // Move right
                float newLon = mapReader.bounds.MaxLon + mapReader.bboxSize;
                StartCoroutine(mapReader.FetchDataForBoundingBox(mapReader.CurrentLat, newLon, mapReader.bboxSize));
            }
            else
            {
                // Move left
                float newLon = mapReader.bounds.MinLon - mapReader.bboxSize;
                StartCoroutine(mapReader.FetchDataForBoundingBox(mapReader.CurrentLat, newLon, mapReader.bboxSize));
            }
        }
        else
        {
            // Vertical movement
            if (cameraMovement.z > 0)
            {
                // Move up
                float newLat = mapReader.bounds.MaxLat + mapReader.bboxSize;
                StartCoroutine(mapReader.FetchDataForBoundingBox(newLat, mapReader.CurrentLon, mapReader.bboxSize));
            }
            else
            {
                // Move down
                float newLat = mapReader.bounds.MinLat - mapReader.bboxSize;
                StartCoroutine(mapReader.FetchDataForBoundingBox(newLat, mapReader.CurrentLon, mapReader.bboxSize));
            }
        }
    }

    public void BuildMapAroundLocation(float lat, float lon, float size)
    {
        if (camera == null)
        {
            Debug.LogError("Camera reference not set in MapBuilder!");
            return;
        }

        StartCoroutine(mapReader.FetchDataForBoundingBox(lat, lon, size));
    }
}
