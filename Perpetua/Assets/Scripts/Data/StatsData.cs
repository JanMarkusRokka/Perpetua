using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Characters/Stats")]
public class StatsData : ScriptableObject
{
    public float healthPoints;
    public float baseAttack;
    public float baseDefense;
    public int agility;
    private void Init(float _healthPoints, float _baseAttack, float _baseDefense, int _agility)
    {
        healthPoints = _healthPoints;
        baseAttack = _baseAttack;
        baseDefense = _baseDefense;
        agility = _agility;
    }

    public static StatsData New(float healthPoints, float baseAttack, float baseDefense, int agility)
    {
        var statsData = ScriptableObject.CreateInstance<StatsData>();

        statsData.Init(healthPoints, baseAttack, baseDefense, agility);
        return statsData;
    }
}
