using UnityEngine;

namespace OSM.Model
{
    [System.Serializable]
    public class User
    {
    public int UserID;
    public string Username;
    public string Email;
    public string Password; // Optional, handle with care if needed
    public string Color;
    public string LastTimeOnline;
    public bool Visibility;
    public string CreatedAt;
    public string UpdateDate;
    public float Current_Latitude;
    public float Current_Longitude;

    }
}
