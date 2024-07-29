using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "BattleActions/SpawnEnemy")]
public class SpawnEnemy : BattleAction
{
    public int WillPowerUsage;
    public BattleParticipant participant;
    public EnemyData EnemyToSpawn;
    public string SkillName;
    public override void CommitAction()
    {
        BattleManager battleManager = BattleManager.Instance;
        Debug.Log("CommitAction spawn");

        battleManager.StartCoroutine(AnimateEffect());
    }

    private IEnumerator AnimateEffect()
    {
        Debug.Log("SpawnEnemy AnimateEffect()");
        BattleManager battleManager = BattleManager.Instance;
        BattleEffects battleEffects = battleManager.BattleCanvas.battleEffects;
        EnemyData enemy = EnemyToSpawn.Clone();
        battleManager.agilityOrder.Add(BattleParticipant.New(enemy));
        battleManager.EnemyData.Add(enemy);
        battleManager.SpawnEnemies();
        battleEffects.ShroudedEffect(participant.transform);
        yield return new WaitForSeconds(0.5f);
        battleManager.CommitNextAction();
    }

    public override string GetName()
    {
        return SkillName;
    }

    public override BattleParticipant GetParticipant()
    {
        return participant;
    }

    public static BattleAction New(BattleParticipant _attacker, EnemyData _enemyToSpawn, int _willpowerUsage, string _skillName)
    {
        SpawnEnemy skill = ScriptableObject.CreateInstance<SpawnEnemy>();

        skill.participant = _attacker;
        skill.EnemyToSpawn = _enemyToSpawn;
        skill.WillPowerUsage = _willpowerUsage;
        skill.SkillName = _skillName;

        return skill;
    }

    public override BattleAction Clone()
    {
        SpawnEnemy skill = ScriptableObject.CreateInstance<SpawnEnemy>();

        skill.participant = participant;
        skill.EnemyToSpawn = EnemyToSpawn;
        skill.WillPowerUsage = WillPowerUsage;
        skill.SkillName = SkillName;

        return skill;
    }

    public override int GetWillPowerUsage()
    {
        return WillPowerUsage;
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        BattleManager battleManager = BattleManager.Instance;
        SpawnEnemy skill = (SpawnEnemy) Clone();
        skill.participant = participants[0];
        return skill;
    }

    public override bool SelectEnemy()
    {
        return false;
    }
    public override bool SelectPartyMember()
    {
        return false;
    }
}