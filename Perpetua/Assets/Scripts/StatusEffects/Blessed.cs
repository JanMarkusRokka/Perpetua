using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "StatusEffect/Types/Blessed")]
public class Blessed : StatusEffect
{
    private int turnsLeft = 3;
    public float defenseMultiplier = 1.5f;
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
        turnsLeft -= 1;
        yield break;
    }

    public override void InflictPassiveStatusEffect(StatsData stats)
    {
        stats.PhysicalDefense = (int) (stats.PhysicalDefense * defenseMultiplier);
    }

    public override StatusEffect Clone()
    {
        Blessed blessed = ScriptableObject.CreateInstance<Blessed>();
        blessed.turnsLeft = turnsLeft;
        blessed.defenseMultiplier = defenseMultiplier;
        blessed.CopyBase(this);
        return blessed;
    }

}