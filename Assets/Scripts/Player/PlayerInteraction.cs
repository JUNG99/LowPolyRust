using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 2f; // 상호작용 거리
    public LayerMask interactableLayer; // 아이템이 있는 레이어

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
                Destroy(hit.collider.gameObject); // 아이템 삭제
                Debug.Log($"{hit.collider.gameObject.name} 아이템을 주웠다!");
            }
        }
    }
}
