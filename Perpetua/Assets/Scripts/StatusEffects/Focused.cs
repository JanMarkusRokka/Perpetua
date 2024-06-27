using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "StatusEffect/Types/Focused")]
public class Focused : StatusEffect
{
    public int turnsLeft = 10;
    public float criticalChanceMultiplier = 2f;
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
        stats.CriticalChance = (int)(Mathf.Max(100, stats.CriticalChance * criticalChanceMultiplier));
    }

    public override StatusEffect Clone()
    {
        Focused focused = ScriptableObject.CreateInstance<Focused>();
        focused.turnsLeft = turnsLeft;
        focused.criticalChanceMultiplier = criticalChanceMultiplier;
        focused.CopyBase(this);
        return focused;
    }

}