using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TMP_Text quantityText;
    private ItemSlot itemSlot;

    public void SetItemSlot(ItemSlot slot)
    {
        itemSlot = slot;
        if (itemSlot.Item == null)
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
