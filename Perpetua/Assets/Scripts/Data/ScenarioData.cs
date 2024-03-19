using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Scenario")]
public class ScenarioData : ScriptableObject
{
    public new string name;
    public PartyData StartingParty;
    public InventoryData StartingInventory;
    public string scene;
    public bool isSave = false;
    public bool isLoadNextScene = false;
    [Header("Save-only data")]
    public Vector3 PlayerLocation;
    public Dictionary<string, ChestData> Chests;
    public Dictionary<string, EnemyData> Enemies;
    //public List<EnemyData> SlowedEnemies;

    private void Init(string _name, PartyData _StartingParty, InventoryData _StartingInventory, string _scene, bool _isSave, bool _isLoadNextScene, Vector3 _PlayerLocation, Dictionary<string, ChestData> _Chests, Dictionary<string, EnemyData> _Enemies)
    {
        name = _name;
        StartingParty = _StartingParty;
        StartingInventory = _StartingInventory;
        scene = _scene;
        isSave = _isSave;
        isLoadNextScene = _isLoadNextScene;
        PlayerLocation = _PlayerLocation;
        Chests = _Chests;
        Enemies = _Enemies;
    }

    public static ScenarioData Clone(ScenarioData scenario)
    {
        var scenarioData = ScriptableObject.CreateInstance<ScenarioData>();

        scenarioData.Init(scenario.name, scenario.StartingParty, scenario.StartingInventory, scenario.name, scenario.isSave, scenario.isLoadNextScene, scenario.PlayerLocation, scenario.Chests, scenario.Enemies);

        return scenarioData;
    }

    public static ScenarioData New(string name, PartyData StartingParty, InventoryData StartingInventory, string scene, bool isSave, bool isLoadNextScene, Vector3 PlayerLocation, Dictionary<string, ChestData> Chests, Dictionary<string, EnemyData> Enemies)
    {
        var scenarioData = ScriptableObject.CreateInstance<ScenarioData>();

        scenarioData.Init(name, StartingParty, StartingInventory, scene, isSave, isLoadNextScene, PlayerLocation, Chests, Enemies);

        return scenarioData;
    }
}
