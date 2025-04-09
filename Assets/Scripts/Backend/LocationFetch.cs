using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using OSM.Model; // Import the Location and User classes from the Model namespace

public class LocationFetch : MonoBehaviour
{
    public string getUrl = "http://localhost:3000/locations"; // URL for getting location data

    // Start is called before the first frame update
    void Start()
    {
        // Fetch and print locations when the game starts
        StartCoroutine(GetLocations());
    }

    // Method to fetch locations from the backend
    IEnumerator GetLocations()
    {
        UnityWebRequest request = UnityWebRequest.Get(getUrl);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching locations: " + request.error);
        }
        else
        {
            Debug.Log("Locations fetched successfully!");

            // Deserialize JSON array into array of Location objects
            Location[] locations = JsonHelperForLocations.FromJson<Location>(request.downloadHandler.text);

            // Print out each fetched location separately
            foreach (Location location in locations)
            {
                Debug.Log("LocationID: " + location.LocationID);
                Debug.Log("Timestamp: " + location.Timestamp);
                Debug.Log("XmlLocationID: " + location.XmlLocationID);
                Debug.Log("XmlLocationName: " + location.XmlLocationName);
                Debug.Log("XmlLocationCity: " + location.XmlLocationCity);
                Debug.Log("XmlLocationCountry: " + location.XmlLocationCountry);
                Debug.Log("UserID: " + location.User.UserID);
                Debug.Log("Username: " + location.User.Username);
                Debug.Log("------------------------------------");
            }
        }
    }
    public static class JsonHelperForLocations
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T> { Items = array };
            return JsonUtility.ToJson(wrapper);
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
