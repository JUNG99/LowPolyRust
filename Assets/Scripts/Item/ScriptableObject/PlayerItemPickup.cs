using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public float pickupRange = 3f; // 아이템을 감지할 거리
    public Camera playerCamera; // 플레이어의 카메라
    private UIInventory inventory;
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
        Debug.Log($"{item.name} 아이템을 획득했습니다!");
        // item을 UIInventory.cs에 존재하는 slots에 순차적으로 추가하는 코드 구현
        Destroy(item);
    }
}
