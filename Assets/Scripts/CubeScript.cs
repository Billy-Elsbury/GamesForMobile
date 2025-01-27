using UnityEngine;

public class CubeScript : MonoBehaviour, ITouchable
{
    Renderer r;

    private Vector3 lastPanPosition;
    private Vector2 lastTouchPosition;
    private float lastPinchDistance;
    private float scaleSpeed = 0.00f;
    private float panSpeed = 0.001f;
    GameManager gameManager;

    // Add enum for gesture state
    private GestureState gestureState = GestureState.None;

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
        // Check if we are currently manipulating this object
        if (Input.touchCount > 0 && gameManager.selectedObject == this)
        {
            if (Input.touchCount == 1 && gestureState == GestureState.None) // Single finger move
            {
                Touch t = Input.GetTouch(0);
                if (t.phase == TouchPhase.Moved)
                {
                    gestureState = GestureState.Moving;
                    MoveCube(t.position);
                    gestureState = GestureState.None;
                }
            }
            else if (Input.touchCount == 2 && gestureState == GestureState.None) // Two finger rotate/scale
            {
                Touch t1 = Input.GetTouch(0);
                Touch t2 = Input.GetTouch(1);

                if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
                {
                    if (Vector2.Distance(t1.position, t2.position) > lastPinchDistance) // Pinch to scale
                    {
                        gestureState = GestureState.Scaling;
                        ScaleCube(t1, t2);
                    }
                    else // Rotate
                    {
                        gestureState = GestureState.Rotating;
                        RotateCube(t1, t2);
                    }
                    lastPinchDistance = Vector2.Distance(t1.position, t2.position);
                }
            }
        }
    }

    // Method to move the cube
    void MoveCube(Vector3 newPanPosition)
    {
        Vector3 offset = Camera.main.ScreenToWorldPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x, 0, offset.y) * panSpeed;

        transform.Translate(move, Space.World);
        lastPanPosition = newPanPosition;
    }

    // Method to rotate the cube using two fingers
    void RotateCube(Touch t1, Touch t2)
    {
        Vector2 currentTouchPosition = t1.position - t2.position;
        float angle = Vector2.SignedAngle(lastTouchPosition, currentTouchPosition);
        transform.Rotate(Vector3.up, angle, Space.World);

        lastTouchPosition = currentTouchPosition;
    }

    // Method to scale the cube using pinch
    void ScaleCube(Touch t1, Touch t2)
    {
        float currentPinchDistance = Vector2.Distance(t1.position, t2.position);
        float pinchDelta = currentPinchDistance - lastPinchDistance;
        float scaleFactor = 1 + pinchDelta * scaleSpeed;

        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(scaleFactor, scaleFactor, scaleFactor));
        lastPinchDistance = currentPinchDistance;
    }
}
