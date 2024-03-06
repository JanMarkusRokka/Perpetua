using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/AttackWithSpecificStatusEffect")]
public class AttackWithSpecificStatusEffect : Attack
{
    public StatusEffect statusEffect;
    public int WillPowerUsage;
    public override void CommitAction()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;

        int damage = Attack.CalculateAttackDamage(participant, recipient);
        if (damage > -1)
        {
            recipient.InflictStatusEffect(statusEffect);
            battleManager.StartCoroutine(AnimateAttack(battleManager, battleCanvas, damage, true));
        }
        else
        {
            battleManager.StartCoroutine(AnimateMiss(battleManager, battleCanvas, true));
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

    public override int GetWillPowerUsage()
    {
        return WillPowerUsage;
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        BattleManager battleManager = BattleManager.Instance;
        AttackWithSpecificStatusEffect awsse = (AttackWithSpecificStatusEffect)Clone();
        awsse.participant = participants[0];
        awsse.recipient = participants[1];
        return awsse;
    }
}