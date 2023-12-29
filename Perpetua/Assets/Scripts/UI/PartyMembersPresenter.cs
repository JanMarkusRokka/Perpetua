using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class PartyMembersPresenter : MonoBehaviour
{
    public static PartyMembersPresenter Instance;
    private PartyData party;
    public Button PartyMemberPresenter;
    public GameObject CharacterCustomizer;
    private IEnumerator RevealTextCoroutine = null;
    public GameObject ItemPresenterPrefab;

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
            pres.GetComponentInChildren<Image>().sprite = member.image;
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

    public void FocusOnMember(PartyCharacterData member)
    {
        Clear();
        Button pres = Instantiate(PartyMemberPresenter, transform);
        pres.onClick.AddListener(delegate { FocusOnMember(member); });
        pres.GetComponentInChildren<Image>().sprite = member.image;
        pres.GetComponentInChildren<TextMeshProUGUI>().text = member.name;

        CharacterCustomizer.SetActive(true);

        CharacterCustomizer.transform.Find("EquipmentOptionsPanel").gameObject.SetActive(false);
        CharacterCustomizer.transform.Find("EquipmentPanel").Find("WeaponSlot").GetComponent<ItemSlot>().member = member;

        if (member.weapon)
            CharacterCustomizer.transform.Find("EquipmentPanel").Find("WeaponSlot").GetComponent<ItemSlot>().SetItem(member.weapon);
        else
        {
            CharacterCustomizer.transform.Find("EquipmentPanel").Find("WeaponSlot").GetComponent<ItemSlot>().SetItem(null);
        }

        CharacterCustomizer.transform.Find("DescriptionPanel").gameObject.SetActive(true);
        StopCoroutines();
        CharacterCustomizer.transform.Find("DescriptionPanel").GetComponentInChildren<TextMeshProUGUI>().text = "";
        RevealTextCoroutine = TextMethods.RevealText(member.description, CharacterCustomizer.transform.Find("DescriptionPanel").GetComponentInChildren<TextMeshProUGUI>(), 0.05f);
        this.StartCoroutine(RevealTextCoroutine);
    }

    public void SelectEquipmentSlot(ItemSlot slot, PartyCharacterData member)
    {
        GameObject panel = CharacterCustomizer.transform.Find("EquipmentOptionsPanel").gameObject;
        ClearEquipmentOptions();
        CharacterCustomizer.transform.Find("DescriptionPanel").gameObject.SetActive(false);
        panel.SetActive(true);
        InventoryData inventory = InventoryManager.Instance.inventory;
        GameObject nullItemPres = Instantiate(ItemPresenterPrefab, panel.transform);
        nullItemPres.GetComponent<Image>().sprite = null;
        nullItemPres.GetComponentInChildren<TextMeshProUGUI>().text = null;
        nullItemPres.GetComponent<Button>().onClick.AddListener(delegate { slot.SetItem(null); FocusOnMember(member); });

        foreach (ItemData item in inventory.items)
        {
            if (item.type == slot.itemType && !item.equipped)
            {
                Debug.Log("shown: " + item);
                GameObject itemPres = Instantiate(ItemPresenterPrefab, panel.transform);
                itemPres.GetComponent<Image>().sprite = item.image;
                itemPres.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
                itemPres.GetComponent<Button>().onClick.AddListener(delegate { slot.SetItem(item); FocusOnMember(member); });
            }
        }
    }

    public void StopCoroutines()
    {
        if (RevealTextCoroutine != null)
        StopCoroutine(RevealTextCoroutine);
    }
}