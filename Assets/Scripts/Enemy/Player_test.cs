//using System.Collections;
//using UnityEngine;

//public class Player : MonoBehaviour
//{
//    [Header("Player Settings")]
//    public float moveSpeed = 5f;

//    [Header("Ground Check")]
//    private bool isGrounded;

//    private Rigidbody rigidbody;

//    void Start()
//    {
//        rigidbody = GetComponent<Rigidbody>();
//    }

//    void Update()
//    {
//        Move();
//    }
    
//    void Move()
//    {
//        float moveX = Input.GetAxis("Horizontal");
//        float moveZ = Input.GetAxis("Vertical");
//        Vector3 moveDir = transform.right * moveX + transform.forward * moveZ;
//        transform.position += moveDir * moveSpeed * Time.deltaTime;
//    }

//    // ?�레?�어가 ?�과 충돌 ???�에�??��?지�??�히??로직 추�?
//    void OnCollisionEnter(Collision collision)
//    {
//        if (collision.gameObject.CompareTag("Enemy"))
//        {
//            EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
//            if (enemy != null)
//            {
//                enemy.TakeDamage(10f);
//                Debug.Log("Hit enemy, applied 10 damage.");
//            }
//        }
//    }
//}