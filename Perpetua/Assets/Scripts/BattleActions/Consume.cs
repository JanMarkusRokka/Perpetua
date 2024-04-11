using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consume : BattleAction
{
    public BattleParticipant participant;
    public ItemData item;
    public override BattleAction Clone()
    {
        Consume consume = ScriptableObject.CreateInstance<Consume>();
        consume.participant = participant;
        consume.item = item;
        return consume;
    }

    public override void CommitAction()
    {
        StatsData stats = participant.GetPartyMember().stats;
        stats.HealthPoints = Mathf.Min(stats.MaxHealthPoints, participant.GetPartyMember().stats.HealthPoints + item.ConsumableVariables.healthChange);
        stats.WillPower = Mathf.Min(stats.MaxWillPower, participant.GetPartyMember().stats.WillPower + item.ConsumableVariables.willpowerChange);
        BattleManager.Instance.StartCoroutine(AnimateConsumable());
    }

    private IEnumerator AnimateConsumable()
    {
        BattleCanvas battleCanvas = BattleManager.Instance.BattleCanvas;
        battleCanvas.SetPartyMemberColor(participant.transform, Color.green);
        battleCanvas.battleEffects.DisplayFloatingTextHUD(participant.transform, item.ConsumableVariables.healthChange.ToString() + "HP");
        yield return new WaitForSeconds(1f);
        battleCanvas.battleEffects.DisplayFloatingTextHUD(participant.transform, item.ConsumableVariables.willpowerChange.ToString() + "WP");
        yield return new WaitForSeconds(0.5f);
        battleCanvas.UpdatePartyTabStats();
        battleCanvas.ResetPartyMemberColor(participant.transform);
        BattleManager.Instance.CommitNextAction();
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        throw new System.NotImplementedException();
        //finish this
    }

    public override string GetName()
    {
        return "Consume";
    }

    public override BattleParticipant GetParticipant()
    {
        return participant;
    }

    public override int GetWillPowerUsage()
    {
        return 0;
    }

    public override bool SelectEnemy()
    {
        return false;
    }
}
