using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveConditionalDoor : DialogueObject
{
    [SerializeField]
    private string SceneName;
    [SerializeField]
    public int objectiveId;
    public override void Interact()
    {
        Objective objective = PartyManager.Instance.party.objectives.Find(o => o.id == objectiveId);
        if (objective != null)
        {
            if (objective.IsCompleted())
            {
                ScenarioData sceneScenarioData = ScenarioManager.NextSceneScenarioData(SceneName);
                MenuPresenter.Instance.LoadSave(sceneScenarioData);
                return;
            }
        }
        dialogueTrigger.TriggerDialogues();
        isActive = false;
    }
}
