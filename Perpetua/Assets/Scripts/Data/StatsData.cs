using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Characters/Stats")]
public class StatsData : ScriptableObject
{
    public int HealthPoints;
    public int MaxHealthPoints;

    public int PhysicalDamage;
    public int MagicDamage;

    public int PhysicalDefense;
    public int MagicDefense;

    public float AilmentResistance;

    public int AttackSpeed;

    public float Dodge;
    public int Accuracy;

    public int CriticalChance;
    public float CriticalMultiplier;

    private void Init(int _HealthPoints, int _MaxHealthPoints, int _PhysicalDamage, int _MagicDamage, int _PhysicalDefense,
        int _MagicDefense, float _AilmentResistance, int _AttackSpeed, float _Dodge, int _Accuracy,
        int _CriticalChance, float _CriticalMultiplier)
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

    public static StatsData New(int HealthPoints, int MaxHealthPoints, int PhysicalDamage, int MagicDamage, int PhysicalDefense,
        int MagicDefense, float AilmentResistance, int AttackSpeed, float Dodge, int Accuracy,
        int CriticalChance, float CriticalMultiplier)
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
