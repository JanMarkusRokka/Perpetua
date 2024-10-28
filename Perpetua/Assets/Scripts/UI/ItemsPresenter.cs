using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemsPresenter : MonoBehaviour
{
    public Button ItemPresenter;
    [SerializeField]
    private GameObject ConsumeMenu;
    [SerializeField]
    private Button PartyMemberPresenter;

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
                    if (item.type.name.Equals("Consumable"))
                    {
                        itemPres.onClick.AddListener(delegate { OpenConsumeMenu(item); });
                    }
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

    private void OpenConsumeMenu(ItemData item)
    {
        ConsumeMenu.SetActive(true);
        TabsController.ClearTab(ConsumeMenu);
        foreach (PartyCharacterData member in PartyManager.Instance.party.PartyMembers)
        {
            Button pres = Instantiate(PartyMemberPresenter, ConsumeMenu.transform);
            pres.transform.Find("Image").GetComponent<Image>().sprite = member.image;
            if (member.stats.HealthPoints <= 0)
            {
                Color grey = Color.grey;
                grey.a = 0.5f;
                pres.transform.Find("Image").GetComponent<Image>().color = grey;
            }
            else
            {
                pres.onClick.AddListener(delegate { ConsumeItem(item, member); });
            }
            pres.transform.Find("Image").GetComponent<RectTransform>().sizeDelta = member.image.textureRect.size * 6;
            pres.GetComponentInChildren<TextMeshProUGUI>().text = member.name;
            pres.transform.Find("HealthBar").GetChild(0).GetComponent<Image>().fillAmount = ((float)member.stats.HealthPoints + item.ConsumableVariables.healthChange) / ((float)member.stats.MaxHealthPoints);
            pres.transform.Find("WillPowerBar").GetChild(0).GetComponent<Image>().fillAmount = ((float)member.stats.WillPower + item.ConsumableVariables.willpowerChange) / ((float)member.stats.MaxWillPower);
            pres.transform.Find("HealthBar").GetChild(1).GetComponent<Image>().fillAmount = ((float)member.stats.HealthPoints) / ((float)member.stats.MaxHealthPoints);
            pres.transform.Find("WillPowerBar").GetChild(1).GetComponent<Image>().fillAmount = ((float)member.stats.WillPower) / ((float)member.stats.MaxWillPower);

        }
    }

    private void ConsumeItem(ItemData item, PartyCharacterData partyCharacter)
    {
        ConsumeMenu.SetActive(false);
        BattleParticipant participant = BattleParticipant.New(partyCharacter);
        Consume consume = ScriptableObject.CreateInstance<Consume>();
        consume.participant = participant;
        consume.item = item;
        consume.CommitAction();
        InventoryManager.Instance.inventory.items.Remove(item);
        RefreshInventory();
        Destroy(consume);
    }
}
