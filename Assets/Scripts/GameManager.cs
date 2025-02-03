using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    ITouchable selectedObject;
    private CameraController cameraController;
    private Vector2 initialTouchPosition;
    private Transform selectedTransform;

    void Start()
    {
        cameraController = FindObjectOfType<CameraController>();
    }

    void Update()
    {
        // Enable/Disable camera controls based on selection
        cameraController.enabled = selectedObject == null;

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                initialTouchPosition = touch.position;
                OnTapRegistered(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (selectedTransform != null)
                {
                    OnTouchMove(touch);
                }
            }
        }
    }

    public void OnTapRegistered(Vector2 tapPosition)
    {
        Ray r = Camera.main.ScreenPointToRay(tapPosition);
        RaycastHit info;
        if (Physics.Raycast(r, out info))
        {
            ITouchable newObject = info.collider.gameObject.GetComponent<ITouchable>();
            if (newObject != null)
            {
                if (selectedObject == newObject)
                {
                    selectedObject.SelectToggle(false);
                    selectedObject = null;
                    selectedTransform = null;
                }
                else
                {
                    if (selectedObject != null)
                    {
                        selectedObject.SelectToggle(false);
                    }
                    selectedObject = newObject;
                    selectedTransform = info.collider.transform;
                    newObject.SelectToggle(true);
                }
            }
        }
    }

    private void OnTouchMove(Touch touch)
    {
        if (selectedObject is BaseObjectScript obj)
        {
            obj.MoveObject(selectedTransform, touch);
        }
    }

    public void OnObjectScaleAndRotate(Touch t1, Touch t2)
    {
        if (selectedObject is BaseObjectScript obj)
        {
            obj.ScaleObject(t1, t2);
            obj.RotateObject(t1, t2);
        }
    }
}