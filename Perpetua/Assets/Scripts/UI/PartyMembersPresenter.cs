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
            pres.onClick.AddListener(delegate { FocusOnMember(member); });
            pres.transform.Find("Image").GetComponent<Image>().sprite = member.image;
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
        nullItemPres.GetComponent<Image>().sprite = EmptySlot;
        nullItemPres.GetComponentInChildren<TextMeshProUGUI>().text = null;
        nullItemPres.GetComponent<Button>().onClick.AddListener(delegate { slot.SetItem(null); FocusOnMember(member); });

        if (inventory.items.Count > 0)
        {
            foreach (ItemData item in inventory.items)
            {
                if (item.type == slot.itemType && !item.equipped)
                {
                    Debug.Log("shown: " + item);
                    GameObject itemPres = Instantiate(ItemPresenterPrefab, panel.transform);
                    itemPres.GetComponent<Image>().sprite = item.image;
                    itemPres.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
                    Debug.Log(item.GetType());
                    itemPres.GetComponent<Button>().onClick.AddListener( delegate { slot.SetItem(item); FocusOnMember(member); } );
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
