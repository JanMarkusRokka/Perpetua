using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.VisualScripting;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "StatusEffect/StatusEffectList")]
[Serializable]
public class StatusEffectsData : ScriptableObject
{
    public List<StatusEffect> statusEffects;

    private void Init(List<StatusEffect> _statusEffects)
    {
        statusEffects = _statusEffects;
    }

    public static StatusEffectsData Clone(StatusEffectsData statusEffectsInstance)
    {
        var statusEffectsData = ScriptableObject.CreateInstance<StatusEffectsData>();
        var statusEffects = new List<StatusEffect>();
        foreach (StatusEffect statusEffect in statusEffectsInstance.statusEffects)
        {
            statusEffects.Add(statusEffect.Clone());
        }
        statusEffectsData.Init(statusEffects);

        return statusEffectsData;
    }
}
