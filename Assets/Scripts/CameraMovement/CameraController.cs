using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float dragSpeed = 2f; 

    private Vector2 dragOrigin;
    private bool isDragging = false;

    void Update()
    {
        
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                dragOrigin = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 touchPosition = touch.position;
                Vector2 delta = touchPosition - dragOrigin;
                dragOrigin = touchPosition; 

                Vector3 movement = new Vector3(-delta.x, 0f, -delta.y) * dragSpeed * Time.deltaTime;
                transform.Translate(movement, Space.World); 
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
        else
        {
         
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput) * moveSpeed * Time.deltaTime;
            transform.Translate(movement, Space.World); 
        }
    }
}
