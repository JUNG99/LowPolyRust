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
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, interactableLayer))
        {
            // IInteractable 인터페이스가 있는 오브젝트라면 실행
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                return;
            }

            // 적을 공격하는 경우
            //Enemy enemy = hit.collider.GetComponent<Enemy>();
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(attackDamage);
            //    Debug.Log($"{enemy.gameObject.name} 에게 {attackDamage} 데미지를 줌!");
            //}
        }
    }
}
