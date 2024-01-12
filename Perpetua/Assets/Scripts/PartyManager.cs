using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
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

    public static PartyCharacterData DataToScriptable(PartyMemberSaveData data)
    {
        PartyCharacterData member = PartyCharacterData.Clone(PartyManager.Instance.allCharactersDict[data.name]);
        member.stats = StatsData.New(data.healthPoints);
        member.equipment = EquipmentData.New(InventoryManager.Instance.allItemsDict[data.weapon],
            InventoryManager.Instance.allItemsDict[data.rune1],
            InventoryManager.Instance.allItemsDict[data.rune2],
            InventoryManager.Instance.allItemsDict[data.armour],
            InventoryManager.Instance.allItemsDict[data.accessory]);
        AddToInventory(member.equipment.weapon);
        AddToInventory(member.equipment.rune1);
        AddToInventory(member.equipment.rune2);
        AddToInventory(member.equipment.armour);
        AddToInventory(member.equipment.accessory);


        return member;
    }

    public static void AddToInventory(ItemData item)
    {
        if (item != null)
        {
            item.equipped = true;
            InventoryManager.Instance.inventory.items.Add(item);
        }
    }
}
public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;
    public PartyData startParty;
    public PartyData party;
    public List<PartyCharacterData> allCharacters;
    public Dictionary<string, PartyCharacterData> allCharactersDict;
    
    public void Awake()
    {
        Instance = this;
        Events.OnSave += OnSave;
        setupDict();
    }

    public void ChangeParty(PartyData _party)
    {
        party = _party;
        Events.Save(0);
    }

    private void setupDict()
    {
        allCharactersDict = new Dictionary<string, PartyCharacterData>();
        for (int i = 0; i < allCharacters.Count; i++)
        {
            allCharactersDict.Add(allCharacters[i].name, allCharacters[i]);
        }
    }

    public void OnDestroy()
    {
        Events.OnSave -= OnSave;
    }

    private void OnSave(int value)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Party"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Party");
        System.IO.DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/Party/");

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        for (int i = 0; i < party.PartyMembers.Count; i++)
        {
            PartyCharacterData member = party.PartyMembers[i];
            PartyMemberSaveData saveData = PartyMemberSaveData.ScriptableToData(member);

            string json = JsonUtility.ToJson(saveData);

            System.IO.File.WriteAllText(Application.persistentDataPath + "/Party/" + i +".json", json);
        }
    }

    public void OnLoad()
    {
        if (Directory.Exists(Application.persistentDataPath + "/Party"))
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/Party/");
            
            if (di.GetFiles().Length > 0)
            {
                party.PartyMembers = new List<PartyCharacterData>();
                foreach (string file in Directory.EnumerateFiles(Application.persistentDataPath + "/Party/", "*.json"))
                {
                    string json = File.ReadAllText(file);
                    Debug.Log(json);
                    if (json.Length > 2)
                    {
                        PartyMemberSaveData data = PartyMemberSaveData.NewDefault();
                        Debug.Log(data.name);
                        JsonUtility.FromJsonOverwrite(json, data);

                        Debug.Log(data.name);
                        party.PartyMembers.Add(PartyMemberSaveData.DataToScriptable(data));
                    }
                }
            }
        }

    }
}
