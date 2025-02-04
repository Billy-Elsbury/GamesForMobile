using UnityEngine;

public abstract class BaseObjectScript : MonoBehaviour, ITouchable
{
    protected Vector3 lastPanPosition;
    protected Vector2 lastTouchPosition;
    protected float lastPinchDistance;
    protected float scaleSpeed = 0.001f;
    protected float panSpeed = 10f;
    protected Renderer objectRenderer;

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    public virtual void SelectToggle(bool selected)
    {
    }

    public virtual void MoveObject(Transform transform, Touch touch)
    {
    }

    public virtual void ScaleObject(Touch t1, Touch t2)
    {
        float currentDistance = Vector2.Distance(t1.position, t2.position);
        float scaleFactor = (currentDistance - lastPinchDistance) * scaleSpeed;
        transform.localScale += Vector3.one * scaleFactor;
        lastPinchDistance = currentDistance;


    }

    public virtual void RotateObject(Touch t1, Touch t2)
    {
        // Calculate the angle between the two touch points
        float theta = Mathf.Atan2(t2.position.y - t1.position.y, t2.position.x - t1.position.x);

        // Get the camera's forward direction
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0; // Ignore the y component to keep the rotation horizontal

        // Define the starting angle and rotation
        float startAngle = Mathf.Atan2(lastTouchPosition.y - lastPanPosition.y, lastTouchPosition.x - lastPanPosition.x);
        Quaternion startRotation = transform.rotation;

        // Calculate the current angle and the difference from the start angle
        float currentAngle = theta;
        float angleDifference = currentAngle - startAngle;

        // Apply the rotation based on the camera's forward direction
        transform.rotation = startRotation * Quaternion.AngleAxis(angleDifference * Mathf.Rad2Deg, cameraForward);

        // Update the last touch positions
        lastTouchPosition = (t1.position + t2.position) / 2;
    }

}
