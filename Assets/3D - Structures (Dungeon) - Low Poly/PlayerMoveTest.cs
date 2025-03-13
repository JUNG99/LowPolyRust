using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;  // �⺻ �̵� �ӵ�

    [Header("Look")]
    public Transform cameraContainer;
    public float lookSensitivity = 2f;  // ���콺 �ΰ���
    public float minXLook = -60f;
    public float maxXLook = 60f;
    private float camCurXRot;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;  // Ŀ�� ���
    }

    private void Update()
    {
        // �̵� �� ī�޶� ȸ�� ó��
        Move();
        Look();
    }

    void Move()
    {
        // Ű���� �Է¿� ���� �̵�
        float horizontal = Input.GetAxisRaw("Horizontal");  // A/D, Left/Right
        float vertical = Input.GetAxisRaw("Vertical");  // W/S, Up/Down

        Vector3 moveDir = transform.right * horizontal + transform.forward * vertical;  // �̵� ����
        rb.velocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.z * moveSpeed);  // �̵� ����
    }

    void Look()
    {
        // ���콺 �Է¿� ���� ȸ��
        float mouseX = Input.GetAxis("Mouse X");  // �¿� ȸ��
        float mouseY = Input.GetAxis("Mouse Y");  // ���� ȸ��

        // ī�޶��� ���� ȸ��
        camCurXRot -= mouseY * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        cameraContainer.localEulerAngles = new Vector3(camCurXRot, 0f, 0f);

        // ĳ������ �¿� ȸ��
        transform.eulerAngles += new Vector3(0f, mouseX * lookSensitivity, 0f);
    }
}
