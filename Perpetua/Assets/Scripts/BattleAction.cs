using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class BattleAction : ScriptableObject
{
    public abstract string GetName();
    public abstract BattleParticipant GetParticipant();
    public abstract void CommitAction();
    public abstract BattleAction Clone();
    public abstract int GetWillPowerUsage();
    public abstract BattleAction CreateFromUI(List<BattleParticipant> participants);
    public abstract bool SelectEnemy();
}

// Main class for all attacks (skills) that inflict status effects
public class AttackAction : BattleAction
{
    public BattleParticipant participant;
    public BattleParticipant recipient;
    int willPowerUsage = 5;
    public override void CommitAction()
    {
        InflictRuneStatusEffects();
        CommitAttack();
    }

    public void InflictRuneStatusEffects()
    {
        List<StatusEffect> statusEffects = new();
        EquipmentData equipmentData = participant.GetEquipmentData();
        if (equipmentData)
        {
            InflictRuneStatusEffects(equipmentData.rune1);
            InflictRuneStatusEffects(equipmentData.rune2);
        }
    }

    public void InflictRuneStatusEffects(ItemData rune)
    {
        if (rune)
        {
            RuneVariables runeVariables = rune.RuneVariables;
            foreach (StatusEffect statusEffect in runeVariables.recipientStatusEffects)
            {
                recipient.InflictStatusEffect(statusEffect);
            }
        }

    }

    public virtual void CommitAttack() 
    {
        Debug.Log("CommitAttack");
    }

    public override BattleParticipant GetParticipant()
    {
        return participant;
    }

    public override string GetName()
    {
        return "Attack";
    }

    public override BattleAction Clone()
    {
        throw new System.NotImplementedException();
    }

    public static IEnumerator ShowNegativeStatusEffectColor(BattleParticipant participant, float length)
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;

        Color purple = Color.white;
        purple.r = 216;
        purple.g = 191;
        purple.b = 216;
        battleCanvas.SetPartyMemberColor(participant.transform, purple);
        yield return new WaitForSeconds(length);
        battleCanvas.ResetPartyMemberColor(participant.transform);
    }

    public override int GetWillPowerUsage()
    {
        return willPowerUsage;
    }
    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        throw new NotImplementedException();
    }

    public override bool SelectEnemy()
    {
        return true;
    }
}

// Regular attack
public class Attack : AttackAction
{
    int willPowerUsage = 5;
    public override void CommitAction()
    {
        CommitAttack();
    }
    public override void CommitAttack()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;
        int damage = CalculateAttackDamage(participant, recipient);
        if (damage > -1)
        {
            InflictRuneStatusEffects();
            battleManager.StartCoroutine(AnimateAttack(battleManager, battleCanvas, damage, true));
        }
        else
        {
            battleManager.StartCoroutine(AnimateMiss(battleManager, battleCanvas, true));
        }

    }

    public static int CalculateAttackDamage(BattleParticipant participant, BattleParticipant recipient)
    {
        StatsData attackerStats = participant.GetStatsData();
        StatsData recipientStats = recipient.GetStatsData();
        EquipmentData attackerEquipment = participant.GetEquipmentData();
        EquipmentData recipientEquipment = recipient.GetEquipmentData();
        BattleManager battleManager = BattleManager.Instance;

        float landChance = attackerStats.Accuracy - attackerStats.Accuracy * recipientStats.Dodge;

        if (UnityEngine.Random.Range(0, 100) > landChance)
        {
            //Debug.Log(participant.participant.name + " Miss");
            return -1;
        }
        //Debug.Log(participant.participant.name + "Hit");
        int baseDamage = attackerStats.PhysicalDamage;
        int baseMagicDamage = attackerStats.MagicDamage;
        if (attackerEquipment)
        {
            if (attackerEquipment.weapon)
            {
                baseDamage += attackerEquipment.weapon.WeaponVariables.WeaponDamage;
                baseMagicDamage += attackerEquipment.weapon.WeaponVariables.WeaponMagicDamage;
            }
        }

        baseDamage += (int)(UnityEngine.Random.Range(-0.15f, 0.15f) * baseDamage);
        baseMagicDamage += (int)(UnityEngine.Random.Range(-0.15f, 0.15f) * baseMagicDamage);

        float criticalMultiplier = 1f;

        if (UnityEngine.Random.Range(0, 100) < attackerStats.CriticalChance)
        {
            //Critical Strike
            criticalMultiplier = attackerStats.CriticalMultiplier;
        }

        int physicalDefense = recipientStats.PhysicalDefense;
        int magicDefense = recipientStats.MagicDefense;
        if (recipientEquipment)
        {
            if (recipientEquipment.armour)
            {
                physicalDefense += recipientEquipment.armour.ArmorVariables.ArmorDefense;
                magicDefense += recipientEquipment.armour.ArmorVariables.ArmorMagicDefense;
            }
        }

        bool recipientGuarding = (battleManager.GuardDuringTurn.Contains(recipient));

        physicalDefense = physicalDefense * (recipientGuarding ? 2 : 1);
        magicDefense = (int)(magicDefense * (recipientGuarding ? 1.5 : 1));

        int totalDamage = (int)Mathf.Round(Mathf.Max(0f, baseDamage * criticalMultiplier - physicalDefense) +
            Mathf.Max(0f, baseMagicDamage * criticalMultiplier - magicDefense));
        return totalDamage;
    }

    public IEnumerator AnimateAttack(BattleManager battleManager, BattleCanvas battleCanvas, int totalDamage, bool commitNextAction)
    {
        yield return new WaitForSeconds(0.1f);
        if (participant.IsPartyMember)
        {
            BattleEffects battleEffects = battleCanvas.battleEffects;
            battleCanvas.SetEnemyHealthBars(true);
            battleCanvas.SetPartyMemberColor(participant.transform, Color.blue);
            Transform recipientTransform = recipient.transform;
            battleEffects.DisplayAttackEnemyEffect(recipientTransform);

            yield return new WaitForSeconds(0.5f);
            recipient.participant.stats.HealthPoints = Mathf.Max(0, recipient.GetStatsData().HealthPoints - totalDamage);

            Color defaultColor = recipientTransform.GetComponent<SpriteRenderer>().color;
            recipientTransform.GetComponent<SpriteRenderer>().color = Color.red;
            battleCanvas.RefreshEnemyHealthBars();
            battleCanvas.RefreshEnemyStatusEffects();

            battleEffects.DisplayDamageValue(recipientTransform, totalDamage);

            yield return new WaitForSeconds(0.5f);
            recipientTransform.GetComponent<SpriteRenderer>().color = defaultColor;
            battleCanvas.ResetPartyMemberColor(participant.transform);
            battleCanvas.SetEnemyHealthBars(false);
        }
        else
        {
            // Enemy attacks player
            BattleEffects battleEffects = battleCanvas.battleEffects;
            Transform attackerTransform = participant.transform;
            attackerTransform.GetComponent<BattleEnemyAnimator>().PlayAttackAnimation();
            battleCanvas.SetPartyMemberColor(recipient.transform, Color.red);
            yield return new WaitForSeconds(0.75f);
            participant.GetEnemy().attackSound.Play();
            battleEffects.DisplayDamageValueHUD(recipient.transform, totalDamage);
            recipient.participant.stats.HealthPoints = Mathf.Max(0, recipient.GetStatsData().HealthPoints - totalDamage);
            StatusEffectsData recipientSF = recipient.GetStatusEffectsData();
            List<StatusEffect> shrouded = recipientSF.statusEffects.Where(a => a.GetType() == typeof(Shrouded)).ToList();
            if (shrouded.Count > 0) recipientSF.statusEffects.Remove(shrouded[0]);
            battleCanvas.UpdatePartyTabStats();
            yield return new WaitForSeconds(0.75f);
            battleCanvas.ResetPartyMemberColor(recipient.transform);
        }
        if (commitNextAction)
        battleManager.CommitNextAction();
        yield break;
    }

    public IEnumerator AnimateMiss(BattleManager battleManager, BattleCanvas battleCanvas, bool commitNextAction)
    {
        yield return new WaitForSeconds(0.1f);

        if (participant.IsPartyMember)
        {
            BattleEffects battleEffects = battleCanvas.battleEffects;
            battleCanvas.SetPartyMemberColor(participant.transform, Color.blue);
            yield return new WaitForSeconds(0.5f);
            battleEffects.DisplayFloatingText(recipient.transform, "Miss");
            yield return new WaitForSeconds(0.5f);
            battleCanvas.ResetPartyMemberColor(participant.transform);
        }
        if (commitNextAction)
        battleManager.CommitNextAction();
        yield break;
    }

    public static Attack New(BattleParticipant _attacker, BattleParticipant _recipient)
    {
        Attack attack = ScriptableObject.CreateInstance<Attack>();

        attack.participant = _attacker;
        attack.recipient = _recipient;

        return attack;
    }

    public override BattleAction Clone()
    {
        Attack attack = ScriptableObject.CreateInstance<Attack>();

        attack.participant = participant;
        attack.recipient = recipient;

        return attack;
    }

    public override string GetName()
    {
        return "Regular Attack";
    }
    public override int GetWillPowerUsage()
    {
        return willPowerUsage;
    }
    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        Attack attack = (Attack) Clone();
        attack.participant = participants[0];
        attack.recipient = participants[1];
        return attack;
    }
}

public class Guard : BattleAction
{
    public BattleParticipant participant;
    int willPowerUsage = 0;
    public override void CommitAction()
    {
        BattleManager battleManager = BattleManager.Instance;
        battleManager.GuardDuringTurn.Add(participant);

        battleManager.StartCoroutine(AnimateGuard());

        //make attack take guard into account (-50% damage taken and heal?)
    }

    IEnumerator AnimateGuard()
    {
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;
        BattleEffects battleEffects = battleCanvas.battleEffects;
        if (participant.IsPartyMember)
        {
            Transform participantTransform = participant.transform;
            battleEffects.DisplayGuardEffect(participantTransform, true);
        }
        else
        {
            Transform participantTransform = participant.transform;
            battleEffects.DisplayGuardEffect(participantTransform, false);
        }
        yield return new WaitForSeconds(0.5f);
        battleManager.CommitNextAction();
    }

    public static Guard New(BattleParticipant _participant)
    {
        Guard guard = ScriptableObject.CreateInstance<Guard>();
        guard.participant = _participant;
        return guard;
    }
    public override BattleParticipant GetParticipant()
    {
        return participant;
    }

    public override string GetName()
    {
        return "Guard";
    }

    public override BattleAction Clone()
    {
        Guard guard = ScriptableObject.CreateInstance<Guard>();

        guard.participant = participant;

        return guard;
    }

    public override int GetWillPowerUsage()
    {
        return willPowerUsage;
    }
    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        Guard guard = (Guard) Clone();
        guard.participant = participants[0];
        return guard;
    }

    public override bool SelectEnemy()
    {
        return false;
    }
}

public class EnemyTurn : BattleAction
{
    public BattleParticipant participant;

    public override BattleAction Clone()
    {
        EnemyTurn enemyTurn = ScriptableObject.CreateInstance<EnemyTurn>();
        enemyTurn.participant = participant;
        return enemyTurn;
    }

    public static BattleAction New(BattleParticipant participant)
    {
        EnemyTurn enemyTurn = ScriptableObject.CreateInstance<EnemyTurn>();
        enemyTurn.participant = participant;
        return enemyTurn;
    }

    public override void CommitAction()
    {
        participant.GetEnemy().SelectTurn(participant, false).CommitAction();
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        throw new NotImplementedException();
    }

    public override string GetName()
    {
        return "Enemy Turn";
    }

    public override BattleParticipant GetParticipant()
    {
        return participant;
    }

    public override int GetWillPowerUsage()
    {
        return 0;
    }

    public override bool SelectEnemy()
    {
        return false;
    }
}