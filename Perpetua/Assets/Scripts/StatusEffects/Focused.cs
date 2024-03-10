using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "StatusEffect/Types/Focused")]
public class Focused : StatusEffect
{
    private int turnsLeft = 1;
    public float criticalChanceMultiplier = 2f;
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
        turnsLeft -= 1;
    }

    public override void InflictPassiveStatusEffect(StatsData stats)
    {
        stats.CriticalChance = (int)(Mathf.Max(100, stats.CriticalChance * criticalChanceMultiplier));
    }

    private void Init(int _turnsLeft, float _criticalChanceMultiplier, Sprite _image)
    {
        turnsLeft = _turnsLeft;
        criticalChanceMultiplier = _criticalChanceMultiplier;
        image = _image;
    }

    public override StatusEffect Clone()
    {
        Focused focused = ScriptableObject.CreateInstance<Focused>();
        focused.Init(turnsLeft, criticalChanceMultiplier, image);
        return focused;
    }

}