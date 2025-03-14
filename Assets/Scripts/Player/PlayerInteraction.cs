using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // 상호작용 거리
    public LayerMask interactableLayer; // 아이템이 있는 레이어

    private Camera playerCamera;

    void Start()
    {
        playerCamera = Camera.main; // 메인 카메라 가져오기
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
            else if (hit.collider.CompareTag("Item")) // 인터페이스가 없을 경우 기본 처리
            {
                PickupItem(hit.collider.gameObject);
            }
        }
    }

    void PickupItem(GameObject item)
    {
        Debug.Log($"{item.name} 획득");
        Destroy(item); // 아이템 삭제
    }
}
