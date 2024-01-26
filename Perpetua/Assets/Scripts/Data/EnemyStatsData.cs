using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Enemies/Stats")]
public class EnemyStatsData : ScriptableObject
{
    public float healthPoints;
    public float maxHealth;
    public int agility;
    public int armor;
    public float baseAttack;
    private void Init(float _healthPoints, float _maxHealth, int _agility, int _armor, float _baseAttack)
    {
        healthPoints = _healthPoints;
        maxHealth = _maxHealth;
        agility = _agility;
        armor = _armor;
        baseAttack = _baseAttack;
    }

    public static EnemyStatsData New(float healthPoints, float maxHealth, int agility, int armor, float baseAttack)
    {
        var statsData = ScriptableObject.CreateInstance<EnemyStatsData>();

        statsData.Init(healthPoints, maxHealth, agility, armor, baseAttack);
        return statsData;
    }

    public static EnemyStatsData Clone(EnemyStatsData enemyStatsData)
    {
        var statsData = ScriptableObject.CreateInstance<EnemyStatsData>();

        statsData.Init(enemyStatsData.healthPoints, enemyStatsData.maxHealth, enemyStatsData.agility, enemyStatsData.armor, enemyStatsData.baseAttack);
        return statsData;
    }
}
