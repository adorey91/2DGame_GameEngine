using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private ItemSlot itemSlot;

    public void SetItemSlot(ItemSlot slot)
    {
        itemSlot = slot;
        if(itemSlot.Item == null)
        {
            icon.enabled = false;
            quantityText.text = string.Empty;
        }
        else
        {
            icon.enabled = true;
            icon.sprite = slot.Item.Icon;
            quantityText.text = itemSlot.Quantity > 1 ? itemSlot.Quantity.ToString() : string.Empty;
        }
    }
}
