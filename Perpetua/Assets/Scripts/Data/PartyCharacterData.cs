using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Characters/PartyCharacter")]
public class PartyCharacterData : ScriptableObject
{
    public Sprite image;
    public new string name;
    public string description;
    public StatsData stats;
    public EquipmentData equipment;

    private void Init(Sprite _image, string _name, string _description, StatsData _stats, EquipmentData _equipment)
    {
        image = _image;
        name = _name;
        description = _description;
        stats = _stats;
        equipment = _equipment;
    }

    public static PartyCharacterData Clone(PartyCharacterData character)
    {
        var characterData = ScriptableObject.CreateInstance<PartyCharacterData>();

        characterData.Init(character.image, character.name, character.description, character.stats, character.equipment);
        return characterData;
    }
}
