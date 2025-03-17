using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2f; // ����/ä�� ����
    public int attackDamage = 10; // ���ݷ�
    public LayerMask interactableLayer; // ��ȣ�ۿ� ������ ���̾�

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 ��Ŭ��
        {
            Attack();
        }
    }

    void Attack()
    {
        void Attack()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
            {
                EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
                if (enemy != null)
                {
                    enemy.TakeDamage(10f);  // �� ü�� ����
                }
            }
        }

    }


}
