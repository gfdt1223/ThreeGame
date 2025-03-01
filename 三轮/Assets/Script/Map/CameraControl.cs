using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;        // 基础移动速度
    [SerializeField] private float shiftMultiplier = 2f; // 加速倍数
    [SerializeField] private float edgePadding = 50f;    // 屏幕边缘触发移动的像素距离
    [SerializeField] private bool edgeScrolling = true;  // 是否启用边缘滚动

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;       // 缩放速度
    [SerializeField] private float minZoom = 2f;         // 最小视野
    [SerializeField] private float maxZoom = 10f;        // 最大视野
    [SerializeField] private float zoomSmoothing = 5f;   // 缩放平滑系数

    [Header("Map Bounds")]
    [SerializeField] private float mapMinX = 0f;         // 地图左边界
    [SerializeField] private float mapMaxX = 100f;       // 地图右边界
    [SerializeField] private float mapMinY = 0f;         // 地图下边界
    [SerializeField] private float mapMaxY = 100f;       // 地图上边界

    private Camera mainCamera;
    private float targetZoom;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
        targetZoom = mainCamera.orthographicSize;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
        ClampCameraPosition(); // 每帧限制摄像机位置
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        // 键盘输入（WSAD）
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");

        // 屏幕边缘滚动（可选）
        if (edgeScrolling)
        {
            Vector2 mousePosition = Input.mousePosition;

            if (mousePosition.x <= edgePadding)
                moveDirection.x = -1;
            else if (mousePosition.x >= Screen.width - edgePadding)
                moveDirection.x = 1;

            if (mousePosition.y <= edgePadding)
                moveDirection.y = -1;
            else if (mousePosition.y >= Screen.height - edgePadding)
                moveDirection.y = 1;
        }

        // 计算实际移动速度（按住Shift加速）
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= shiftMultiplier;
        }

        // 应用移动
        Vector3 moveVector = moveDirection.normalized * currentSpeed * Time.deltaTime;
        transform.position += moveVector;
    }

    private void HandleZoom()
    {
        // 鼠标滚轮输入
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        if (scrollData != 0)
        {
            targetZoom -= scrollData * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        // 平滑缩放过渡
        mainCamera.orthographicSize = Mathf.Lerp(
            mainCamera.orthographicSize,
            targetZoom,
            Time.deltaTime * zoomSmoothing
        );
    }

    private void ClampCameraPosition()
    {
        // 计算摄像机视口范围
        float vertExtent = mainCamera.orthographicSize;
        float horzExtent = vertExtent * mainCamera.aspect;

        // 计算有效移动范围
        float effectiveMinX = mapMinX + horzExtent;
        float effectiveMaxX = mapMaxX - horzExtent;
        float effectiveMinY = mapMinY + vertExtent;
        float effectiveMaxY = mapMaxY - vertExtent;

        // 限制摄像机位置
        float clampedX = Mathf.Clamp(transform.position.x, effectiveMinX, effectiveMaxX);
        float clampedY = Mathf.Clamp(transform.position.y, effectiveMinY, effectiveMaxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

   
}