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

    public override void InflictActiveStatusEffect(BattleParticipant participant)
    {
        turnsLeft--;
    }

    public override void InflictPassiveStatusEffect(StatsData stats)
    {
        stats.Accuracy = (int)(stats.Accuracy * accuracyMultiplier); ;
    }

    private void Init(int _turnsLeft, float _accuracyMultiplier, Sprite _image)
    {
        turnsLeft = _turnsLeft;
        accuracyMultiplier = _accuracyMultiplier;
        image = _image;
    }

    public override StatusEffect Clone()
    {
        Blinded blinded = ScriptableObject.CreateInstance<Blinded>();
        blinded.Init(turnsLeft, accuracyMultiplier, image);
        return blinded;
    }

}