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
        //inventoryUIManager.UpdateItemsUI();
    }

    public void EmptyInventory()
    {
        itemSlots = new ItemSlot[inventorySize];
        for(int i = 0; i < inventorySize; i++)
            itemSlots[i] = new ItemSlot();
    }

    public void AddItem(ItemData item)
    {
        ItemSlot slot = FindAvailableItemSlot(item);

        if(slot != null)
        {
            slot.Quantity++;
            inventoryUIManager.UpdateItemsUI(itemSlots);
        }
    }

    public void RemoveItem(ItemData item)
    {
        for(int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == item)
            {
                RemoveItem(itemSlots[i]);
                return;
            }
        }
        Debug.Log("Item not found in inventory");
    }

    private void RemoveItem(ItemSlot slot)
    {
        if(slot ==  null)
        {
            Debug.Log("Can't remove item from inventory");
            return;
        }

        slot.Quantity = Mathf.Max(0, slot.Quantity - 1);
        if(slot.Quantity <= 0)
            slot.Item = null;

        inventoryUIManager.UpdateItemsUI(itemSlots);
    }

    private ItemSlot FindAvailableItemSlot(ItemData item)
    {
        foreach(var slot in itemSlots)
        {
            if(slot.Item == item && slot.Quantity < item.MaxStackSize)
                return slot;
        }
        return null;
    }

    private ItemSlot GetEmptySlot()
    {
        foreach (var slot in itemSlots)
            return slot;
        return null;
    }

    public int GetItemQuantity(ItemData item)
    {
        foreach(var slot in itemSlots)
        {
            if(slot.Item == item)
                return slot.Quantity;
        }
        return 0;
    }
}