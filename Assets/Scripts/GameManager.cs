using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ITouchable selectedObject;
    CameraController cameraController;

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        if (selectedObject != null)
        {
            cameraController.enabled = false;
        }
        else
        {
            cameraController.enabled = true;
        }
    }

    public void tapRegisteredAt(Vector2 tapPosition)
    {
        Ray r = Camera.main.ScreenPointToRay(tapPosition);
        RaycastHit info;

        if (Physics.Raycast(r, out info))
        {
            ITouchable newObject = info.collider.gameObject.GetComponent<ITouchable>();
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
}
