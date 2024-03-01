using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyData : CharacterData
{
    public List<ItemData> loot;
    public Sprite gonerSprite;
    public float stunSeconds;
    public Vector3 stunLocation;
    public AudioClipGroup attackSound;

    public void Init(Sprite _image, string _name, string _description, StatsData _stats, List<ItemData> _loot, Sprite _gonerSprite, float _stunSeconds, AudioClipGroup _attackSound, StatusEffectsData _statusEffects, List<BattleAction> _skills)
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
        skills = _skills;
    }

    public abstract EnemyData Clone(EnemyData character);

    public abstract void SelectTurn(BattleParticipant participant);
}
