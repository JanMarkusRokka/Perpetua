using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public static PartyManager Instance;
    public PartyData party;
    
    public void Awake()
    {
        Instance = this;
    }

    public void SetParty(PartyData _party)
    {
        party = _party;
    }

    public void SetPartyInstantiate(PartyData _party, InventoryData inventory)
    {
        party = ScriptableObject.CreateInstance<PartyData>();
        party.PartyMembers = new List<PartyCharacterData>();
        foreach(PartyCharacterData member in _party.PartyMembers)
        {
            party.PartyMembers.Add(PartyCharacterData.CloneCharAndAddEquipmentToInventory(member, inventory));
        }
    }
    /*
    private void OnSave(int value)
    {
        if (!Directory.Exists(Application.persistentDataPath + "/Saves/Party"))
            Directory.CreateDirectory(Application.persistentDataPath + "/Saves/Party");
        System.IO.DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/Saves/Party/");

        foreach (FileInfo file in di.GetFiles())
        {
            file.Delete();
        }

        for (int i = 0; i < party.PartyMembers.Count; i++)
        {
            PartyCharacterData member = party.PartyMembers[i];
            PartyMemberSaveData saveData = PartyMemberSaveData.ScriptableToData(member);

            string json = JsonUtility.ToJson(saveData);

            System.IO.File.WriteAllText(Application.persistentDataPath + "/Saves/Party" + i +".json", json);
        }
    }*/
}
