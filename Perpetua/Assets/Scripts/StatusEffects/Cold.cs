using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Init(int _turnsLeft, float _defenseMultiplier, Sprite _image, string _tooltip)
    {
        turnsLeft = _turnsLeft;
        defenseMultiplier = _defenseMultiplier;
        image = _image;
        tooltip = _tooltip;
    }

    public override StatusEffect Clone()
    {
        Cold cold = ScriptableObject.CreateInstance<Cold>();
        cold.Init(turnsLeft, defenseMultiplier, image, tooltip);
        return cold;
    }

}