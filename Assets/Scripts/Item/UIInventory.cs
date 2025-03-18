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

        // ���� �ʱ�ȭ
        slots = new ItemSlot[slotPanel.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        // ���õ� ������ â�� �ʱ�ȭ
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
        // 1. ���� �������� �ְ�, ���� �����ϸ� ������ ������Ŵ
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null && slots[i].item == newItem)
            {
                slots[i].quantity += 1;  // ������ 1 ������Ŵ
                UpdateInvetoryUI();
                Debug.Log($"{newItem.displayName} : {slots[i].quantity}��.");

                return true;
            }
        }

        // 2. ���� �������� ������ �� ������ ã�� �� ������ �߰�
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = newItem;
                slots[i].quantity = 1;  // �� �������� 1���� ����
                UpdateInvetoryUI();
                Debug.Log($"{newItem.displayName} �κ��丮�� �߰�.");

                return true;
            }
        }

        // 3. �� ������ ���� ��� ���� ��ȯ
        Debug.LogWarning("�κ��丮�� ���� ��");
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
