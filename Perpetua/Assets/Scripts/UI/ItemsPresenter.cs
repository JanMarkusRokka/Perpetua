using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsPresenter : MonoBehaviour
{
    private InventoryData inventory;
    public Button ItemPresenter;

    private void Awake()
    {
        inventory = InventoryManager.Instance.inventory;
    }

    private void OnEnable()
    {
        RefreshInventory();
    }

    private void RefreshInventory()
    {
        int children = transform.childCount;

        for (int i = children - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }

        foreach (ItemData item in inventory.items)
        {
            Button itemPres = Instantiate(ItemPresenter, transform);
            itemPres.GetComponentInChildren<Image>().sprite = item.image;
            itemPres.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
        }
    }
}
