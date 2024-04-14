using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/PurgeAndPunish")]
public class PurgeAndPunish : Attack
{
    public int WPowerUsage;

    public override void CommitAction()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;
        StatusEffectsData statusEffectsData = participant.GetStatusEffectsData();
        int count = statusEffectsData.statusEffects.FindAll(sf => sf.isAilment).Count;
        battleManager.StartCoroutine(commitAttacks(count));
    }

    private IEnumerator commitAttacks(int count)
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;
        int damage = Attack.CalculateAttackDamage(participant, recipient);
        if (participant.GetStatusEffectsData().statusEffects.FindAll(sf => sf.isAilment).Count > 0)
        {
            if (count > 0)
            {
                damage = (int) (damage * 0.3f);
            }
            participant.GetStatusEffectsData().statusEffects.Remove(participant.GetStatusEffectsData().statusEffects.Find(sf => sf.isAilment));
            battleCanvas.UpdatePartyTabStats();
        }
        if (damage > -1)
        {
            InflictRuneStatusEffects();
            yield return battleManager.StartCoroutine(AnimateAttack(battleManager, battleCanvas, damage, false));
        }
        else
        {
            yield return battleManager.StartCoroutine(AnimateMiss(battleManager, battleCanvas, false));
        }
        Debug.Log("punish");
        if (count > 0) battleManager.StartCoroutine(commitAttacks(count - 1));
        else battleManager.CommitNextAction();
    }

    public override string GetName()
    {
        return "Purge and Punish";
    }

    public override BattleParticipant GetParticipant()
    {
        return participant;
    }

    public static BattleAction New(BattleParticipant _attacker, BattleParticipant _recipient, StatusEffect _statusEffect, int _willPowerUsage)
    {
        PurgeAndPunish attack = ScriptableObject.CreateInstance<PurgeAndPunish>();

        attack.participant = _attacker;
        attack.recipient = _recipient;
        attack.WPowerUsage = _willPowerUsage;

        return attack;
    }

    public override BattleAction Clone()
    {
        PurgeAndPunish attack = ScriptableObject.CreateInstance<PurgeAndPunish>();

        attack.participant = participant;
        attack.recipient = recipient;
        attack.WPowerUsage = WPowerUsage;

        return attack;
    }

    public override int GetWillPowerUsage()
    {
        return WPowerUsage;
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        BattleManager battleManager = BattleManager.Instance;
        PurgeAndPunish purgeAndPunish = (PurgeAndPunish) Clone();
        purgeAndPunish.participant = participants[0];
        purgeAndPunish.recipient = participants[1];
        return purgeAndPunish;
    }
    public override bool SelectPartyMember()
    {
        return false;
    }
}
