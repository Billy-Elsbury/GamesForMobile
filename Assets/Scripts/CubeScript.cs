using UnityEngine;

public class CubeScript : BaseObjectScript
{
    private MiniGameManager miniGameManager;
    private bool isStacked = false; //Tracks if this cube has been placed on the stack
    private float dropHeightDistanceSpace = 1.5f;
    private float slowTimeMinHeight = 2f;

 
    [SerializeField] private AudioClip stackSound;
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        miniGameManager = FindObjectOfType<MiniGameManager>();
        // --- Get the AudioSource ---
        audioSource = GetComponent<AudioSource>(); // Assumes an AudioSource component exists on the cube GameObject/Prefab
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
            audioSource.volume = 0.5f;
        }
    }

    public override void SelectToggle(bool selected)
    {
        if (isStacked) return; // Prevent selection if stacked
        base.SelectToggle(selected);
        objectRenderer.material.color = selected ? Color.red : Color.white;
    }

    public override void MoveObject(Transform transform, Touch touch)
    {
        if (isStacked) return; // Prevent movement if stacked

        // Calculate the highest point plus one meter
        float minDropHeight = miniGameManager.GetHighestPoint() + dropHeightDistanceSpace;

        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, Camera.main.WorldToScreenPoint(transform.position).z));

        // Restrict movement to one meter above the highest point
        if (touchPosition.y < minDropHeight)
        {
            touchPosition.y = minDropHeight;
        }

        transform.position = new Vector3(touchPosition.x, touchPosition.y, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (miniGameManager == null) return;

        if (collision.gameObject.CompareTag("Ground"))
        {
            miniGameManager.GameOver();
        }
        else if (collision.gameObject.CompareTag("Cube") && !isStacked)
        {
            if (audioSource != null && stackSound != null)
            {
                audioSource.PlayOneShot(stackSound);
            }

            isStacked = true; // Prevent multiple updates
            miniGameManager.UpdateHighestPoint(transform.position.y);
            miniGameManager.SpawnNewCube(); // Spawn a new cube
            objectRenderer.material.color = Color.yellow;
        }
    }
    private void Update()
    {
        if (transform.position.y < slowTimeMinHeight)
        {
            miniGameManager.SlowMotion();
        }
    }
}