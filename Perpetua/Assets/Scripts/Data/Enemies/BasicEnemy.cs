using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Enemies/BasicEnemy")]
public class BasicEnemy : EnemyData
{
    // make this into a battleAction (EnemyTurn or sth), have enemy decide when turn comes and then execute action
    public override BattleAction SelectTurn(BattleParticipant participant, bool guardIncluded)
    {
        List<BattleParticipant> notGoner = GetNotGoner();
        BattleParticipant target = ScriptableObject.CreateInstance<BattleParticipant>();
        if (notGoner.Count > 0)
            target = DetectTarget(notGoner);
        else
        {
            return Skip.New(participant);
        }
        Attack attack = Attack.New(participant, target);

        Guard guard = Guard.New(participant);

        Dictionary<BattleAction, int> actionsAndWeights = new Dictionary<BattleAction, int>
        {
            {attack, 10},
        };

        if (guardIncluded) actionsAndWeights.Add(guard, 10);

        return SelectAction(actionsAndWeights);
    }

    public static List<BattleParticipant> GetNotGoner()
    {   
        return BattleManager.Instance.party.FindAll(bp => bp.GetStatsData().HealthPoints > 0);
    }

    public override EnemyData Clone()
    {
        var enemyData = ScriptableObject.CreateInstance<BasicEnemy>();
        CloneData(this, enemyData);
        return enemyData;
    }
}
