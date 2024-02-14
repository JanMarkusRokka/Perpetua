using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct WeaponVariables
{
    [SerializeField]
    public int WeaponDamage;
    [SerializeField]
    public int WeaponMagicDamage;
}

[Serializable]
public struct ArmorVariables
{
    [SerializeField]
    public int ArmorDefense;
    [SerializeField]
    public int ArmorMagicDefense;
}

[Serializable]
public struct RuneVariables
{
    [SerializeField]
    public List<StatusEffect> recipientStatusEffects;
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

    private void Init(Sprite _image, string _name, ItemTypeData _type, bool _equipped, WeaponVariables _WeaponVariables, ArmorVariables _ArmorVariables, RuneVariables _RuneVariables)
    {
        image = _image;
        name = _name;
        type = _type;
        equipped = _equipped;
        WeaponVariables = _WeaponVariables;
        ArmorVariables = _ArmorVariables;
        RuneVariables = _RuneVariables;
    }

    public static ItemData Clone(ItemData item)
    {
        if (item == null) return null;
        var itemData = ScriptableObject.CreateInstance<ItemData>();

        itemData.Init(item.image, item.name, item.type, item.equipped, item.WeaponVariables, item.ArmorVariables, item.RuneVariables);
        return itemData;
    }

    public ItemData Clone()
    {
        var itemData = ScriptableObject.CreateInstance<ItemData>();

        itemData.Init(image, name, type, equipped, WeaponVariables, ArmorVariables, RuneVariables);
        return itemData;
    }

}
