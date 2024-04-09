using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemData[] allItems;

    public void ResetAllItems()
    {
        foreach (var item in allItems)
        {
            item.isCollected = false;
        }
    }

    public void CollectItem(ItemData item)
    {
        item.isCollected = true;
    }
}
