using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Items/RegularItem")]
public class ItemData : ScriptableObject
{
    public Sprite image;
    public string name;
    public ItemTypeData type;
}
