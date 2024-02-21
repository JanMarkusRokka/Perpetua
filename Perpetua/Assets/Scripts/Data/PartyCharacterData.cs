using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Characters/PartyCharacter")]
public class PartyCharacterData : CharacterData
{
    public EquipmentData equipment;

    private void Init(Sprite _image, string _name, string _description, StatsData _stats, EquipmentData _equipment, StatusEffectsData _statusEffects, List<BattleAction> _skills)
    {
        image = _image;
        name = _name;
        description = _description;
        stats = _stats;
        equipment = _equipment;
        statusEffects = _statusEffects;
        skills = _skills;
    }

    public override StatsData GetStatsWithAllEffects()
    {
        StatsData statsWithEffects = StatsData.Clone(stats);
        statsWithEffects.ApplyStatusEffects(statusEffects.statusEffects);
        statsWithEffects.ApplyEquipment(equipment);
        return statsWithEffects;
    }

    public static PartyCharacterData Clone(PartyCharacterData character)
    {
        var characterData = ScriptableObject.CreateInstance<PartyCharacterData>();
        var statsData = StatsData.Clone(character.stats);
        var equipmentData = EquipmentData.Clone(character.equipment);
        var statusEffects = StatusEffectsData.Clone(character.statusEffects);

        characterData.Init(character.image, character.name, character.description, statsData, equipmentData, statusEffects, character.skills);
        return characterData;
    }

    public static PartyCharacterData CloneCharAndAddEquipmentToInventory(PartyCharacterData character, InventoryData inventory)
    {
        var characterData = Clone(character);
        characterData.equipment.AddItemsToInventory(inventory);
        return characterData;
    }
}
