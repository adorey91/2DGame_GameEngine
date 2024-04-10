using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private ItemSlot[] itemSlots;
    [SerializeField] int inventorySize = 5;
    private UIManager _uiManager;

    public void Start()
    {
        // Find the UIManager in the scene
        _uiManager = FindObjectOfType<UIManager>();

        EmptyInventory();
        _uiManager.UpdateItemsUI(itemSlots);
    }


    // Adds the item either to the spot that there currently is one of that type or puts it into a new spot.
    public void AddItem(ItemData item)
    {
        ItemSlot slot = FindAvailableItemSlot(item);

        item.isCollected = true;

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

    // Removes the item
    public void RemoveItem(ItemData item)
    {
        // Find and remove the specified item from the inventory
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item)
            {
                RemoveItem(itemSlots[i]);
                return;
            }
        }
        Debug.Log("Item not found in inventory");
    }

    // Removes item from the inventory slot if there is an item to remove.
    public void RemoveItem(ItemSlot slot)
    {
        if (slot == null)
        {
            Debug.Log("Can't remove item from inventory");
            return;
        }

        slot.Quantity = Mathf.Max(0, slot.Quantity - 1);
        if (slot.Quantity <= 0)
            slot.Item = null;
        
        _uiManager.UpdateItemsUI(itemSlots);
    }

    // Find a slot with the same item type and available space
    ItemSlot FindAvailableItemSlot(ItemData item)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.Item == item && slot.Quantity < item.MaxStackSize)
                return slot;
        }
        return null;
    }

    // Finds an empty slot
    ItemSlot GetEmptySlot()
    {
        foreach (var slot in itemSlots)
        {
            if (slot.Item == null)
                return slot;
        }
        return null;
    }

    // Returns the item quantity of a specific item - Using for quests
    public int GetItemQuantity(ItemData item)
    {
        foreach (var slot in itemSlots)
        {
            if (slot.Item == item)
                return slot.Quantity;
        }
        return 0; // If the item is not found in any slot, return 0 quantity
    }

    // Initialize the item slots array with empty slots
    public void EmptyInventory()
    {
        itemSlots = new ItemSlot[inventorySize];
        for (int i = 0; i < inventorySize; i++)
            itemSlots[i] = new ItemSlot();
    }
}
