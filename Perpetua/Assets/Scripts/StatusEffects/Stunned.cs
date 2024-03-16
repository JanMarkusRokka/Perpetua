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

    public override void InflictActiveStatusEffect(BattleParticipant participant)
    {
        turnsLeft--;
    }

    public override void InflictPassiveStatusEffect(StatsData stats)
    {
        stats.AttackSpeed = 0;
    }

    private void Init(int _turnsLeft, Sprite _image)
    {
        turnsLeft = _turnsLeft;
        image = _image;
    }

    public override StatusEffect Clone()
    {
        Stunned stunned = ScriptableObject.CreateInstance<Stunned>();
        stunned.Init(turnsLeft, image);
        return stunned;
    }

}