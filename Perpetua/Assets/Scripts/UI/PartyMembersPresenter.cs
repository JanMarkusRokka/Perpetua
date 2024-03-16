using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PartyMembersPresenter : MonoBehaviour
{
    public static PartyMembersPresenter Instance;
    private PartyData party;
    public Button PartyMemberPresenter;
    public GameObject CharacterCustomizer;
    private IEnumerator RevealTextCoroutine = null;
    public GameObject ItemPresenterPrefab;
    public GameObject StatisticPresenterPrefab;
    public Sprite EmptySlot;

    public void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        party = PartyManager.Instance.party;
        DisplayMembers();
    }

    public void DisplayMembers()
    {
        Clear();
        foreach (PartyCharacterData member in party.PartyMembers)
        {
            Button pres = Instantiate(PartyMemberPresenter, transform);
            pres.onClick.AddListener(delegate { FocusOnMember(member); CharacterCustomizer.transform.Find("EquipmentPanel").Find("WeaponSlot").GetComponentInChildren<Button>().Select(); });
            pres.transform.Find("Image").GetComponent<Image>().sprite = member.image;
            if (member.stats.HealthPoints <= 0)
            {
                Color grey = Color.grey;
                grey.a = 0.5f;
                pres.transform.Find("Image").GetComponent<Image>().color = grey;
            }
            pres.transform.Find("Image").GetComponent<RectTransform>().sizeDelta = member.image.textureRect.size * 6;
            pres.GetComponentInChildren<TextMeshProUGUI>().text = member.name;
        }
    }

    private void Clear()
    {
        CharacterCustomizer.SetActive(false);
        int children = transform.childCount;

        for (int i = children - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void ClearEquipmentOptions()
    {
        Transform panel = CharacterCustomizer.transform.Find("EquipmentOptionsPanel");
        int children = panel.childCount;

        for (int i = children - 1; i >= 0; i--)
        {
            GameObject.Destroy(panel.GetChild(i).gameObject);
        }
    }

    private void RefreshStats(StatsData stats)
    {
        Transform pres = CharacterCustomizer.transform.Find("StatsPanel").Find("StatsPresenter");

        int children = pres.childCount;

        for (int i = children - 1; i >= 0; i--)
        {
            GameObject.Destroy(pres.GetChild(i).gameObject);
        }

        GameObject healthPresenter = Instantiate(StatisticPresenterPrefab, pres);
        healthPresenter.GetComponent<TextMeshProUGUI>().text = "Health: " + stats.HealthPoints;
    }

    public void FocusOnMember(PartyCharacterData member)
    {
        Clear();
        Button pres = Instantiate(PartyMemberPresenter, transform);
        pres.onClick.AddListener(delegate { FocusOnMember(member); });
        pres.transform.Find("Image").GetComponent<Image>().sprite = member.image;
        pres.transform.Find("Image").GetComponent<RectTransform>().sizeDelta = member.image.textureRect.size * 6;
        pres.GetComponentInChildren<TextMeshProUGUI>().text = member.name;

        Destroy(pres);
        CharacterCustomizer.SetActive(true);
        CharacterCustomizer.transform.Find("EquipmentOptionsPanel").gameObject.SetActive(false);
        RefreshEquipmentSlots(member);
        CharacterCustomizer.transform.Find("DescriptionPanel").gameObject.SetActive(true);

        StopCoroutines();
        CharacterCustomizer.transform.Find("DescriptionPanel").GetComponentInChildren<TextMeshProUGUI>().text = "";
        RefreshStats(member.stats);
        RevealTextCoroutine = TextMethods.RevealText(member.description, CharacterCustomizer.transform.Find("DescriptionPanel").GetComponentInChildren<TextMeshProUGUI>(), 0.05f);
        this.StartCoroutine(RevealTextCoroutine);
    }

    public void RefreshEquipmentSlots(PartyCharacterData member)
    {
        string[] slots = { "WeaponSlot", "Rune1Slot", "Rune2Slot", "ArmourSlot", "AccessorySlot" };

        foreach (string slotName in slots)
        {
            ItemSlotPresenter slot = CharacterCustomizer.transform.Find("EquipmentPanel").Find(slotName).GetComponent<ItemSlotPresenter>();
            slot.member = member;
            slot.SetItem(slot.GetMemberSlotItem());
        }
    }

    public void SelectEquipmentSlot(ItemSlotPresenter slot, PartyCharacterData member)
    { 
        GameObject panel = CharacterCustomizer.transform.Find("EquipmentOptionsPanel").gameObject;
        ClearEquipmentOptions();
        CharacterCustomizer.transform.Find("DescriptionPanel").gameObject.SetActive(false);
        panel.SetActive(true);
        InventoryData inventory = InventoryManager.Instance.inventory;
        GameObject nullItemPres = Instantiate(ItemPresenterPrefab, panel.transform);
        nullItemPres.transform.Find("Image").GetComponent<Image>().sprite = EmptySlot;
        nullItemPres.GetComponentInChildren<TextMeshProUGUI>().text = null;
        nullItemPres.GetComponent<Button>().onClick.AddListener(delegate { slot.SetItem(null); FocusOnMember(member); slot.GetComponent<Button>().Select(); });
        nullItemPres.GetComponent<Button>().Select();
        if (inventory.items.Count > 0)
        {
            foreach (ItemData item in inventory.items)
            {
                if (item.type == slot.itemType && !item.equipped)
                {
                    GameObject itemPres = Instantiate(ItemPresenterPrefab, panel.transform);
                    itemPres.transform.Find("Image").GetComponent<Image>().sprite = item.image;
                    itemPres.GetComponentInChildren<TextMeshProUGUI>().text = null;
                    TooltipTrigger tooltip = itemPres.GetComponent<TooltipTrigger>();
                    tooltip.header = item.name;
                    tooltip.description = item.GetDescription();
                    itemPres.GetComponent<Button>().onClick.AddListener( delegate { slot.GetComponent<Button>().Select(); slot.SetItem(item); FocusOnMember(member); } );
                }
            }
        }
    }

    public void StopCoroutines()
    {
        if (RevealTextCoroutine != null)
        StopCoroutine(RevealTextCoroutine);
    }
}
