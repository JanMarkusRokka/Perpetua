using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;

public abstract class BattleAction : ScriptableObject
{
    public string tooltip;
    public AudioClipGroup Sound;
    public abstract string GetName();
    public abstract BattleParticipant GetParticipant();
    public abstract void CommitAction();
    public abstract BattleAction Clone();
    public abstract int GetWillPowerUsage();
    public abstract BattleAction CreateFromUI(List<BattleParticipant> participants);
    public abstract bool SelectEnemy();
    public abstract bool SelectPartyMember();
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
        bool partyMemberAttacksEnemy = participant.IsPartyMember && !recipient.IsPartyMember;

        if (rune)
        {
            RuneVariables runeVariables = rune.RuneVariables;
            foreach (StatusEffect statusEffect in runeVariables.recipientStatusEffects)
            {
                if (partyMemberAttacksEnemy)
                {
                    Debug.Log(statusEffect.name + " resistance " + recipient.GetEnemy().GetSpecificAilmentResistance(statusEffect));
                    if (UnityEngine.Random.Range(0, 100) > recipient.GetEnemy().GetSpecificAilmentResistance(statusEffect)) recipient.InflictStatusEffect(statusEffect);

                }
                else
                {
                    recipient.InflictStatusEffect(statusEffect);
                }
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

    public override bool SelectPartyMember()
    {
        return false;
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
        yield return BattleManager.Instance.StartCoroutine(AnimateAttack(battleManager, battleCanvas, totalDamage, commitNextAction, recipient));
        yield break;
    }
    public IEnumerator AnimateAttack(BattleManager battleManager, BattleCanvas battleCanvas, int totalDamage, bool commitNextAction, BattleParticipant attackRecipient)
    {
        yield return new WaitForSeconds(0.1f);
        if (participant.IsPartyMember)
        {
            BattleEffects battleEffects = battleCanvas.battleEffects;
            battleCanvas.SetEnemyHealthBars(true);
            battleCanvas.SetPartyMemberColor(participant.transform, Color.blue);
            Transform recipientTransform = attackRecipient.transform;
            battleEffects.DisplayAttackEnemyEffect(recipientTransform);
            battleManager.DefaultAttackSound.Play();
            yield return new WaitForSeconds(0.5f);
            attackRecipient.participant.stats.HealthPoints = Mathf.Max(0, recipient.GetStatsData().HealthPoints - totalDamage);

            Color defaultColor = recipientTransform.GetComponent<SpriteRenderer>().color;
            recipientTransform.GetComponent<SpriteRenderer>().color = Color.red;
            battleCanvas.RefreshEnemyHealthBars();
            battleCanvas.RefreshEnemyStatusEffects();

            battleEffects.DisplayDamageValue(recipientTransform, totalDamage);

            HashSet<Type> types = new HashSet<Type> { typeof(Shrouded), typeof(Focused) };
            StatusEffectsData participantSF = participant.GetStatusEffectsData();
            participantSF.statusEffects.RemoveAll(a => types.Contains(a.GetType()));

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
            battleCanvas.SetPartyMemberColor(attackRecipient.transform, Color.red);
            yield return new WaitForSeconds(0.75f);
            participant.GetEnemy().attackSound.Play();
            battleEffects.DisplayDamageValueHUD(recipient.transform, totalDamage);
            attackRecipient.participant.stats.HealthPoints = Mathf.Max(0, recipient.GetStatsData().HealthPoints - totalDamage);

            HashSet<Type> types = new HashSet<Type> { typeof(Shrouded), typeof(Focused) };
            StatusEffectsData recipientSF = attackRecipient.GetStatusEffectsData();
            recipientSF.statusEffects.RemoveAll(a => types.Contains(a.GetType()));

            battleCanvas.UpdatePartyTabStats();
            yield return new WaitForSeconds(0.75f);
            battleCanvas.ResetPartyMemberColor(attackRecipient.transform);
        }
        if (commitNextAction)
        battleManager.CommitNextAction();
        yield break;
    }

    public IEnumerator AnimateMiss(BattleManager battleManager, BattleCanvas battleCanvas, bool commitNextAction)
    {
        yield return BattleManager.Instance.StartCoroutine(AnimateMiss(battleManager, battleCanvas, commitNextAction, recipient));
        yield break;
    }

    public IEnumerator AnimateMiss(BattleManager battleManager, BattleCanvas battleCanvas, bool commitNextAction, BattleParticipant attackRecipient)
    {
        yield return new WaitForSeconds(0.1f);

        if (participant.IsPartyMember)
        {
            BattleEffects battleEffects = battleCanvas.battleEffects;
            battleCanvas.SetPartyMemberColor(participant.transform, Color.blue);
            yield return new WaitForSeconds(0.5f);
            battleEffects.DisplayFloatingText(attackRecipient.transform, "Miss");
            yield return new WaitForSeconds(0.5f);
            battleCanvas.ResetPartyMemberColor(participant.transform);
        }
        else
        {
            // Enemy attacks player
            BattleEffects battleEffects = battleCanvas.battleEffects;
            Transform attackerTransform = participant.transform;
            attackerTransform.GetComponent<BattleEnemyAnimator>().PlayAttackAnimation();
            battleCanvas.SetPartyMemberColor(attackRecipient.transform, Color.red);
            yield return new WaitForSeconds(0.75f);
            participant.GetEnemy().attackSound.Play();
            battleEffects.DisplayFloatingTextHUD(attackRecipient.transform, "Miss");

            battleCanvas.UpdatePartyTabStats();
            yield return new WaitForSeconds(0.75f);
            battleCanvas.ResetPartyMemberColor(attackRecipient.transform);
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
        attack.Sound = Sound;

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
        guard.Sound = Sound;

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
    public override bool SelectPartyMember()
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
        participant.GetEnemy().SelectTurn(participant, true).CommitAction();
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
    public override bool SelectPartyMember()
    {
        return false;
    }
}

public class Skip : BattleAction
{
    public BattleParticipant participant;

    public override BattleAction Clone()
    {
        Skip skip = ScriptableObject.CreateInstance<Skip>();
        skip.participant = participant;
        return skip;
    }

    public static BattleAction New(BattleParticipant participant)
    {
        Skip skip = ScriptableObject.CreateInstance<Skip>();
        skip.participant = participant;
        return skip;
    }

    public override void CommitAction()
    {
        BattleManager.Instance.CommitNextAction();
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        throw new NotImplementedException();
    }

    public override string GetName()
    {
        return "Skip";
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
    public override bool SelectPartyMember()
    {
        return false;
    }
}

public class IntentionalMiss : Attack
{
    public override BattleAction Clone()
    {
        IntentionalMiss miss = ScriptableObject.CreateInstance<IntentionalMiss>();
        miss.participant = participant;
        miss.recipient = recipient;
        return miss;
    }

    public new static Attack New(BattleParticipant _attacker, BattleParticipant _recipient)
    {
        IntentionalMiss miss = ScriptableObject.CreateInstance<IntentionalMiss>();
        miss.participant = _attacker;
        miss.recipient = _recipient;
        return miss;
    }

    public override void CommitAction()
    {
        BattleManager.Instance.StartCoroutine(AnimateMiss(BattleManager.Instance, BattleManager.Instance.BattleCanvas, true));
    }

    public override BattleAction CreateFromUI(List<BattleParticipant> participants)
    {
        throw new NotImplementedException();
    }

    public override string GetName()
    {
        return "Intentional Miss";
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
        return true;
    }
    public override bool SelectPartyMember()
    {
        return false;
    }
}