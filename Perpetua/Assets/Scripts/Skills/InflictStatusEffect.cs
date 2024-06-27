using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/Inflict Status Effect")]
public class InflictStatusEffect : Attack
{
    public StatusEffect statusEffect;
    public int WillPowerUsage;
    [Tooltip("True if the selection of an enemy is intended, false if the selection of a party member is intended.")]
    public bool EnemySelection;
    public string SkillName;

    public override void CommitAction()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;
        // Only used to find if the action missed
        int damage = 1;
        if (EnemySelection)
        {
            damage = Attack.CalculateAttackDamage(participant, recipient);
        }
         

        if (damage <= -1)
        {
            battleManager.StartCoroutine(AnimateMiss(battleManager, battleCanvas, true));
            return;
        }

        recipient.InflictStatusEffect(statusEffect);

        battleManager.StartCoroutine(AnimateStatusEffectInfliction());
    }

    private IEnumerator AnimateStatusEffectInfliction()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;

        if (participant.IsPartyMember) battleCanvas.SetPartyMemberColor(participant.transform, Color.blue);

        if (recipient.IsPartyMember)
        {
            battleCanvas.SetPartyMemberColor(recipient.transform, Color.yellow);
            yield return new WaitForSeconds(0.5f);
            battleCanvas.battleEffects.ShroudedEffect(recipient.transform);
            battleCanvas.UpdatePartyTabStats();
            yield return new WaitForSeconds(0.5f);
            battleCanvas.ResetPartyMemberColor(recipient.transform);
        }
        else
        {
            SpriteRenderer recipientRenderer = recipient.transform.GetComponent<SpriteRenderer>();
            Color baseColor = recipientRenderer.color;
            recipientRenderer.color = Color.red;
            yield return new WaitForSeconds(1f);
            recipientRenderer.color = baseColor;
            battleCanvas.RefreshEnemyStatusEffects();
        }

        if (participant.IsPartyMember) battleCanvas.ResetPartyMemberColor(participant.transform);

        yield return new WaitForSeconds(0.2f);
        battleManager.CommitNextAction();
    }

    public override string GetName()
    {
        return SkillName;
    }

    public override BattleParticipant GetParticipant()
    {
        return participant;
    }

    public static BattleAction New(BattleParticipant _attacker, BattleParticipant _recipient, StatusEffect _statusEffect, int _willPowerUsage, bool _EnemySelection, string _SkillName)
    {
        InflictStatusEffect ise = ScriptableObject.CreateInstance<InflictStatusEffect>();

        ise.participant = _attacker;
        ise.recipient = _recipient;
        ise.statusEffect = _statusEffect;
        ise.WillPowerUsage = _willPowerUsage;
        ise.EnemySelection = _EnemySelection;
        ise.SkillName = _SkillName;

        return ise;
    }

    public override BattleAction Clone()
    {
        InflictStatusEffect ise = ScriptableObject.CreateInstance<InflictStatusEffect>();

        ise.participant = participant;
        ise.recipient = recipient;
        ise.statusEffect = statusEffect;
        ise.WillPowerUsage = WillPowerUsage;
        ise.EnemySelection = EnemySelection;
        ise.SkillName = SkillName;

        return ise;
    }

    public override int GetWillPowerUsage()
    {
        return WillPowerUsage;
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        InflictStatusEffect ise = (InflictStatusEffect)Clone();
        ise.participant = participants[0];
        ise.recipient = participants[1];
        return ise;
    }

    public override bool SelectEnemy()
    {
        return EnemySelection;
    }
    public override bool SelectPartyMember()
    {
        return !EnemySelection;
    }
}