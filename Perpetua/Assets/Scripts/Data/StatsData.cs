using System.Collections.Generic;
using UnityEngine;

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

    public int WillPower;
    public int MaxWillPower;

    public int Detectability;

    public int ExtraTurnCount;

    private void Init(int _HealthPoints, int _MaxHealthPoints, int _PhysicalDamage, int _MagicDamage, int _PhysicalDefense,
        int _MagicDefense, float _AilmentResistance, int _AttackSpeed, float _Dodge, int _Accuracy,
        int _CriticalChance, float _CriticalMultiplier, int _WillPower, int _MaxWillPower, int _Detectability, int _ExtraTurnCount)
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

        WillPower = _WillPower;
        MaxWillPower = _MaxWillPower;

        Detectability = _Detectability;
        ExtraTurnCount = _ExtraTurnCount;
    }

    public static StatsData New(int HealthPoints, int MaxHealthPoints, int PhysicalDamage, int MagicDamage, int PhysicalDefense,
        int MagicDefense, float AilmentResistance, int AttackSpeed, float Dodge, int Accuracy,
        int CriticalChance, float CriticalMultiplier, int WillPower, int MaxWillPower, int Detectability, int ExtraTurnCount)
    {
        var statsData = ScriptableObject.CreateInstance<StatsData>();

        statsData.Init(HealthPoints, MaxHealthPoints, PhysicalDamage, MagicDamage, PhysicalDefense,
        MagicDefense, AilmentResistance, AttackSpeed, Dodge, Accuracy,
        CriticalChance, CriticalMultiplier, WillPower, MaxWillPower, Detectability, ExtraTurnCount);
        return statsData;
    }

    public static StatsData Clone(StatsData stats)
    {
        if (stats == null) return null;

        var statsData = ScriptableObject.CreateInstance<StatsData>();

        statsData.Init(stats.HealthPoints, stats.MaxHealthPoints, stats.PhysicalDamage, stats.MagicDamage, stats.PhysicalDefense,
        stats.MagicDefense, stats.AilmentResistance, stats.AttackSpeed, stats.Dodge, stats.Accuracy,
        stats.CriticalChance, stats.CriticalMultiplier, stats.WillPower, stats.MaxWillPower, stats.Detectability, stats.ExtraTurnCount);
        return statsData;
    }

    public void ApplyStatusEffects(List<StatusEffect> statusEffects)
    {
        foreach(StatusEffect statusEffect in statusEffects)
        {
            statusEffect.InflictPassiveStatusEffect(this);
        }
    }

    public void ApplyEquipment(EquipmentData equipment)
    {
        ApplyItem(equipment.armour);
        ApplyItem(equipment.weapon);
        ApplyItem(equipment.accessory);
        ApplyItem(equipment.rune1);
        ApplyItem(equipment.rune2);
    }

    public void ApplyItem(ItemData item)
    {
        if (item)
        {
            PhysicalDamage += item.WeaponVariables.WeaponDamage;
            MagicDamage += item.WeaponVariables.WeaponMagicDamage;
            if (item.WeaponVariables.WeaponAccuracyMultiplier > 0)
            Accuracy =  Mathf.Min((int) (Accuracy * item.WeaponVariables.WeaponAccuracyMultiplier), 100);
            PhysicalDefense += item.ArmorVariables.ArmorDefense;
            MagicDefense += item.ArmorVariables.ArmorMagicDefense;
            if (item.ArmorVariables.DodgeMultiplier > 0)
            {
                Dodge = Mathf.Min((int)(Dodge * item.ArmorVariables.DodgeMultiplier), 100);
            }
            AttackSpeed = Mathf.Clamp(AttackSpeed + item.ArmorVariables.AttackSpeedModifier, 0, 100);
            ExtraTurnCount += item.RuneVariables.extraTurns;
            if (item.RuneVariables.accuracyMultiplier != 0)
            Accuracy = (int) Mathf.Clamp(Accuracy * item.RuneVariables.accuracyMultiplier, 0f, 100f);
        }
    }
}
