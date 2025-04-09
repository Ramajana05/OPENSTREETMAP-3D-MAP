using System.Runtime.InteropServices;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using OSM.Model;

public class UserFetch : MonoBehaviour
{
    private string url = "http://localhost:3000/users";

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetUsers());
    }

    IEnumerator GetUsers()
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            // Show results as text
            Debug.Log(request.downloadHandler.text);

            // Parse the JSON data
            User[] users = JsonHelper.FromJson<User>(request.downloadHandler.text);

            // Output the user details to the console
            foreach (var user in users)
            {
                Debug.Log($"ID: {user.UserID}, Name: {user.Username}, Email: {user.Email}, Color: {user.Color}, Last Online: {user.LastTimeOnline}");
            }
        }
    }
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
}
