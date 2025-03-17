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

    // 아이템을 인벤토리 슬롯에 추가하는 메서드
    //public void AddItem(GameObject item)
    //{
    //    foreach (var slot in slots)
    //    {
    //        if (slot.IsEmpty()) // 빈 슬롯 찾기
    //        {
    //            slot.SetItem(item); // 슬롯에 아이템 추가
    //            Debug.Log($"{item.name}이(가) 인벤토리에 추가되었습니다.");
    //            return;
    //        }
    //    }

    //    Debug.Log("인벤토리가 가득 찼습니다!");
    //}
}
