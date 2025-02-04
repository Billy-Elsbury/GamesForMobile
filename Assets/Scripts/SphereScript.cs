using UnityEngine;

public class SphereScript : BaseObjectScript
{
    private Vector3 initialTouchWorldPosition;
    private float rotateMagnifier = 0.05f;


    protected override void Start()
    {
        base.Start();
    }

    public override void SelectToggle(bool selected)
    {
        objectRenderer.material.color = selected ? Color.blue : Color.white;
    }

    public override void MoveObject(Transform transform, Touch touch)
    {
        Vector3 cameraPosition = Camera.main.transform.position;

        // touch position to world coordinates
        Vector3 touchDelta = new Vector3(touch.deltaPosition.x, touch.deltaPosition.y, 0) * rotateMagnifier;
        
        Vector3 direction = transform.position - cameraPosition;

        // Rotate direction based on touch delta
        direction = Quaternion.Euler(-touchDelta.y, touchDelta.x, 0) * direction;
        transform.position = cameraPosition + direction;
    }
}
