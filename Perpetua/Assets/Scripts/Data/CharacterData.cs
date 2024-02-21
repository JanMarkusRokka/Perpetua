using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CharacterData : ScriptableObject
{
    public Sprite image;
    public new string name;
    public string description;
    public StatsData stats;
    public StatusEffectsData statusEffects;
    public List<BattleAction> skills;
    
    public virtual StatsData GetStatsWithAllEffects()
    {
        StatsData statsWithEffects = StatsData.Clone(stats);
        statsWithEffects.ApplyStatusEffects(statusEffects.statusEffects);
        return statsWithEffects;
    }
}
