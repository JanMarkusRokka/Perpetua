using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Trigger Actions/Trigger Battle")]
public class TriggerBattle : TriggerAction
{
    public OverworldEnemy overworldEnemy;
    public override void DoAction()
    {
        overworldEnemy.EnemyData = overworldEnemy.EnemyData;
        overworldEnemy.TriggerFight();
    }
}