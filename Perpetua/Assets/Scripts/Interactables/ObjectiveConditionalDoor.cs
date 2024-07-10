using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveConditionalDoor : DialogueObject
{
    [SerializeField]
    private string SceneName;
    [SerializeField]
    public Objective objective;
    public override void Interact()
    {
        if (PartyManager.Instance.party.isObjectiveCompleted(objective))
        {
            ScenarioData sceneScenarioData = ScenarioManager.NextSceneScenarioData(SceneName);
            MenuPresenter.Instance.LoadSave(sceneScenarioData);
            return;
        }
        dialogueTrigger.TriggerDialogues();
        isActive = false;
    }
}
