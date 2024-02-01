using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public InventoryData inventory;

    public void Awake()
    {
        Instance = this;
        Events.OnItemReceived += OnItemReceived;
    }

    public void OnDestroy()
    {
        Events.OnItemReceived -= OnItemReceived;
    }

    public void OnItemReceived(ItemData item)
    {
        inventory.items.Add(item.Clone());
    }

    public void SetInventory(InventoryData _inventory)
    {
        inventory = _inventory;
    }

    public void SetInventoryInstantiate(InventoryData _inventory)
    {
        inventory = ScriptableObject.CreateInstance<InventoryData>();
        inventory.items = new List<ItemData>();
        foreach (ItemData item in _inventory.items)
        {
            inventory.items.Add(ItemData.Clone(item));
        }
        inventory = _inventory;
    }
    /*
    public void saveInventory()
    {
        List<string> ids = new List<string>();
        foreach(ItemData item in inventory.items)
        {
            if (!item.equipped)
            ids.Add(item.name);
        }
        StringListContainer idsList = new StringListContainer();
        idsList.strings = ids;

        string json = JsonUtility.ToJson(idsList);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/Saves/Inventory.json", json);
    }*/
}
