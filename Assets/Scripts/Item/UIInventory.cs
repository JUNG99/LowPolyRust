using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;

    [Header("Selected Item")]
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;

    public GameObject DiscardButton;

    private PlayerController controller;
    private PlayerStats stats;

    void Start()
    {
        
        // controller = CharacterManager.Instance.Player.controller;
        // stats = CharacterManager.Instance.Player.stats;

        inventoryWindow.SetActive(false);

        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
        }

        ClearSelectedItemWindow();
    }

    void Update()
    {
        
    }

    void ClearSelectedItemWindow()
    {
        //selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
       
        DiscardButton.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    //PlayerController 먼저 수정

    //public void AddItem()
    //{
    //    ItemData data = CharacterManager.Instance.Player.itemData;

    //    if (data.canStack)
    //    {
    //        ItemSlot slot = GetItemStack(data);
    //        if (slot != null)
    //        {
    //            slot.quantity++;
    //            UpdateUI();
    //            CharacterManager.Instance.Player.itemData = null;
    //            return;
    //        }
    //    }

    //    ItemSlot emptySlot = GetEmptySlot();

    //    if (emptySlot != null)
    //    {
    //        emptySlot.item = data;
    //        emptySlot.quantity = 1;
    //        UpdateUI();
    //        CharacterManager.Instance.Player.itemData = null;
    //        return;
    //    }

    //    ThrowItem(data);
    //    CharacterManager.Instance.Player.itemData = null;
    //}

    //public void UpdateUI()
    //{
    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        if (slots[i].item != null)
    //        {
    //            slots[i].Set();
    //        }
    //        else
    //        {
    //            slots[i].Clear();
    //        }
    //    }
    //}

    //ItemSlot GetItemStack(ItemData data)
    //{
    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
    //        {
    //            return slots[i];
    //        }
    //    }
    //    return null;
    //}

    //ItemSlot GetEmptySlot()
    //{
    //    for (int i = 0; i < slots.Length; i++)
    //    {
    //        if (slots[i].item == null)
    //        {
    //            return slots[i];
    //        }
    //    }
    //    return null;
    //}

    //Player 스크립트 먼저 수정
    //public void ThrowItem(ItemData data)
    //{
    //    Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    //}


    //ItemSlot 스크립트 먼저 수정
    //public void SelectItem(int index)
    //{
    //    if (slots[index].item == null) return;

    //    selectedItem = slots[index];
    //    selectedItemIndex = index;

    //    selectedItemName.text = selectedItem.item.displayName;
    //    selectedItemDescription.text = selectedItem.item.description;

    //    selectedItemStatName.text = string.Empty;
    //    selectedItemStatValue.text = string.Empty;

    //    for (int i = 0; i < selectedItem.item.consumables.Length; i++)
    //    {
    //        selectedItemStatName.text += selectedItem.item.consumables[i].type.ToString() + "\n";
    //        selectedItemStatValue.text += selectedItem.item.consumables[i].value.ToString() + "\n";
    //    }

    //    useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
    //    equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !slots[index].equipped);
    //    unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && slots[index].equipped);
    //    dropButton.SetActive(true);
    //}

    //public void OnUseButton()
    //{
    //    if (selectedItem.item.type == ItemType.Consumable)
    //    {
    //        for (int i = 0; i < selectedItem.item.consumables.Length; i++)
    //        {
    //            switch (selectedItem.item.consumables[i].type)
    //            {

    //            }
    //        }
    //        RemoveSelctedItem();
    //    }
    //}

    //public void OnDropButton()
    //{
    //    ThrowItem(selectedItem.item);
    //    RemoveSelctedItem();
    //}

    //void RemoveSelctedItem()
    //{
    //    selectedItem.quantity--;

    //    if (selectedItem.quantity <= 0)
    //    {
    //        if (slots[selectedItemIndex].equipped)
    //        {
    //            UnEquip(selectedItemIndex);
    //        }

    //        selectedItem.item = null;
    //        ClearSelectedItemWindow();
    //    }

    //    UpdateUI();
    //}

    //public bool HasItem(ItemData item, int quantity)
    //{
    //    return false;
    //}
}