using UnityEngine;

public class PlayerItemPickup : MonoBehaviour
{
    public float pickupRange = 3f; // 아이템을 감지할 거리
    public Camera playerCamera; // 플레이어의 카메라

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
        Destroy(item); // 아이템 제거 (인벤토리 시스템이 있다면 추가하는 방식으로 변경 가능)
    }
}
