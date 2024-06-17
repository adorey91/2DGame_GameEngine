using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private ItemSlot[] itemSlots;
    [SerializeField] private int inventorySize = 5;
    [SerializeField] private InventoryUIManager inventoryUIManager;

    private void Start()
    {
        EmptyInventory();
        inventoryUIManager.UpdateItemsUI(itemSlots);
    }

    public void EmptyInventory()
    {
        itemSlots = new ItemSlot[inventorySize];
        for(int i = 0; i < inventorySize; i++)
            itemSlots[i] = new ItemSlot();
    }

    // Adds item to visual inventory
    public void AddItem(ItemData item)
    {
        ItemSlot slot = FindAvailableItemSlot(item);

        if (slot != null)
        {
            slot.Quantity++;
            inventoryUIManager.UpdateItemsUI(itemSlots);
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
        inventoryUIManager.UpdateItemsUI(itemSlots);
    }

    public void RemoveItem(ItemData item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item)
            {
                ItemSlot slot = itemSlots[i];
                slot.Quantity = Mathf.Max(0, slot.Quantity - 1);
                if (slot.Quantity <= 0)
                    slot.Item = null;

                inventoryUIManager.UpdateItemsUI(itemSlots);
                return;
            }
        }
        Debug.Log("Item not found in inventory");
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
}