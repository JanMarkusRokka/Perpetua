using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Inventory")]
[Serializable]
public class InventoryData : ScriptableObject
{
    public List<ItemData> items;
    public InventoryData Clone()
    {
        InventoryData inv = CreateInstance<InventoryData>();
        inv.items = new();
        foreach(ItemData item in items)
        {
            inv.items.Add(item.Clone());
        }
        return inv;
    }
}
