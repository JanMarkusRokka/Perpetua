using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect : ScriptableObject
{
    public Sprite image;
    public string tooltip;
    public bool isAilment;
    public virtual int GetTurnsLeft() { return 0; }
    public virtual IEnumerator InflictActiveStatusEffect(BattleParticipant participant) { yield break; }
    public virtual void InflictPassiveStatusEffect(StatsData stats) { }
    public void CopyBase(StatusEffect se)
    {
        image = se.image;
        tooltip = se.tooltip;
        isAilment = se.isAilment;
    }
    public virtual StatusEffect Clone() { return null; }

}