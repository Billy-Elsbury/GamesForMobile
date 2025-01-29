using UnityEngine;

public class CubeScript : BaseObjectScript
{
    protected override void Start()
    {
        base.Start();
    }

    public override void SelectToggle(bool selected)
    {
        objectRenderer.material.color = selected ? Color.red : Color.white;
    }

    public override void MoveObject(Vector3 newPanPosition)
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            // Set initial pan position to flat plane perpendicular to camera
            lastPanPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.WorldToScreenPoint(transform.position).z));
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            Vector3 currentPanPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.WorldToScreenPoint(transform.position).z));

            Vector3 offset = currentPanPosition - lastPanPosition;

            transform.position += offset;

            lastPanPosition = currentPanPosition;
        }
    }
}
