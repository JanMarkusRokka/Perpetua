using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Trigger Actions/Advance Objective")]
public class AdvanceObjective : TriggerAction
{
    public Objective objective;
    public override void DoAction()
    {
        if (objective)
        {
            Objective partyObjective = PartyManager.Instance.party.objectives.Find(o => o.id == objective.id);
            if (partyObjective != null)
            {
                partyObjective.Advance();
            }
        }
    }
}