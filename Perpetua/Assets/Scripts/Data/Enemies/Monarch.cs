using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Enemies/MonarchOfBlissfulDreams")]
public class MonarchData : EnemyData
{
    // make this into a battleAction (EnemyTurn or sth), have enemy decide when turn comes and then execute action
    public override BattleAction SelectTurn(BattleParticipant participant, bool guardIncluded)
    {
        BattleParticipant target = DetectTarget(BattleManager.Instance.party);

        AttackWithSpecificStatusEffect awsse = (AttackWithSpecificStatusEffect)skills[0].Clone();
        awsse.participant = participant;
        awsse.recipient = target;
        AttackWithSpecificStatusEffect stunAttack = (AttackWithSpecificStatusEffect)skills[1].Clone();
        stunAttack.participant = participant;
        stunAttack.recipient = target;

        Attack attack = Attack.New(participant, target);

        Guard guard = Guard.New(participant);

        Dictionary<BattleAction, int> actionsAndWeights = new Dictionary<BattleAction, int>
        {
            {awsse, 10},
            {attack, 15},
            {stunAttack, 10}
        };

        if (guardIncluded) actionsAndWeights.Add(guard, 10);

        return SelectAction(actionsAndWeights);
    }

    public override EnemyData Clone()
    {
        var enemyData = ScriptableObject.CreateInstance<MonarchData>();
        CloneData(this, enemyData);
        return enemyData;
    }
}
