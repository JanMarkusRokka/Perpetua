using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PartyMemberSaveData
{
    public string name;
    public float healthPoints;
    public string weapon;
    public string rune1;
    public string rune2;
    public string armour;
    public string accessory;

    public static PartyMemberSaveData NewDefault()
    {
        var partyMemberSaveData = new PartyMemberSaveData();

        partyMemberSaveData.name = "";
        partyMemberSaveData.healthPoints = 100;
        partyMemberSaveData.weapon = "";
        partyMemberSaveData.rune1 = "";
        partyMemberSaveData.rune2 = "";
        partyMemberSaveData.armour = "";
        partyMemberSaveData.accessory = "";

        return partyMemberSaveData;
    }
    public static PartyMemberSaveData ScriptableToData(PartyCharacterData member)
    {
        PartyMemberSaveData data = new PartyMemberSaveData();
        data.name = member.name;
        data.healthPoints = member.stats.healthPoints;
        EquipmentData equipment = member.equipment;
        data.weapon = NameOrNull(equipment.weapon);
        data.rune1 = NameOrNull(equipment.rune1);
        data.rune2 = NameOrNull(equipment.rune2);
        data.armour = NameOrNull(equipment.armour);
        data.accessory = NameOrNull(equipment.accessory);
        return data;
    }

    private static string NameOrNull(ItemData item)
    {
        if (item == null) return null;
        else return item.name;
    }

    public static PartyCharacterData DataToScriptable(PartyMemberSaveData data, InventoryData inventory)
    {
        PartyCharacterData member = PartyCharacterData.Clone(SerializationMethods.allCharactersDict[data.name]);
        member.stats = StatsData.New(data.healthPoints);
        member.equipment = EquipmentData.New(SerializationMethods.GetItemFromDict(data.weapon),
            SerializationMethods.GetItemFromDict(data.rune1),
            SerializationMethods.GetItemFromDict(data.rune2),
            SerializationMethods.GetItemFromDict(data.armour),
            SerializationMethods.GetItemFromDict(data.accessory));
        AddToInventory(member.equipment.weapon, inventory);
        AddToInventory(member.equipment.rune1, inventory);
        AddToInventory(member.equipment.rune2, inventory);
        AddToInventory(member.equipment.armour, inventory);
        AddToInventory(member.equipment.accessory, inventory);

        return member;
    }

    public static void AddToInventory(ItemData item, InventoryData inventory)
    {
        if (item != null)
        {
            item.equipped = true;
            inventory.items.Add(item);
        }
    }
}

[Serializable]
public struct StringListContainer
{
    public List<string> strings;
}

public class SerializationMethods : MonoBehaviour
{
    public static Dictionary<string, PartyCharacterData> allCharactersDict;
    public List<PartyCharacterData> allCharacters;
    public List<ItemData> allItems;
    public static Dictionary<string, ItemData> allItemsDict;

    public void Awake()
    {
        setupItemsDict(allItems);
        setupCharactersDict(allCharacters);
    }

    private void setupItemsDict(List<ItemData> items)
    {
        allItemsDict = new Dictionary<string, ItemData>();
        for (int i = 0; i < items.Count; i++)
        {
            allItemsDict.Add(items[i].name, items[i]);
        }
        allItemsDict.Add("", null);
    }

    private static void setupCharactersDict(List<PartyCharacterData> characters)
    {
        allCharactersDict = new Dictionary<string, PartyCharacterData>();
        for (int i = 0; i < characters.Count; i++)
        {
            allCharactersDict.Add(characters[i].name, characters[i]);
        }
    }

    public static ItemData GetItemFromDict(string name)
    {
        if (name != null) return allItemsDict[name];
        return null;
    }

    public static InventoryData LoadInventory(int saveNum)
    {
        string location = Application.persistentDataPath + "/Saves/" + saveNum + "/Inventory.json";
        if (File.Exists(location))
        {
            string inventoryJson = System.IO.File.ReadAllText(location);
            StringListContainer idsList = new StringListContainer();
            idsList.strings = new List<string>();
            JsonUtility.FromJsonOverwrite(inventoryJson, idsList);
            InventoryData inventory = ScriptableObject.CreateInstance<InventoryData>();
            inventory.items = new List<ItemData>();
            foreach (string itemId in idsList.strings)
            {
                inventory.items.Add(ItemData.Clone(GetItemFromDict(itemId)));
            }
        }
        return null;
    }

    public static PartyData LoadParty(int saveNum, InventoryData inventory)
    {
        string location = Application.persistentDataPath + "/Saves/" + saveNum + "/Party";
        if (Directory.Exists(location))
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(location + "/");
            PartyData party = ScriptableObject.CreateInstance<PartyData>();
            party.PartyMembers = new List<PartyCharacterData>();
            if (di.GetFiles().Length > 0)
            {
                foreach (string file in Directory.EnumerateFiles(location + "/", "*.json"))
                {
                    string json = File.ReadAllText(file);
                    Debug.Log(json);
                    if (json.Length > 2)
                    {
                        PartyMemberSaveData data = PartyMemberSaveData.NewDefault();
                        JsonUtility.FromJsonOverwrite(json, data);
                        party.PartyMembers.Add(PartyMemberSaveData.DataToScriptable(data, inventory));
                    }
                }
            }
            return party;
        }
        return null;
    }
}
