using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "StatusEffect/Types/Slowed")]
public class Slowed : StatusEffect
{
    private int turnsLeft = 3;
    public float slowMultiplier = 0.5f;
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
        stats.AttackSpeed = (int) (stats.AttackSpeed * slowMultiplier);
    }

    public override StatusEffect Clone()
    {
        Slowed slowed = ScriptableObject.CreateInstance<Slowed>();
        slowed.turnsLeft = turnsLeft;
        slowed.slowMultiplier = slowMultiplier;
        slowed.CopyBase(this);
        return slowed;
    }

}