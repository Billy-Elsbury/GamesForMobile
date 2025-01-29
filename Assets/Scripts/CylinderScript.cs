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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 groundMovement = new Vector3(newPanPosition.x, 0, newPanPosition.y);
                transform.position += groundMovement * panSpeed * Time.deltaTime;
            }
        }
    }
}
