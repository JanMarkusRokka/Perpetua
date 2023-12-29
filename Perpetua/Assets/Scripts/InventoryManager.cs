using System.Collections;
using System.Collections.Generic;
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
        inventory.items.Add(item);
    }
}
