using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct WeaponVariables
{
    [SerializeField]
    public int WeaponDamage;
    [SerializeField]
    public int WeaponMagicDamage;

    public string GetDescripion()
    {
        string desc = "";
        if (WeaponDamage > 0)
        {
            desc += "Physical damage: " + WeaponDamage;
        }
        if (WeaponMagicDamage > 0)
        {
            desc += "\nMagic damage: " + WeaponMagicDamage;
        }
        return desc;
    }
}

[Serializable]
public struct ArmorVariables
{
    [SerializeField]
    public int ArmorDefense;
    [SerializeField]
    public int ArmorMagicDefense;
    public string GetDescripion()
    {
        string desc = "";
        if (ArmorDefense > 0)
        {
            desc += "Physical defense: " + ArmorDefense;
        }
        if (ArmorMagicDefense > 0)
        {
            desc += "\nMagic defense: " + ArmorMagicDefense;
        }
        return desc;
    }
}

[Serializable]
public struct RuneVariables
{
    [SerializeField]
    public List<StatusEffect> recipientStatusEffects;

    public string GetDescripion()
    {
        string desc = "";
        if (recipientStatusEffects.Count > 0)
        {
            desc = "\nInflicts:";
        }
        for (int i = 0; i < recipientStatusEffects.Count; i++)
        {
            desc += "\n" + recipientStatusEffects[i].name;
            
        }
        return desc;
    }
}

[Serializable]
public struct ConsumableVariables
{
    [SerializeField]
    public int healthChange;

    [SerializeField]
    public int willpowerChange;

    public string GetDescription()
    {
        string desc = "";
        if (healthChange != 0)
        {
            desc += "\nHealth: " + healthChange;
        }
        if (willpowerChange != 0)
        {
            desc += "\nWillpower: " + willpowerChange;
        }
        return desc;
    }
}

[CreateAssetMenu(menuName = "Items/RegularItem")]
[Serializable]
public class ItemData : ScriptableObject
{
    public Sprite image;
    public new string name;
    public ItemTypeData type;
    public bool equipped;
    public WeaponVariables WeaponVariables;
    public ArmorVariables ArmorVariables;
    public RuneVariables RuneVariables;
    public ConsumableVariables ConsumableVariables;
    public string description;

    private void Init(Sprite _image, string _name, ItemTypeData _type, bool _equipped, WeaponVariables _WeaponVariables, ArmorVariables _ArmorVariables, RuneVariables _RuneVariables, ConsumableVariables _ConsumableVariables, string _description)
    {
        image = _image;
        name = _name;
        type = _type;
        equipped = _equipped;
        WeaponVariables = _WeaponVariables;
        ArmorVariables = _ArmorVariables;
        RuneVariables = _RuneVariables;
        ConsumableVariables = _ConsumableVariables;
        description = _description;
    }

    public static ItemData Clone(ItemData item)
    {
        if (item == null) return null;
        var itemData = ScriptableObject.CreateInstance<ItemData>();

        itemData.Init(item.image, item.name, item.type, item.equipped, item.WeaponVariables, item.ArmorVariables, item.RuneVariables, item.ConsumableVariables, item.description);
        return itemData;
    }

    public ItemData Clone()
    {
        var itemData = ScriptableObject.CreateInstance<ItemData>();

        itemData.Init(image, name, type, equipped, WeaponVariables, ArmorVariables, RuneVariables, ConsumableVariables, description);
        return itemData;
    }

    public string GetDescription()
    {
        string desc = description;
        if (!WeaponVariables.Equals(null))
        {
            desc += WeaponVariables.GetDescripion();
        }
        if (!ArmorVariables.Equals(null))
        {
            desc += ArmorVariables.GetDescripion();
        }
        if (!RuneVariables.Equals(null))
        {
            desc += RuneVariables.GetDescripion();
        }
        if (!ConsumableVariables.Equals(null))
        {
            desc += ConsumableVariables.GetDescription();
        }
        return desc;
    }
}
