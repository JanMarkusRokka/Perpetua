using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Chest")]

public class ChestData : ScriptableObject
{
    public List<ItemData> items;
    public Sprite ClosedSprite;
    public Sprite OpenSprite;
    public string name;
}
