using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioManager : MonoBehaviour
{
    private ScenarioData currentScenario;
    private string currentBattleScene;
    private Vector3 lastPlayerLocation;
    private List<EnemyData> enemyData;
    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Events.OnSelectedScenario += OnSelectedScenario;
        Events.OnBattleTriggered += OnBattleTriggered;
        Events.OnBattleSceneChanged += OnBattleSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDestroy()
    {
        Events.OnSelectedScenario -= OnSelectedScenario;
        Events.OnBattleTriggered -= OnBattleTriggered;
        Events.OnBattleSceneChanged -= OnBattleSceneChanged;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnBattleTriggered(GameObject enemy, GameObject player)
    {
        enemyData = new List<EnemyData>{ enemy.GetComponent<OverworldEnemy>().EnemyData};
        List<PartyCharacterData> partyMembersData = PartyManager.Instance.party.PartyMembers;
        lastPlayerLocation = player.transform.position;
        SceneManager.LoadScene(currentBattleScene);
        Debug.Log("ScenarioManager: Battle triggered");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (currentBattleScene == scene.name)
        {
            Events.SetEnemy(enemyData);
        }
    }

    private void OnBattleSceneChanged(string battleScene)
    {
        currentBattleScene = battleScene;
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
            // Temp for
            InventoryData inventory = ScriptableObject.CreateInstance<InventoryData>();
            inventory.items = new List<ItemData>();
            InventoryManager.Instance.SetInventory(inventory);

            //InventoryManager.Instance.SetInventoryInstantiate(scenario.StartingInventory);
            PartyManager.Instance.SetPartyInstantiate(scenario.StartingParty, InventoryManager.Instance.inventory);
            Debug.Log(PartyManager.Instance.party.PartyMembers);
        }

        //Events.Save(0);
    }

}
