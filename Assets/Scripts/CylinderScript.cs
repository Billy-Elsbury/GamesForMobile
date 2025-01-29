using UnityEngine;

public class CylinderScript : BaseObjectScript
{
    protected override void Start()
    {
        base.Start();
    }

    public override void SelectToggle(bool selected)
    {
        objectRenderer.material.color = selected ? Color.yellow : Color.white;
    }

    public override void MoveObject(Vector3 newPanPosition)
    {
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            // Cast a ray to get the initial position on the ground
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                lastPanPosition = hit.point;
            }
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            // raycast to get new position on the ground
            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 currentPanPosition = hit.point;
                Vector3 offset = currentPanPosition - lastPanPosition;

                transform.position += new Vector3(offset.x, 0, offset.z);

                lastPanPosition = currentPanPosition;
            }
        }
    }
}
