using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Objectives/Defeat Enemy Objective")]

public class DefeatObjective : Objective
{
    public string EnemyName;
    public override bool IsCompleted()
    {
        List<EnemyData> enemies = PartyManager.Instance.party.Enemies.Values.Where(enemy => enemy.name == EnemyName).ToList();
        foreach (EnemyData enemy in enemies)
        {
            if (enemy.GetStatsWithAllEffects().HealthPoints <= 0) return true;
        }
        return false;
    }

    public override Objective Clone()
    {
        DefeatObjective defeatObjective = ScriptableObject.CreateInstance<DefeatObjective>();
        defeatObjective.name = name;
        defeatObjective.id = id;
        defeatObjective.description = description;
        defeatObjective.EnemyName = EnemyName;
        defeatObjective.NextObjective = NextObjective;
        defeatObjective.isVisible = isVisible;
        return defeatObjective;
    }

    public override void AdvanceSpecific()
    {
    }
}
