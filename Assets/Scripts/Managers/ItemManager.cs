using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemData[] allItems;

    //Resets all items to not collected
    public void ResetAllItems()
    {
        foreach (var item in allItems)
        {
            item.isCollected = false;
        }
    }

    // Used for when the scenes change. If an item is collected it won't be displayed.
    public void CollectItem(ItemData item)
    {
        item.isCollected = true;
    }
}
