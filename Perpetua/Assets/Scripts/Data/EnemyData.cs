using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Enemies/Enemy")]
public class EnemyData : ScriptableObject
{
    public Sprite image;
    public new string name;
    public string description;
    public StatsData stats;
    public List<ItemData> loot;
    public Sprite gonerSprite;
    public float stunSeconds;
    public Vector3 stunLocation;
    public AudioClipGroup attackSound;
    public StatusEffectsData statusEffects;

    private void Init(Sprite _image, string _name, string _description, StatsData _stats, List<ItemData> _loot, Sprite _gonerSprite, float _stunSeconds, AudioClipGroup _attackSound, StatusEffectsData _statusEffects)
    {
        image = _image;
        name = _name;
        description = _description;
        stats = _stats;
        loot = _loot;
        gonerSprite = _gonerSprite;
        stunSeconds = _stunSeconds;
        attackSound = _attackSound;
        statusEffects = _statusEffects;
    }

    public static EnemyData Clone(EnemyData character)
    {
        var enemyData = ScriptableObject.CreateInstance<EnemyData>();
        StatusEffectsData statusEffectsData = StatusEffectsData.Clone(character.statusEffects);
        enemyData.Init(character.image, character.name, character.description, character.stats, character.loot, character.gonerSprite, character.stunSeconds, character.attackSound, statusEffectsData);
        return enemyData;
    }
}
