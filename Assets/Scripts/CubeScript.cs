using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour, ITouchable
{
    Renderer r;

    void Start()
    {
        r = GetComponent<Renderer>();
    }
    public void SelectToggle(bool selected)
    {
        if (selected)
        {
            changeColor(Color.red);
        }
        else 
            changeColor(Color.white);
        
    }
    public void changeColor(Color color)
    {
        r.material.color = color;
    }

    void Update()
    {

    }
}
