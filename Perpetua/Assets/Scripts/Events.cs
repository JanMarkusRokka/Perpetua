using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class Events
{
    public static event Action<ItemData> OnItemReceived;
    public static void ReceiveItem(ItemData data) => OnItemReceived?.Invoke(data);

    public static event Action<int> OnDialogueEnded;
    public static void EndDialogue(int value) => OnDialogueEnded?.Invoke(value);
    /**
    public static event Action<int> OnSave;
    public static void Save(int value) => OnSave?.Invoke(value);
    public static event Action<int> OnLoad;
    public static void Load(int value) => OnLoad?.Invoke(value);*/
    public static event Action<ScenarioData> OnSelectedScenario;
    public static void SelectScenario(ScenarioData scenario) => OnSelectedScenario?.Invoke(scenario);
    public static event Action<GameObject, GameObject> OnBattleTriggered;
    public static void TriggerBattle(GameObject enemy, GameObject player) => OnBattleTriggered?.Invoke(enemy, player);

    public static event Action<string> OnBattleSceneChanged;
    public static void SetBattleScene(string battleScene) => OnBattleSceneChanged?.Invoke(battleScene);

    public static event Action<List<EnemyData>> OnSetEnemy;
    public static void SetEnemy(List<EnemyData> enemyData) => OnSetEnemy?.Invoke(enemyData);
}
