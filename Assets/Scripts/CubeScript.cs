using UnityEngine;

public class CubeScript : BaseObjectScript
{
    private MiniGameManager miniGameManager;
    private bool isStacked = false; // Tracks if this cube has been placed on the stack

    protected override void Start()
    {
        base.Start();
        miniGameManager = FindObjectOfType<MiniGameManager>();
    }

    public override void SelectToggle(bool selected)
    {
        base.SelectToggle(selected);
        objectRenderer.material.color = selected ? Color.red : Color.white;
    }

    public override void MoveObject(Transform transform, Touch touch)
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.WorldToScreenPoint(transform.position).z));
        transform.position = new Vector3(touchPosition.x, touchPosition.y, transform.position.z);

        // Notify MiniGameManager that the cube has moved
        MiniGameManager miniGameManager = FindObjectOfType<MiniGameManager>();
        if (miniGameManager != null)
        {
            miniGameManager.OnCubeMoved();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (miniGameManager == null) return;

        // If the cube touches the ground, trigger game over
        if (collision.gameObject.CompareTag("Ground"))
        {
            miniGameManager.GameOver();
        }
        // If the cube lands on another cube, update the highest point **only once**
        else if (collision.gameObject.CompareTag("Cube") && !isStacked)
        {
            isStacked = true; // Prevent multiple updates from bouncing
            miniGameManager.UpdateHighestPoint(transform.position.y);
        }
    }
}
