using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class ResetScene : MonoBehaviour
{
    public void RestartLevel()
    {
        // Reload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        Debug.Log("Level restarted");
    }
}