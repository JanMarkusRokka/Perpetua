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
    public RuntimeAnimatorController animatorController;
    public AudioClipGroup attackSound;
    // Used to keep track of different past ailments. Taken into account when calculating whether to inflict ailments (for example: an enemy that has been stunned 3 times is less likely to have Stunned inflicted on them than an enemy with 3 stuns in their ailment history)
    public Dictionary<string, int> ailmentsHistory;
    public Objective objectiveToAdvance;
    public bool isStaticDialogueEnemy;
    public bool isEscapable;
    public AudioClipGroup battleMusic;
    public bool continueMusicFromScene;

    public void Init(Sprite _image, string _name, string _description, StatsData _stats, List<ItemData> _loot, Sprite _gonerSprite, float _stunSeconds, AudioClipGroup _attackSound, StatusEffectsData _statusEffects, List<BattleAction> _skills, Dictionary<string, int> _ailmentsHistory, Objective _objectiveToAdvance, bool _isStaticDialogueEnemy, bool _isEscapable, RuntimeAnimatorController _animatorController, AudioClipGroup _battleMusic, bool _continueMusicFromScene)
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
        ailmentsHistory = _ailmentsHistory;
        objectiveToAdvance = _objectiveToAdvance;
        isStaticDialogueEnemy = _isStaticDialogueEnemy;
        isEscapable = _isEscapable;
        animatorController = _animatorController;
        battleMusic = _battleMusic;
        continueMusicFromScene = _continueMusicFromScene;
        if (_ailmentsHistory == null) ailmentsHistory = new();
    }

    public static void CloneData(EnemyData enemyData, EnemyData newData)
    {
        Dictionary<string, int> ailmentsH = (enemyData.ailmentsHistory == null) ? new() : new Dictionary<string, int>(enemyData.ailmentsHistory);
        newData.Init(enemyData.image, enemyData.name, enemyData.description, StatsData.Clone(enemyData.stats), enemyData.loot, enemyData.gonerSprite, enemyData.stunSeconds,
            enemyData.attackSound, StatusEffectsData.Clone(enemyData.statusEffects), enemyData.skills, ailmentsH, enemyData.objectiveToAdvance,
            enemyData.isStaticDialogueEnemy, enemyData.isEscapable, enemyData.animatorController, enemyData.battleMusic, enemyData.continueMusicFromScene);
    }

    public abstract EnemyData Clone();

    public abstract BattleAction SelectTurn(BattleParticipant participant, bool guardIncluded);

    /***
     * Returns a random BattleParticipant from the list of BattleParticipants based on the weighted random algorithm.
     * https://dev.to/jacktt/understanding-the-weighted-random-algorithm-581p
     */
    public static BattleParticipant DetectTarget(List<BattleParticipant> participantsAndWeights)
    {
        int total = 0;
        foreach(BattleParticipant participant in participantsAndWeights)
        {
            total += participant.GetStatsData().Detectability;
        }

        int random = Random.Range(1, total + 1);
        int cursor = 0;

        foreach (BattleParticipant participant in participantsAndWeights) 
        {
            cursor += participant.GetStatsData().Detectability;
            if (cursor >= random)
            {
                return participant;
            }
        }

        return null;
    }
    // Weighted random for actions
    // Could implement a more universal system, but works for now
    public static BattleAction SelectAction(Dictionary<BattleAction, int> actionsAndWeights)
    {
        int total = 0;
        foreach (BattleAction action in actionsAndWeights.Keys)
        {
            total += actionsAndWeights[action];
        }

        int random = Random.Range(1, total + 1);
        int cursor = 0;

        foreach (BattleAction action in actionsAndWeights.Keys)
        {
            cursor += actionsAndWeights[action];
            if (cursor >= random)
            {
                return action;
            }
        }

        return null;
    }

    public float GetSpecificAilmentResistance(StatusEffect statusEffect)
    {
        StatsData stats = GetStatsWithAllEffects();
        float ailmentResistance = stats.AilmentResistance;
        if (ailmentsHistory.ContainsKey(statusEffect.name))
        {
            ailmentResistance *= ailmentsHistory[statusEffect.name];
        }
        return ailmentResistance;
    }
}
