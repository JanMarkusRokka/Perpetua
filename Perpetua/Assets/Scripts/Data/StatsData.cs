using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Characters/Stats")]
public class StatsData : ScriptableObject
{
    public float HealthPoints;
    public float MaxHealthPoints;

    public float PhysicalDamage;
    public float MagicDamage;

    public float PhysicalDefense;
    public float MagicDefense;

    public float AilmentResistance;

    public int AttackSpeed;

    public float Dodge;
    public float Accuracy;

    public float CriticalChance;
    public float CriticalMultiplier;

    private void Init(float _HealthPoints, float _MaxHealthPoints, float _PhysicalDamage, float _MagicDamage, float _PhysicalDefense,
        float _MagicDefense, float _AilmentResistance, int _AttackSpeed, float _Dodge, float _Accuracy,
        float _CriticalChance, float _CriticalMultiplier)
    {
        HealthPoints = _HealthPoints;
        MaxHealthPoints = _MaxHealthPoints;

        PhysicalDamage = _PhysicalDamage;
        MagicDamage = _MagicDamage;

        PhysicalDefense = _PhysicalDefense;
        MagicDefense = _MagicDefense;

        AilmentResistance = _AilmentResistance;

        AttackSpeed = _AttackSpeed;

        Dodge = _Dodge;
        Accuracy = _Accuracy;

        CriticalChance = _CriticalChance;
        CriticalMultiplier = _CriticalMultiplier;
    }

    public static StatsData New(float HealthPoints, float MaxHealthPoints, float PhysicalDamage, float MagicDamage, float PhysicalDefense,
        float MagicDefense, float AilmentResistance, int AttackSpeed, float Dodge, float Accuracy,
        float CriticalChance, float CriticalMultiplier)
    {
        var statsData = ScriptableObject.CreateInstance<StatsData>();

        statsData.Init(HealthPoints, MaxHealthPoints, PhysicalDamage, MagicDamage, PhysicalDefense,
        MagicDefense, AilmentResistance, AttackSpeed, Dodge, Accuracy,
        CriticalChance, CriticalMultiplier);
        return statsData;
    }

    public static StatsData Clone(StatsData stats)
    {
        if (stats == null) return null;

        var statsData = ScriptableObject.CreateInstance<StatsData>();

        statsData.Init(stats.HealthPoints, stats.MaxHealthPoints, stats.PhysicalDamage, stats.MagicDamage, stats.PhysicalDefense,
        stats.MagicDefense, stats.AilmentResistance, stats.AttackSpeed, stats.Dodge, stats.Accuracy,
        stats.CriticalChance, stats.CriticalMultiplier);
        return statsData;
    }
}
