using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField] private ItemSlot[] itemSlots;
    [SerializeField] int inventorySize = 4;
    UIManager _uiManager;

    public void Start()
    {
        _uiManager = FindAnyObjectByType<UIManager>();
        itemSlots = new ItemSlot[inventorySize];

        for (int i = 0; i < inventorySize; i++)
            itemSlots[i] = new ItemSlot();

        _uiManager.UpdateUI(itemSlots);
    }

    public void AddItem(ItemData item)
    {
        ItemSlot slot = FindAvailableItemSlot(item);

        if (slot != null)
        {
            slot.Quantity++;
            _uiManager.UpdateUI(itemSlots);
            return;
        }

        slot = GetEmptySlot();

        if (slot != null)
        {
            slot.Item = item;
            slot.Quantity = 1;
        }
        else
        {
            Debug.Log("Inventory Is Full");
            return;
        }
        _uiManager.UpdateUI(itemSlots);
    }

    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item)
            {
                RemoveItem(itemSlots[i]);
                return;
            }
        }
    }

    public void RemoveItem(ItemSlot slot)
    {
        if (slot == null)
        {
            Debug.Log("Can't remove item from inventory");
            return;
        }

        slot.Quantity--;

        if (slot.Quantity <= 0)
        {
            slot.Item = null;
            slot.Quantity = 0;
        }
        _uiManager.UpdateUI(itemSlots);
    }

    ItemSlot FindAvailableItemSlot(ItemData item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item && itemSlots[i].Quantity < item.MaxStackSize)
                return itemSlots[i];
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == null)
                return itemSlots[i];
        }
        return null;
    }

    public bool HasItem(ItemData item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item && itemSlots[i].Quantity > 0)
                return true;
        }
        return false;
    }
}
