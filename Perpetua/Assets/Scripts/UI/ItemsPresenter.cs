using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            List<ItemData> removables = new List<ItemData>();
            foreach (ItemData item in InventoryManager.Instance.inventory.items)
            {
                if (item != null)
                {
                    Button itemPres = Instantiate(ItemPresenter, transform);
                    itemPres.transform.Find("Image").GetComponentInChildren<Image>().sprite = item.image;
                    itemPres.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
                    if (item.equipped)
                        itemPres.transform.Find("Equipped").GetComponent<TextMeshProUGUI>().text = "eq.";
                    TooltipTrigger tooltip = itemPres.GetComponent<TooltipTrigger>();
                    tooltip.header = item.name;
                    tooltip.description = item.GetDescription();
                }
                else
                {
                    removables.Add(item);
                }
            }
            foreach(ItemData item in removables)
            {
                InventoryManager.Instance.inventory.items.Remove(item);
            }
        }
    }
}
