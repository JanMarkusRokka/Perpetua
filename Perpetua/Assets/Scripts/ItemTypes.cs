using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTypes : MonoBehaviour
{
    public static ItemTypes Instance;
    public ItemTypeData Weapon;
    public ItemTypeData Rune;
    public ItemTypeData Armour;
    public ItemTypeData Accessory;
    public ItemTypeData Miscellaneous;
    void Awake()
    {
        Instance = this;
    }
}
