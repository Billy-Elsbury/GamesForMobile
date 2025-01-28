using UnityEngine;

public class GestureManager : MonoBehaviour
{
    float timer = 0f;
    bool touchMoved = false;
    private float maxTapTime = 0.3f;
    GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            timer += Time.deltaTime;

            switch (t.phase)
            {
                case TouchPhase.Began:
                    timer = 0;
                    touchMoved = false;
                    break;

                case TouchPhase.Moved:
                    touchMoved = true;
                    if (gameManager.selectedObject != null)
                    {
                        gameManager.selectedObject.MoveObject(t.position);
                    }
                    break;

                case TouchPhase.Ended:
                    if (timer < maxTapTime && !touchMoved)
                    {
                        gameManager.tapRegisteredAt(t.position);
                    }
                    break;
            }
        }
    }
}
