using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomRotationController : MonoBehaviour
{
    public float zoomSpeed = 10f; // Speed for mouse wheel zooming
    public float touchZoomSpeed = 0.1f; // Speed for pinch-to-zoom on touch devices
    public float minZoom = 10f; // Minimum zoom level (closer view)
    public float maxZoom = 500f; // Maximum zoom level (further view) increased for map screen

    public float rotationXDecreaseRate = 1f; // Rate at which rotation on X-axis decreases during zoom out
    public float rotationXIncreaseRate = 1f; // Rate at which rotation on X-axis increases during zoom in
    public float minRotationX = 5f; // Minimum rotation on X-axis
    public float maxRotationX = 90f; // Maximum rotation on X-axis

    private Camera _camera;
    private float initialRotationX; // Initial rotation on X-axis

    void Start()
    {
        _camera = GetComponent<Camera>();
        initialRotationX = transform.eulerAngles.x; // Store initial rotation on X-axis
    }

    void Update()
    {
        // Handle mouse scroll wheel zoom
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput != 0f)
        {
            if (zoomInput > 0)
            {
                ZoomCamera(zoomInput * zoomSpeed, rotationXIncreaseRate);
            }
            else
            {
                ZoomCamera(zoomInput * zoomSpeed, -rotationXDecreaseRate);
            }
        }

        // Handle pinch-to-zoom for touch devices
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            // Find the position in the previous frame of each touch
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame
            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            // Find the difference in the distances between each frame
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Apply zoom based on the change in distance between touches
            ZoomCamera(deltaMagnitudeDiff * touchZoomSpeed, deltaMagnitudeDiff > 0 ? rotationXIncreaseRate : -rotationXDecreaseRate);
        }
    }

    void ZoomCamera(float zoomAmount, float rotationRate)
    {
        // Update rotation on X-axis
        float newRotationX = Mathf.Clamp(transform.eulerAngles.x + rotationRate * Time.deltaTime, minRotationX, maxRotationX);
        transform.rotation = Quaternion.Euler(newRotationX, transform.eulerAngles.y, transform.eulerAngles.z);

        // Adjust the field of view or orthographic size based on camera type
        if (_camera.orthographic)
        {
            _camera.orthographicSize += zoomAmount;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);
        }
        else
        {
            _camera.fieldOfView += zoomAmount;
            _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, minZoom, maxZoom);
        }
    }
}