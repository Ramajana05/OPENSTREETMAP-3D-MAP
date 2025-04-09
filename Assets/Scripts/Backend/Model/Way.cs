using UnityEngine;

namespace OSM.Model
{
    [System.Serializable]
    public class Way : MonoBehaviour
    {
        public ulong ID;
        public string Name;
        public float Longitude;
        public float Latitude;
        public string City;
    }
}
