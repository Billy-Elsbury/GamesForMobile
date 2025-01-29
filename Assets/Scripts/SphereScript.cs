using UnityEngine;

public class SphereScript : BaseObjectScript
{
    private Vector3 initialTouchWorldPosition;

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
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
            Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, distanceToCamera);
            initialTouchWorldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        }
        else if (touch.phase == TouchPhase.Moved)
        {
            float distanceToCamera = Vector3.Distance(transform.position, Camera.main.transform.position);
            Vector3 touchPosition = new Vector3(touch.position.x, touch.position.y, distanceToCamera);
            Vector3 currentTouchWorldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
            Vector3 moveDelta = currentTouchWorldPosition - initialTouchWorldPosition;

            // movement in x and y axis, keeping distance to the camera constant
            Vector3 moveDirection = Camera.main.transform.right * moveDelta.x + Camera.main.transform.up * moveDelta.y;
            transform.position += moveDirection;

            initialTouchWorldPosition = currentTouchWorldPosition;
        }
    }
}
