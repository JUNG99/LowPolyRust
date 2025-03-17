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

    // Start는 게임 시작 시 한 번만 호출됩니다.
    void Start()
    {
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

    // 인벤토리 열기/닫기 토글 함수
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
            Aim.SetActive(true);
            // 인벤토리가 닫히면 커서를 숨깁니다.
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked; // 마우스를 잠급니다.
        }
        else
        {
            inventoryWindow.SetActive(true);
            Aim.SetActive(false);
            // 인벤토리가 열리면 커서를 보이게 합니다.
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // 마우스 잠금을 해제하여 자유롭게 움직일 수 있게 합니다.
        }
    }

    // 인벤토리가 열려있는지 확인하는 함수
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }
}
