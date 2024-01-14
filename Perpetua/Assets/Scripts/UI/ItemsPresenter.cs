using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsPresenter : MonoBehaviour
{
    public Button ItemPresenter;

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

        if (InventoryManager.Instance.inventory.items.Count > 0)
        {
            foreach (ItemData item in InventoryManager.Instance.inventory.items)
            {
                Button itemPres = Instantiate(ItemPresenter, transform);
                itemPres.GetComponentInChildren<Image>().sprite = item.image;
                itemPres.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            }
        }
    }
}
