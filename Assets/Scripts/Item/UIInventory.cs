using System;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public GameObject Aim;
    public Transform slotPanel;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;

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
        // 1. 같은 아이템이 있고, 스택 가능하면 수량을 증가시킴
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null && slots[i].item == newItem)
            {
                slots[i].quantity += 1;  // 수량을 1 증가시킴
                UpdateInvetoryUI();
                Debug.Log($"{newItem.displayName} : {slots[i].quantity}개.");

                return true;
            }
        }

        // 2. 같은 아이템이 없으면 빈 슬롯을 찾아 새 아이템 추가
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = newItem;
                slots[i].quantity = 1;  // 새 아이템을 1개로 설정
                UpdateInvetoryUI();
                Debug.Log($"{newItem.displayName} 인벤토리에 추가.");

                return true;
            }
        }

        // 3. 빈 슬롯이 없을 경우 실패 반환
        Debug.LogWarning("인벤토리가 가득 참");
        return false;
    }


    public void UpdateInvetoryUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].Set();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;
    }

}
