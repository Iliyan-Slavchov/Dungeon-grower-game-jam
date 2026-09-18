using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    private PlayerActions playerActions;
    private Camera camera;

    [Header("Pan")]
    [SerializeField] private float worldPlaneZ = 0f;

    [Header("Zoom")]
    [SerializeField, Min(0.0001f)] private float zoomSensitivity = 0.1f;

    [SerializeField, Min(0.01f)] private float minZoom = 1.5f;

    [SerializeField, Min(0.01f)] private float maxZoom = 6f;

    [Header("Bounds")]
    [SerializeField] private float minX = -9.5f;
    [SerializeField] private float maxX = 9.5f;
    [SerializeField] private float minY = -5.1f;
    [SerializeField] private float maxY = 5.6f;

    private Vector3 grabbedWorldPosition;
    private bool isDragging;

    private void Awake()
    {
        playerActions = new PlayerActions();
        camera = GetComponent<Camera>();

        if (!camera.orthographic)
        {
            Debug.LogError(
                "CameraMovement requires an orthographic camera.",
                this
            );
        }
    }

    private void OnValidate()
    {
        minZoom = Mathf.Max(0.01f, minZoom);
        maxZoom = Mathf.Max(minZoom, maxZoom);

        if (minX >= maxX)
            Debug.LogWarning("Camera minX must be less than maxX.", this);

        if (minY >= maxY)
            Debug.LogWarning("Camera minY must be less than maxY.", this);
    }

    private void OnEnable()
    {
        playerActions.CameraMovement.Enable();
    }

    private void OnDisable()
    {
        playerActions.CameraMovement.Disable();
    }

    private void OnDestroy()
    {
        playerActions.Dispose();
    }

    private void Update()
    {
        Vector2 screenPointerPosition =
            playerActions.CameraMovement.Point.ReadValue<Vector2>();

        HandlePan(screenPointerPosition);
        HandleZoom(screenPointerPosition);

        ClampCamera();

        if (isDragging)
        {
            grabbedWorldPosition =
                ScreenToWorld(screenPointerPosition);
        }
    }

    private void HandlePan(Vector2 screenPointerPosition)
    {
        if (playerActions.CameraMovement.Pan.WasPressedThisFrame())
        {
            grabbedWorldPosition = ScreenToWorld(screenPointerPosition);
            isDragging = true;
        }

        if (isDragging && playerActions.CameraMovement.Pan.IsPressed())
        {
            Vector3 currentWorldPosition =
                ScreenToWorld(screenPointerPosition);

            Vector3 difference =
                grabbedWorldPosition - currentWorldPosition;

            transform.position += new Vector3(
                difference.x,
                difference.y,
                0f
            );
        }

        if (playerActions.CameraMovement.Pan.WasReleasedThisFrame())
        {
            isDragging = false;
        }
    }

    private void HandleZoom(Vector2 screenPointerPosition)
    {
        float zoomInput =
            playerActions.CameraMovement.Zoom.ReadValue<float>();

        if (Mathf.Approximately(zoomInput, 0f))
            return;

        Vector3 worldPositionBeforeZoom =
            ScreenToWorld(screenPointerPosition);

        float zoomMultiplier =
            Mathf.Exp(-zoomInput * zoomSensitivity);

        camera.orthographicSize = GetClampedZoom(
            camera.orthographicSize * zoomMultiplier);

        Vector3 worldPositionAfterZoom =
            ScreenToWorld(screenPointerPosition);

        Vector3 difference =
            worldPositionBeforeZoom - worldPositionAfterZoom;

        transform.position += new Vector3(
            difference.x,
            difference.y,
            0f
        );
    }

    private Vector3 ScreenToWorld(Vector2 screenPosition)
    {
        float distanceFromCamera =
            worldPlaneZ - transform.position.z;

        Vector3 worldPosition = camera.ScreenToWorldPoint(
            new Vector3(
                screenPosition.x,
                screenPosition.y,
                distanceFromCamera
            )
        );

        worldPosition.z = worldPlaneZ;

        return worldPosition;
    }

    private void ClampCamera()
    {
        camera.orthographicSize =
            GetClampedZoom(camera.orthographicSize);

        float halfHeight = camera.orthographicSize;
        float halfWidth = halfHeight * camera.aspect;

        float clampedX = Mathf.Clamp(
            transform.position.x,
            minX + halfWidth,
            maxX - halfWidth
        );

        float clampedY = Mathf.Clamp(
            transform.position.y,
            minY + halfHeight,
            maxY - halfHeight
        );

        transform.position = new Vector3(
            clampedX,
            clampedY,
            transform.position.z
        );
    }

    private float GetMaximumAllowedZoom()
    {
        float worldWidth = maxX - minX;
        float worldHeight = maxY - minY;

        float maxZoomFromHeight =
            worldHeight / 2f;

        float maxZoomFromWidth =
            worldWidth / (2f * camera.aspect);

        return Mathf.Min(
            maxZoom,
            maxZoomFromHeight,
            maxZoomFromWidth
        );
    }

    private float GetClampedZoom(float desiredZoom)
    {
        float maximumAllowedZoom = GetMaximumAllowedZoom();

        if (maximumAllowedZoom < minZoom)
            return maximumAllowedZoom;

        return Mathf.Clamp(
            desiredZoom,
            minZoom,
            maximumAllowedZoom
        );
    }
}