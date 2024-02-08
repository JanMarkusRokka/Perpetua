using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemiesLoader : MonoBehaviour
{
    public static EnemiesLoader Instance;

    private void Awake()
    {
        if (FindObjectsOfType(typeof(EnemiesLoader)).Count() > 1)
        {
            Destroy(this);
        }
        Instance = this;
    }

    public Dictionary<string, OverworldEnemy> GetAllEnemies()
    {
        Dictionary<string, OverworldEnemy> enemies = new();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform enemyTransform = transform.GetChild(i);
            enemies.Add(enemyTransform.name, enemyTransform.GetComponent<OverworldEnemy>());
        }
        return enemies;
    }

    public void SetAllEnemies(Dictionary<string, EnemyData> enemyData)
    {
        Dictionary<string, OverworldEnemy> enemies = GetAllEnemies();
        foreach (string enemyName in enemies.Keys)
        {
            if (enemyData.ContainsKey(enemyName))
            {
                enemies[enemyName].SetupEnemy(enemyData[enemyName]);
            }
        }
    }
}
