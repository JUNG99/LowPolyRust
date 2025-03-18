using System;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public GameObject inventoryWindow;
    public GameObject Aim;
    public Transform slotPanel;

    [Header("Selected Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public GameObject DiscardButton;

    void Start()
    {
        inventoryWindow.SetActive(false);

        // 슬롯 초기화
        slots = new ItemSlot[slotPanel.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        // 선택된 아이템 창을 초기화
        ClearSelectedItemWindow();
    }

    void ClearSelectedItemWindow()
    {
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        DiscardButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
            Aim.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            inventoryWindow.SetActive(true);
            Aim.SetActive(false);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public bool AddItem(ItemData newItem, int amount = 1)
    {
        // 1. 같은 아이템이 있고, 스택 가능하면 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null && slots[i].item == newItem && newItem.canStack)
            {
                slots[i].item = newItem;  // ItemData 설정
                slots[i].quantity = amount;  // 개수 설정
                Debug.Log($"{newItem.displayName}의 개수가 {slots[i].quantity}개로 증가했습니다!");
                return true;
            }
        }

        // 2. 빈 슬롯을 찾아 새 아이템 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = newItem;
                slots[i].quantity = amount;
                Debug.Log($"{newItem.displayName}을(를) 인벤토리에 추가했습니다! 개수: {amount}");
                return true;
            }
        }

        // 3. 빈 슬롯이 없을 경우 실패 반환
        Debug.LogWarning("인벤토리가 가득 찼습니다!");
        return false;
    }

}
