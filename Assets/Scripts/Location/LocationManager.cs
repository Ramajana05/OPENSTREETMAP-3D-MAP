using UnityEngine;

public class LocationManager : MonoBehaviour
{
    private void Start()
    {
        // Check if location services are enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogError("Location services are not enabled.");
            return;
        }

        // Start location service updates
        Input.location.Start();
    }

    private void Update()
    {
        // Check if location services are running
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // Get the current GPS coordinates
            float latitude = Input.location.lastData.latitude;
            float longitude = Input.location.lastData.longitude;

            // Update the position of the GameObject representing the user's location
            transform.position = new Vector3(longitude, 0f, latitude);
        }
    }

    private void OnDestroy()
    {
        // Stop location service updates when the script is destroyed
        Input.location.Stop();
    }
}
