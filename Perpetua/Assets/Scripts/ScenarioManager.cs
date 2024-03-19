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
    private string currentBattleScene;
    private List<EnemyData> enemyData;
    public void Awake()
    {
        if (FindObjectsOfType(typeof(ScenarioManager)).Count() > 1)
        {
            Destroy(gameObject);
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
        enemyData = new List<EnemyData> { enemy.GetComponent<OverworldEnemy>().EnemyData };
        List<PartyCharacterData> partyMembersData = PartyManager.Instance.party.PartyMembers;
        currentScenario = SaveDataBeforeBattle(player.transform);
        Time.timeScale = 0.1f;
        ColorOverlay.FadeToBlack();
        StartCoroutine(WaitBeforeBattle());
    }

    private IEnumerator WaitBeforeBattle()
    {
        yield return new WaitForSecondsRealtime(3f);
        SceneManager.LoadScene(currentBattleScene);
        Debug.Log("ScenarioManager: Battle triggered");
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Time.timeScale = 1f;
        ColorOverlay.FadeToTransparent();
        if (currentBattleScene == scene.name)
        {
            Events.SetEnemy(enemyData);
            BattleManager.Instance.returnScenario = currentScenario;
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
            RestoreDataFromSave(scenario);
        } else
        {
            if (scenario.isLoadNextScene)
            {
                InventoryManager.Instance.SetInventory(scenario.StartingInventory);
                PartyManager.Instance.SetParty(scenario.StartingParty);
            }
            else
            {
                InventoryData inventory = ScriptableObject.CreateInstance<InventoryData>();
                inventory.items = new List<ItemData>();
                InventoryManager.Instance.SetInventory(inventory);
                PartyManager.Instance.SetPartyInstantiate(scenario.StartingParty, InventoryManager.Instance.inventory);
            }

            //InventoryManager.Instance.SetInventoryInstantiate(scenario.StartingInventory);
        }

        //Events.Save(0);
    }

    public static ScenarioData NextSceneScenarioData(string nextScene)
    {
        return ScenarioData.New("temp", PartyManager.Instance.party, InventoryManager.Instance.inventory, nextScene, false, true, Vector3.zero, null, null);
    }

    private ScenarioData SaveDataBeforeBattle(Transform player)
    {
        Debug.Log("save data before battle");
        ScenarioData scenarioData = ScenarioData.New("temp", PartyManager.Instance.party, InventoryManager.Instance.inventory, SceneManager.GetActiveScene().name, true, false, player.position, GetAllChestStates(), GetAllEnemyStates());
        return scenarioData;
    }

    private Dictionary<string, EnemyData> GetAllEnemyStates()
    {
        Dictionary<string, EnemyData> enemies = new();
        OverworldEnemy[] enemiesInScene = FindObjectsOfType(typeof(OverworldEnemy)) as OverworldEnemy[];

        foreach (OverworldEnemy enemy in enemiesInScene)
        {
            enemies.Add(enemy.GetComponent<GuidGenerator>().guidString, enemy.GetComponent<OverworldEnemy>().EnemyData);
            Debug.Log(enemy.GetComponent<GuidGenerator>().guidString + " " + enemy.GetComponent<OverworldEnemy>().EnemyData + " " + enemy.gameObject.name);
        }

        return enemies;
    }

    private Dictionary<string, ChestData> GetAllChestStates()
    {
        Dictionary<string, ChestData> chests = new();
        Chest[] chestsInScene = FindObjectsOfType(typeof(Chest)) as Chest[];

        foreach (Chest chest in chestsInScene)
        {
            chests.Add(chest.GetComponent<GuidGenerator>().guidString, chest.GetComponent<Chest>().chestData);
            Debug.Log(chest.GetComponent<GuidGenerator>().guidString + " " + chest.GetComponent<Chest>().chestData + " " + chest.gameObject.name);
        }

        return chests;
    }

    //Presumes that scene has been loaded
    private void RestoreDataFromSave(ScenarioData saveData)
    {
        PartyManager.Instance.SetParty(saveData.StartingParty);
        InventoryManager.Instance.SetInventory(saveData.StartingInventory);
        FindObjectOfType<OverworldPlayer>().transform.position = saveData.PlayerLocation;
        SetChestsFromSave(saveData);
        SetEnemiesFromSave(saveData);
    }

    private void SetChestsFromSave(ScenarioData saveData)
    {
        Dictionary<string, ChestData> chestsData = saveData.Chests;
        ChestsLoader.Instance.SetAllChests(chestsData);
    }

    private void SetEnemiesFromSave(ScenarioData saveData)
    {
        Dictionary<string, EnemyData> enemiesData = saveData.Enemies;
        EnemiesLoader.Instance.SetAllEnemies(enemiesData);
    }
}
