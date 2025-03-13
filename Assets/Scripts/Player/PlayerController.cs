using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f; //�̵�->�ȱ�� ����
    public float jumpForce = 5f;
    public float crouchHeight = 0.5f;
    public float standHeight = 2f;
    public float sprintSpeed = 10f;
    public float crouchSpeed = 0.5f; //�ɾ��� �� �ӵ�

    private Rigidbody rb;
    private float originalHeight;
    private bool isCrouching = false;
    private float moveSpeed; //�̵�

    public Transform playerCamera;
    private float mouseSensitivity = 2f;
    private float xRotation = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalHeight = transform.localScale.y; // �⺻ ���̸� ����
        Cursor.lockState = CursorLockMode.Locked; // ���콺�� ���
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

        if(Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            moveSpeed = sprintSpeed;
        }

        else if(isCrouching)
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
        playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, crouchHeight, playerCamera.localPosition.z);
        Debug.Log("�ɱ�Ϸ�");
    }

    void StandUp()
    {
        isCrouching = false;
        transform.localScale = new Vector3(transform.localScale.x, standHeight, transform.localScale.z);
        playerCamera.localPosition = new Vector3(playerCamera.localPosition.x, standHeight, playerCamera.localPosition.z);
        
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
