using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/AttackWithSpecificStatusEffect")]
public class AttackWithSpecificStatusEffect : Attack
{
    public StatusEffect statusEffect;
    public int WillPowerUsage;
    public bool areaOfEffect;
    public override void CommitAction()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;
        // If aoe, then only used to calculate missing an attack
        int damage = Attack.CalculateAttackDamage(participant, recipient);
        if (damage > -1)
        {
            if (areaOfEffect)
            {
                List<BattleParticipant> recipients = new();

                if (recipient.IsPartyMember)
                    recipients = BattleManager.Instance.party;
                else
                    recipients = BattleManager.Instance.agilityOrder.FindAll(bp => !bp.IsPartyMember);

                bool last = false;

                for (int i = 0; i < recipients.Count; i++)
                {
                    BattleParticipant recipientMember = recipients[i];
                    if (Random.Range(0, 100) > recipientMember.GetStatsData().AilmentResistance)
                    recipientMember.InflictStatusEffect(statusEffect);

                    int recipientDamage = Attack.CalculateAttackDamage(participant, recipientMember);
                    if (i == recipients.Count - 1)
                    {
                        last = true;
                    }
                    if (recipientDamage > -1)
                    {
                        battleManager.StartCoroutine(AnimateAttack(battleManager, battleCanvas, (int) (recipientDamage*0.5f), last, recipientMember));
                    }
                    else
                    {
                        battleManager.StartCoroutine(AnimateMiss(battleManager, battleCanvas, last, recipientMember));
                    }
                }
            }
            else
            {
                if (Random.Range(0, 100) > recipient.GetStatsData().AilmentResistance)
                    recipient.InflictStatusEffect(statusEffect);
                battleManager.StartCoroutine(AnimateAttack(battleManager, battleCanvas, damage, true));
            }
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

    public static BattleAction New(BattleParticipant _attacker, BattleParticipant _recipient, StatusEffect _statusEffect, int _willPowerUsage, bool _areaOfEffect, AudioClipGroup _Sound)
    {
        AttackWithSpecificStatusEffect attack = ScriptableObject.CreateInstance<AttackWithSpecificStatusEffect>();

        attack.participant = _attacker;
        attack.recipient = _recipient;
        attack.statusEffect = _statusEffect;
        attack.WillPowerUsage = _willPowerUsage;
        attack.areaOfEffect = _areaOfEffect;
        attack.Sound = _Sound;

        return attack;
    }

    public override BattleAction Clone()
    {
        AttackWithSpecificStatusEffect attack = ScriptableObject.CreateInstance<AttackWithSpecificStatusEffect>();

        attack.participant = participant;
        attack.recipient = recipient;
        attack.statusEffect = statusEffect;
        attack.WillPowerUsage = WillPowerUsage;
        attack.areaOfEffect = areaOfEffect;
        attack.Sound = Sound;

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
    public override bool SelectPartyMember()
    {
        return false;
    }
}