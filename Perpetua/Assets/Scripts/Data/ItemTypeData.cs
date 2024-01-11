using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Items/ItemType")]
[Serializable]
public class ItemTypeData : ScriptableObject
{
    public Sprite image;
    public new string name;
}
