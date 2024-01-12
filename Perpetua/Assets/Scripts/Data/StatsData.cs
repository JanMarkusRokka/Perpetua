using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Characters/Stats")]
public class StatsData : ScriptableObject
{
    public float healthPoints;
    private void Init(float _healthPoints)
    {
        healthPoints = _healthPoints;
    }

    public static StatsData New(float healthPoints)
    {
        var statsData = ScriptableObject.CreateInstance<StatsData>();

        statsData.Init(healthPoints);
        return statsData;
    }
}
