using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    public Transform target; // The target to center on (for the player camera)
    public float baseZoomSpeed = 10f; // Reduced base speed for zooming
    public float touchZoomSpeed = 0.1f; // Reduced speed for pinch-to-zoom on touch devices
    public float minZoom = 50f; // Minimum zoom level (closer view)
    public float maxZoom = 350f; // Maximum zoom level (further view) increased for map screen
    public float panSpeed = 1f; // Reduced speed for panning the camera

    private Camera _camera;
    private float initialHeight;
    private bool isPanning;

    void Start()
    {
        _camera = GetComponent<Camera>();
        if (_camera.transform.name == "PlayerCamera")
        {
            _camera.transform.rotation = Quaternion.Euler(85f, 0f, 0f); 
        }
        initialHeight = _camera.transform.position.y; 
    }

    void Update()
    {
        if (_camera.gameObject.activeSelf)
        {
            HandleMouseZoom();
            HandleTouchZoom();

            if (_camera.transform.name == "PlayerCamera")
            {
                CenterCameraOnTarget();
            }
            else if (Input.touchCount < 2) 
            {
                HandleMousePan();
            }
        }
    }

    private void HandleMouseZoom()
    {
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        if (zoomInput != 0f)
        {
            float currentZoomLevel = _camera.transform.position.y;
            float dynamicZoomSpeed = baseZoomSpeed * (currentZoomLevel / maxZoom);
            dynamicZoomSpeed = Mathf.Max(dynamicZoomSpeed, 0.1f); 

            Vector3 position = _camera.transform.position;
            position.y -= zoomInput * dynamicZoomSpeed;
            position.y = Mathf.Clamp(position.y, minZoom, maxZoom);
            _camera.transform.position = position;
        }
    }

    private void HandleTouchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;
            Vector2 touch2PrevPos = touch2.position - touch2.deltaPosition;

            float prevTouchDeltaMag = (touch1PrevPos - touch2PrevPos).magnitude;
            float touchDeltaMag = (touch1.position - touch2.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            float currentZoomLevel = _camera.transform.position.y;
            float dynamicZoomSpeed = baseZoomSpeed * (currentZoomLevel / maxZoom);
            dynamicZoomSpeed = Mathf.Max(dynamicZoomSpeed, 0.1f); 

            Vector3 position = _camera.transform.position;
            position.y += deltaMagnitudeDiff * touchZoomSpeed;
            position.y = Mathf.Clamp(position.y, minZoom, maxZoom);
            _camera.transform.position = position;
        }
    }

    private void HandleMousePan()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isPanning = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            float h = -Input.GetAxis("Mouse X") * panSpeed;
            float v = -Input.GetAxis("Mouse Y") * panSpeed;

            Vector3 move = new Vector3(h, 0, v);
            _camera.transform.Translate(move, Space.World);
        }
    }

    public void CenterCameraOnTarget()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            _camera.transform.position = new Vector3(targetPosition.x, _camera.transform.position.y, targetPosition.z);
        }
    }
}
