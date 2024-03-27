using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Enemies/BigSkeleton")]
public class BigSkeletonData : EnemyData
{
    // make this into a battleAction (EnemyTurn or sth), have enemy decide when turn comes and then execute action
    public override BattleAction SelectTurn(BattleParticipant participant, bool guardIncluded)
    {
        BattleParticipant target = DetectTarget(BattleManager.Instance.party);

        AttackWithSpecificStatusEffect awsse = (AttackWithSpecificStatusEffect)skills[0].Clone();
        awsse.participant = participant;
        awsse.recipient = target;

        Attack attack = Attack.New(participant, target);

        Guard guard = Guard.New(participant);

        Dictionary<BattleAction, int> actionsAndWeights = new Dictionary<BattleAction, int>
        {
            {awsse, 10},
            {attack, 10},
        };

        if (guardIncluded) actionsAndWeights.Add(guard, 10);

        return SelectAction(actionsAndWeights);
    }

    public override EnemyData Clone(EnemyData character)
    {
        var enemyData = ScriptableObject.CreateInstance<BigSkeletonData>();
        StatusEffectsData statusEffectsData = StatusEffectsData.Clone(character.statusEffects);
        enemyData.Init(character.image, character.name, character.description, character.stats, character.loot, character.gonerSprite, character.stunSeconds, character.attackSound, statusEffectsData, character.skills, character.ailmentsHistory, character.objectiveToAdvance);
        return enemyData;
    }
}
