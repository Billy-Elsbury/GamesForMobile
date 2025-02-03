using UnityEngine;

public class GestureManager : MonoBehaviour
{
    private float maxTapTime = 0.3f;
    private float tapTimer = 0f;
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
            Touch t = Input.GetTouch(0);
            tapTimer += Time.deltaTime;

            switch (t.phase)
            {
                case TouchPhase.Began:
                    tapTimer = 0;
                    touchMoved = false;
                    break;
                case TouchPhase.Moved:
                    touchMoved = true;
                    break;
                case TouchPhase.Ended:
                    if (tapTimer < maxTapTime && !touchMoved)
                    {
                        gameManager.OnTapRegistered(t.position);
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
