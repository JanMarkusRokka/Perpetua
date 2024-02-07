using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Save")]
public class SaveData : ScriptableObject
{
    public new string name;
    public Vector3 PlayerLocation;
    public ScenarioData ScenarioData;
    public Dictionary<string, ChestData> chests;
    public Dictionary<string, EnemyData> enemies;
}
