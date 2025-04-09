using UnityEngine;

public class CameraRotationController : MonoBehaviour
{
    // Adjust the rotation speed as needed
    public float rotationSpeed = 1f;

    void Start()
    {
        // Enable the gyroscope
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported on this device.");
        }
    }

    void Update()
    {
        // Handle device rotation
        if (SystemInfo.supportsGyroscope)
        {
            // Read gyroscope rotation rate
            Vector3 gyroRotationRate = Input.gyro.rotationRateUnbiased;

            // Convert rotation rate to rotation angles
            Vector3 rotationAngles = gyroRotationRate * Mathf.Rad2Deg * Time.deltaTime * rotationSpeed;

            // Apply rotation to the camera
            transform.Rotate(-rotationAngles);
        }
    }
}
