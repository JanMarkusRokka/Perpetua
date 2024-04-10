using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActionPlayer : MonoBehaviour
{
    [SerializeField]
    private TriggerAction triggerAction;
    public void TriggerAction()
    {
        triggerAction.DoAction();
    }
}
