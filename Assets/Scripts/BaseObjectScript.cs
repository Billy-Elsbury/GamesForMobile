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
        Vector2 currentMidpoint = (t1.position + t2.position) / 2;
        Vector2 delta = currentMidpoint - lastTouchPosition;
        float rotationSpeed = 0.2f;
        transform.Rotate(Vector3.up, delta.x * rotationSpeed, Space.World);
        lastTouchPosition = currentMidpoint;
    }
}
