using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "StatusEffect/Types/Taunting")]
public class Taunting : StatusEffect
{
    private int turnsLeft = 1;
    public float detectabilityMultiplier = 2f;
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
        stats.Detectability = (int) (stats.Detectability * detectabilityMultiplier);
    }

    public override StatusEffect Clone()
    {
        Taunting taunting = ScriptableObject.CreateInstance<Taunting>();
        taunting.turnsLeft = turnsLeft;
        taunting.detectabilityMultiplier = detectabilityMultiplier;
        taunting.CopyBase(this);
        return taunting;
    }

}