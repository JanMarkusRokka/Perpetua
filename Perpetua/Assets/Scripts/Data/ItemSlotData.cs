using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Items/ItemSlotData")]
public class ItemSlotData : ScriptableObject
{
    //Could add specific images to specific slots
    //public Sprite image;
    public new string name;
    public ItemTypeData type;
    public ItemData item;
}
