using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Enemies/MadMan")]
public class MadManData : EnemyData
{
    int turn;
    // make this into a battleAction (EnemyTurn or sth), have enemy decide when turn comes and then execute action
    public override BattleAction SelectTurn(BattleParticipant participant, bool guardIncluded)
    {
        BattleParticipant target = DetectTarget(BattleManager.Instance.party);

        AttackWithSpecificStatusEffect awsse = (AttackWithSpecificStatusEffect)skills[0].Clone();
        awsse.participant = participant;
        awsse.recipient = target;

        if (turn < 3)
        {
            IntentionalMiss miss = (IntentionalMiss)IntentionalMiss.New(participant, target);
            turn++;
            return miss;
        }

        Attack attack = Attack.New(participant, target);

        Guard guard = Guard.New(participant);

        Dictionary<BattleAction, int> actionsAndWeights = new Dictionary<BattleAction, int>
        {
            {awsse, 10},
            {attack, 10}
        };

        if (guardIncluded) actionsAndWeights.Add(guard, 10);

        return SelectAction(actionsAndWeights);
    }

    public override EnemyData Clone()
    {
        var enemyData = ScriptableObject.CreateInstance<MadManData>();
        CloneData(this, enemyData);
        enemyData.turn = turn;
        return enemyData;
    }
}
