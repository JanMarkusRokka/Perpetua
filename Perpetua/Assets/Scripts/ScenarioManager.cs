using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenarioManager : MonoBehaviour
{
    private ScenarioData currentScenario;
    private string currentOverworldScene;
    private string currentBattleScene;
    private Vector3 lastPlayerLocation;
    private List<EnemyData> enemyData;
    private SaveData save;
    public void Awake()
    {
        if (FindObjectOfType(typeof(ScenarioManager)).GetInstanceID() != this.GetInstanceID())
        {
            Destroy(this);
        }
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
        Debug.Log("Battle Triggered");
        enemyData = new List<EnemyData>{ enemy.GetComponent<OverworldEnemy>().EnemyData};
        List<PartyCharacterData> partyMembersData = PartyManager.Instance.party.PartyMembers;
        SaveData saveData = SaveDataBeforeBattle(player.transform);
        lastPlayerLocation = player.transform.position;
        SceneManager.LoadScene(currentBattleScene);
        Debug.Log("ScenarioManager: Battle triggered");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (currentBattleScene == scene.name)
        {
            Events.SetEnemy(enemyData);
            BattleManager.Instance.CurrentOverworldScene = currentOverworldScene;
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
        currentOverworldScene = SceneManager.GetActiveScene().name;
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

    private SaveData SaveDataBeforeBattle(Transform player)
    {
        Debug.Log("save data before battle");
        SaveData saveData = ScriptableObject.CreateInstance<SaveData>();
        saveData.name = "temp";
        saveData.PlayerLocation = player.position;
        saveData.ScenarioData = ScriptableObject.CreateInstance<ScenarioData>();
        saveData.ScenarioData.StartingParty = PartyManager.Instance.party;
        saveData.ScenarioData.scene = currentOverworldScene;
        saveData.ScenarioData.isSave = true;
        saveData.ScenarioData.StartingInventory = InventoryManager.Instance.inventory;
        saveData.ScenarioData.name = "temp";
        Dictionary<string, ChestData> chests = GetAllChestStates();
        return saveData;
    }

    private Dictionary<string, ChestData> GetAllChestStates()
    {
        Dictionary<string, ChestData> chests = new Dictionary<string, ChestData>();
        Chest[] chestsInScene = FindObjectsOfType(typeof(Chest)) as Chest[];

        foreach(Chest chest in chestsInScene)
        {
            chests.Add(chest.GetComponent<GuidGenerator>().guidString, chest.GetComponent<Chest>().chestData);
            Debug.Log(chest.GetComponent<GuidGenerator>().guidString + " " + chest.GetComponent<Chest>().chestData + " " + chest.gameObject.name);
        }

        return chests;
    }
}
