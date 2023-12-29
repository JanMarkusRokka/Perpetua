using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData item;
    public PartyCharacterData member;
    public ItemTypeData itemType;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }
    public void SetItem(ItemData newItem)
    {
        if (newItem == null)
        {
            if (member.weapon)
                member.weapon.equipped = false;
            if (itemType.name == "Weapon") member.weapon = null;
            GetComponent<Image>().sprite = null;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { PartyMembersPresenter.Instance.SelectEquipmentSlot(this, member); });
        } else if (itemType == newItem.type)
        {
            item = newItem;
            if (member.weapon)
            member.weapon.equipped = false;
            if (itemType.name == "Weapon") member.weapon = item;
            item.equipped = true;
            GetComponent<Image>().sprite = item.image;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { PartyMembersPresenter.Instance.SelectEquipmentSlot(this, member); });
        } 
    }
}
