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
        Objective obj = PartyManager.Instance.party.objectives.Find(o => o.id == objective.id);
        if (obj != null)
        {
            if (obj.IsCompleted())
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
