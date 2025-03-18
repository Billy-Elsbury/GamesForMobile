using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    private float highestPoint = 0f;
    private bool isGameOver = false;
    private bool isSpawning = false; // Flag to prevent multiple spawns

    public TMP_Text currentScoreText;
    public TMP_Text finalScoreText;
    public GameObject gameOverPanel;
    public GameObject cubePrefab;
    public Transform spawnPoint;

    public bool useRandomSizedCubes = true; // Public toggle for game mode

    private GameObject currentCube;

    private void Start()
    {
        highestPoint = 0f;
        isGameOver = false;
        gameOverPanel.SetActive(false);
        SpawnNewCube(); // Spawn the first cube
    }

    public void UpdateHighestPoint(float cubeY)
    {
        if (cubeY > highestPoint)
        {
            highestPoint = cubeY;
        }
        currentScoreText.text = "Highest Stack: " + highestPoint.ToString("F2");
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("Game Over! Highest Stack: " + highestPoint);
            finalScoreText.text = "Highest Stack: " + highestPoint.ToString("F2");
            gameOverPanel.SetActive(true);
        }
    }

    public void SpawnNewCube()
    {
        if (isSpawning) return; // Prevent multiple spawns

        StartCoroutine(WaitForStableTower());
    }

    private IEnumerator WaitForStableTower()
    {
        isSpawning = true; // Set the flag to true

        // Wait until the tower is stable
        while (!IsTowerStable())
        {
            yield return null;
        }

        // Spawn the new cube
        if (useRandomSizedCubes)
        {
            Vector3 randomSize = new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f));
            currentCube = Instantiate(cubePrefab, spawnPoint.position, Quaternion.identity);
            currentCube.transform.localScale = randomSize;
        }
        else
        {
            currentCube = Instantiate(cubePrefab, spawnPoint.position, Quaternion.identity);
            currentCube.transform.localScale = Vector3.one;
        }

        currentCube.GetComponent<Rigidbody>().isKinematic = true; // Keep it floating
        StartCoroutine(ResetSpawnFlag()); // Reset the flag after a short delay
    }

    private bool IsTowerStable()
    {
        // Get all cubes in the scene
        Rigidbody[] cubes = FindObjectsOfType<Rigidbody>();

        foreach (Rigidbody cube in cubes)
        {
            // If any cube is still moving, the tower is not stable
            if (cube.velocity.magnitude > 0.1f)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator ResetSpawnFlag()
    {
        yield return new WaitForSeconds(0.5f); // Adjust the delay as needed
        isSpawning = false; // Reset the flag
    }

    public void SlowMotion()
    {
        StartCoroutine(SlowMotionCoroutine());
    }

    private IEnumerator SlowMotionCoroutine()
    {
        Time.timeScale = 0.5f; // Slow down time
        yield return new WaitForSecondsRealtime(2f); // Wait for 2 real-time seconds
        Time.timeScale = 1f; // Restore normal time
    }

    public float GetHighestPoint()
    {
        return highestPoint;
    }
}
