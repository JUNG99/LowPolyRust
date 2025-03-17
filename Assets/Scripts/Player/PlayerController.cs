using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpForce = 5f;
    public float crouchSpeed = 2f;
    public float crouchHeight = 0.5f;
    public float standHeight = 1f;

    private Rigidbody rb;
    private float moveSpeed;
    private bool isCrouching = false;

    public Transform cameraHolder;  // MainCamera를 여기 연결
    private float mouseSensitivity = 2f;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked; // 마우스 잠금
        moveSpeed = walkSpeed; // 기본 이동 속도 설정
    }

    void Update()
    {
        MovePlayer();
        Jump();
        Crouch();
        LookAround();
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;

        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            moveSpeed = sprintSpeed;
        }
        else if (isCrouching)
        {
            moveSpeed = crouchSpeed;
        }
        else
        {
            moveSpeed = walkSpeed;
        }

        rb.MovePosition(transform.position + moveDirection.normalized * moveSpeed * Time.deltaTime);
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && Mathf.Abs(rb.velocity.y) < 0.01f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching)
            {
                StandUp();
            }
            else
            {
                CrouchDown();
            }
        }
    }

    void CrouchDown()
    {
        isCrouching = true;
        transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, crouchHeight, cameraHolder.localPosition.z);
        Debug.Log("앉기 완료");
    }

    void StandUp()
    {
        isCrouching = false;
        transform.localScale = new Vector3(transform.localScale.x, standHeight, transform.localScale.z);
        cameraHolder.localPosition = new Vector3(cameraHolder.localPosition.x, standHeight, cameraHolder.localPosition.z);
        Debug.Log("서기 완료");
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //MainCamera 회전
        transform.Rotate(Vector3.up * mouseX);
    }
}
