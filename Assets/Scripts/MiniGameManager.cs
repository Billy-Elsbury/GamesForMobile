using TMPro;
using UnityEngine;
using System.Collections; // Needed for the delay
using UnityEngine.UI;

public class MiniGameManager : MonoBehaviour
{
    private float highestPoint = 0f;
    private bool isGameOver = false;
    private bool cubeMoved = false; // Track if the cube has been moved

    public TMP_Text scoreText;
    public GameObject gameOverPanel;
    public GameObject cubePrefab;
    public Transform spawnPoint;

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
        scoreText.text = "Highest Stack: " + highestPoint.ToString("F2");
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Debug.Log("Game Over! Highest Stack: " + highestPoint);
            scoreText.text = "Highest Stack: " + highestPoint.ToString("F2");
            gameOverPanel.SetActive(true);
        }
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void SpawnNewCube()
    {
        // Spawn cube at random size
        Vector3 randomSize = new Vector3(Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f), Random.Range(0.5f, 1.5f));
        currentCube = Instantiate(cubePrefab, spawnPoint.position, Quaternion.identity);
        currentCube.transform.localScale = randomSize;
        currentCube.GetComponent<Rigidbody>().isKinematic = true; // Keep it floating
        cubeMoved = false; // Reset move tracking
    }

    public void OnCubeMoved()
    {
        if (!cubeMoved)
        {
            cubeMoved = true;
            StartCoroutine(SpawnNextCubeWithDelay());
        }
    }

    private IEnumerator SpawnNextCubeWithDelay()
    {
        yield return new WaitForSeconds(2f);

        if (currentCube != null)
        {
            currentCube.GetComponent<Rigidbody>().isKinematic = true; // Enable physics
        }

        SpawnNewCube();
    }
}
