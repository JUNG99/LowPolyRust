using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2f; // 공격/채집 범위
    public int attackDamage = 10; // 공격력
    public LayerMask interactableLayer; // 상호작용 가능한 레이어

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

        // Raycast로 공격 범위 내의 물체를 탐지
        if (Physics.Raycast(rayStartPosition,transform.position, out hit, attackRange, interactableLayer))
        {
            // 적과의 상호작용
            EnemyAI enemy = hit.collider.GetComponent<EnemyAI>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);  // 적 체력 감소
            }

            // NaturalObject와의 상호작용
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
