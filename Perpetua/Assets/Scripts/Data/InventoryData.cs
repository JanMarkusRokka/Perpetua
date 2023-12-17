using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Inventory")]
public class InventoryData : ScriptableObject
{
    public List<ItemData> items;
}
