using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory Data", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    public Sprite Icon;
    public string Description;
    public int MaxStackSize;
}
