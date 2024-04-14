using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/TestAction")]
public class TestAction : BattleAction
{
    public BattleParticipant participant;
    int willPowerUsage = 5;
    public override void CommitAction()
    {
        BattleManager battleManager = BattleManager.Instance;
        battleManager.GuardDuringTurn.Add(participant);

        battleManager.StartCoroutine(AnimateGuard());

        //make attack take guard into account (-50% damage taken and heal?)
    }

    IEnumerator AnimateGuard()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;
        BattleEffects battleEffects = battleCanvas.battleEffects;
        if (participant.IsPartyMember)
        {
            Transform participantTransform = battleCanvas.PartyPresenter.transform.Find(battleManager.agilityOrder.IndexOf(participant).ToString());
            battleEffects.DisplayGuardEffect(participantTransform, true);
        }
        else
        {
            Transform participantTransform = battleManager.Enemies.Find(battleManager.agilityOrder.IndexOf(participant).ToString());
            battleEffects.DisplayGuardEffect(participantTransform, false);
        }
        yield return new WaitForSeconds(0.5f);
        battleManager.CommitNextAction();
    }

    public static TestAction New(BattleParticipant _participant)
    {
        TestAction testAction = ScriptableObject.CreateInstance<TestAction>();
        testAction.participant = _participant;
        return testAction;
    }
    public override BattleParticipant GetParticipant()
    {
        return participant;
    }

    public override string GetName()
    {
        return "Test Action";
    }

    public override BattleAction Clone()
    {
        TestAction testAction = ScriptableObject.CreateInstance<TestAction>();
        testAction.participant = participant;
        return testAction;
    }

    public override int GetWillPowerUsage()
    {
        return willPowerUsage;
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        throw new System.NotImplementedException();
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