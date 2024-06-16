using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
    [Header("Inventory UI")]
    [SerializeField] private InventorySlotUI[] itemUISlots;

    public void UpdateItemsUI(ItemSlot[] items)
    {
        for (int i = 0; i < itemUISlots.Length; i++)
        {
            itemUISlots[i].SetItemSlot(items[i]);
        }
    }
}