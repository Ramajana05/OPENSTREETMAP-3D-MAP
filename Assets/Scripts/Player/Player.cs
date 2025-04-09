using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSM.Model;

public class Player : MonoBehaviour
{
    public float startingLong; // Starting longitude
    public float startingLat; // Starting latitude

    public float currentLong; // Current longitude
    public float currentLat; // Current latitude

    private Vector3 mapCenter; // The center of the map in Unity coordinates
    private MapReader mapReader; // Reference to the MapReader

    public Material collisionMaterial; // Material to apply on collision
    private BuildingMaker buildingMaker; // Reference to BuildingMaker

    public float moveSpeed = 1.0f; // Speed of smooth movement
    public float updateThreshold = 0.01f; // Threshold for updating map data (in degrees)

    private float previousLong;
    private float previousLat;
    

    public float CurrentLong
    {
        get => currentLong;
        set
        {
            currentLong = value;
            StartCoroutine(SmoothMoveToNewPosition());
            CheckForMapUpdate();
        }
    }

    public float CurrentLat
    {
        get => currentLat;
        set
        {
            currentLat = value;
            StartCoroutine(SmoothMoveToNewPosition());
            CheckForMapUpdate();
        }
    }

    void Start()
    {
 
        currentLong = startingLong;
        currentLat = startingLat;
        previousLong = startingLong;
        previousLat = startingLat;

        mapReader = FindObjectOfType<MapReader>();

        if (mapReader == null)
        {
           
            return;
        }

   
        buildingMaker = FindObjectOfType<BuildingMaker>();

        if (buildingMaker == null)
        {
         
            return;
        }

        StartCoroutine(WaitForMapReaderReady());
    }

    private IEnumerator WaitForMapReaderReady()
    {
        
        while (!mapReader.IsReady)
        {
            yield return null;
        }

        if (mapReader.bounds == null)
        {
           
            yield break;
        }

       
        mapCenter = CalculateMapCenter();

       
        UpdatePosition();
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.W))
        {
            CurrentLat += 0.0001f; // Move north
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            CurrentLat -= 0.0001f; // Move south
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            CurrentLong -= 0.0001f; // Move west
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            CurrentLong += 0.0001f; // Move east
        }

       
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            UpdateLatLongFromPosition();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            UpdateLatLongFromPosition();
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * moveSpeed * Time.deltaTime;
            UpdateLatLongFromPosition();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * moveSpeed * Time.deltaTime;
            UpdateLatLongFromPosition();
        }
    }

    void UpdatePosition()
    {
       
        float x = MercatorProjection.lonToX(currentLong);
        float z = MercatorProjection.latToY(currentLat);

        
        Vector3 position = new Vector3(x, 0f, z) - mapCenter;

    
        transform.position = position;
    }

    void UpdateLatLongFromPosition()
    {
       
        Vector3 worldPosition = transform.position + mapCenter;
        currentLong = MercatorProjection.xToLon(worldPosition.x);
        currentLat = MercatorProjection.yToLat(worldPosition.z);
        CheckForMapUpdate();
    }

    private IEnumerator SmoothMoveToNewPosition()
    {
        Vector3 startPosition = transform.position;
        float x = MercatorProjection.lonToX(currentLong);
        float z = MercatorProjection.latToY(currentLat);
        Vector3 targetPosition = new Vector3(x, 0f, z) - mapCenter;

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime);
            elapsedTime += Time.deltaTime * moveSpeed;
            yield return null;
        }

        transform.position = targetPosition;
    }

   private void OnTriggerEnter(Collider other)
{
   
    if (other.CompareTag("Building") || other.CompareTag("Road") || other.CompareTag("Leisure") || other.CompareTag("Landuse") || other.CompareTag("Object"))
    {
        
        Renderer renderer = other.GetComponent<Renderer>();
        if (renderer != null && collisionMaterial != null)
        {
            renderer.material = collisionMaterial;
          
        }
        else
        {
            
        }
        }
        
          Way way = other.GetComponent<Way>(); 
        if (way != null)
        {
            SendLocationData(way);
        }

         if (other.CompareTag("Tree"))
    {
       
        Renderer[] renderers = other.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer treeRenderer in renderers)
        {
            if (treeRenderer != null && collisionMaterial != null)
            {
                treeRenderer.material = collisionMaterial;
                Debug.Log("Tree material changed to green.");
            }
        }
    
    }
}

private void SendLocationData(Way way)
{
    string[] nameParts = way.Name.Split(',');
    if (nameParts.Length < 2)
    {
        Debug.LogError("Invalid way name format: " + way.Name);
        return;
    }

    ulong id;
    if (!ulong.TryParse(nameParts[0], out id))
    {
        Debug.LogError("Failed to parse ID in way name: " + way.Name);
        return;
    }

    string name = nameParts[1].Trim();
    


    Location location = new Location
    {
        XmlLocationID = (int)way.ID, // Explicitly cast ulong to int
        XmlLocationName = name,
        XmlLocationCity = way.City,
        XmlLocationCountry = "DE", // Replace with appropriate country if necessary
        User = new User { UserID = 1, Username = "Saaasshra" }
    };

    
    Debug.Log("Sending location data: " + JsonUtility.ToJson(location));
    
}



    Vector3 CalculateMapCenter()
    {
        if (mapReader.bounds == null)
        {
            
            return Vector3.zero;
        }

     
        float centerX = MercatorProjection.lonToX((mapReader.bounds.MinLon + mapReader.bounds.MaxLon) / 2);
        float centerZ = MercatorProjection.latToY((mapReader.bounds.MinLat + mapReader.bounds.MaxLat) / 2);

        return new Vector3(centerX, 0f, centerZ);
    }

    private void CheckForMapUpdate()
    {
        if (Mathf.Abs(currentLat - previousLat) > updateThreshold || Mathf.Abs(currentLong - previousLong) > updateThreshold)
        {
            previousLat = currentLat;
            previousLong = currentLong;

            mapReader.CurrentLat = currentLat;
            mapReader.CurrentLon = currentLong;

            StartCoroutine(UpdateMapData());
        }
    }

    public IEnumerator UpdateMapData()
    {
      
        yield return mapReader.FetchDataAndInitialize();

       
        if (buildingMaker != null)
        {
            buildingMaker.GenerateBuildings();
        }
    }
   public void ReceiveCoordinates(string jsonString)
    {
        var coords = JsonUtility.FromJson<Coordinates>(jsonString);
        CurrentLat = float.Parse(coords.latitude);
        CurrentLong = float.Parse(coords.longitude);
        startingLat = CurrentLat;
        startingLong = CurrentLong;
    }

    [System.Serializable]
    public class Coordinates
    {
        public string latitude;
        public string longitude;
    }


    public static class MercatorProjection
    {
        public static float lonToX(float lon)
        {
            return lon * 20037508.34f / 180f;
        }

        public static float latToY(float lat)
        {
            float rad = lat * Mathf.Deg2Rad;
            return Mathf.Log(Mathf.Tan(rad) + 1 / Mathf.Cos(rad)) * 20037508.34f / Mathf.PI;
        }

        public static float xToLon(float x)
        {
            return x * 180f / 20037508.34f;
        }

        public static float yToLat(float y)
        {
            float rad = y * Mathf.PI / 20037508.34f;
            return Mathf.Rad2Deg * (2 * Mathf.Atan(Mathf.Exp(rad)) - Mathf.PI / 2);
        }
    }
}
