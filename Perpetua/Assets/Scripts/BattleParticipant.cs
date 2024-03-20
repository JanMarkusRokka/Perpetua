using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleParticipant : ScriptableObject
{
    public CharacterData participant;
    public bool IsPartyMember;
    private PartyCharacterData participantPartyMember;
    private EnemyData enemyData;

    public Transform transform;

    private void Init(CharacterData _participant)
    {
        participant = _participant;
        if (participant.GetType() == typeof(PartyCharacterData))
        {
            IsPartyMember = true;
            participantPartyMember = (PartyCharacterData)participant;
        }
        else { 
            IsPartyMember = false;
            enemyData = (EnemyData)participant;
        }
    }

    public static BattleParticipant New(CharacterData participant)
    {
        BattleParticipant battleParticipant = ScriptableObject.CreateInstance<BattleParticipant>();
        battleParticipant.Init(participant);
        return battleParticipant;
    }

    public int Agility()
    {
        return participant.GetStatsWithAllEffects().AttackSpeed;
    }

    public float Health()
    {
        return participant.GetStatsWithAllEffects().HealthPoints;
    }

    public PartyCharacterData GetPartyMember()
    {
        return participantPartyMember;
    }

    public EnemyData GetEnemy()
    {
        return enemyData;
    }

    public StatsData GetStatsData()
    {
        return participant.GetStatsWithAllEffects();
    }

    public EquipmentData GetEquipmentData()
    {
        if (IsPartyMember) return participantPartyMember.equipment;
        return null;
    }

    public StatusEffectsData GetStatusEffectsData()
    {
        return participant.statusEffects;
    }

    public void InflictStatusEffect(StatusEffect statusEffect)
    {
        if (!IsPartyMember)
        {
            if (GetEnemy().ailmentsHistory.ContainsKey(statusEffect.name)) GetEnemy().ailmentsHistory[statusEffect.name] += 1;
            else GetEnemy().ailmentsHistory[statusEffect.name] = 1;
            
            foreach(string ailment in GetEnemy().ailmentsHistory.Keys)
            {
                Debug.Log(ailment + ": " + GetEnemy().ailmentsHistory[ailment]);
            }
        }
        StatusEffectsData statusEffectsData = GetStatusEffectsData();

        statusEffectsData.statusEffects.RemoveAll(statusEf => statusEf.GetType() == statusEffect.GetType());

        statusEffectsData.statusEffects.Add(statusEffect.Clone());
    }
    
    public static List<BattleParticipant> GetPartyMembers(List<BattleParticipant> participants)
    {
        List<BattleParticipant> partyMembers = new();
        foreach (BattleParticipant participant in participants)
        {
            if (participant.IsPartyMember) partyMembers.Add(participant);
        }
        return partyMembers;
    }
}