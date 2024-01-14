using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Init(string _name, PartyData _StartingParty, InventoryData _StartingInventory, string _scene)
    {
        name = _name;
        StartingParty = _StartingParty;
        StartingInventory = _StartingInventory;
        scene = _scene;
    }

    public static ScenarioData Clone(ScenarioData scenario)
    {
        var scenarioData = ScriptableObject.CreateInstance<ScenarioData>();

        scenarioData.Init(scenario.name, scenario.StartingParty, scenario.StartingInventory, scenario.name);
        return scenarioData;
    }

}
