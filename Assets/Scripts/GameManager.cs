using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ITouchable selectedObject;
    private CameraController cameraController;

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        // Enable/Disable camera controls based on selection
        cameraController.enabled = selectedObject == null;
    }

    public void tapRegisteredAt(Vector2 tapPosition)
    {
        Ray r = Camera.main.ScreenPointToRay(tapPosition);
        if (Physics.Raycast(r, out RaycastHit hit))
        {
            ITouchable newObject = hit.collider.gameObject.GetComponent<ITouchable>();
            if (newObject != null)
            {
                if (selectedObject != null)
                {
                    selectedObject.SelectToggle(false);
                }
                selectedObject = newObject;
                newObject.SelectToggle(true);
            }
        }
        else
        {
            if (selectedObject != null)
            {
                selectedObject.SelectToggle(false);
                selectedObject = null;
            }
        }
    }

    public void OnObjectMove(Touch touch)
    {
        if (selectedObject is BaseObjectScript obj)
        {
            Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Mathf.Abs(Camera.main.transform.position.z)));
            obj.MoveObject(touchPosition);
        }
    }

    public void OnObjectScaleAndRotate(Touch t1, Touch t2)
    {
        if (selectedObject is BaseObjectScript obj)
        {
            obj.ScaleObject(t1, t2);
            obj.RotateObject(t1, t2);
        }
    }
}
