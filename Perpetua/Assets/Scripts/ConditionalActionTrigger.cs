using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Trigger Actions/Conditional Action Trigger")]
public class ConditionalActionTrigger : TriggerAction
{
    [SerializeField]
    private TriggerAction action;
    [SerializeField]
    private Objective objective;

    public override void DoAction()
    {
        if (objective.IsCompleted())
            action.DoAction();
    }
}
