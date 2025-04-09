using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml;


public class DataFetcher : MonoBehaviour

{
    // URL of the Overpass API
    private string overpassUrl = "http://overpass-api.de/api/interpreter";
private string overpassQuery = @"
<osm-script output='xml'>
  <union>
    <query type='node'>
      <bbox-query s='48.8' n='48.8663994' w='9.0386007' e='9.2160228'/>
    </query>
  </union>
  <print/>
</osm-script>";


    void Start()
    {
        // Start the coroutine to fetch OSM data
        StartCoroutine(FetchOSMData());
    }

    IEnumerator FetchOSMData()
    {
        // Create the full URL with the query
        string fullUrl = overpassUrl + "?data=" + UnityWebRequest.EscapeURL(overpassQuery);

        // Create a UnityWebRequest
        UnityWebRequest www = UnityWebRequest.Get(fullUrl);

        // Send the request and wait for the response
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("Error fetching OSM data: " + www.error);
        }
        else
        {
            // Get the XML data from the response
            string xmlData = www.downloadHandler.text;
            // Process the XML data
            ProcessOSMData(xmlData);
        }
    }

    void ProcessOSMData(string xmlData)
{
    XmlDocument doc = new XmlDocument();
    doc.LoadXml(xmlData);

    Debug.Log("XML Data:");
    Debug.Log(xmlData);

   
    
}

}
