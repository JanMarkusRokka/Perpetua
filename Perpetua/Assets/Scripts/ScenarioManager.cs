using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScenarioManager : MonoBehaviour
{
    private ScenarioData currentScenario;

    public void Awake()
    {
        Events.OnSelectedScenario += OnSelectedScenario;
    }

    public void OnDestroy()
    {
        Events.OnSelectedScenario -= OnSelectedScenario;
    }

    public void OnSelectedScenario(ScenarioData scenario)
    {
        Debug.Log("Setting up scenario: " + scenario.name);
        currentScenario = scenario;
        if (scenario.isSave)
        {
            // Instantiates new items, party members, etc.
            InventoryManager.Instance.SetInventory(scenario.StartingInventory);
            PartyManager.Instance.SetParty(scenario.StartingParty);
        } else
        {
            // Instantiates new items, party members, etc.
            InventoryManager.Instance.SetInventoryInstantiate(scenario.StartingInventory);
            PartyManager.Instance.SetPartyInstantiate(scenario.StartingParty, InventoryManager.Instance.inventory);
        }

        //Events.Save(0);
    }

}
