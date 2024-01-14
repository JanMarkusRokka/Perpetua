using System.Collections;
using System.Collections.Generic;
using System;

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
}
