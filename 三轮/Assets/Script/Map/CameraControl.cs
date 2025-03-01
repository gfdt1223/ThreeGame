using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;        // �����ƶ��ٶ�
    [SerializeField] private float shiftMultiplier = 2f; // ���ٱ���
    [SerializeField] private float edgePadding = 50f;    // ��Ļ��Ե�����ƶ������ؾ���
    [SerializeField] private bool edgeScrolling = true;  // �Ƿ����ñ�Ե����

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 5f;       // �����ٶ�
    [SerializeField] private float minZoom = 2f;         // ��С��Ұ
    [SerializeField] private float maxZoom = 10f;        // �����Ұ
    [SerializeField] private float zoomSmoothing = 5f;   // ����ƽ��ϵ��

    [Header("Map Bounds")]
    [SerializeField] private float mapMinX = 0f;         // ��ͼ��߽�
    [SerializeField] private float mapMaxX = 100f;       // ��ͼ�ұ߽�
    [SerializeField] private float mapMinY = 0f;         // ��ͼ�±߽�
    [SerializeField] private float mapMaxY = 100f;       // ��ͼ�ϱ߽�

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
        ClampCameraPosition(); // ÿ֡���������λ��
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = Vector3.zero;

        // �������루WSAD��
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");

        // ��Ļ��Ե��������ѡ��
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

        // ����ʵ���ƶ��ٶȣ���סShift���٣�
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            currentSpeed *= shiftMultiplier;
        }

        // Ӧ���ƶ�
        Vector3 moveVector = moveDirection.normalized * currentSpeed * Time.deltaTime;
        transform.position += moveVector;
    }

    private void HandleZoom()
    {
        // ����������
        float scrollData = Input.GetAxis("Mouse ScrollWheel");

        if (scrollData != 0)
        {
            targetZoom -= scrollData * zoomSpeed;
            targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        }

        // ƽ�����Ź���
        mainCamera.orthographicSize = Mathf.Lerp(
            mainCamera.orthographicSize,
            targetZoom,
            Time.deltaTime * zoomSmoothing
        );
    }

    private void ClampCameraPosition()
    {
        // ����������ӿڷ�Χ
        float vertExtent = mainCamera.orthographicSize;
        float horzExtent = vertExtent * mainCamera.aspect;

        // ������Ч�ƶ���Χ
        float effectiveMinX = mapMinX + horzExtent;
        float effectiveMaxX = mapMaxX - horzExtent;
        float effectiveMinY = mapMinY + vertExtent;
        float effectiveMaxY = mapMaxY - vertExtent;

        // ���������λ��
        float clampedX = Mathf.Clamp(transform.position.x, effectiveMinX, effectiveMaxX);
        float clampedY = Mathf.Clamp(transform.position.y, effectiveMinY, effectiveMaxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

   
}