using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleParticipant : ScriptableObject
{
    public ScriptableObject participant;
    public bool IsPartyMember;
    private PartyCharacterData participantPartyMember;
    private EnemyData enemyData;

    private void Init(ScriptableObject _participant)
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

    public static BattleParticipant New(ScriptableObject participant)
    {
        BattleParticipant battleParticipant = ScriptableObject.CreateInstance<BattleParticipant>();
        battleParticipant.Init(participant);
        return battleParticipant;
    }

    public int Agility()
    {
        if (IsPartyMember) return participantPartyMember.stats.AttackSpeed;
        else return enemyData.stats.AttackSpeed;
    }

    public float Health()
    {
        if (IsPartyMember) return participantPartyMember.stats.HealthPoints;
        else return enemyData.stats.HealthPoints;
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
        if (IsPartyMember) return participantPartyMember.stats;
        else return enemyData.stats;
    }

    public EquipmentData GetEquipmentData()
    {
        if (IsPartyMember) return participantPartyMember.equipment;
        return null;
    }
}