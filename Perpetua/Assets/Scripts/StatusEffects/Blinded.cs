using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "StatusEffect/Types/Blinded")]
public class Blinded : StatusEffect
{
    private int turnsLeft = 2;
    private float accuracyMultiplier = 0.5f;
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
        stats.Accuracy = (int)(stats.Accuracy * accuracyMultiplier); ;
    }

    public override StatusEffect Clone()
    {
        Blinded blinded = ScriptableObject.CreateInstance<Blinded>();
        blinded.turnsLeft = turnsLeft;
        blinded.accuracyMultiplier = accuracyMultiplier;
        blinded.CopyBase(this);
        return blinded;
    }

}