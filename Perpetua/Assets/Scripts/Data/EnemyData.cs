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
            Debug.Log(participant.GetStatsData().Detectability + participant.GetPartyMember().name);
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
}
