using System.Collections.Generic;
using UnityEngine;

public class ResetScene : MonoBehaviour
{
    public List<GameObject> objectsToReset;

    //temp variables to store initial states
    private List<Vector3> initialPositions = new List<Vector3>();
    private List<Quaternion> initialRotations = new List<Quaternion>();
    private List<Vector3> initialScales = new List<Vector3>();
    private List<Color> initialColors = new List<Color>();

    void Start()
    {
        FindAllTouchableObjects();

        //store initial state for each object
        foreach (var obj in objectsToReset)
        {
            if (obj != null)
            {
                initialPositions.Add(obj.transform.position);
                initialRotations.Add(obj.transform.rotation);
                initialScales.Add(obj.transform.localScale);

                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    initialColors.Add(renderer.material.color);
                }
                else
                {
                    initialColors.Add(Color.white);
                }
            }
        }
    }

    private void FindAllTouchableObjects()
    {
        // Clear the list before populating it
        //objectsToReset.Clear();

        MonoBehaviour[] allObjects = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour obj in allObjects)
        {
            if (obj is ITouchable)
            {
                objectsToReset.Add(obj.gameObject);
            }
        }

        Debug.Log($"Found {objectsToReset.Count} objects implementing ITouchable.");
    }

    public void ResetObjects()
    {
        for (int i = 0; i < objectsToReset.Count; i++)
        {
            GameObject obj = objectsToReset[i];

            if (obj != null)
            {
                obj.transform.position = initialPositions[i];
                obj.transform.rotation = initialRotations[i];
                obj.transform.localScale = initialScales[i];

                Renderer renderer = obj.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = initialColors[i];
                }
            }
        }

        Debug.Log("All objects reset");
    }
}