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
            if (!PartyManager.Instance.party.isObjectiveAlreadyAdded(objective))
            {
                PartyManager.Instance.party.objectives.Add(objective.Clone());
            }
        }
    }
}