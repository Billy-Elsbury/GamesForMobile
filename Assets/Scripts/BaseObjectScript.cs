using TMPro.Examples;
using UnityEngine;

public abstract class BaseObjectScript : MonoBehaviour, ITouchable
{
    protected Vector3 lastPanPosition;
    protected Vector2 lastTouchPosition;
    protected float lastPinchDistance;
    protected float scaleSpeed = 0.001f;
    protected float panSpeed = 10f;
    protected Renderer objectRenderer;
    protected Rigidbody rigidBody;

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        rigidBody = GetComponent<Rigidbody>();
    }

    public virtual void SelectToggle(bool selected)
    {
        rigidBody.isKinematic = selected == true;
    }

    public virtual void MoveObject(Transform transform, Touch touch)
    {
    }

    public virtual void ScaleObject(Touch t1, Touch t2)
    {
        float currentDistance = Vector2.Distance(t1.position, t2.position);

        if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
        {
            lastPinchDistance = currentDistance; //reset pinch distance on new pinch
        }

        float scaleFactor = (currentDistance - lastPinchDistance) * scaleSpeed;
        transform.localScale += Vector3.one * scaleFactor;

        lastPinchDistance = currentDistance;
    }


    public virtual void RotateObject(Touch t1, Touch t2)
    {
        Vector3 worldPos1 = Camera.main.ScreenToWorldPoint(new Vector3(t1.position.x, t1.position.y, Camera.main.nearClipPlane));
        Vector3 worldPos2 = Camera.main.ScreenToWorldPoint(new Vector3(t2.position.x, t2.position.y, Camera.main.nearClipPlane));

        Vector3 direction = worldPos2 - worldPos1;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.AngleAxis(angle, Camera.main.transform.forward);
    }
}
