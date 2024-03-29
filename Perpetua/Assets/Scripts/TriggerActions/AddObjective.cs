using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Trigger Actions/Add Objective")]
public class AddObjective : TriggerAction
{
    public Objective objective;
    public override void DoAction()
    {
        if (objective)
        {
            Objective partyObjective = PartyManager.Instance.party.objectives.Find(o => o.id == objective.id);
            if (partyObjective == null)
            {
                PartyManager.Instance.party.objectives.Add(objective.Clone());
            }
        }
    }
}