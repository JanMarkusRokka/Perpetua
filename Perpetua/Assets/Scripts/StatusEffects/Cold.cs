using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "StatusEffect/Types/Cold")]
public class Cold : StatusEffect
{
    private int turnsLeft = 3;
    public float defenseMultiplier = 0.5f;
    public void SetTurnsLeft(int value)
    {
        turnsLeft = value;
    }
    public override int GetTurnsLeft()
    {
        return turnsLeft;
    }

    public override void InflictPassiveStatusEffect(StatsData stats)
    {
        stats.PhysicalDefense = (int) (stats.PhysicalDefense * defenseMultiplier);
    }

    public override StatusEffect Clone()
    {
        Cold cold = ScriptableObject.CreateInstance<Cold>();
        cold.turnsLeft = turnsLeft;
        cold.defenseMultiplier = defenseMultiplier;
        cold.CopyBase(this);
        return cold;
    }

}