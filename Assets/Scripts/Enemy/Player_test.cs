using System.Collections;
using UnityEngine;

public class Player: MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 5f;

    [Header("Ground Check")]
    private bool isGrounded;

    private Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move();
    }
    
    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 moveDir = transform.right * moveX + transform.forward * moveZ;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    
}