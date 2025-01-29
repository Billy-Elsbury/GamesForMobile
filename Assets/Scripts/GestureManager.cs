using UnityEngine;

public class GestureManager : MonoBehaviour
{
    private float maxTapTime = 0.3f;
    private float timer = 0f;
    private bool touchMoved = false;
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch t1 = Input.GetTouch(0);
            timer += Time.deltaTime;

            switch (t1.phase)
            {
                case TouchPhase.Began:
                    timer = 0;
                    touchMoved = false;
                    break;

                case TouchPhase.Moved:
                    touchMoved = true;
                    gameManager.OnObjectMove(t1);
                    break;

                case TouchPhase.Ended:
                    if (timer < maxTapTime && !touchMoved)
                    {
                        gameManager.tapRegisteredAt(t1.position);
                    }
                    break;
            }

            if (Input.touchCount == 2)
            {
                gameManager.OnObjectScaleAndRotate(Input.GetTouch(0), Input.GetTouch(1));
            }
        }
    }
}
