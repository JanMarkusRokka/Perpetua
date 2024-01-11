using System.Collections;
using System.Collections.Generic;
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
}
