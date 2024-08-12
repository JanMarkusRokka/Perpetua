using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/Flee")]
public class Flee : BattleAction
{
    public BattleParticipant participant;

    int willPowerUsage = 0;
    public override void CommitAction()
    {
        bool canFlee = true;
        List<EnemyData> enemies = BattleManager.Instance.EnemyData;
        foreach(EnemyData enemy in enemies)
        {
            StatsData statsData = enemy.GetStatsWithAllEffects();
            if (statsData.AttackSpeed <= 0 || statsData.HealthPoints <= 0 || !enemy.isEscapable)
            {
                canFlee = false;
            }
        }
        if (canFlee) 
            BattleManager.Instance.Flee();
        else
        {
            BattleManager.Instance.StartCoroutine(DisplayFailure());
        }
        //make attack take guard into account (-50% damage taken and heal?)
    }

    IEnumerator DisplayFailure()
    {
        BattleManager.Instance.BattleCanvas.SetPartyMemberColor(participant.transform, Color.red);
        BattleManager.Instance.BattleCanvas.battleEffects.DisplayFloatingTextHUD(participant.transform, "Failed to flee...");
        yield return new WaitForSeconds(0.5f);
        BattleManager.Instance.BattleCanvas.ResetPartyMemberColor(participant.transform);
        BattleManager.Instance.CommitNextAction();
    }

    public static Flee New(BattleParticipant _participant)
    {
        Flee flee = ScriptableObject.CreateInstance<Flee>();
        flee.participant = _participant;
        return flee;
    }
    public override BattleParticipant GetParticipant()
    {
        return participant;
    }

    public override string GetName()
    {
        return "Flee";
    }

    public override BattleAction Clone()
    {
        Flee flee = ScriptableObject.CreateInstance<Flee>();

        flee.participant = participant;

        return flee;
    }

    public override int GetWillPowerUsage()
    {
        return willPowerUsage;
    }
    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        Flee flee = (Flee)Clone();
        flee.participant = participants[0];
        return flee;
    }

    public override bool SelectEnemy()
    {
        return false;
    }
    public override bool SelectPartyMember()
    {
        return false;
    }
}
