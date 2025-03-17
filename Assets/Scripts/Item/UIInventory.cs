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

    [Header("Look")]
    public Transform camerContainer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;

    public Action inventory;
    private Rigidbody _Rigidbody;

    private PlayerController controller;
    private PlayerStats stats;

    // Start�� ���� ���� �� �� ���� ȣ��˴ϴ�.
    void Start()
    {
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

    // �κ��丮 ����/�ݱ� ��� �Լ�
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
            Aim.SetActive(true);
            // �κ��丮�� ������ Ŀ���� ����ϴ�.
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // ���콺�� ��޴ϴ�.
        }
        else
        {
            inventoryWindow.SetActive(true);
            Aim.SetActive(false);
            // �κ��丮�� ������ Ŀ���� ���̰� �մϴ�.
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // ���콺 ����� �����Ͽ� �����Ӱ� ������ �� �ְ� �մϴ�.
        }
    }

    // �κ��丮�� �����ִ��� Ȯ���ϴ� �Լ�
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }
}
