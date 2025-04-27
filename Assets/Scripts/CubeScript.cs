using System.Collections;
using UnityEngine;

public class CubeScript : BaseObjectScript
{
    private MiniGameManager miniGameManager;
    private bool isStacked = false; //Tracks if this cube has been placed on the stack
    private float dropHeightDistanceSpace = 1.5f;
    private float slowTimeMinHeight = 2f;

    private Color originalColor;
    private Coroutine flashingCoroutine = null;

    [SerializeField] private AudioClip stackSound;
    private AudioSource audioSource;

    protected override void Start()
    {
        base.Start();
        miniGameManager = FindObjectOfType<MiniGameManager>();
        audioSource = GetComponent<AudioSource>(); // Assumes an AudioSource component exists on the cube GameObject/Prefab
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.spatialBlend = 0f;
            audioSource.volume = 0.8f;
        }
    }

    public override void SelectToggle(bool selected)
    {
        if (isStacked) return; // Prevent selection if stacked
        base.SelectToggle(selected);
        objectRenderer.material.color = selected ? Color.white : Color.white;
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
            miniGameManager.TriggerExplosionAndGameOverSequence(collision.contacts[0].point);
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
            objectRenderer.material.color = Color.grey;
        }
    }
    private void Update()
    {
    }

    public void StartFlashing(float interval)
    {
        if (objectRenderer == null)
        {
            Debug.LogWarning("Object Renderer not found on cube!", this);
            return;
        }

        if (flashingCoroutine != null)
        {
            StopCoroutine(flashingCoroutine);
        }

        originalColor = objectRenderer.material.color;
        flashingCoroutine = StartCoroutine(FlashCoroutine(interval));
    }

    public void StopFlashing()
    {
        if (flashingCoroutine != null)
        {
            StopCoroutine(flashingCoroutine);
            flashingCoroutine = null;

            if (objectRenderer != null)
            {
                objectRenderer.material.color = originalColor;
            }
        }
    }

    private IEnumerator FlashCoroutine(float interval)
    {
        while (true)
        {
            objectRenderer.material.color = Color.red;
            yield return new WaitForSeconds(interval / 2f);

            objectRenderer.material.color = originalColor;
            yield return new WaitForSeconds(interval / 2f);
        }
    }
}