using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(menuName = "Enemies/BigSkeleton")]
public class BigSkeletonData : EnemyData
{
    public override void SelectTurn(BattleParticipant participant)
    {
        Debug.Log("Big skelly");
        if (UnityEngine.Random.Range(0, 100) > 25)
        {
            int recipientId = UnityEngine.Random.Range(0, BattleManager.Instance.party.Count);

            if (UnityEngine.Random.Range(0,100) > 50)
            {
                AttackWithSpecificStatusEffect awsse = (AttackWithSpecificStatusEffect) skills[0].Clone();
                awsse.participant = participant;
                awsse.recipient = BattleManager.Instance.GetPartyMemberFromNumber(recipientId);
                BattleManager.Instance.AddActionToQueue(awsse);
            }
            else
            {

                Attack attack = ScriptableObject.CreateInstance<Attack>();
                attack.participant = participant;
                attack.recipient = BattleManager.Instance.GetPartyMemberFromNumber(recipientId);
                BattleManager.Instance.AddActionToQueue(attack);
            }
        }
        else
        {
            BattleManager.Instance.AddActionToQueue(Guard.New(participant));
        }
    }

    public override EnemyData Clone(EnemyData character)
    {
        var enemyData = ScriptableObject.CreateInstance<BigSkeletonData>();
        StatusEffectsData statusEffectsData = StatusEffectsData.Clone(character.statusEffects);
        enemyData.Init(character.image, character.name, character.description, character.stats, character.loot, character.gonerSprite, character.stunSeconds, character.attackSound, statusEffectsData, character.skills);
        return enemyData;
    }
}
