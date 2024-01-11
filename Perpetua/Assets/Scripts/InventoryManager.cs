using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public InventoryData inventory;

    public void Awake()
    {
        Instance = this;
        //loadInventory();
        Events.OnItemReceived += OnItemReceived;
    }

    public void OnDestroy()
    {
        Events.OnItemReceived -= OnItemReceived;
    }

    public void OnApplicationQuit()
    {
        //saveInventory();
    }

    public void OnItemReceived(ItemData item)
    {
        inventory.items.Add(ItemData.Clone(item));
    }


    public void saveInventory()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/Inventory.dat");
        InventoryData inventoryData = ScriptableObject.CreateInstance<InventoryData>();

        inventoryData.items = inventory.items;

        bf.Serialize(file, inventoryData);
        file.Close();
    }

    public void loadInventory()
    {
        if (File.Exists(Application.persistentDataPath + "/Inventory.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/Inventory.dat", FileMode.Open);
            InventoryData loaded = (InventoryData)bf.Deserialize(file);
            file.Close();
            inventory.items = loaded.items;
        }
    }
}
