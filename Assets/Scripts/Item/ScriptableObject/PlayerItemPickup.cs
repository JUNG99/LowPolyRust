using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public float pickupRange = 3f; // �������� ������ �Ÿ�
    public Camera playerCamera; // �÷��̾��� ī�޶�
    private UIInventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<UIInventory>();
    }
    void Update()
    {
        // Ray�� �ð������� ��� ǥ��
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * pickupRange, Color.red);

        if (Input.GetKeyDown(KeyCode.E))
        {
            TryPickupItem();
        }
    }

    void TryPickupItem()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickupRange))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                PickupItem(hit.collider.gameObject);

            }
        }
    }

    void PickupItem(GameObject item)
    {
        ItemData data = item.GetComponent<ItemObject>()?.itemdata; 
        if (data == null)
        {
            Debug.LogWarning($"{item.name}�� ItemData�� �����ϴ�!");
            return;
        }

        bool added = inventory.AddItem(data); // �κ��丮�� ������ �߰�
        if (added)
        {
            Debug.Log($"{data.displayName}��(��) ȹ���߽��ϴ�!");
            Destroy(item); // ���������� �߰��Ǹ� ������ ����
        }
        else
        {
            Debug.LogWarning("�κ��丮�� ���� á���ϴ�!");
        }
    }
}

