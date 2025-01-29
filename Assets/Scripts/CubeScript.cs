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
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector3 movement = new Vector3(newPanPosition.x, newPanPosition.y, 0);
                transform.Translate(movement * panSpeed * Time.deltaTime, Space.World);
            }
        }
    }
}
