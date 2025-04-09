using UnityEngine;

namespace OSM.Model
{
    [System.Serializable]
    public class Location
    {
        public int LocationID;
        public string Timestamp;
        public int XmlLocationID;
        public string XmlLocationName;
        public string XmlLocationCity;
        public string XmlLocationCountry;
        public User User;
    }
}
