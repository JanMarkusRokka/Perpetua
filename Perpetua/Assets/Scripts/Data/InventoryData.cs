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

    // Removes party equipment from the inventory for them to be added later
    public InventoryData CloneForSave(PartyData partyData)
    {
        List<ItemData> dontAdd = new();
        foreach(PartyCharacterData member in partyData.PartyMembers)
        {
            dontAdd.AddRange(member.equipment.GetItems());
        }
        InventoryData inv = CreateInstance<InventoryData>();
        inv.items = new();
        foreach (ItemData item in items)
        {
            if (!dontAdd.Contains(item))
            inv.items.Add(item.Clone());
        }
        return inv;
    }
}
