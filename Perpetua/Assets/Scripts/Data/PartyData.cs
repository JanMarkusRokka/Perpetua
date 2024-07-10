using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Characters/Party")]
public class PartyData : ScriptableObject
{
    public List<PartyCharacterData> PartyMembers;
    public List<Objective> objectives;
    public List<Objective> completedObjectives;

    public Dictionary<string, ChestData> Chests;
    public Dictionary<string, EnemyData> Enemies;
    // Temporary solution, planning to implement serialization
    public ScenarioData lastSave;

    public bool isObjectiveCompleted(Objective objective)
    { // could add an extra IsCompleted check
        return completedObjectives.Find(obj => obj.id == objective.id) != null;
    }
    public bool isObjectiveAlreadyAdded(Objective objective)
    {
        return (objectives.Find(obj => obj.id == objective.id) != null) || (completedObjectives.Find(obj => obj.id == objective.id) != null);
    }

    public PartyData CloneAndAddItemsToInventory(InventoryData inventory)
    {
        PartyData party = ScriptableObject.CreateInstance<PartyData>();
        party.PartyMembers = new();
        party.objectives = new();
        party.completedObjectives = new();
        foreach (PartyCharacterData member in PartyMembers)
        {
            party.PartyMembers.Add(PartyCharacterData.CloneCharAndAddEquipmentToInventory(member, inventory));
        }
        foreach (Objective objective in objectives)
        {
            party.objectives.Add(objective.Clone());
        }
        foreach (Objective objective in completedObjectives)
        {
            party.completedObjectives.Add(objective.Clone());
        }
        return party;
    }

    public PartyData CloneForSave()
    {
        PartyData party = ScriptableObject.CreateInstance<PartyData>();
        party.PartyMembers = new();
        party.objectives = new();
        party.completedObjectives = new();
        foreach (PartyCharacterData member in PartyMembers)
        {
            party.PartyMembers.Add(PartyCharacterData.Clone(member));
        }
        foreach (Objective objective in objectives)
        {
            party.objectives.Add(objective.Clone());
        }
        foreach (Objective objective in completedObjectives)
        {
            party.completedObjectives.Add(objective.Clone());
        }
        party.Chests = new();
        foreach(string guid in Chests.Keys)
        {
            party.Chests.Add(guid, ChestData.Clone(Chests[guid]));
        }
        party.Enemies = new();
        foreach(string guid in Enemies.Keys)
        {
            party.Enemies.Add(guid, Enemies[guid].Clone());
        }
        return party;
    }
}
