using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatusEffect : ScriptableObject
{
    public Sprite image;
    public virtual int GetTurnsLeft() { return 0; }
    public virtual void InflictStatusEffect(BattleParticipant participant){}
    public virtual StatusEffect Clone() { return null; }

}