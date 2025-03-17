using UnityEngine;


public class ItemSlot : MonoBehaviour
{
    public ItemData item;

    public GameObject storedItem;

    public UIInventory inventory;
    public bool equipped;
    public int quantity;

    public int index;
    public bool IsEmpty()
    {
        return storedItem == null;
    }

    public void SetItem(GameObject item)
    {
        storedItem = item;
    }

}