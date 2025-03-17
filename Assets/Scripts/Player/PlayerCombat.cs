using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2f; // 공격/채집 범위
    public int attackDamage = 10; // 공격력
    public LayerMask interactableLayer; // 상호작용 가능한 레이어

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 좌클릭
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
                    enemy.TakeDamage(10f);  // 적 체력 감소
                }
            }
        }

    }


}
