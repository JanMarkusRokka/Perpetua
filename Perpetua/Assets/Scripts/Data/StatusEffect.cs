using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect : ScriptableObject
{
    public Sprite image;
    public virtual int GetTurnsLeft() { return 0; }
    public virtual void InflictActiveStatusEffect(BattleParticipant participant) { }
    public virtual void InflictPassiveStatusEffect(StatsData stats) { }
    public virtual StatusEffect Clone() { return null; }

}