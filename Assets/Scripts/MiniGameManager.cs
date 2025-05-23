using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System;
using System.Collections.Generic;

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

    private string googleAppsScriptURL = "https://script.google.com/macros/s/AKfycbwI4A7jdvK8IMcC6Lbzmk5WUHZonvx9090A_jD_Q3_Ucf7BxIXt6rrrGs8DxnGGgL71/exec";

    // Explosion Settings
    [Header("Explosion Settings")]
    public AudioClip explosionSound;
    public float explosionForce = 0f;
    public float explosionRadius = 0f;
    public float explosionUpwardModifier = 0f;
    public float soundToExplosionDelay = 0.3f;
    public float flashInterval = 0.6f;

    private AudioSource managerAudioSource;

    public ParticleSystem explosionParticlesPrefab;

    private void Start()
    {
        managerAudioSource = GetComponent<AudioSource>();
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
            isGameOver = true;
            Debug.Log("Game Over! Highest Stack: " + highestPoint);
            finalScoreText.text = "Highest Stack: " + highestPoint.ToString("F2");
            gameOverPanel.SetActive(true);

            //Google Docs implementation
            if (!string.IsNullOrEmpty(googleAppsScriptURL))
            {
                StartCoroutine(SendScoreToGoogleSheet(highestPoint));
            }
    }

    private IEnumerator SendScoreToGoogleSheet(float score)
    {
        //JSON format for payload
        string jsonData = $"{{\"score\": {score.ToString(System.Globalization.CultureInfo.InvariantCulture)}}}"; // Use InvariantCulture for consistent decimal format
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);

        //UnityWebRequest creation
        using (UnityWebRequest request = new UnityWebRequest(googleAppsScriptURL, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            Debug.Log($"Sending score to Google Sheet: {jsonData}");

            //send request and wait for the response
            yield return request.SendWebRequest();

            //error handling
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error sending score: {request.error}");
                Debug.LogError($"Response Code: {request.responseCode}");
                Debug.LogError($"Response Text: {request.downloadHandler.text}"); // Log error response from GAS
            }
            else
            {
                Debug.Log("Score sent successfully!");
                Debug.Log($"Response: {request.downloadHandler.text}"); // Log success response from GAS
            }
        }
    }

    public void SpawnNewCube()
    {
        if (isSpawning) return; //prevent multiple spawns

        StartCoroutine(WaitForStableTower());
    }

    private IEnumerator WaitForStableTower()
    {
        isSpawning = true;

        //yield until tower is stable
        while (!IsTowerStable())
        {
            yield return null;
        }

        //Spawn the new cube
        if (useRandomSizedCubes)
        {
            Vector3 randomSize = new Vector3(UnityEngine.Random.Range(0.5f, 1.5f), UnityEngine.Random.Range(0.5f, 1.5f), UnityEngine.Random.Range(0.5f, 1.5f));
            currentCube = Instantiate(cubePrefab, spawnPoint.position, Quaternion.identity);
            currentCube.transform.localScale = randomSize;
        }
        else
        {
            currentCube = Instantiate(cubePrefab, spawnPoint.position, Quaternion.identity);
            currentCube.transform.localScale = Vector3.one;
        }

        currentCube.GetComponent<Rigidbody>().isKinematic = true; //Keep floating
        StartCoroutine(ResetSpawnFlag()); //Reset the flag after a short delay
    }

    private bool IsTowerStable()
    {
        Rigidbody[] cubes = FindObjectsOfType<Rigidbody>();

        foreach (Rigidbody cube in cubes)
        {
            //If moving, tower is not stable
            if (cube.velocity.magnitude > 0.1f)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator ResetSpawnFlag()
    {
        yield return new WaitForSeconds(0.5f);
        isSpawning = false;
    }

    public float GetHighestPoint()
    {
        return highestPoint;
    }

    public void TriggerExplosionAndGameOverSequence(Vector3 hitPosition)
    {
        if (isGameOver) return; 
        isGameOver = true; 

        if (managerAudioSource != null && explosionSound != null)
        {
            managerAudioSource.PlayOneShot(explosionSound);
        }

        StartCoroutine(ExplosionSequenceCoroutine(hitPosition));
    }

    private IEnumerator ExplosionSequenceCoroutine(Vector3 hitPosition) // hitPosition is still used for AddExplosionForce origin
    {
        CubeScript[] allCubesScripts = FindObjectsOfType<CubeScript>();
        foreach (CubeScript cube in allCubesScripts)
        {
            if (cube != null)
            {
                cube.StartFlashing(flashInterval);
            }
        }

        yield return new WaitForSeconds(soundToExplosionDelay);

        // Find all cube rigidbodies to get their positions and apply force
        Rigidbody[] allRigidbodies = FindObjectsOfType<Rigidbody>();

        // Instantiate particles and stop flashing for each cube
        foreach (Rigidbody rb in allRigidbodies)
        {
            if (rb != null && rb.CompareTag("Cube"))
            {
                // Instantiate particles at this cube's position
                if (explosionParticlesPrefab != null)
                {
                    Instantiate(explosionParticlesPrefab, rb.position, Quaternion.identity);
                }

                // Stop flashing for this cube (find its script)
                CubeScript cubeScript = rb.GetComponent<CubeScript>();
                if (cubeScript != null)
                {
                    cubeScript.StopFlashing();
                }
            }
        }

        // Apply explosion force (still originates from the initial hit point for direction)
        foreach (Rigidbody rb in allRigidbodies)
        {
            if (rb != null && rb.CompareTag("Cube"))
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(explosionForce, hitPosition, explosionRadius, explosionUpwardModifier, ForceMode.Impulse);
            }
        }

        yield return new WaitForSeconds(soundToExplosionDelay); // Using existing delay before GameOver

        GameOver();
    }
}
