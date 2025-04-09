using UnityEngine;
using UnityEngine.UI;

class StreetNameDisplay : MonoBehaviour
{
    public Transform textContainer; 
    public MapReader mapReader; 

    void Update()
    {
        if (mapReader == null)
        {
            Debug.LogError("MapReader reference not set in StreetNameDisplay script!");
            return;
        }

        foreach (OsmWay w in mapReader.ways)
        {
            if (w.Visible && w.Tags.ContainsKey("name"))
            {
                string streetName = w.Tags["name"];

                Vector3 position = Vector3.zero;
                foreach (ulong nodeId in w.NodeIDs)
                {
                    position += (mapReader.nodes[nodeId] - mapReader.bounds.Centre);
                }
                position /= w.NodeIDs.Count;

          
                GameObject streetNameTextObject = new GameObject("StreetNameText");
                streetNameTextObject.transform.SetParent(textContainer, false);

                Text streetNameText = streetNameTextObject.AddComponent<Text>();
                streetNameText.font = Resources.GetBuiltinResource<Font>("Arial.ttf"); 
                streetNameText.rectTransform.position = position;
                streetNameText.text = streetName;
            }
        }
    }
}
