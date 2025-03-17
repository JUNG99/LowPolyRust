using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // ��ȣ�ۿ� �Ÿ�
    public LayerMask interactableLayer; // �������� �ִ� ���̾�

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main; // ���� ī�޶� ��������
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Interact()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactionRange, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                //interactable.Interact();
            }
            else if (hit.collider.CompareTag("Item")) // �������̽��� ���� ��� �⺻ ó��
            {
                PickupItem(hit.collider.gameObject);
            }
        }
    }

    void PickupItem(GameObject item)
    {
        Debug.Log($"{item.name} ȹ��");
        Destroy(item); // ������ ����
    }
}
