using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public float pickupRange = 3f; // �������� ������ �Ÿ�
    public Camera playerCamera; // �÷��̾��� ī�޶�
    private UIInventory inventory;
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
        Debug.Log($"{item.name} �������� ȹ���߽��ϴ�!");
        // item�� UIInventory.cs�� �����ϴ� slots�� ���������� �߰��ϴ� �ڵ� ����
        Destroy(item);
    }
}
