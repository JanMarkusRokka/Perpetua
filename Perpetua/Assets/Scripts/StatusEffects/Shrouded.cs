using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "StatusEffect/Types/Shrouded")]
public class Shrouded : StatusEffect
{
    private int turnsLeft = 3;
    public float detectabilityMultiplier = 0.5f;
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
        stats.Detectability = (int)(stats.Detectability * detectabilityMultiplier);
    }

    public override StatusEffect Clone()
    {
        Shrouded shrouded = ScriptableObject.CreateInstance<Shrouded>();
        shrouded.turnsLeft = turnsLeft;
        shrouded.detectabilityMultiplier = detectabilityMultiplier;
        shrouded.CopyBase(this);
        return shrouded;
    }

}