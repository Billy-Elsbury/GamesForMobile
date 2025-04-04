using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    ITouchable selectedObject;

    public float panSpeed = 20f;
    public float rotationSpeed = 0.2f;
    public float zoomSpeed = 1f;
    public float gyroSensitivity = 2.0f;
    public Transform groundPlane;

    private GameManager gameManager;
    private Vector3 lastPanPosition;
    private Vector2 lastMidpoint;
    private float lastPinchDistance;
    private bool isPanning = false;

    private bool useGyro = false;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 45;
    }

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (useGyro)
        {
            ApplyGyroRotation();
        }

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                if (selectedObject == null)
                {
                    lastPanPosition = t.position;
                    isPanning = true;
                }
            }
            else if (t.phase == TouchPhase.Moved && isPanning)
            {
                PanCamera(t.position);
            }
            else if (t.phase == TouchPhase.Ended)
            {
                isPanning = false;
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            Vector2 currentMidpoint = (t1.position + t2.position) / 2;

            if (t1.phase == TouchPhase.Began || t2.phase == TouchPhase.Began)
            {
                lastMidpoint = currentMidpoint;
                lastPinchDistance = Vector2.Distance(t1.position, t2.position);
            }
            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {
                //rotation touch
                Vector2 delta = currentMidpoint - lastMidpoint;

                float horizontalAngle = delta.x * rotationSpeed;
                OrbitCameraHorizontal(horizontalAngle);

                float verticalAngle = -delta.y * rotationSpeed;
                OrbitCameraVertical(verticalAngle);

                lastMidpoint = currentMidpoint;

                //pinch zoom
                float currentPinchDistance = Vector2.Distance(t1.position, t2.position);
                float pinchDelta = currentPinchDistance - lastPinchDistance;

                ZoomCamera(pinchDelta);

                lastPinchDistance = currentPinchDistance;
            }
        }
    }

    void ApplyGyroRotation()
    {
        if (!SystemInfo.supportsGyroscope) return;

        Vector3 gyroDelta = Input.gyro.rotationRateUnbiased;

        transform.Rotate(Vector3.up, -gyroDelta.y * gyroSensitivity, Space.World);
        transform.Rotate(Vector3.right, -gyroDelta.x * gyroSensitivity, Space.Self);
    }

    public void ToggleGyro()
    {
        useGyro = !useGyro;
        if (useGyro) EnableGyro();
    }

    private void EnableGyro()
    {
        Input.gyro.enabled = true;
        useGyro = true;
    }

    void PanCamera(Vector3 newPanPosition)
    {
        Vector3 offset = Camera.main.ScreenToViewportPoint(lastPanPosition - newPanPosition);

        Vector3 right = Vector3.ProjectOnPlane(transform.right, Vector3.up);
        Vector3 forward = Vector3.ProjectOnPlane(transform.forward, Vector3.up);

        Vector3 move = (right * offset.x + forward * offset.y) * panSpeed;

        transform.Translate(move, Space.World);

        lastPanPosition = newPanPosition;
    }

    void OrbitCameraHorizontal(float angle)
    {
        transform.RotateAround(transform.position, Vector3.up, angle);
    }

    void OrbitCameraVertical(float angle)
    {
        transform.RotateAround(transform.position, transform.right, angle);
    }

    void ZoomCamera(float pinchDelta)
    {
        Vector3 zoomDirection = transform.forward * (pinchDelta * zoomSpeed * Time.deltaTime);
        transform.position += zoomDirection;
    }
}
