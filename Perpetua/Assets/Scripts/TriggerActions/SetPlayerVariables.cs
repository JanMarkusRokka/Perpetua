using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Trigger Actions/Set Player Variables")]
public class SetPlayerVariables : TriggerAction
{
    public bool movement;
    public override void DoAction()
    {
        GameObject player = GameObject.Find("Player");
        player.GetComponent<CapsuleCollider>().enabled = true;
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        if (movement)
            player.GetComponent<OverworldPlayer>().EnableMovement();
        else player.GetComponent<OverworldPlayer>().DisableMovement();
    }
}