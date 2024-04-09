using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Data", menuName = "New Item/Pickup Item")]
public class ItemData : ScriptableObject
{
    /// <summary>
    /// ItemData holds the icon, description and max size that it can stack in the inventory UI
    /// </summary>
    public Sprite Icon;
    public string Description;
    public int MaxStackSize;
    public State itemState;

    public enum State
    {
        Available,
        Collected,
    }
}
