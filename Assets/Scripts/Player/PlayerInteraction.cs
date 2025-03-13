using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // ��ȣ�ۿ� �Ÿ�
    public LayerMask interactableLayer; // �������� �ִ� ���̾�

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange, interactableLayer))
        {
            if (hit.collider.CompareTag("Item"))
            {
                Destroy(hit.collider.gameObject); // ������ ����
                Debug.Log($"{hit.collider.gameObject.name} �������� �ֿ���!");
            }
        }
    }
}
