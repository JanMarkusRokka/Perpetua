using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/PurgeAndPunish")]
public class ShroudedPath : BattleAction
{
    public BattleParticipant participant;
    public int WPowerUsage;

    public override void CommitAction()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;
        StatusEffectsData statusEffectsData = participant.GetStatusEffectsData();
        battleManager.StartCoroutine(CommitSkill());
    }

    private IEnumerator CommitSkill()
    {
        yield return new WaitForSeconds(0.5f);
    }

    public override string GetName()
    {
        return "Shrouded Path";
    }

    public override BattleParticipant GetParticipant()
    {
        return participant;
    }

    public static BattleAction New(BattleParticipant _attacker, BattleParticipant _recipient, StatusEffect _statusEffect)
    {
        ShroudedPath skill = ScriptableObject.CreateInstance<ShroudedPath>();

        skill.participant = _attacker;

        return skill;
    }

    public override BattleAction Clone()
    {
        ShroudedPath skill = ScriptableObject.CreateInstance<ShroudedPath>();

        skill.participant = participant;

        return skill;
    }

    public override int GetWillPowerUsage()
    {
        return WPowerUsage;
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        ShroudedPath shroudedPath = (ShroudedPath) Clone();
        shroudedPath.participant = participants[0];
        return shroudedPath;
    }

    public override bool SelectEnemy()
    {
        return true;
    }
}
