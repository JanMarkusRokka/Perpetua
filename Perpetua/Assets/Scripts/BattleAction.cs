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
}

// Main class for all attacks (skills) that inflict status effects
public class AttackAction : BattleAction
{
    public BattleParticipant participant;
    public BattleParticipant recipient;
    public override void CommitAction()
    {
        InflictStatusEffects();
        CommitAttack();
    }

    public void InflictStatusEffects()
    {
        Debug.Log("Inflicting status effects");
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
                StatusEffectsData statusEffectsData = recipient.GetStatusEffectsData();
                List<StatusEffect> sameType = statusEffectsData.statusEffects.Where(statusEf => statusEf.GetType() == statusEffect.GetType()).ToList();
                if (sameType.Count > 0)
                {
                    statusEffectsData.statusEffects.Remove(sameType[0]);
                }
                statusEffectsData.statusEffects.Add(statusEffect.Clone());
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
}

// Regular attack
public class Attack : AttackAction
{
    public override void CommitAction()
    {
        CommitAttack();
    }
    public override void CommitAttack()
    {
        StatsData attackerStats = participant.GetStatsData();
        StatsData recipientStats = recipient.GetStatsData();
        EquipmentData attackerEquipment = participant.GetEquipmentData();
        EquipmentData recipientEquipment = recipient.GetEquipmentData();
        BattleManager battleManager = BattleManager.Instance;
        BattleCanvas battleCanvas = battleManager.BattleCanvas;

        float landChance = attackerStats.Accuracy - attackerStats.Accuracy * recipientStats.Dodge;

        if (Random.Range(0, 100) > landChance)
        {
            //Debug.Log(participant.participant.name + " Miss");
            battleManager.StartCoroutine(AnimateMiss(battleManager, battleCanvas));
            return;
        }
        //Debug.Log(participant.participant.name + "Hit");
        InflictStatusEffects();
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

        baseDamage += (int) (Random.Range(-0.15f, 0.15f) * baseDamage);
        baseMagicDamage += (int) (Random.Range(-0.15f, 0.15f) * baseMagicDamage);

        float criticalMultiplier = 1f;

        if (Random.Range(0, 100) < attackerStats.CriticalChance)
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

        Debug.Log("Damage without guarding: " + (int)Mathf.Round(Mathf.Max(0f, baseDamage * criticalMultiplier - physicalDefense) +
    Mathf.Max(0f, baseMagicDamage * criticalMultiplier - magicDefense)));

        physicalDefense = physicalDefense * ( recipientGuarding ? 2 : 1 );
        magicDefense = (int) (magicDefense * (recipientGuarding ? 1.5 : 1));

        int totalDamage = (int) Mathf.Round(Mathf.Max(0f, baseDamage * criticalMultiplier - physicalDefense) + 
            Mathf.Max(0f, baseMagicDamage * criticalMultiplier - magicDefense));

        battleManager.StartCoroutine(AnimateAttack(battleManager, battleCanvas, totalDamage));
        
    }

    IEnumerator AnimateAttack(BattleManager battleManager, BattleCanvas battleCanvas, int totalDamage)
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
            Debug.Log(recipient);
            battleCanvas.SetPartyMemberColor(recipient.transform, Color.red);
            yield return new WaitForSeconds(0.75f);
            participant.GetEnemy().attackSound.Play();

            battleEffects.DisplayDamageValueHUD(recipient.transform, totalDamage);
            recipient.participant.stats.HealthPoints = Mathf.Max(0, recipient.GetStatsData().HealthPoints - totalDamage);
            battleCanvas.UpdatePartyTabStats();
            yield return new WaitForSeconds(0.75f);
            battleCanvas.ResetPartyMemberColor(recipient.transform);

        }

        battleManager.CommitNextAction();
    }

    IEnumerator AnimateMiss(BattleManager battleManager, BattleCanvas battleCanvas)
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

        battleManager.CommitNextAction();

    }

    public static Attack New(BattleParticipant _attacker, BattleParticipant _recipient)
    {
        Attack attack = ScriptableObject.CreateInstance<Attack>();

        attack.participant = _attacker;
        attack.recipient = _recipient;

        return attack;
    }

    public override string GetName()
    {
        return "Regular Attack";
    }
}

public class Guard : BattleAction
{
    public BattleParticipant participant;
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
}