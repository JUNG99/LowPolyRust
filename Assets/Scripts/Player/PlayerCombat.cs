using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2f; // ����/ä�� ����
    public int attackDamage = 10; // ���ݷ�
    public LayerMask interactableLayer; // ��ȣ�ۿ� ������ ���̾�

    private Build buildScript;


  
    void Update()
    {
        
        
        if (Input.GetMouseButtonDown(0) && !buildScript.buildMode)
        {
            Attack();
        }
       
                
        
    }

    void Attack()
    {
        RaycastHit hit;
        Vector3 rayStartPosition = transform.position + Vector3.up * 1.5f;

        // Raycast�� ���� ���� ���� ��ü�� Ž��
        if (Physics.Raycast(rayStartPosition,transform.position, out hit, attackRange, interactableLayer))
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

        Debug.DrawRay(rayStartPosition, transform.forward * attackRange, Color.red, 0.1f);
    }

  
}
