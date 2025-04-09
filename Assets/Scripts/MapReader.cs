using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml;


class MapReader : MonoBehaviour
{
    [HideInInspector]
    public Dictionary<ulong, OsmNode> nodes;

    [HideInInspector]
    public List<OsmWay> ways;

    [HideInInspector]
    public List<OsmRelation> relations;

    [HideInInspector]
    public OsmBounds bounds;

    public GameObject groundPlane;
    public GameObject debugTextObject; // Assign a Text object in the inspector to display debug info

    public bool IsReady { get; private set; }

    private float _currentLat = 48.7851448f;
    private float _currentLon = 9.20584f;
    public float bboxSize = 0.006f; // Size of the bounding box in degrees

    private string overpassUrl = "http://overpass-api.de/api/interpreter";

    private MapBuilder mapBuilder;

    void Start()
    {
        nodes = new Dictionary<ulong, OsmNode>();
        ways = new List<OsmWay>();
        relations = new List<OsmRelation>();

        mapBuilder = GetComponent<MapBuilder>();

        // Initial coordinates must be valid
        if (_currentLat != 0 && _currentLon != 0)
        {
            StartCoroutine(FetchDataAndInitialize());
        }
    }

    public IEnumerator FetchDataAndInitialize()
    {
        if (_currentLat == 0 || _currentLon == 0)
        {
            Debug.LogError("Invalid coordinates, skipping FetchDataAndInitialize.");
            yield break;
        }

        yield return FetchData();
        if (bounds != null)
        {
            UpdateGroundPlaneScale();
        }
    }

    IEnumerator FetchData()
    {
        var (south, north, west, east) = GetBoundingBox(_currentLat, _currentLon, bboxSize);
        string overpassQuery = GetOverpassQuery(south, north, west, east);
        string fullUrl = overpassUrl + "?data=" + UnityWebRequest.EscapeURL(overpassQuery);
        UnityWebRequest www = UnityWebRequest.Get(fullUrl);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching OSM data: " + www.error);
        }
        else
        {
            string xmlData = www.downloadHandler.text;
            Task.Run(() => ProcessOSMDataAsync(xmlData)).ContinueWith((task) =>
            {
                if (bounds != null)
                {
                    UpdateGroundPlaneScale();
                }
                else
                {
                    Debug.LogError("Bounds are not set after processing OSM data.");
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    public float CurrentLat
    {
        get { return _currentLat; }
        set
        {
            if (_currentLat != value)
            {
                _currentLat = value;
                UpdateBoundingBox();
            }
        }
    }

    public float CurrentLon
    {
        get { return _currentLon; }
        set
        {
            if (_currentLon != value)
            {
                _currentLon = value;
                UpdateBoundingBox();
            }
        }
    }

    void UpdateBoundingBox()
    {
        if (_currentLat == 0 || _currentLon == 0)
        {
            Debug.LogError("Invalid coordinates, skipping UpdateBoundingBox.");
            return;
        }

        SetBounds(_currentLat, _currentLon, bboxSize);
        StartCoroutine(FetchDataAndInitialize());
        Debug.Log($"Updated location: Lat: {_currentLat}, Lon: {_currentLon}");
        var (south, north, west, east) = GetBoundingBox(_currentLat, _currentLon, bboxSize);
        Debug.Log($"Bounding Box: South: {south}, North: {north}, West: {west}, East: {east}");

        // Display on screen
        if (debugTextObject != null)
        {
            var textComponent = debugTextObject.GetComponent<UnityEngine.UI.Text>();
            if (textComponent != null)
            {
                textComponent.text = $"Lat: {_currentLat}, Lon: {_currentLon}";
            }
        }
    }

    void SetBounds(float lat, float lon, float size)
    {
        var (minLat, maxLat, minLon, maxLon) = GetBoundingBox(lat, lon, size);
        bounds = new OsmBounds(minLat, minLon, maxLat, maxLon);
        Debug.Log($"Bounds set: MinLat = {minLat}, MinLon = {minLon}, MaxLat = {maxLat}, MaxLon = {maxLon}");
    }

    (float, float, float, float) GetBoundingBox(float lat, float lon, float size)
    {
        float south = lat - size;
        float north = lat + size;
        float west = lon - size;
        float east = lon + size;
        return (south, north, west, east);
    }

    string GetOverpassQuery(float south, float north, float west, float east)
    {
        return $@"
<osm-script output='xml' timeout='25'>
  <union>
    <bbox-query s='{south}' n='{north}' w='{west}' e='{east}'/>
    <query type='node'>
      <bbox-query s='{south}' n='{north}' w='{west}' e='{east}'/>
    </query>
    <query type='way'>
      <bbox-query s='{south}' n='{north}' w='{west}' e='{east}'/>
    </query>
  </union>
  <union>
    <item/>
    <recurse type='down'/>
  </union>
  <print mode='meta' order='quadtile'/>
</osm-script>
";
    }

    void UpdateGroundPlaneScale()
    {
        if (bounds == null)
        {
            Debug.LogError("Bounds not set before updating ground plane scale.");
            return;
        }

        float minx = (float)MercatorProjection.lonToX(bounds.MinLon);
        float maxx = (float)MercatorProjection.lonToX(bounds.MaxLon);
        float miny = (float)MercatorProjection.latToY(bounds.MinLat);
        float maxy = (float)MercatorProjection.latToY(bounds.MaxLat);

        groundPlane.transform.localScale = new Vector3((maxx - minx) / 2, 1, (maxy - miny) / 2);
    }

    async Task ProcessOSMDataAsync(string xmlData)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlData);

        Debug.Log("Parsing nodes...");
        await Task.Run(() => GetNodes(doc.SelectNodes("/osm/node")));
        Debug.Log("Parsing ways...");
        await Task.Run(() => GetWays(doc.SelectNodes("/osm/way")));
        Debug.Log("Parsing relations...");
        await Task.Run(() => GetRelations(doc.SelectNodes("/osm/relation")));

        if (nodes.Count > 0 && ways.Count > 0)
        {
            SetBounds(_currentLat, _currentLon, bboxSize);
            IsReady = true;
        
        }
        else
        {
            Debug.LogError("Nodes or ways are not parsed.");
        }
    }

    IEnumerator ContinuousMapBuilding()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // Adjust the time interval as needed
            mapBuilder.BuildMapAroundLocation(_currentLat, _currentLon, bboxSize);
        }
    }

     IEnumerator FetchAdjacentBoundingBoxes()
    {
        // Fetch top bounding box
        yield return FetchDataForBoundingBox(bounds.MaxLat + bboxSize, _currentLon, bboxSize);
        // Fetch bottom bounding box
        yield return FetchDataForBoundingBox(bounds.MinLat - bboxSize, _currentLon, bboxSize);
        // Fetch left bounding box
        yield return FetchDataForBoundingBox(_currentLat, bounds.MinLon - bboxSize, bboxSize);
        // Fetch right bounding box
        yield return FetchDataForBoundingBox(_currentLat, bounds.MaxLon + bboxSize, bboxSize);
    }

    public IEnumerator FetchDataForBoundingBox(float lat, float lon, float size)
    {
        if (lat == 0 || lon == 0)
        {
            Debug.LogError("Invalid coordinates, skipping FetchDataForBoundingBox.");
            yield break;
        }

        var (south, north, west, east) = GetBoundingBox(lat, lon, size);
        string overpassQuery = GetOverpassQuery(south, north, west, east);
        string fullUrl = overpassUrl + "?data=" + UnityWebRequest.EscapeURL(overpassQuery);
        UnityWebRequest www = UnityWebRequest.Get(fullUrl);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching OSM data: " + www.error);
        }
        else
        {
            string xmlData = www.downloadHandler.text;
            Task.Run(() => ProcessOSMDataAsync(xmlData)).ContinueWith((task) =>
            {
                if (bounds != null)
                {
                    UpdateGroundPlaneScale();
                }
                else
                {
                    Debug.LogError("Bounds are not set after processing OSM data.");
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }

    public void ReceiveCoordinates(string jsonString)
    {
        var coords = JsonUtility.FromJson<Coordinates>(jsonString);
        _currentLat = float.Parse(coords.latitude);
        _currentLon = float.Parse(coords.longitude);

        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            player.ReceiveCoordinates(jsonString);
        }

        UpdateBoundingBox();
    }

    [System.Serializable]
    public class Coordinates
    {
        public string latitude;
        public string longitude;
    }

    void GetNodes(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode n in xmlNodeList)
        {
            OsmNode node = new OsmNode(n);
            nodes[node.ID] = node;
        }
        Debug.Log("Number of nodes parsed: " + nodes.Count);
    }

    void GetWays(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            OsmWay way = new OsmWay(node);
            ways.Add(way);
        }
        Debug.Log("Number of ways parsed: " + ways.Count);
    }

    void GetRelations(XmlNodeList xmlNodeList)
    {
        foreach (XmlNode node in xmlNodeList)
        {
            OsmRelation relation = new OsmRelation(node);
            relations.Add(relation);
        }
        Debug.Log("Number of relations parsed: " + relations.Count);
    }
}
