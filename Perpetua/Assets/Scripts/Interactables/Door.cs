using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField]
    private string SceneName;
    public override void Interact()
    {
        ScenarioData sceneScenarioData = ScenarioManager.NextSceneScenarioData(SceneName);
        MenuPresenter.Instance.LoadSave(sceneScenarioData);
    }
}
