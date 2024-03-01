using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/AttackWithSpecificStatusEffect")]
public class AttackWithSpecificStatusEffect : Attack
{
    public StatusEffect statusEffect;
    public override void CommitAction()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;

        int damage = Attack.CalculateAttackDamage(participant, recipient);
        if (damage > -1)
        {
            InflictStatusEffect(statusEffect, recipient);
            battleManager.StartCoroutine(AnimateAttack(battleManager, battleCanvas, damage));
        }
        else
        {
            battleManager.StartCoroutine(AnimateMiss(battleManager, battleCanvas));
        }
    }

    public override string GetName()
    {
        return "Slash";
    }

    public override BattleParticipant GetParticipant()
    {
        return participant;
    }

    public static BattleAction New(BattleParticipant _attacker, BattleParticipant _recipient, StatusEffect _statusEffect)
    {
        AttackWithSpecificStatusEffect attack = ScriptableObject.CreateInstance<AttackWithSpecificStatusEffect>();

        attack.participant = _attacker;
        attack.recipient = _recipient;
        attack.statusEffect = _statusEffect;

        return attack;
    }

    public override BattleAction Clone()
    {
        AttackWithSpecificStatusEffect attack = ScriptableObject.CreateInstance<AttackWithSpecificStatusEffect>();

        attack.participant = participant;
        attack.recipient = recipient;
        attack.statusEffect = statusEffect;

        return attack;
    }
}