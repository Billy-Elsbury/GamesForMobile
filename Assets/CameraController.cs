using UnityEngine;

public class CameraController : MonoBehaviour
{
    ITouchable selectedObject;

    public float panSpeed = 20f; 
    public float rotationSpeed = 0.2f; 
    public Transform groundPlane; 

    private GameManager gameManager;
    private Vector3 lastPanPosition;
    private bool isPanning = false;
    private Vector2 lastTouchPosition;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                if (selectedObject == null)
                {
                    lastPanPosition = t.position;
                    isPanning = true;
                }
            }
            else if (t.phase == TouchPhase.Moved && isPanning)
            {
                PanCamera(t.position);
            }
            else if (t.phase == TouchPhase.Ended)
            {
                isPanning = false;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                lastTouchPosition = t1.position - t2.position;
            }
            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {
                Vector2 currentTouchPosition = t1.position - t2.position;
                float angle = Vector2.SignedAngle(lastTouchPosition, currentTouchPosition);

                OrbitCamera(angle);
                lastTouchPosition = currentTouchPosition;
            }
        }
    }

    void PanCamera(Vector3 newPanPosition)
    {
        Vector3 offset = Camera.main.ScreenToViewportPoint(lastPanPosition - newPanPosition);

        // Move the camera in world space
        Vector3 move = new Vector3(offset.x * panSpeed, 0, offset.y * panSpeed);
        transform.Translate(move, Space.World);

        lastPanPosition = newPanPosition;

        // Project the camera onto ground plane
        Vector3 projectedPosition = Vector3.ProjectOnPlane(transform.position, groundPlane.up);
        transform.position = new Vector3(projectedPosition.x, transform.position.y, projectedPosition.z);
    }

    void OrbitCamera(float angle)
    {
        transform.RotateAround(transform.position, groundPlane.up, angle * rotationSpeed);
    }
}
