using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/Remove Status Effects")]
public class RemoveStatusEffects : Attack
{
    public int WillPowerUsage;
    public bool RemoveOnlyAilments;
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

        if (RemoveOnlyAilments)
        {
            recipient.GetPartyMember().statusEffects.statusEffects.RemoveAll(sf => sf.isAilment);
        }
        else
        {
            recipient.GetPartyMember().statusEffects = new();
        }

        battleManager.StartCoroutine(AnimateStatusEffectRemoval());
    }

    private IEnumerator AnimateStatusEffectRemoval()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;

        if (recipient.IsPartyMember)
        {
            battleCanvas.SetPartyMemberColor(recipient.transform, Color.yellow);
            yield return new WaitForSeconds(0.5f);
            battleCanvas.UpdatePartyTabStats();
            battleCanvas.battleEffects.ShroudedEffect(recipient.transform);
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

    public static BattleAction New(BattleParticipant _attacker, BattleParticipant _recipient, int _willPowerUsage, bool _EnemySelection, bool _RemoveOnlyAilments, string _SkillName)
    {
        RemoveStatusEffects rsf = ScriptableObject.CreateInstance<RemoveStatusEffects>();

        rsf.participant = _attacker;
        rsf.recipient = _recipient;
        rsf.WillPowerUsage = _willPowerUsage;
        rsf.EnemySelection = _EnemySelection;
        rsf.RemoveOnlyAilments = _RemoveOnlyAilments;
        rsf.SkillName = _SkillName;

        return rsf;
    }

    public override BattleAction Clone()
    {
        RemoveStatusEffects rsf = ScriptableObject.CreateInstance<RemoveStatusEffects>();

        rsf.participant = participant;
        rsf.recipient = recipient;
        rsf.WillPowerUsage = WillPowerUsage;
        rsf.EnemySelection = EnemySelection;
        rsf.RemoveOnlyAilments = RemoveOnlyAilments;
        rsf.SkillName = SkillName;

        return rsf;
    }

    public override int GetWillPowerUsage()
    {
        return WillPowerUsage;
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        RemoveStatusEffects rsf = (RemoveStatusEffects)Clone();
        rsf.participant = participants[0];
        rsf.recipient = participants[1];
        return rsf;
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