using UnityEngine;
using UnityEngine.Events;

public static class CameraEvent
{
    public static UnityEvent<Vector3> OnCameraMoved = new UnityEvent<Vector3>();
}
