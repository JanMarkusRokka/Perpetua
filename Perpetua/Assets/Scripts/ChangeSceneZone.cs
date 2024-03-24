using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSceneZone : MonoBehaviour
{
    [SerializeField]
    private string SceneName;
    [SerializeField]
    private Vector3 PlayerLocation;
    private void OnTriggerEnter(Collider other)
    {
        OverworldPlayer player = other.GetComponent<OverworldPlayer>();
        if (player)
        {
            ScenarioData sceneScenarioData = ScenarioManager.NextSceneScenarioData(SceneName);
            sceneScenarioData.PlayerLocation = PlayerLocation;
            MenuPresenter.Instance.LoadSave(sceneScenarioData);
        }
    }
}
