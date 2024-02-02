using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BattleAction
{
    public void CommitAction();
}

public class Attack : BattleAction
{
    public BattleParticipant attacker;
    public BattleParticipant recipient;
    public void CommitAction()
    {
        //recipient.health();
        Debug.Log("Attack action");
    }

    public static Attack New(BattleParticipant _attacker, BattleParticipant _recipient)
    {
        return new Attack() {
        attacker = _attacker,
        recipient = _recipient
        };
    }
}

public class Guard : BattleAction
{
    public BattleParticipant participant;
    public void CommitAction()
    {
        Debug.Log("Guard action");
    }
    public static Guard New(BattleParticipant _participant)
    {
        return new Guard()
        {
            participant = _participant
        };
    }
}