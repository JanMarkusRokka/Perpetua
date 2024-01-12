using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Chest")]

public class ChestData : ScriptableObject
{
    public List<ItemData> items;
    public Sprite ClosedSprite;
    public Sprite OpenSprite;
    public new string name;

    private void Init(List<ItemData> _items, Sprite _closedSprite, Sprite _openSprite ,string _name)
    {
        items = _items;
        ClosedSprite = _closedSprite;
        OpenSprite = _openSprite;
        name = _name;
    }

    public static ChestData Clone(ChestData chest)
    {
        var chestData = ScriptableObject.CreateInstance<ChestData>();

        chestData.Init(chest.items, chest.ClosedSprite, chest.OpenSprite, chest.name);
        return chestData;
    }

}
