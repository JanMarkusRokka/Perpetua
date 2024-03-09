using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/ApplyStatusEffectToSelf")]
public class ApplyStatusEffectToSelf : BattleAction
{
    public StatusEffect statusEffect;
    public int WillPowerUsage;
    public BattleParticipant participant;
    public string SkillName;
    public override void CommitAction()
    {
        BattleManager battleManager = BattleManager.Instance;

        battleManager.StartCoroutine(AnimateEffect());
    }

    private IEnumerator AnimateEffect()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleEffects battleEffects = battleManager.BattleCanvas.battleEffects;

        participant.InflictStatusEffect(statusEffect);

        battleEffects.ShroudedEffect(participant.transform);
        battleManager.BattleCanvas.UpdatePartyTabStats();
        yield return new WaitForSeconds(0.5f);
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

    public static BattleAction New(BattleParticipant _attacker, StatusEffect _statusEffect)
    {
        ApplyStatusEffectToSelf skill = ScriptableObject.CreateInstance<ApplyStatusEffectToSelf>();

        skill.participant = _attacker;
        skill.statusEffect = _statusEffect;

        return skill;
    }

    public override BattleAction Clone()
    {
        ApplyStatusEffectToSelf skill = ScriptableObject.CreateInstance<ApplyStatusEffectToSelf>();

        skill.participant = participant;
        skill.statusEffect = statusEffect;

        return skill;
    }

    public override int GetWillPowerUsage()
    {
        return WillPowerUsage;
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        BattleManager battleManager = BattleManager.Instance;
        ApplyStatusEffectToSelf skill = (ApplyStatusEffectToSelf) Clone();
        skill.participant = participants[0];
        return skill;
    }

    public override bool SelectEnemy()
    {
        return false;
    }
}