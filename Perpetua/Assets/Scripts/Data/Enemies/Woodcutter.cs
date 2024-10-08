using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Enemies/Woodcutter")]
public class Woodcutter : EnemyData
{
    // make this into a battleAction (EnemyTurn or sth), have enemy decide when turn comes and then execute action
    public override BattleAction SelectTurn(BattleParticipant participant, bool guardIncluded)
    {
        List<BattleParticipant> notGoner = BasicEnemy.GetNotGoner();
        BattleParticipant target = ScriptableObject.CreateInstance<BattleParticipant>();

        if (notGoner.Count == 0)
            target = DetectTarget(BattleManager.Instance.party);
        else
            target = DetectTarget(notGoner);

        AttackWithSpecificStatusEffect awsse = (AttackWithSpecificStatusEffect)skills[0].Clone();
        awsse.participant = participant;
        awsse.recipient = target;

        Attack attack = Attack.New(participant, target);

        Guard guard = Guard.New(participant);

        Dictionary<BattleAction, int> actionsAndWeights = new Dictionary<BattleAction, int>
        {
            {awsse, 10},
            {attack, 10},
            {Skip.New(participant), 10 }
        };

        if (guardIncluded) actionsAndWeights.Add(guard, 10);

        return SelectAction(actionsAndWeights);
    }

    public override EnemyData Clone()
    {
        var enemyData = ScriptableObject.CreateInstance<Woodcutter>();
        CloneData(this, enemyData);
        return enemyData;
    }
}
