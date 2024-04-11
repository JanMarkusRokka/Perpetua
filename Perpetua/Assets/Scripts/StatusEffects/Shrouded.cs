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

    public override void InflictActiveStatusEffect(BattleParticipant participant)
    {
        turnsLeft -= 1;
    }

    public override void InflictPassiveStatusEffect(StatsData stats)
    {
        stats.Detectability = (int)(stats.Detectability * detectabilityMultiplier);
    }

    private void Init(int _turnsLeft, float _detectabilityMultiplier, Sprite _image, string _tooltip)
    {
        turnsLeft = _turnsLeft;
        detectabilityMultiplier = _detectabilityMultiplier;
        image = _image;
        tooltip = _tooltip;
    }

    public override StatusEffect Clone()
    {
        Shrouded shrouded = ScriptableObject.CreateInstance<Shrouded>();
        shrouded.Init(turnsLeft, detectabilityMultiplier, image, tooltip);
        return shrouded;
    }

}