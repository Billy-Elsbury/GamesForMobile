using System.Collections.Generic;
using UnityEngine;

public class ResetScene : MonoBehaviour
{
    // List of objects to reset (drag them into the inspector, or find them at runtime)
    public List<GameObject> objectsToReset;

    // Temporary variables to store initial states
    private List<Vector3> initialPositions = new List<Vector3>();
    private List<Quaternion> initialRotations = new List<Quaternion>();
    private List<Vector3> initialScales = new List<Vector3>();
    private List<Color> initialColors = new List<Color>();

    void Start()
    {
        // Store the initial state for each object
        foreach (var obj in objectsToReset)
        {
            if (obj != null)
            {
                // Store the initial position, rotation, and scale
                initialPositions.Add(obj.transform.position);
                initialRotations.Add(obj.transform.rotation);
                initialScales.Add(obj.transform.localScale);

                // Store the initial color if the object has a renderer
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    initialColors.Add(renderer.material.color);
                }
                else
                {
                    initialColors.Add(Color.white);  // Default to white if no renderer found
                }
            }
        }
    }

    // Method to reset all objects to their initial states
    public void ResetObjects()
    {
        for (int i = 0; i < objectsToReset.Count; i++)
        {
            GameObject obj = objectsToReset[i];

            if (obj != null)
            {
                // Reset position, rotation, and scale
                obj.transform.position = initialPositions[i];
                obj.transform.rotation = initialRotations[i];
                obj.transform.localScale = initialScales[i];

                // Reset color if the object has a renderer
                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = initialColors[i];
                }
            }
        }

        Debug.Log("All objects reset to their initial state.");
    }
}
