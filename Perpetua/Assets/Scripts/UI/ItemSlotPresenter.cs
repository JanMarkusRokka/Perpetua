using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotPresenter : MonoBehaviour
{
    public ItemData item;
    public PartyCharacterData member;
    public ItemTypeData itemType;
    public enum Slot { weapon, rune1, rune2, armour, accessory};
    public Slot slot;
    private Button button;
    private Image itemImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        itemImage = transform.Find("Image").GetComponentInChildren<Image>();
    }
    public void SetItem(ItemData newItem)
    {
        if (newItem == null)
        {
            ItemData currentlyInSlot = GetMemberSlotItem();
            if (currentlyInSlot != null)
                currentlyInSlot.equipped = false;
            SetMemberSlotItem(null);
            itemImage.sprite = null;
            itemImage.color = Color.clear;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { PartyMembersPresenter.Instance.SelectEquipmentSlot(this, member); });
        } else if (itemType == newItem.type)
        {
            item = newItem;
            ItemData currentlyInSlot = GetMemberSlotItem();
            if (currentlyInSlot != null)
                currentlyInSlot.equipped = false;
            SetMemberSlotItem(newItem);
            item.equipped = true;
            itemImage.sprite = item.image;
            itemImage.color = Color.white;
            TooltipTrigger tooltip = GetComponent<TooltipTrigger>();
            tooltip.header = item.name;
            tooltip.description = item.GetDescription();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { PartyMembersPresenter.Instance.SelectEquipmentSlot(this, member); });
        }
    }

    public ItemData GetMemberSlotItem()
    {
        switch (slot) {
            case Slot.weapon:
                return member.equipment.weapon;
            case Slot.armour:
                return member.equipment.armour;
            case Slot.rune1:
                return member.equipment.rune1;
            case Slot.rune2:
                return member.equipment.rune2;
            case Slot.accessory:
                return member.equipment.accessory;
            default:
                return null;
        }
    }

    public void SetMemberSlotItem(ItemData newItem)
    {
        // Item type already checked in SetItem
        switch (slot) {
            case Slot.weapon:
                member.equipment.weapon = newItem;
                break;
            case Slot.armour:
                member.equipment.armour = newItem;
                break;
            case Slot.rune1:
                member.equipment.rune1 = newItem;
                break;
            case Slot.rune2:
                member.equipment.rune2 = newItem;
                break;
            case Slot.accessory:
                member.equipment.accessory = newItem;
                break;
            default:
                break;
        }
    }
}
