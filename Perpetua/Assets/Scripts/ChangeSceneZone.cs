using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneZone : MonoBehaviour
{
    [SerializeField]
    private string SceneName;
    [SerializeField] // Not used if magnitude is smaller than 1
    private Vector3 PlayerLocation;
    [Header("Second scene (conditional)")]
    [SerializeField]
    private bool IsConditional;
    [SerializeField]
    private Objective Objective;
    [SerializeField]
    private string SecondSceneName;
    [SerializeField]
    private Vector3 SecondPlayerLocation;
    private void OnTriggerEnter(Collider other)
    {
        OverworldPlayer player = other.GetComponent<OverworldPlayer>();
        if (player)
        {
            if (PartyManager.Instance.party.isObjectiveCompleted(Objective))
            {
                ScenarioData sceneScenarioData = ScenarioManager.NextSceneScenarioData(SecondSceneName);
                sceneScenarioData.PlayerLocation = SecondPlayerLocation;
                MenuPresenter.Instance.LoadSave(sceneScenarioData);
            }
            else
            {
                ScenarioData sceneScenarioData = ScenarioManager.NextSceneScenarioData(SceneName);
                sceneScenarioData.PlayerLocation = PlayerLocation;
                MenuPresenter.Instance.LoadSave(sceneScenarioData);
            }
        }
    }
}
