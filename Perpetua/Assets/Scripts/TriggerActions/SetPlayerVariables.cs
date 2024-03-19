using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Trigger Actions/Set Player Variables")]
public class SetPlayerVariables : TriggerAction
{
    public bool movement;
    public override void DoAction()
    {
        if (movement)
            GameObject.Find("Player").GetComponent<OverworldPlayer>().EnableMovement();
        else GameObject.Find("Player").GetComponent<OverworldPlayer>().DisableMovement();
    }
}