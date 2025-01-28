using UnityEngine;

public class CubeScript : MonoBehaviour, ITouchable
{
    private Renderer r;
    private Vector3 lastPanPosition;
    private float lastPinchDistance;
    private Vector2 lastTouchPosition;

    private float panSpeed = 0.1f;
    private float scaleSpeed = 0.01f;

    void Start()
    {
        r = GetComponent<Renderer>();
    }

    // Implement SelectToggle
    public void SelectToggle(bool selected)
    {
        r.material.color = selected ? Color.red : Color.white;
    }

    // Implement MoveObject
    public void MoveObject(Vector3 newPanPosition)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = Camera.main.ScreenToWorldPoint(new Vector3(newPanPosition.x, newPanPosition.y, Mathf.Abs(Camera.main.transform.position.z)));
            }

            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 currentPanPosition = Camera.main.ScreenToWorldPoint(new Vector3(newPanPosition.x, newPanPosition.y, Mathf.Abs(Camera.main.transform.position.z)));
                Vector3 offset = currentPanPosition - lastPanPosition;

                transform.Translate(offset.x * panSpeed, offset.y * panSpeed, 0, Space.World);

                lastPanPosition = currentPanPosition;
            }
        }
    }

    // Implement ScaleObject
    public void ScaleObject(Touch t1, Touch t2)
    {
        // Calculate pinch distance
        float currentPinchDistance = Vector2.Distance(t1.position, t2.position);
        float pinchDelta = currentPinchDistance - lastPinchDistance;

        // Scale based on pinch distance
        float scaleFactor = 1 + pinchDelta * scaleSpeed;

        transform.localScale = Vector3.Scale(transform.localScale, new Vector3(scaleFactor, scaleFactor, scaleFactor));
        lastPinchDistance = currentPinchDistance;
    }

    // Implement RotateObject
    public void RotateObject(Touch t1, Touch t2)
    {
        // Calculate angle between the two touches
        Vector2 currentTouchPosition = t1.position - t2.position;
        float angle = Vector2.SignedAngle(lastTouchPosition, currentTouchPosition);

        // Apply rotation to the object
        transform.Rotate(Vector3.up, angle * 100, Space.World);

        lastTouchPosition = currentTouchPosition;
    }
}
