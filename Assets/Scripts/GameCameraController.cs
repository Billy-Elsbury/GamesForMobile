using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float rotationSpeed = 0.2f;
    public float gyroSensitivity = 2.0f;

    private Vector3 lastPanPosition;
    private Vector2 lastMidpoint;
    private bool isPanning = false;
    private bool useGyro = false;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 45;
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
                lastPanPosition = t.position;
                isPanning = true;
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
            }
            else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
            {
                Vector2 delta = currentMidpoint - lastMidpoint;

                float horizontalAngle = delta.x * rotationSpeed;
                OrbitCameraHorizontal(horizontalAngle);

                float verticalAngle = -delta.y * rotationSpeed;
                OrbitCameraVertical(verticalAngle);

                lastMidpoint = currentMidpoint;
            }
        }
    }

    void ApplyGyroRotation()
    {
        if (!SystemInfo.supportsGyroscope) return;

        Vector3 gyroDelta = Input.gyro.rotationRateUnbiased;

        transform.Rotate(Vector3.up, -gyroDelta.y * gyroSensitivity, Space.World);
        transform.Rotate(Vector3.right, -gyroDelta.x * gyroSensitivity, Space.World); // Use world space for gyro rotation
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

        Vector3 up = Vector3.ProjectOnPlane(Vector3.up, Vector3.right);

        Vector3 move = (up * offset.y) * panSpeed;

        transform.Translate(move, Space.World);

        lastPanPosition = newPanPosition;
    }

    void OrbitCameraHorizontal(float angle)
    {
        // Rotate around the world Y-axis
        transform.RotateAround(transform.position, Vector3.up, angle);
    }

    void OrbitCameraVertical(float angle)
    {
        // Rotate around the world X-axis
        transform.RotateAround(transform.position, Vector3.right, angle);
    }
}