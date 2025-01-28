using UnityEngine;

public class CylinderScript : MonoBehaviour
{
    private Renderer r;
    private Vector3 lastPanPosition;

    void Start()
    {
        r = GetComponent<Renderer>();
    }

    public void SelectToggle(bool selected)
    {
        r.material.color = selected ? Color.yellow : Color.white;
    }

    public void MoveObject(Vector3 newPanPosition)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = Camera.main.ScreenToWorldPoint(new Vector3(newPanPosition.x, newPanPosition.y, Mathf.Abs(Camera.main.transform.position.z)));
            }

            if (touch.phase == TouchPhase.Moved)
            {
                // Get the current touch position in world space
                Vector3 currentPanPosition = Camera.main.ScreenToWorldPoint(new Vector3(newPanPosition.x, newPanPosition.y, Mathf.Abs(Camera.main.transform.position.z)));
                Vector3 offset = currentPanPosition - lastPanPosition;

                // Move the object based on the touch movement
                transform.Translate(offset.x, 0, offset.y, Space.World);

                // Update the last position for the next movement
                lastPanPosition = currentPanPosition;
            }
        }
    }
}
