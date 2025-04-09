using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera playerCamera;
    private CameraZoomController playerCameraController;

    void Start()
    {
        playerCameraController = playerCamera.GetComponent<CameraZoomController>();
        CenterCameraOnTarget();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CenterCameraOnTarget();
        }
    }

    public void CenterCameraOnTarget()
    {
        playerCameraController.CenterCameraOnTarget(); // Ensure the camera centers on the target
    }


     public void ReceiveCommand(string command)
    {
        if (command == "SwitchToPlayerCamera")
        {
            CenterCameraOnTarget();
        }
    }
}
