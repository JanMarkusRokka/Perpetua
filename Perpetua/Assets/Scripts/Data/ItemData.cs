using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Items/RegularItem")]
[Serializable]
public class ItemData : ScriptableObject
{
    public Sprite image;
    public new string name;
    public ItemTypeData type;
    public bool equipped;
    
    private void Init(Sprite _image, string _name, ItemTypeData _type, bool _equipped)
    {
        image = _image;
        name = _name;
        type = _type;
        equipped = _equipped;
    }

    public static ItemData Clone(ItemData item)
    {
        var itemData = ScriptableObject.CreateInstance<ItemData>();

        itemData.Init(item.image, item.name, item.type, item.equipped);
        return itemData;
    }

}
