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
        // 1. ���� �������� �ְ�, ���� �����ϸ� �߰�
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null && slots[i].item == newItem && newItem.canStack)
            {
                slots[i].item = newItem;  // ItemData ����
                slots[i].quantity = amount;  // ���� ����
                Debug.Log($"{newItem.displayName}�� ������ {slots[i].quantity}���� �����߽��ϴ�!");
                return true;
            }
        }

        // 2. �� ������ ã�� �� ������ �߰�
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].item = newItem;
                slots[i].quantity = amount;
                Debug.Log($"{newItem.displayName}��(��) �κ��丮�� �߰��߽��ϴ�! ����: {amount}");
                return true;
            }
        }

        // 3. �� ������ ���� ��� ���� ��ȯ
        Debug.LogWarning("�κ��丮�� ���� á���ϴ�!");
        return false;
    }

}
