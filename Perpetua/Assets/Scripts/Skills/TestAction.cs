using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/TestAction")]
public class TestAction : BattleAction
{
    public BattleParticipant participant;
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
}