using UnityEngine;

public class SphereScript : BaseObjectScript
{
    protected override void Start()
    {
        base.Start();
    }

    public override void SelectToggle(bool selected)
    {
        objectRenderer.material.color = selected ? Color.blue : Color.white;
    }

    public override void MoveObject(Vector3 newPanPosition)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                float orbitSpeed = 2.0f;
                Vector2 touchDelta = touch.deltaPosition;
                transform.RotateAround(Camera.main.transform.position, Vector3.up, touchDelta.x * orbitSpeed);
                transform.RotateAround(Camera.main.transform.position, Camera.main.transform.right, -touchDelta.y * orbitSpeed);
            }
        }
    }
}
