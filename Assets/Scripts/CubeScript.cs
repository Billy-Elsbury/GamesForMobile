using UnityEngine;

public class CubeScript : MonoBehaviour, ITouchable
{
    Renderer r;

    private Vector3 lastPanPosition;
    private Vector2 lastTouchPosition;
    private float lastPinchDistance;
    private float scaleSpeed = 0.001f;
    private float panSpeed = 10f;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        r = GetComponent<Renderer>();
    }

    public void SelectToggle(bool selected)
    {
        if (selected)
        {
            changeColor(Color.red);
        }
        else
        {
            changeColor(Color.white);
        }
    }

    public void changeColor(Color color)
    {
        r.material.color = color;
    }

    void Update()
    {
        // Check if the current object is selected
        if (Input.touchCount > 0 && gameManager.selectedObject == this)
        {
            switch (Input.touchCount)
            {
                case 1: // Single finger for moving
                    HandleSingleTouch(Input.GetTouch(0));
                    break;

                case 2: // Two fingers for scaling or rotating
                    HandleTwoTouches(Input.GetTouch(0), Input.GetTouch(1));
                    break;

                default:
                    break;
            }
        }
    }

    // Handle single-finger touch for moving
    private void HandleSingleTouch(Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                lastPanPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.nearClipPlane));
                break;

            case TouchPhase.Moved:
                MoveCube(touch.position);
                break;

            case TouchPhase.Ended:
                // Reset if needed (not strictly required for moving)
                break;
        }
    }

    // Handle two-finger touch for scaling and rotating
    private void HandleTwoTouches(Touch t1, Touch t2)
    {
        switch (t1.phase)
        {
            case TouchPhase.Began:
            case TouchPhase.Stationary:
                lastPinchDistance = Vector2.Distance(t1.position, t2.position);
                lastTouchPosition = t1.position - t2.position;
                break;

            case TouchPhase.Moved:
                float currentPinchDistance = Vector2.Distance(t1.position, t2.position);

                // Determine if it's a pinch or a rotation based on pinch delta
                if (Mathf.Abs(currentPinchDistance - lastPinchDistance) > 5f) // Pinch
                {
                    ScaleCube(t1, t2);
                }
                else
                {
                    RotateCube(t1, t2);
                }

                // Update the last positions
                lastPinchDistance = currentPinchDistance;
                lastTouchPosition = t1.position - t2.position;
                break;

            case TouchPhase.Ended:
            case TouchPhase.Canceled:
                // Reset for safety
                break;
        }
    }

    // Method to move the cube
    private void MoveCube(Vector3 newPanPosition)
    {
        Vector3 currentPanPosition = Camera.main.ScreenToWorldPoint(new Vector3(newPanPosition.x, newPanPosition.y, Camera.main.nearClipPlane));
        Vector3 offset = currentPanPosition - lastPanPosition;
        Vector3 move = new Vector3(offset.x, 0, offset.z) * panSpeed;

        transform.Translate(move, Space.World);
        lastPanPosition = currentPanPosition; // Update last position
        print("Moving");
    }

    // Method to rotate the cube using two fingers
    private void RotateCube(Touch t1, Touch t2)
    {
        Vector2 currentTouchPosition = t1.position - t2.position;
        float angle = Vector2.SignedAngle(lastTouchPosition, currentTouchPosition);
        transform.Rotate(Vector3.up, angle * 1, Space.World);

        lastTouchPosition = currentTouchPosition;
        print("Rotating");
    }

    // Method to scale the cube using pinch
    private void ScaleCube(Touch t1, Touch t2)
    {
        float currentPinchDistance = Vector2.Distance(t1.position, t2.position);
        float pinchDelta = currentPinchDistance - lastPinchDistance;
        float scaleFactor = 1 + pinchDelta * scaleSpeed;

        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(scaleFactor, scaleFactor, scaleFactor));
        lastPinchDistance = currentPinchDistance;
        print("Scaling");
    }
}
