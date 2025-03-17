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
        RaycastHit hit;
        Vector3 rayStartPosition = transform.position + Vector3.up * 1.5f;

        // Raycast�� ���� ���� ���� ��ü�� Ž��
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, interactableLayer))
        {
            // ������ ��ȣ�ۿ�
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);  // �� ü�� ����
            }

            // NaturalObject���� ��ȣ�ۿ�
            {
                NaturalObject naturalObject = hit.collider.GetComponent<NaturalObject>();
                if (naturalObject != null)
                {
                    naturalObject.HarvesstNatureObject();
                }
            }
        }
    }

  
}
