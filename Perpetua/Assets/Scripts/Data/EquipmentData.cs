using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Characters/EquipmentData")]
public class EquipmentData : ScriptableObject
{
    public ItemData weapon;
    public ItemData rune1;
    public ItemData rune2;
    public ItemData armour;
    public ItemData accessory;
    private void Init(ItemData _weapon, ItemData _rune1, ItemData _rune2, ItemData _armour, ItemData _accessory)
    {
        weapon = _weapon;
        rune1 = _rune1;
        rune2 = _rune2;
        armour = _armour;
        accessory = _accessory;
    }

    public static EquipmentData New(ItemData weapon, ItemData rune1, ItemData rune2, ItemData armour, ItemData accessory)
    {
        var equipmentData = ScriptableObject.CreateInstance<EquipmentData>();

        equipmentData.Init(weapon, rune1, rune2, armour, accessory);
        return equipmentData;
    }
    public static EquipmentData Clone(EquipmentData equipment)
    {
        var equipmentData = ScriptableObject.CreateInstance<EquipmentData>();

        equipmentData.Init(ItemData.Clone(equipment.weapon), ItemData.Clone(equipment.rune1), ItemData.Clone(equipment.rune2), ItemData.Clone(equipment.armour), ItemData.Clone(equipment.accessory));
        return equipmentData;
    }

    public void AddItemsToInventory(InventoryData inventory)
    {
        if (weapon != null)
            inventory.items.Add(weapon);
        if (rune1 != null)
            inventory.items.Add(rune1);
        if (rune2 != null)
            inventory.items.Add(rune2);
        if (armour != null)
            inventory.items.Add(armour);
        if (accessory != null)
            inventory.items.Add(accessory);
    }
}
