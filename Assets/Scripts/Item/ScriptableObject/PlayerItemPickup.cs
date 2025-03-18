using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public float pickupRange = 3f; // 아이템을 감지할 거리
    public Camera playerCamera; // 플레이어의 카메라
    private UIInventory inventory;

    private void Start()
    {
        inventory = FindObjectOfType<UIInventory>();
    }
    void Update()
    {
        // Ray를 시각적으로 계속 표시
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
            Debug.LogWarning($"{item.name}에 ItemData가 없습니다!");
            return;
        }

        bool added = inventory.AddItem(data); // 인벤토리에 아이템 추가
        if (added)
        {
            Debug.Log($"{data.displayName}을(를) 획득했습니다!");
            Destroy(item); // 성공적으로 추가되면 아이템 삭제
        }
        else
        {
            Debug.LogWarning("인벤토리가 가득 찼습니다!");
        }
    }
}

