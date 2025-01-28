using UnityEngine;

public abstract class BaseObjectScript : MonoBehaviour, ITouchable
{
    protected Vector3 lastPanPosition;
    protected Vector2 lastTouchPosition;
    protected float lastPinchDistance;
    protected float scaleSpeed = 0.001f;
    protected float panSpeed = 10f;

    public virtual void SelectToggle(bool selected)
    {
        // Handle selection toggle here (color change, etc.)
    }

    public virtual void MoveObject(Vector3 newPanPosition)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Initialize the lastPanPosition when the touch begins
                lastPanPosition = Camera.main.ScreenToWorldPoint(new Vector3(newPanPosition.x, newPanPosition.y, Mathf.Abs(Camera.main.transform.position.z)));
            }

            if (touch.phase == TouchPhase.Moved)
            {
                // Get the current touch position in world space
                Vector3 currentPanPosition = Camera.main.ScreenToWorldPoint(new Vector3(newPanPosition.x, newPanPosition.y, Mathf.Abs(Camera.main.transform.position.z)));
                Vector3 offset = currentPanPosition - lastPanPosition;

                // Move the object based on the touch movement
                transform.Translate(offset.x * panSpeed, offset.y * panSpeed, 0, Space.World);

                // Update the lastPanPosition
                lastPanPosition = currentPanPosition;
            }
        }
    }

    public virtual void ScaleObject(Touch t1, Touch t2)
    {
        // Scaling logic
    }

    public virtual void RotateObject(Touch t1, Touch t2)
    {
        // Rotation logic
    }
}
