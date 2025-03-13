using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;  // 기본 이동 속도

    [Header("Look")]
    public Transform cameraContainer;
    public float lookSensitivity = 2f;  // 마우스 민감도
    public float minXLook = -60f;
    public float maxXLook = 60f;
    private float camCurXRot;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;  // 커서 잠금
    }

    private void Update()
    {
        // 이동 및 카메라 회전 처리
        Move();
        Look();
    }

    void Move()
    {
        // 키보드 입력에 따른 이동
        float horizontal = Input.GetAxisRaw("Horizontal");  // A/D, Left/Right
        float vertical = Input.GetAxisRaw("Vertical");  // W/S, Up/Down

        Vector3 moveDir = transform.right * horizontal + transform.forward * vertical;  // 이동 방향
        rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);  // 이동 적용
    }

    void Look()
    {
        // 마우스 입력에 따른 회전
        float mouseX = Input.GetAxis("Mouse X");  // 좌우 회전
        float mouseY = Input.GetAxis("Mouse Y");  // 상하 회전

        // 카메라의 상하 회전
        camCurXRot -= mouseY * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(camCurXRot, 0f, 0f);

        // 캐릭터의 좌우 회전
        transform.eulerAngles += new Vector3(0f, mouseX * lookSensitivity, 0f);
    }
}
