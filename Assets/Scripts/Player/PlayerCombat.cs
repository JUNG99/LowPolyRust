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
        if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange, interactableLayer))
        {
            // IInteractable �������̽��� �ִ� ������Ʈ��� ����
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                return;
            }

            // ���� �����ϴ� ���
            //Enemy enemy = hit.collider.GetComponent<Enemy>();
            //if (enemy != null)
            //{
            //    enemy.TakeDamage(attackDamage);
            //    Debug.Log($"{enemy.gameObject.name} ���� {attackDamage} �������� ��!");
            //}
        }
    }
}
