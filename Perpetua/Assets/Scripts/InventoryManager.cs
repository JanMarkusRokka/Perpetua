using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[Serializable]
public struct StringListContainer
{
    public List<string> strings;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public InventoryData inventory;
    public List<ItemData> allItems;
    public Dictionary<string, ItemData> allItemsDict;
    public List<ItemData> addToLoad;

    public void Awake()
    {
        Instance = this;
        Events.OnItemReceived += OnItemReceived;
        Events.OnSave += OnSave;
        Events.OnLoad += OnLoad;
        setupDict();
        addToLoad = new List<ItemData>();
        Events.Load(0);
    }

    private void setupDict()
    {
        allItemsDict = new Dictionary<string, ItemData>();
        for (int i = 0; i < allItems.Count; i++)
        {
            allItemsDict.Add(allItems[i].name, allItems[i]);
        }
        allItemsDict.Add("", null);
    }

    public void OnDestroy()
    {
        Events.OnItemReceived -= OnItemReceived;
        Events.OnSave -= OnSave;
        Events.OnLoad -= OnLoad;
    }

    public void OnApplicationQuit()
    {
        Events.Save(0);
    }

    public void OnItemReceived(ItemData item)
    {
        inventory.items.Add(ItemData.Clone(item));
    }

    public void OnSave(int value)
    {
        saveInventory();
    }

    public void OnLoad(int value)
    {
        loadInventory();
    }

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

        System.IO.File.WriteAllText(Application.persistentDataPath + "/Inventory.json", json);
    }

    public void loadInventory()
    {
        if (File.Exists(Application.persistentDataPath + "/Inventory.json"))
        {
            string inventoryJson = System.IO.File.ReadAllText(Application.persistentDataPath + "/Inventory.json");
            StringListContainer idsList = new StringListContainer();
            idsList.strings = new List<string>();
            JsonUtility.FromJsonOverwrite(inventoryJson, idsList);
            inventory.items = new List<ItemData>();
            foreach(string itemId in idsList.strings)
            {
                inventory.items.Add(ItemData.Clone(allItemsDict[itemId]));
            }
            // Works for now, relies on party being loaded before inventory
            PartyManager.Instance.OnLoad();
            addToLoad = new List<ItemData>();
        }
    }
}
