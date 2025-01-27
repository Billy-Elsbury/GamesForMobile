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
        // Check if an object is selected
        if (selectedObject != null)
        {
            // Disable camera controls when object is selected
            cameraController.enabled = false;
        }
        else
        {
            // Re-enable camera controls when no object is selected
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
            // No object hit
            if (selectedObject != null)
            {
                selectedObject.SelectToggle(false);
                selectedObject = null;
            }
        }
    }
}
