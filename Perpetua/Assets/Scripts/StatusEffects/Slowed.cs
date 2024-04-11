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

    private void Init(int _turnsLeft, float _slowMultiplier, Sprite _image, string _tooltip)
    {
        turnsLeft = _turnsLeft;
        slowMultiplier = _slowMultiplier;
        image = _image;
        tooltip = _tooltip;
    }

    public override StatusEffect Clone()
    {
        Slowed slowed = ScriptableObject.CreateInstance<Slowed>();
        slowed.Init(turnsLeft, slowMultiplier, image, tooltip);
        return slowed;
    }

}