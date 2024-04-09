using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private ItemSlot[] itemSlots;
    [SerializeField] int inventorySize = 5;
    UIManager _uiManager;

    public void Start()
    {
        _uiManager = FindAnyObjectByType<UIManager>();
        itemSlots = new ItemSlot[inventorySize];

        for (int i = 0; i < inventorySize; i++)
            itemSlots[i] = new ItemSlot();

        _uiManager.UpdateItemsUI(itemSlots); // Updates them so they're empty on the first load.
    }


    /// <summary>
    /// Adds the item either to the spot that there currently is one of that type or puts it into a new spot.
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(ItemData item)
    {
        ItemSlot slot = FindAvailableItemSlot(item);

        item.itemState = ItemData.State.Collected;

        if (slot != null)
        {
            slot.Quantity++;
            _uiManager.UpdateItemsUI(itemSlots);
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
        _uiManager.UpdateItemsUI(itemSlots);
    }

    /// <summary>
    /// Removes the item from the spot it's in if there is an item there.
    /// </summary>
    /// <param name="item"></param>
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
        _uiManager.UpdateItemsUI(itemSlots);
    }

    // Finds available itemslot
    ItemSlot FindAvailableItemSlot(ItemData item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item && itemSlots[i].Quantity < item.MaxStackSize)
                return itemSlots[i];
        }
        return null;
    }

    // finds an empty slot
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == null)
                return itemSlots[i];
        }
        return null;
    }

    // Returns the item quantity - Using for quests
    public int GetItemQuantity(ItemData item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item)
            {
                return itemSlots[i].Quantity;
            }
        }
        return 0; // If the item is not found in any slot, return 0 quantity
    }
}
