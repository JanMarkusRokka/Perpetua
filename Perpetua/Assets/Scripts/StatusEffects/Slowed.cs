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

    private void Init(int _turnsLeft, float _slowMultiplier, Sprite _image)
    {
        turnsLeft = _turnsLeft;
        slowMultiplier = _slowMultiplier;
        image = _image;
    }

    public override StatusEffect Clone()
    {
        Slowed slowed = ScriptableObject.CreateInstance<Slowed>();
        slowed.Init(turnsLeft, slowMultiplier, image);
        return slowed;
    }

}