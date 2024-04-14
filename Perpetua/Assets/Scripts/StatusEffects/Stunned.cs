using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "StatusEffect/Types/Stunned")]
public class Stunned : StatusEffect
{
    private int turnsLeft = 2;
    public void SetTurnsLeft(int value)
    {
        turnsLeft = value;
    }
    public override int GetTurnsLeft()
    {
        return turnsLeft;
    }

    public override IEnumerator InflictActiveStatusEffect(BattleParticipant participant)
    {
        turnsLeft--;
        yield break;
    }

    public override void InflictPassiveStatusEffect(StatsData stats)
    {
        stats.AttackSpeed = 0;
    }

    public override StatusEffect Clone()
    {
        Stunned stunned = ScriptableObject.CreateInstance<Stunned>();
        stunned.turnsLeft = turnsLeft;
        stunned.CopyBase(this);
        return stunned;
    }

}